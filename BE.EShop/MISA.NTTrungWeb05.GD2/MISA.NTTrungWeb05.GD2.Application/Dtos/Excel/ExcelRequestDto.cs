using MISA.NTTrungWeb05.GD2.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.Excel
{
    public class ExcelRequestDto
    {
        /// <summary>
        /// Mảng các cột
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        [Required]
        public IEnumerable<ColumnExcel> Columns { get; set; }
        /// <summary>
        /// Chuỗi các Id cần xuất
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public string? Ids { get; set; }
        /// <summary>
        /// tìm kiếm theo nội dung search
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public string? Search { get; set; }
        /// <summary>
        /// tên của tiêu đề bảng excel vd: Danh sách nhân viên
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public string? Title { get; set; }
        /// <summary>
        /// tên của sheet
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public string? Sheet { get; set; }
    }
}
