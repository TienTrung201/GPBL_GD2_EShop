using AutoMapper;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_baseWebAPI_Domain.Interface.Repository;
using NTTRUNG_BaseWebAPI_Domain.Model;
using NTTRUNG_BaseWebAPI_Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;

namespace NTTRUNG_BaseWebAPI_Application.Service
{
    public class RoleService : CRUDService<Role, RoleModel, RoleDto, RoleDto>, IRoleService
    {
        public RoleService(IRoleRepository roleRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(roleRepository, mapper, unitOfWork)
        {
        }
    }
}
