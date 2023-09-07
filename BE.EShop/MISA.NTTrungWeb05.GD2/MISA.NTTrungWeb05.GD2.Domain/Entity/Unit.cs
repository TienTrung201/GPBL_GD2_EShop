using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Entity
{
    /// <summary>
    /// Entity đơn vị tính
    /// </summary>
    /// CreatedBy NTTrung (21/08/2023)
    public class Unit : BaseAudiEntity
    {
        /// <summary>
        /// Định danh
        /// </summary>
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string UnitName { get; set; } = string.Empty;
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// Lấy Id Entity
        /// </summary>
        public override Guid GetKey()
        {
            return UnitId;
        }
    }
}
