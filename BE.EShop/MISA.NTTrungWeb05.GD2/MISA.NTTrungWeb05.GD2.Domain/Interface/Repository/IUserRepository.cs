
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTRUNG_baseWebAPI_Domain.Interface.Repository
{
    public interface IUserRepository : ICodeRepository<User, UserModel>
    {
        Task<UserModel> GetUserByCodeOrEmail(string email = "", string userCode = "");
    }
}
