using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.Unit
{
    public class UnitRequestDto : BaseDto
    {
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string UnitName { get; set; } = string.Empty;
        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string UnitCode { get; set; } = string.Empty;
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        public override Guid GetKey()
        {
            return UnitId;
        }
    }
}
