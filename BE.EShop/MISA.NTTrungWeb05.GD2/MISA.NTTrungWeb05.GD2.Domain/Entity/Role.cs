using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTRUNG_BaseWebAPI_Domain.Entity
{
    /// <summary>
    /// Role
    /// </summary>
    /// CreatedBy NTTrung 21.04.24
    public class Role : BaseAudiEntity
    {
        /// <summary>
        /// Định danh
        /// </summary>
        public Guid RoleId { get; set; }
        public EnumRole? RoleType { get; set; }
        /// <summary>
        /// Tên user
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; } = string.Empty;
    }
}
