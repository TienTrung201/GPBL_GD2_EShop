using MISA.NTTrungWeb05.GD2.Domain.Enum;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTRUNG_BaseWebAPI_Domain.Model
{
    public class UserModel : User
    {
        public EnumRole? RoleType { get; set; }
        public string? RoleName { get; set; }
    }
}
