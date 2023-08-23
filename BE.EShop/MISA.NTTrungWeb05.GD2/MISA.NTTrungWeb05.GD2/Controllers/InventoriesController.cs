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
    public class InventoriesController : CodeController<InventoryResponseDto, InventoryRequestDto, InventoryModel>
    {
        #region Field
        private readonly IInventoryExcelService _InventoryExcelService;
        private readonly IInventoryService _InventoryService;
        #endregion
        public InventoriesController(IInventoryService inventoryService, IInventoryExcelService unitExcelService) : base(inventoryService)
        {
            _InventoryExcelService = unitExcelService;
            _InventoryService = inventoryService;
        }
        // <summary>
        // Lưu thông tin hàng hóa
        // </summary>
        // <returns>Số bản ghi thay đổi trong db</returns>
        // createdby: nttrung (22/08/2023)
        [HttpPost("SaveData")]
        public async Task<IActionResult> SaveData([FromBody] InventoryRequestDto data)
        {
            var result = await _InventoryService.SaveData(data);
            return Ok(result);
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
