using Dapper;
using MISA.NTTrungWeb05.GD2.Domain;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base;
using NTTRUNG_baseWebAPI_Domain.Interface.Repository;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using System.Data;
using System.Text;

namespace NTTRUNG_BaseWebAPI_Infastructurce.Repository
{
    public class UserRepository : CodeRepository<User, UserModel>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<UserModel> GetUserByCodeOrEmail(string email = "", string userCode = "")
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();
            var pram = !string.IsNullOrWhiteSpace(email) ? "@email" : "@userCode";
            var value = !string.IsNullOrWhiteSpace(email) ? email : userCode;
            var columnWhere = !string.IsNullOrWhiteSpace(email) ? "email" : $"{TableName}Code";
            parameters.Add(pram, value);
            query.Append($"Select UserName, UserCode, PassWord, Email From `{TableName}` where {columnWhere} = {pram}; ");
            var queryString = query.ToString();

            var result = await _uow.Connection.QueryFirstOrDefaultAsync<UserModel>(queryString, parameters, commandType: CommandType.Text, transaction: _uow.Transaction);
            //if (result == null)
            //{
            //    throw new NotFoundException(string.Format(ErrorMessage.NotFound, userCode), (int)ErrorCode.LoginError);
            //}
            return result;
        }
    }
}
