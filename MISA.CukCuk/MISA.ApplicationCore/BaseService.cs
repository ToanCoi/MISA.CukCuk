using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface.Repository;
using MISA.ApplicationCore.Interface.Service;
using OfficeOpenXml;
using System;
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
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { Code = MISACode.Success };
        }
        #endregion

        #region Method
        public ServiceResult DeleteEntity(Guid Id)
        {
            var rowAffect = _baseRepository.DeleteEntity(Id);

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

            var list = new List<TEntity>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    //Lấy tất cả property
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
                        //dynamic entity = new ExpandoObject();
                        //IDictionary<string, object> myUnderlyingObject = entity;

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
                        list.Add(entity);
                    }
                }

                _serviceResult.Data = list;
                _serviceResult.Code = MISACode.Success;
                _serviceResult.Message = "Thành công";
            }


            return _serviceResult;
        }

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

        public virtual ServiceResult InsertEntity(TEntity entity)
        {
            entity.EntityState = EntityState.Add;
            //validate dữ liệu
            this.Validate(entity);

            return _serviceResult;
        }

        public virtual ServiceResult UpdateEntity(Guid Id, TEntity entity)
        {
            entity.EntityState = EntityState.Update;
            //validate dữ liệu
            this.Validate(entity);

            return _serviceResult;
        }

        #region Validate
        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>Dữ liệu đã đúng hay chưa</returns>
        /// CreatedBy: NVTOAN 29/06/2021
        private bool Validate(TEntity entity)
        {
            var isValid = true;
            entity.Status = new List<String>();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var displayName = "";
                //Tên hiển thị của property
                if(prop.IsDefined(typeof(DisplayNameAttribute), false))
                {
                    displayName = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                                        .Cast<DisplayNameAttribute>().Single().DisplayName;
                }
                

                //Validate required
                if (prop.IsDefined(typeof(Required), false))
                {
                    isValid = validateRequired(prop.GetValue(entity), displayName);
                    if (!isValid)
                    {
                        string errorMsg = $"{displayName} không được để trống";
                        entity.Status.Add(errorMsg);

                        _serviceResult.Data = entity.Status;

                        return false;
                    }
                }

                //Validate Unique
                if (isValid && prop.IsDefined(typeof(Unique), false))
                {
                    isValid = ValidateUnique(entity, prop.Name, displayName);
                }
            }

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
        private bool ValidateUnique(TEntity entity, string propName, object displayName)
        {
            var entitySearch = _baseRepository.GetEntityByProperty(entity, propName);


            if (entitySearch != null)
            {
                if (entity.EntityState == EntityState.Add)
                {
                    var msg = new
                    {
                        devMsg = $"{displayName} đã tồn tại",
                        userMsg = $"{displayName} đã tồn tại",
                        Code = MISACode.Invalid
                    };

                    _serviceResult.Data = msg;
                    _serviceResult.Code = MISACode.Invalid;
                    _serviceResult.Message = "Dữ liệu không hợp lệ";

                    return false;
                }
                else if (entity.EntityState == EntityState.Update && this.GetKeyProperty(entity).GetValue(entity) != this.GetKeyProperty(entitySearch).GetValue(entitySearch))
                {
                    var msg = new
                    {
                        devMsg = $"{displayName} đã tồn tại",
                        userMsg = $"{displayName} đã tồn tại",
                        Code = MISACode.Invalid
                    };

                    _serviceResult.Data = msg;
                    _serviceResult.Code = MISACode.Invalid;
                    _serviceResult.Message = "Dữ liệu không hợp lệ";

                    return false;
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
