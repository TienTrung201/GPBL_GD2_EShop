using MISA.NTTrungWeb05.GD2.Application.Dtos.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Excel
{
    public interface IExcelService<TModel>
    {
        /// <summary>
        /// xuất excel
        /// </summary>
        /// <returns>Byte</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        Task<byte[]> ExportExcelAsync(ExcelRequestDto excelResquest);

    }
}
