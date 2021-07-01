using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface.Repository;
using MISA.ApplicationCore.Interface.Service;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult;
        List<string> _errorMsg;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { Code = MISACode.Valid };
            _errorMsg = new List<string>();
        }
        #endregion

        #region Method
        public ServiceResult DeleteEntity(Guid Id)
        {
            var rowAffect = _baseRepository.DeleteEntity(Id);

            if(rowAffect > 0)
            {
                _serviceResult.Data = rowAffect;
                _serviceResult.Code = MISACode.Success;
                _serviceResult.Message = "Xóa thành công";
            }
            else
            {
                _serviceResult.Code = MISACode.Invalid;
                _serviceResult.Message = "Xóa không thành công";
            }

            return _serviceResult;
        }

        public IEnumerable<TEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        public TEntity GetEntityById(Guid Id)
        {
            return _baseRepository.GetEntityById(Id);
        }

        public virtual ServiceResult InsertEntity(TEntity entity)
        {
            entity.EntityState = EntityState.Add;
            //validate dữ liệu
            this.Validate(entity);

            _serviceResult.Data = entity;

            if (_serviceResult.Code == MISACode.Valid)
            {
                _baseRepository.InsertEntity(entity);
                _serviceResult.Code = MISACode.Success;
            }

            return _serviceResult;
        }

        public virtual ServiceResult UpdateEntity(Guid Id, TEntity entity)
        {
            entity.EntityState = EntityState.Update;
            //validate dữ liệu
            this.Validate(entity);

            _serviceResult.Data = entity;

            return _serviceResult;
        }

        public ServiceResult MutilpleInsert(IEnumerable<TEntity> entities)
        {
            var validRecord = 0;

            //Lấy tất cả dữ liệu trên db để validate
            var allData = _baseRepository.GetEntities();
            //Tạo dictionary để check unique giá trị property
            IDictionary<object, List<string>> uniqueProp = new Dictionary<object, List<string>>();

            //Validate dữ liệu
            foreach (var e in entities)
            {
                e.Status = new List<string>();

                var isValid = this.Validate(e, allData, uniqueProp);

                if (isValid)
                {
                    e.Status.Add("Hợp lệ");
                }
            }

            foreach (var entity in entities)
            {
                if(entity.Status[0].Equals("Hợp lệ"))
                {
                    validRecord++;
                    _baseRepository.InsertEntity(entity);
                }
            }

            var insertInfo = new
            {
                Success = validRecord,
                failed = entities.ToList().Count - validRecord
            };

            _serviceResult.Data = insertInfo;
            _serviceResult.Code = MISACode.Success;

            return _serviceResult;

        }

        /// <summary>
        /// Hàm import dữ liệu từ excel vào db
        /// </summary>
        /// <param name="formFile">File excel cần import</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Kết quả import với từng row</returns>
        /// CreatedBy: NVTOAN 01/07/2021
        public async Task<ServiceResult> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                var msg = new
                {
                    devMsg = "File không có dữ liệu.",
                    userMsg = "File không có dữ liệu.",
                    Code = MISACode.Invalid
                };

                _serviceResult.Data = msg;
                _serviceResult.Code = MISACode.Invalid;
                _serviceResult.Message = "Không có dữ liệu để import";
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                var msg = new
                {
                    devMsg = "Định dạng file không được hỗ trợ.",
                    userMsg = "Định dạng file không được hỗ trợ.",
                    Code = MISACode.Invalid
                };

                _serviceResult.Data = msg;
                _serviceResult.Code = MISACode.Invalid;
                _serviceResult.Message = "Định dạng file không được hỗ trợ";
            }

            var entities = new List<TEntity>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    //Lấy tất cả property để mapping dữ liệu với excel
                    var properties = typeof(TEntity).GetProperties()
                                    .Where(p => p.IsDefined(typeof(DisplayNameAttribute), false))
                                    .Select(p => new
                                    {
                                        PropertyName = p.Name,
                                        DisplayName = p.GetCustomAttributes(typeof(DisplayNameAttribute),
                                                false).Cast<DisplayNameAttribute>().Single().DisplayName,
                                        DataType = p.PropertyType
                                    });

                    for (int row = 3; row <= rowCount; row++)
                    {
                        //Khởi tạo entity
                        var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });

                        for (int col = 1; col <= colCount; col++)
                        {
                            var columnName = worksheet.Cells[2, col].Value != null ?
                                             worksheet.Cells[2, col].Value.ToString().Trim() : "";

                            //Nếu có tên cột trong entity
                            var prop = properties.FirstOrDefault(p => columnName.ToLower().Contains(p.DisplayName.ToLower()));
                            if (prop != null)
                            {
                                var value = worksheet.Cells[row, col].Value != null ?
                                            worksheet.Cells[row, col].Value.ToString().Trim() : "";

                                dynamic temp = this.ConvertDataType(prop.DataType, value);
                                entity.GetType().GetProperty(prop.PropertyName).SetValue(entity, temp);
                            }
                        }

                        //Thêm vào list
                        entities.Add(entity);
                    }

                    //Lấy tất cả dữ liệu trên db để validate
                    var allData = _baseRepository.GetEntities();
                    //Tạo dictionary để check unique giá trị property
                    IDictionary<object, List<string>> uniqueProp = new Dictionary<object, List<string>>();

                    //Validate dữ liệu
                    foreach (var e in entities)
                    {
                        e.Status = new List<string>();

                        var isValid = this.Validate(e, allData, uniqueProp);

                        if(e.Status.Count == 0)
                        {
                            e.Status.Add("Hợp lệ");
                        }
                    }
                }

                _serviceResult.Data = entities;
                _serviceResult.Code = MISACode.Success;
                _serviceResult.Message = "Thành công";
            }


            return _serviceResult;
        }

        /// <summary>
        /// Hàm để chuyển dữ liệu từ dạng string khi lấy từ excel lên đúng kiểu dữ liệu của property
        /// </summary>
        /// <param name="type">Kiểu dữ liệu cần chuyển về</param>
        /// <param name="value">Giá trị dữ liệu cần chuyển</param>
        /// <returns>Dữ liệu đã chuyển về đúng dạng</returns>
        /// CreatedBy: NVTOAN 01/07/2021
        private dynamic ConvertDataType(Type type, string value)
        {
            dynamic res = null;
            if (value == "")
                return res;

            if (type.Name == "Nullable`1")
            {
                type = Nullable.GetUnderlyingType(type);
                //Đổi format ngày tháng
                if (type.Name == "DateTime")
                {
                    var temp = Regex.Split(value, @"/").ToList();
                    while (temp.Count < 3)
                    {
                        temp.Insert(0, "01");
                    }
                    temp.Reverse();
                    value = String.Join('-', temp);
                }
            }

            res = Convert.ChangeType(value, type);

            return res;
        }

        #region Validate
        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>Dữ liệu đã đúng hay chưa</returns>
        /// CreatedBy: NVTOAN 29/06/2021
        private bool Validate(TEntity entity, IEnumerable<TEntity> entities = null, IDictionary<object, List<string>> uniqueProp = null)
        {
            var isValid = true;
            if(entities == null && uniqueProp == null)
            {
                entity.Status = new List<string>();
            }

            foreach (var prop in entity.GetType().GetProperties())
            {
                var displayName = "";
                //Tên hiển thị của property
                if (prop.IsDefined(typeof(DisplayNameAttribute), false))
                {
                    displayName = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                                        .Cast<DisplayNameAttribute>().Single().DisplayName;
                }


                //Validate required
                if (prop.IsDefined(typeof(Required), false))
                {
                    isValid = validateRequired(prop.GetValue(entity), displayName);
                }

                //Validate Unique
                if (prop.IsDefined(typeof(Unique), false) && (isValid || entities != null))
                {
                    isValid = ValidateUnique(entity, prop.Name, displayName, entities, uniqueProp);
                }
            }

            entity.Status.AddRange(_errorMsg);
            _errorMsg.Clear();

            return isValid;
        }

        /// <summary>
        /// Validate không được để trống
        /// </summary>
        /// <param name="value">Dữ liệu cần kiểm tra</param>
        /// <param name="propName">Display name của trường dữ liệu</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        /// CreatedBy: NVTOAN 29/06/2021
        private bool validateRequired(object value, object displayName)
        {
            if (value == null || value.ToString().Length == 0)
            {
                _errorMsg.Add($"{displayName} không được để trống");
                _serviceResult.Code = MISACode.Invalid;
                _serviceResult.Message = "Dữ liệu không hợp lệ";

                return false;
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trùng lặp
        /// </summary>
        /// <param name="entity">Dữ liệu cần kiểm tra</param>
        /// <param name="propName">Field name của dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        /// CrearedBy: NVTOAN 29/06/2021
        private bool ValidateUnique(TEntity entity, string propName, object displayName, IEnumerable<TEntity> entities, IDictionary<object, List<string>> uniqueProp)
        {
            var isUnique = true;
            //insert thông thường
            if(entities == null && uniqueProp == null)
            {
                isUnique = this.ValidateUniqueInsert(entity, propName, displayName);
            } 
            //import
            else
            {
                //Lấy dữ liệu cần kiểm tra
                var value = entity.GetType().GetProperty(propName).GetValue(entity);

                if(value != null)
                {
                    //Validate với dữ liệu trên hệ thống
                    isUnique = this.ValidateUniqueImportDb(entities, propName, value, displayName);

                    //Validate với dữ liệu trong excel
                    isUnique = this.ValidateUniqueImportExcel(uniqueProp, value, propName, displayName);
                }
            }

            if(!isUnique)
            {
                _serviceResult.Code = MISACode.Invalid;
                _serviceResult.Message = "Dữ liệu không hợp lệ";
            }

            return isUnique;
        }

        /// <summary>
        /// Hàm validate Unique khi insert một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// <param name="propName">Tên trường dữ liệu cần kiểm tra</param>
        /// <param name="displayName">Tên hiển thị của trường dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        /// CreatedBy: NVTOAN 01/07/2021
        private bool ValidateUniqueInsert(TEntity entity, string propName, object displayName)
        {
            var entitySearch = _baseRepository.GetEntityByProperty(entity, propName);

            if (entitySearch != null)
            {
                //Nếu là form thêm hoặc là form sửa nhưng id không giống nhau
                if (entity.EntityState == EntityState.Add ||
                    (entity.EntityState == EntityState.Update && this.GetKeyProperty(entity).GetValue(entity) != this.GetKeyProperty(entitySearch).GetValue(entitySearch)))
                {
                    _errorMsg.Add($"{displayName} đã tồn tại");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Hàm kiểm tra dữ liệu đã tồn tại trong database chưa khi import
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="propName">Tên trường dữ liệu cần kiểm tra</param>
        /// <param name="value">Dữ liệu cần kiểm tra</param>
        /// <param name="displayName">Tên hiển thị của trường dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        /// CreatedBy: NVTOAN 01/07/2021
        private bool ValidateUniqueImportDb(IEnumerable<TEntity> entities, string propName, object value, object displayName)
        {
            var entitySearch = entities.Where(e => e.GetType().GetProperty(propName).GetValue(e).ToString() == value.ToString()).FirstOrDefault();

            if (entitySearch != null)
            {
                _errorMsg.Add($"{displayName} đã tồn tại trong hệ thống");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Hàm kiểm tra dữ liệu có trùng trong list dữ liệu import không
        /// </summary>
        /// <param name="uniqueProp">Một map check giá trị trùng lặp</param>
        /// <param name="value">Dữ liệu cần kiểm tra</param>
        /// <param name="propName">Tên trường dữ liệu cần kiểm tra</param>
        /// <param name="displayName">Tên hiển thị của trường dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        /// CreatedBy: NVTOAN 01/07/2021
        private bool ValidateUniqueImportExcel(IDictionary<object, List<string>> uniqueProp, object value, string propName, object displayName)
        {
            //Nếu chưa từng có trong map
            if (!uniqueProp.ContainsKey(value))
            {
                var list = new List<string>();
                list.Add(propName);

                uniqueProp.Add(value, list);
            }
            else
            {
                //Nếu dữ liệu đã từng xuất hiện
                if (uniqueProp[value].Contains(propName))
                {
                    _errorMsg.Add($"{displayName} đã trùng với {displayName} khác nhập khẩu");

                    return false;
                }
                else
                {
                    uniqueProp[value].Add(propName);
                }
            }

            return true;
        }

        private PropertyInfo GetKeyProperty(TEntity entity)
        {
            var keyProperty = entity.GetType()
                .GetProperties()
                .Where(p => p.IsDefined(typeof(PrimaryKey), false))
                .FirstOrDefault();
            return keyProperty;
        }
        #endregion
        #endregion
    }
}
