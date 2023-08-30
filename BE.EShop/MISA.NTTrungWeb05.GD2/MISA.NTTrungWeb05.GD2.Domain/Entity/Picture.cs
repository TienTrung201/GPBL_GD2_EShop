using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Entity
{/// <summary>
 /// Entity ảnh
 /// </summary>
 /// CreatedBy NTTrung (21/08/2023)
    public class Picture
    {
        /// <summary>
        /// Định danh ảnh
        /// </summary>
        public Guid PictureId { get; set; }
        /// <summary>
        /// Tên ảnh
        /// </summary>
        public string PictureName { get; set; } = string.Empty;
        /// <summary>
        /// Tên mở rộng
        /// </summary>
        public string Extension { get; set; } = string.Empty;
    }
}
