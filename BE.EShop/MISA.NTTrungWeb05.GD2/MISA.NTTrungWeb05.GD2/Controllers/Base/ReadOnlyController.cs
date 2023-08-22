using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Common;

namespace MISA.NTTrungWeb05.GD2.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadOnlyController<TModel, TResponseDto> : ControllerBase
    {
        #region Field
        private readonly IReadOnlyService<TModel, TResponseDto> _readOnlyService;
        #endregion
        #region Constructor
        public ReadOnlyController(IReadOnlyService<TModel, TResponseDto> readOnlyService)
        {
            _readOnlyService = readOnlyService;
        }
        #endregion

        // <summary>
        // Phân trang lọc sắp xếp
        // </summary>
        // <returns>danh sách bản ghi tìm thấy</returns>
        // createdby: nttrung (17/07/2023)
        [HttpPost("filter")]
        public async Task<IActionResult> FilterSortAsync([FromBody] FilterSort filter)
        {
            var result = await _readOnlyService.FilterAsync(filter);
            return Ok(result);
        }
        // <summary>
        // Phân trang tìm kiếm bản ghi
        // </summary>
        // <returns>danh sách bản ghi tìm thấy</returns>
        // createdby: nttrung (17/07/2023)
        [HttpGet("filter")]
        public async Task<IActionResult> FilterAsync([FromQuery] int? pageSize, [FromQuery] int? pageNumber, [FromQuery] string? search)
        {
            var result = await _readOnlyService.FilterAsync(pageSize, pageNumber, search);
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _readOnlyService.GetAllAsync();
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Bản ghi được tìm thấy qua Id</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var result = await _readOnlyService.GetByIdAsync(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
