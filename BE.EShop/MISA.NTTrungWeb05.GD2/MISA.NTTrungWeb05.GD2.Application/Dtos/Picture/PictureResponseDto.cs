using MISA.NTTrungWeb05.GD2.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos
{
    /// <summary>
    /// Đối tượng DTO trả về của picture
    /// </summary>
    /// CreatedBy: NTTrung (31/08/2023)
    public class PictureResponseDto
    {
        /// <summary>
        /// Thông tin ảnh
        /// </summary>
        public Picture Picture { get; set; } 
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string PictureUrl { get; set; } = string.Empty;
    }
}
