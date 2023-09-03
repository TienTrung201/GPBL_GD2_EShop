using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;

namespace MISA.NTTrungWeb05.GD2.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController<TResponseDto, TRequestDto, TModel> : CRUDController<TResponseDto, TRequestDto, TModel>
    {
        #region Fields
        protected readonly ICodeService<TResponseDto, TRequestDto, TModel> _codeService;
        #endregion
        #region Constructor

        public CodeController(ICodeService<TResponseDto, TRequestDto, TModel> codeService) : base(codeService)
        {
            _codeService = codeService;
        }
        #endregion
        #region Methods
        /// <summary>
        /// GET new Code
        /// </summary>
        /// <returns>Mã code mới nhất</returns>
        /// CreatedBy: NTTrung (13/07/2023)
        [HttpGet("NewCode/{prefix}")]
        public async Task<IActionResult> GetNewCode([FromRoute] string prefix)
        {
            var result = await _codeService.GetNewCodeAsync(prefix);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        #endregion
    }
}
