using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTRUNG_BaseWebAPI_Application.Interface.Service
{
    public interface IUserService : ICodeService<UserDto, UserDto, UserModel>
    {
    }
}
