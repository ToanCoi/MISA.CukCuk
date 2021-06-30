using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interface.Service
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        /// CreatedBy: NVTOAN 28/06/2021
        IEnumerable<TEntity> GetEntities();

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        /// CreatedBy: NVTOAN 28/06/2021
        TEntity GetEntityById(Guid Id);

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// NVTOAN 28/06/2021
        ServiceResult InsertEntity(TEntity entity);

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: NVTOAN 28/06/2021
        ServiceResult UpdateEntity(Guid Id, TEntity entity);

        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult DeleteEntity(Guid Id);

        Task<ServiceResult> Import(IFormFile formFile, CancellationToken cancellationToken);
    }
}
