using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        string _tableName = string.Empty;
        #endregion

        #region Constructor

        public BaseRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }

        #endregion

        #region Method
        public int DeleteEntity(Guid Id)
        {
            var idParam = new DynamicParameters();
            idParam.Add($"{_tableName}Id", Id);
            int rowAffects = _dbConnection.Execute($"Proc_Delete{_tableName}ById", idParam, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        public IEnumerable<TEntity> GetEntities()
        {
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure);

            return entities;
        }

        public TEntity GetEntityById(Guid Id)
        {
            var idParam = new DynamicParameters();
            idParam.Add($"{_tableName}Id", Id);
            var customer = _dbConnection.QueryFirstOrDefault<TEntity>($"Proc_Get{_tableName}ById", idParam, commandType: CommandType.StoredProcedure);

            return customer;
        }

        public int InsertEntity(TEntity entity)
        {
            int rowAffects = _dbConnection.Execute($"Proc_Insert{_tableName}", entity, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        public int UpdateEntity(Guid Id, TEntity entity)
        {
            var dynamicParam = MappingData(entity);

            int rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}", dynamicParam, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        /// <summary>
        /// Hàm mapping dữ liệu
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Object cần mapping</param>
        /// <returns>Object chứa các data mapping</returns>
        private DynamicParameters MappingData(TEntity entity)
        {
            var dynamicParam = new DynamicParameters();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                var propType = prop.PropertyType;

                if (propType == typeof(Guid) || propType == typeof(Guid?))
                {
                    dynamicParam.Add($"@{propName}", propValue, DbType.String);
                }
                else
                {
                    dynamicParam.Add($"@{propName}", propValue);
                }
            }

            return dynamicParam;
        }

        public TEntity GetEntityByProperty(TEntity entity, string propName)
        {
            var propvalue = entity.GetType().GetProperty(propName).GetValue(entity);

            string query = $"select * FROM {_tableName} where {propName} = '{propvalue}'";
            var entitySearch = _dbConnection.QueryFirstOrDefault<TEntity>(query);

            return entitySearch;
        }

        #endregion
    }
}
