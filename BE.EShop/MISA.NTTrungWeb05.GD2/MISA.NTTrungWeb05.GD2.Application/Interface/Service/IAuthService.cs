using NTTRUNG_BaseWebAPI_Application.Dtos.Entity.Account;

namespace NTTRUNG_BaseWebAPI_Application.Interface.Service
{
    public interface IAuthService
    {
        Task<string> AuthenticateUser(LoginDto loginDto);
        Task<string> RegisterUser(RegisterDto registerDto);
    }
}
