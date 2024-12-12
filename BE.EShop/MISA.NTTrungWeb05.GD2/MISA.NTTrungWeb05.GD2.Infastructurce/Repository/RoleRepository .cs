using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base;
using NTTRUNG_baseWebAPI_Domain.Interface.Repository;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTRUNG_BaseWebAPI_Infastructurce.Repository
{
    public class RoleRepository : CodeRepository<Role,RoleModel>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
