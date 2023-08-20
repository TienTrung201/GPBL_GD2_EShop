using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;

namespace MISA.NTTrungWeb05.GD2.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel> : CRUDController<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel>
    {
        #region Fields
        protected readonly ICodeService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel> _codeService;
        #endregion
        #region Constructor

        public CodeController(ICodeService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, TModel> codeService) : base(codeService)
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
        [Route("NewCode")]
        [HttpGet]
        public async Task<IActionResult> GetNewCode()
        {
            var result = await _codeService.GetNewCodeAsync();
            return StatusCode(StatusCodes.Status200OK, result);
        }
        #endregion
    }
}
