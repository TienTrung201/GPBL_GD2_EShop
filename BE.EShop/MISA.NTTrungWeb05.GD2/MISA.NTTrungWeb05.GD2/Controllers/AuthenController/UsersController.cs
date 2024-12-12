using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NTTRUNG_BaseWebAPI_Application.Service;
using NTTRUNG_BaseWebAPI_Application.Interface.Service;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity;
using MISA.NTTrungWeb05.GD2.Controllers.Base;
namespace NTTRUNG_BaseWebAPI_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CodeController<UserDto, UserDto, UserModel>
    {
        public UsersController(IUserService userService) : base(userService)
        {
        }
    }
}
