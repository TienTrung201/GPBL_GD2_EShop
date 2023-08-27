using MISA.NTTrungWeb05.GD2.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos
{
    public abstract class BaseDto
    {
        /// <summary>
        /// Chế độ chỉnh sửa
        /// </summary>  
        public EditMode EditMode { get; set; } = EditMode.None;
        /// <summary>
        /// Lấy tên id của đối tượng
        /// </summary>
        /// <returns>Tên Id</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public abstract Guid GetKey();
    }
}
