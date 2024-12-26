using MISA.NTTrungWeb05.GD2.Application.Dtos;
using MISA.NTTrungWeb05.GD2.Domain.Enum;

namespace NTTRUNG_BaseWebAPI_Application.Dtos.Entity
{
    public class RoleDto : BaseDto
    {
        /// <summary>
        /// Định danh
        /// </summary>
        public Guid? RoleId { get; set; }
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
