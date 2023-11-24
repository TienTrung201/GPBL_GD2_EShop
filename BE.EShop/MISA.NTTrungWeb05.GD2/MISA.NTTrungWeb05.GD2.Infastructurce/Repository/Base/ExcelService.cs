using MISA.NTTrungWeb05.GD2.Domain.Common;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Excel;
using AutoMapper.Execution;
using System.Globalization;
using MISA.NTTrungWeb05.GD2.Domain.Resources.Value;
using MISA.NTTrungWeb05.GD2.Domain.Entity;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base
{
    public class ExcelService<TModal, TEntity> : IExcelService<TModal> where TEntity : BaseAudiEntity where TModal : TEntity
    {
        #region Field
        private readonly IReadOnlyRepository<TEntity, TModal> _readOnlyRepository;
        public virtual string Title { get; protected set; } = "Danh sách bản ghi";
        public virtual string Sheet { get; protected set; } = "sheet";
        #endregion
        #region Constructor
        public ExcelService(IReadOnlyRepository<TEntity, TModal> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }
        #endregion
        #region Methods
        /// <summary>
        /// cài đặt title cho excel
        /// </summary>
        /// CreatedBy: NTTrung (17/07/2023)
        public void SetTitle(string title)
        {
            Title = title;
        }
        /// <summary>
        /// cài đặt tên sheet cho excel
        /// </summary>
        /// CreatedBy: NTTrung (17/07/2023)
        public void SetSheet(string sheet)
        {
            Sheet = sheet;
        }
        /// <summary>
        /// xuất excel
        /// </summary>
        /// <returns>Byte</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        public async Task<byte[]> ExportExcelAsync(ExcelRequestDto excelResquest)
        {
            if (!string.IsNullOrEmpty(excelResquest.Title))
            {
                SetTitle(excelResquest.Title);
            }
            if (!string.IsNullOrEmpty(excelResquest.Sheet))
            {
                SetSheet(excelResquest.Sheet);
            }
            if (excelResquest.Ids != null)
            {
                return await ExportByIdsAsync(excelResquest.Ids, excelResquest.Columns); ;
            }
            return await ExportAllAsync(excelResquest.Columns);
        }
        /// <summary>
        /// xuất theo danh sách đươc chọn
        /// </summary>
        /// <returns>Byte</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        public async Task<byte[]> ExportByIdsAsync(string ids, IEnumerable<ColumnExcel> columns)
        {
            var listData = await _readOnlyRepository.GetByIdsAsync(ids);
            var result = await GenerateExcelAsync(listData, columns);
            return result;
        }
        /// <summary>
        /// xuất tất cả
        /// </summary>
        /// <returns>byte</returns>
        /// CreatedBy: NTTrung (03/08/2023)
        public async Task<byte[]> ExportAllAsync(IEnumerable<ColumnExcel> columns)
        {
            var listData = await _readOnlyRepository.GetAllAsync();
            var result = await GenerateExcelAsync(listData, columns);
            return result;
        }

        /// <summary>
        /// cài đặt cấu hình file excel và xuất
        /// </summary>
        /// <returns>byte</returns>
        /// CreatedBy: NTTrung (03/08/2023)
        public async Task<byte[]> GenerateExcelAsync(IEnumerable<TModal> listData, IEnumerable<ColumnExcel> columns)
        {
            var convertColumnToList = columns.ToList();

            // Tạo package Excel
            using var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add(Sheet);

            // Định nghĩa kiểu tùy chỉnh
            var customStyle = excelPackage.Workbook.Styles.CreateNamedStyle("CustomStyle");

            // Điền dữ liệu vào worksheet----------------------------------------------------------------------------------------
            var startRow = 4;
            var row = startRow;
            var totalColumns = convertColumnToList.Count();
            worksheet.Cells[1, 1, 1, totalColumns + 1].Value = Title;

            // Thiết lập định dạng cột và chiều rộng cột
            worksheet.Column(1).Width = 10;//STT
            for (int i = 0; i < totalColumns; i++)
            {
                int widthColumn = (int)(convertColumnToList[i].Width / 7.2);
                worksheet.Column(i + 2).Width = widthColumn;
            };


            // Thiết lập font chữ, cỡ chữ, và kiểu chữ cho cột tiêu đề--------------------------------------------------------
            worksheet.Row(1).Height = 40;
            var headerCells = worksheet.Cells[1, 1, 1, totalColumns + 1];
            headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc
            headerCells.Merge = true;
            headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerCells.Style.Font.Bold = true;
            headerCells.Style.Font.Size = 16;
            headerCells.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(222, 222, 222));
            headerCells.Style.Font.Color.SetColor(Color.Black);
            headerCells.Style.Font.Name = "Arial";
            var titlerBorder = headerCells.Style.Border;
            titlerBorder.BorderAround(ExcelBorderStyle.Medium);
            titlerBorder.Top.Style = ExcelBorderStyle.Thin;
            titlerBorder.Bottom.Style = ExcelBorderStyle.Thin;
            titlerBorder.Left.Style = ExcelBorderStyle.Thin;
            titlerBorder.Right.Style = ExcelBorderStyle.Thin;

            //Thiết lập cho thead----------------------------------------------------------------------------------
            worksheet.Row(startRow).Height = 30;
            var tHead = worksheet.Cells[startRow, 1, startRow, totalColumns + 1];
            tHead.Style.Font.Size = 12;
            tHead.Style.Font.Bold = true;
            var headerBorder = tHead.Style.Border;
            headerBorder.BorderAround(ExcelBorderStyle.Medium);
            headerBorder.Top.Style = ExcelBorderStyle.Thin;
            headerBorder.Bottom.Style = ExcelBorderStyle.Thin;
            headerBorder.Left.Style = ExcelBorderStyle.Thin;
            headerBorder.Right.Style = ExcelBorderStyle.Thin;
            tHead.Style.Fill.PatternType = ExcelFillStyle.Solid;
            tHead.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(222, 222, 222));
            // Căn giữa văn bản trong các ô của hàng tiêu đề
            tHead.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa ngang
            tHead.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc

            // thiết lập các tên cột-------------------------------------------------------------------------------
            worksheet.Cells[startRow, 1].Value = "STT";
            for (int i = 0; i < convertColumnToList.Count(); i++)
            {
                worksheet.Cells[startRow, i + 2].Style.WrapText = true;
                worksheet.Cells[startRow, i + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[startRow, i + 2].Value = convertColumnToList[i].Title;
            };

            // thiết lập giá trị cho từng hàng cột-------------------------------------------------------------------------------

            var index = 1;
            foreach (var data in listData)
            {
                //Cài đặt các hàng------------------------------------------------------------------------------------
                var rowStyle = worksheet.Cells[startRow + 1, 1, startRow + 1, totalColumns + 1];
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
                worksheet.Cells[startRow + 1, 1].Value = index;
                for (int i = 0; i < totalColumns; i++)
                {
                    var propertyName = convertColumnToList[i].Key;
                    PropertyInfo propertyInfo = data.GetType().GetProperty(propertyName);
                    var type = convertColumnToList[i].Type;
                    var align = convertColumnToList[i].Align;
                    worksheet.Cells[startRow + 1, i + 2].Style.WrapText = true;
                    await SetValueColumn(worksheet, startRow + 1, i + 2, propertyInfo?.GetValue(data), type, align);
                    //worksheet.Cells[startRow+1, i + 2].Value = value;
                }
                //Custom data
                startRow = await AddDetailDataExcel(worksheet, convertColumnToList, data.GetKey(), startRow);
                index++;
                startRow++; //Tăng vị trí hàng tiếp theo
            }

            // Lưu trữ Excel trong một luồng MemoryStream
            using var stream = new MemoryStream();
            excelPackage.SaveAs(stream);

            // Chuyển đổi MemoryStream thành mảng byte và trả về
            return stream.ToArray();
        }
        /// <summary>
        /// Hàm custom data cho excel
        /// </summary>
        /// <returns>int hàng hiện tại</returns>
        /// CreatedBy: NTTrung (03/08/2023)
        public async virtual Task<int> AddDetailDataExcel(ExcelWorksheet worksheet, List<ColumnExcel> columns, Guid uid, int startRow)
        {
            return startRow;
        }

        /// <summary>
        /// Hàm điền value vào từng cột
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        protected async Task SetValueColumn(ExcelWorksheet worksheet, int row, int col, object? value, TypeColumn? type, AlignColumn? align)
        {
            var horizontalAlign = ExcelHorizontalAlignment.Left;

            switch (align)
            {
                case AlignColumn.Center:
                    horizontalAlign = ExcelHorizontalAlignment.Center;
                    break;
                case AlignColumn.Right:
                    horizontalAlign = ExcelHorizontalAlignment.Right;
                    break;
                default:
                    break;
            }

            worksheet.Cells[row, col].Style.HorizontalAlignment = horizontalAlign;
            worksheet.Cells[row, col].Value = value;
            switch (type)
            {
                case TypeColumn.Date:
                    if (value is DateTime dateValue)
                    {
                        worksheet.Cells[row, col].Value = dateValue.ToString("dd/MM/yyyy");
                    }
                    break;
                case TypeColumn.Gender:
                    var fieldInfo = value.GetType().GetField(value.ToString());//lấy thông tin về trường enum tương ứng với giá trị value
                    var nameGender = "";
                    if (fieldInfo != null)
                    {
                        //để lấy danh sách các thuộc tính tùy chỉnh trên trường đó. có mỗi Description
                        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attributes.Length > 0)
                        {
                            nameGender = attributes[0].Description.ToString();
                        }
                    }
                    worksheet.Cells[row, col].Value = nameGender;
                    break;
                case TypeColumn.Money:
                    // Tạo một CultureInfo phù hợp với ngôn ngữ và vùng
                    CultureInfo cultureInfo = new CultureInfo("vi-VN"); //Tiếng Việt

                    // Định dạng số theo CultureInfo và chèn dấu ngăn cách.
                    if (value is decimal)
                    {
                        var decimalValue = (decimal)value; // Chuyển đối tượng thành decimal
                        worksheet.Cells[row, col].Value = decimalValue.ToString("#,##0", cultureInfo);
                    }
                    break;
                case TypeColumn.Show:
                    if (value is bool)
                    {
                        if ((bool)value == true)
                        {
                            worksheet.Cells[row, col].Value = ValueDefault.Yes;
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = ValueDefault.No;
                        }
                    }
                    break;
                case TypeColumn.Business:
                    if (value is bool)
                    {
                        if ((bool)value == true)
                        {
                            worksheet.Cells[row, col].Value = ValueDefault.Active;
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = ValueDefault.NoActive;
                        }
                    }
                    break;
                default:
                    worksheet.Cells[row, col].Value = value;
                    break;
            }
        }
        #endregion
    }
}
