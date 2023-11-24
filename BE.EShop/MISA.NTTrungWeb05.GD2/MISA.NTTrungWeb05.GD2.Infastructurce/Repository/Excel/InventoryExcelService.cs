using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Domain.Common;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Excel
{
    public class InventoryExcelService : ExcelService<InventoryModel, Inventory>, IInventoryExcelService
    {
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryExcelService(IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        /// <summary>
        /// Hàm thêm các detail cho master
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="columns"></param>
        /// <param name="uid"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        /// CreatedBy: NTTrung (12/03/2023)
        public override async Task<int> AddDetailDataExcel(ExcelWorksheet worksheet, List<ColumnExcel> columns, Guid uid, int startRow)
        {

            // thiết lập giá trị cho từng hàng cột-------------------------------------------------------------------------------
            var listData = await _inventoryRepository.GetDetailByParentId(uid);
            var index = 1;
            foreach (var data in listData)  
            {
                startRow++; //Tăng vị trí hàng tiếp theo
                //Cài đặt các hàng------------------------------------------------------------------------------------
                var rowStyle = worksheet.Cells[startRow + 1, 1, startRow + 1, columns.Count() + 1];
                rowStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rowStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(243, 253, 232));
                rowStyle.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc cho các hàng
                rowStyle.Style.Font.Size = 11;
                rowStyle.Style.Font.Color.SetColor(Color.Black);
                rowStyle.Style.Font.Name = "Arial";
                //Border cho các ô
                var rowBodyBorder = rowStyle.Style.Border;
                rowBodyBorder.BorderAround(ExcelBorderStyle.Medium);
                rowBodyBorder.Top.Style = ExcelBorderStyle.Thin;
                rowBodyBorder.Bottom.Style = ExcelBorderStyle.Thin;
                rowBodyBorder.Left.Style = ExcelBorderStyle.Thin;
                rowBodyBorder.Right.Style = ExcelBorderStyle.Thin;
                //Đặt chiều cao cho ô
                worksheet.Row(startRow + 1).Height = 25;
                //căn giữa cột đầu tiên
                worksheet.Cells[startRow + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[startRow + 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc

                //set giá trị cho các ô-------------------------------------------------------------
                worksheet.Cells[startRow + 1, 1].Value = "";
                for (int i = 0; i < columns.Count(); i++)
                {
                    var propertyName = columns[i].Key;
                    PropertyInfo propertyInfo = data.GetType().GetProperty(propertyName);
                    var type = columns[i].Type;
                    var align = columns[i].Align;
                    worksheet.Cells[startRow + 1, i + 2].Style.WrapText = true;
                    await SetValueColumn(worksheet, startRow + 1, i + 2, propertyInfo?.GetValue(data), type, align);
                }
                index++;
            }
            return startRow;
        }
    }
}
