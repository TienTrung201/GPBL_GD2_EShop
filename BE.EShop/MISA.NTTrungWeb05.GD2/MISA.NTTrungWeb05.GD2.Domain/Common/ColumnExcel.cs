using MISA.NTTrungWeb05.GD2.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Common
{
    public class ColumnExcel
    {
        /// <summary>
        /// Tên cột
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        [Required]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Thuộc tính
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        [Required]
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// ĐỘ rộng cột
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public double? Width { get; set; }
        /// <summary>
        /// Loại dữ liệu
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public TypeColumn? Type { get; set; } // Bạn có thể tùy chỉnh kiểu dữ liệu cho trường Type tùy theo yêu cầu.
        /// <summary>
        /// Căn vị trí
        /// </summary>
        /// CreatedBy: NTTrung (03/08/2023)
        public AlignColumn? Align { get; set; }
    }
}
