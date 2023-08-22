using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Excel;
using MISA.NTTrungWeb05.GD2.Application.Dtos.ItemCategory;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Controllers.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;

namespace MISA.NTTrungWeb05.GD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCategoriesController : CodeController<ItemCategoryResponseDto, ItemCategoryRequestDto, ItemCategoryModel>
    {
        #region Field
        private readonly IItemCategoryExcelService _itemCategoryExcelService;
        #endregion
        public ItemCategoriesController(IItemCategoryService itemCategoryService, IItemCategoryExcelService itemCategoryExcelService) : base(itemCategoryService)
        {
            _itemCategoryExcelService = itemCategoryExcelService;
        }
        // <summary>
        // Xuất file excel
        // </summary>
        // <returns>file</returns>
        // createdby: nttrung (22/08/2023)
        [HttpPost("Excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExcelRequestDto excelResquest)
        {
            var excelFile = await _itemCategoryExcelService.ExportExcelAsync(excelResquest);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel.xlsx");
        }
    }
}
