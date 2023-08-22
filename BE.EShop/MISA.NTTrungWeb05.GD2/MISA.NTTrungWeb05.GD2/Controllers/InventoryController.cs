using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Excel;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Controllers.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;

namespace MISA.NTTrungWeb05.GD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : CodeController<InventoryResponseDto, InventoryRequestDto, InventoryModel>
    {
        #region Field
        private readonly IInventoryExcelService _InventoryExcelService;
        #endregion
        public InventoryController(IInventoryService inventoryService, IInventoryExcelService unitExcelService) : base(inventoryService)
        {
            _InventoryExcelService = unitExcelService;
        }
        // <summary>
        // Xuất file excel
        // </summary>
        // <returns>file</returns>
        // createdby: nttrung (22/08/2023)
        [HttpPost("Excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExcelRequestDto excelResquest)
        {
            var excelFile = await _InventoryExcelService.ExportExcelAsync(excelResquest);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel.xlsx");
        }
    }
}
