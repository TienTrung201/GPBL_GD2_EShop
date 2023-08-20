using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;

namespace MISA.NTTrungWeb05.GD2.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel> : ReadOnlyController<TModel, TEntityDto>
    {
        #region Fields
        protected readonly ICRUDService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel> _crudService;
        #endregion
        #region Constructor
        public CRUDController(ICRUDService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel> baseService) : base(baseService)
        {
            _crudService = baseService;
        }
        #endregion
        #region Methods
        /// <summary>
        /// DELETE - Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi cần xóa</param>
        /// <returns>Trạng thái HTTP 200 OK và số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TEntityCreateDto entity)
        {
            var result = await _crudService.CreatetAsync(entity);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        /// <summary>
        /// DELETE - Xóa nhiều bản ghi
        /// </summary>
        /// <param name="ids">Danh sách Id của các bản ghi cần xóa, phân tách bằng dấu ','</param>
        /// <returns>Trạng thái HTTP 200 OK và số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] TEntityUpdateDto entity)
        {
            var result = await _crudService.UpdateAsync(id, entity);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Trả về số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _crudService.DeleteAsync(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// DELETE Nhiều bản ghi
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Trả về số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpDelete("DeleteMany")]
        public async Task<IActionResult> Delete([FromBody] string ids)
        {
            var result = await _crudService.DeleteManyAsync(ids);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        #endregion
    }
}
