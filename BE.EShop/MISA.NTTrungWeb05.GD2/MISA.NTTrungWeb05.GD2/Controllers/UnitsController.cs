using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Excel;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Controllers.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;

namespace MISA.NTTrungWeb05.GD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : CodeController<UnitResponseDto, UnitRequestDto, UnitModel>
    {
        #region Field
        private readonly IUnitExcelService _unitExcelService;
        #endregion
        public UnitsController(IUnitService unitService, IUnitExcelService unitExcelService) : base(unitService)
        {
            _unitExcelService = unitExcelService;
        }
        // <summary>
        // Xuất file excel
        // </summary>
        // <returns>file</returns>
        // createdby: nttrung (22/08/2023)
        [HttpPost("Excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExcelRequestDto excelResquest)
        {
            var excelFile = await _unitExcelService.ExportExcelAsync(excelResquest);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel.xlsx");
        }
    }
}
