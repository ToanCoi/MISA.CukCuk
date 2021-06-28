using MISA.ApplicationCore.Interface.Repository;
using MISA.ApplicationCore.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        #region Declare
        IBaseRepository<TEntity> _baseRepository;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        #endregion

        #region Method
        public int DeleteEntity(Guid Id)
        {
            var rowAffect = _baseRepository.DeleteEntity(Id);

            return rowAffect;
        }

        public IEnumerable<TEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        public TEntity GetEntityById(Guid Id)
        {
            return _baseRepository.GetEntityById(Id);
        }

        public virtual int InsertEntity(TEntity entity)
        {
            var rowAffect = _baseRepository.InsertEntity(entity);

            return rowAffect;
        }

        public virtual int UpdateEntity(Guid Id, TEntity entity)
        {
            var rowAffect = _baseRepository.UpdateEntity(Id, entity);

            return rowAffect;
        }
        #endregion
    }
}
