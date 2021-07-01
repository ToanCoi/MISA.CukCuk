using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity> : ControllerBase
    {
        #region Declare
        IBaseService<TEntity> _baseService;
        #endregion

        #region Constructor
        public BaseController(IBaseService<TEntity> baseService)
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
            var serviceResult = _baseService.InsertEntity(entity);

            if (serviceResult.Code == MISACode.Invalid)
            {
                return BadRequest(serviceResult);
            }
            else
            {
                return Ok(serviceResult);
            }
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
            var serviceResult = _baseService.UpdateEntity(Id, entity);

            if(serviceResult.Code == MISACode.Invalid)
            {
                return BadRequest(serviceResult);
            }
            else
            {
                return Ok(serviceResult);
            }
        }

        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var serviceResult = _baseService.DeleteEntity(Id);

            if (serviceResult.Code == MISACode.Success)
            {
                return Ok(serviceResult);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("import")]
        public Task<ServiceResult> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            return _baseService.Import(formFile, cancellationToken);
        }

        [HttpPost]
        public IActionResult MutilpleInsert([FromBody] IEnumerable<TEntity> entities)
        {
            var serviceResult = _baseService.MutilpleInsert(entities);

            return Ok(serviceResult);
        }
        #endregion
    }
}
