using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity;
using NTTRUNG_BaseWebAPI_Application.Interface.Service;
using NTTRUNG_baseWebAPI_Domain.Interface.Repository;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTRUNG_BaseWebAPI_Application.Service
{
    public class UserService : CodeService<User, UserModel, UserDto, UserDto>, IUserService
    {
        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(userRepository, mapper, unitOfWork)
        {
        }
    }
}
