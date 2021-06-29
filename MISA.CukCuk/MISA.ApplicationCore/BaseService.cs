using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface.Repository;
using MISA.ApplicationCore.Interface.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        public virtual ServiceResult InsertEntity(TEntity entity)
        {
            entity.EntityState = EntityState.Add;
            //validate dữ liệu
            
            var rowAffect = _baseRepository.InsertEntity(entity);

            return _serviceResult;
        }

        public virtual ServiceResult UpdateEntity(Guid Id, TEntity entity)
        {
            entity.EntityState = EntityState.Update;
            var rowAffect = _baseRepository.UpdateEntity(Id, entity);

            return _serviceResult;
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>Dữ liệu đã đúng hay chưa</returns>
        /// CreatedBy: NVTOAN 29/06/2021
        private bool Validate(TEntity entity)
        {
            var isValid = true;

            foreach (var prop in entity.GetType().GetProperties())
            {
                var displayName = (DisplayNameAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(DisplayNameAttribute));

                if (prop.IsDefined(typeof(Required), false))
                {
                    isValid = validateRequired(entity, displayName);
                }

                if(isValid && prop.IsDefined(typeof(Unique), false))
                {
                    isValid = ValidateUnique(entity, prop.Name);
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
                var msg = new
                {
                    devMsg = $"{displayName} không được để trống",
                    userMsg = $"{displayName} không được để trống",
                    Code = MISACode.Invalid
                };

                _serviceResult.Data = msg;
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
        private bool ValidateUnique(TEntity entity, string propName)
        {
            var entitySearch = _baseRepository.GetEntityByProperty(entity, propName);

            if(entity.EntityState == EntityState.Add)
            {

            } 

            return true;
        }
        #endregion
    }
}
