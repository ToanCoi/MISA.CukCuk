using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseEntityController<TEntity> : ControllerBase
    {
        #region Declare
        IBaseService<TEntity> _baseService;
        #endregion

        #region Constructor
        public BaseEntityController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseService.GetEntities();

            return Ok(entities);
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi</param>
        /// <returns>Một đối tượng tìm được theo Id</returns>
        /// CreateedBy: NVTOAN 24/06/2021
        [HttpGet("{Id}")]
        public IActionResult Get([FromRoute] Guid Id)
        {
            var customer = _baseService.GetEntityById(Id);

            return Ok(customer);
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [HttpPost]
        public IActionResult Post([FromBody] TEntity entity)
        {
            var rowAffect = _baseService.InsertEntity(entity);

            return Ok(rowAffect);
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng với thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [HttpPut("{Id}")]
        public IActionResult Put(Guid Id, TEntity entity)
        {
            var rowAffect = _baseService.UpdateEntity(Id, entity);

            return Ok(rowAffect);
        }

        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var rowAffect = _baseService.DeleteEntity(Id);

            return Ok(rowAffect);
        }
        #endregion
    }
}
