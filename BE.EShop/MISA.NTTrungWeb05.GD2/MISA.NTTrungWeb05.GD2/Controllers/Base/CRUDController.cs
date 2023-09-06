using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;

namespace MISA.NTTrungWeb05.GD2.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController<TResponseDto, TRequestDto, TModel> : ReadOnlyController<TModel, TResponseDto>
    {
        #region Fields
        protected readonly ICRUDService<TResponseDto, TRequestDto, TModel> _crudService;
        #endregion
        #region Constructor
        public CRUDController(ICRUDService<TResponseDto, TRequestDto, TModel> crudService) : base(crudService)
        {
            _crudService = crudService;
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
        public async Task<IActionResult> Post([FromBody] TRequestDto entity)
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
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] TRequestDto entity)
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
        // <summary>
        // Lưu thông tin hàng hóa
        // </summary>
        // <returns>Số bản ghi thay đổi trong db</returns>
        // createdby: nttrung (22/08/2023)
        [HttpPost("SaveData")]
        public async Task<IActionResult> SaveData([FromBody] List<TRequestDto> listData)
        {
            var result = await _crudService.SaveData(listData);
            return Ok(result);
        }
        #endregion
    }
}
