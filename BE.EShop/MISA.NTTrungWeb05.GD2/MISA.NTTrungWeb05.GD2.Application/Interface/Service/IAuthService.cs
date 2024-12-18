using NTTRUNG_BaseWebAPI_Application.Dtos.Entity.Account;
using NTTRUNG_BaseWebAPI_Domain.Model;

namespace NTTRUNG_BaseWebAPI_Application.Interface.Service
{
    public interface IAuthService
    {
        Task<UserModel> AuthenticateUser(LoginDto loginDto);
        Task<string> RegisterUser(RegisterDto registerDto);
        Task<UserModel> ValidateJwtToken(string registerDto);
       
    }
}
