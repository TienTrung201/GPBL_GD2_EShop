using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using NTTRUNG_BaseWebAPI_Application.Interface.Service;
using AutoMapper;
using NTTRUNG_baseWebAPI_Domain.Interface.Repository;
using NTTRUNG_BaseWebAPI_Domain.Model;
using NTTRUNG_BaseWebAPI_Domain;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity.Account;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity;
using MISA.NTTrungWeb05.GD2.Domain;
namespace NTTRUNG_BaseWebAPI_Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public AuthService(IUserService userService, IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _userRepository = userRepository;
            _config = configuration;
            _secretKey = _config["JWT:SecretKey"];
            _issuer = _config["JWT:Issuer"];
        }
        public string GenerateJwtToken(string userCode)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userCode) }),
                Expires = DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
             return tokenHandler.WriteToken(token);
        }

        public async Task<UserModel> ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Thêm thiết lập này để không xác thực thời hạn của token (có thể tắt nếu không cần) ValidateLifetime = false }, out SecurityToken validatedToken); // Trả về ClaimsPrincipal chứa các thông tin trong token return principal;
                }, out SecurityToken validatedToken);
                var user = new UserModel();
                if (principal != null && principal.Identity != null && !string.IsNullOrWhiteSpace(principal.Identity.Name))
                {
                     user = await _userRepository.GetUserByCodeOrEmail(userCode: principal.Identity.Name);
                    if (user != null)
                    {
                        return user;
                    }
                }
                return user;
            }
            catch
            {
                throw new AuthenticationException(ErrorMessage.LoginError, (int)ErrorCode.LoginError);
            }
        }

        // Method to authenticate user - Check credentials and return JWT if valid
        public async Task<UserModel> AuthenticateUser(LoginDto loginDto)
        {
            var user = new UserModel();
            if (!string.IsNullOrWhiteSpace(loginDto.UserCode))
            {
                user = await _userRepository.GetUserByCodeOrEmail(userCode: loginDto.UserCode);
            }else if (!string.IsNullOrWhiteSpace(loginDto.Email))
            {
                user = await _userRepository.GetUserByCodeOrEmail(email: loginDto.Email);
            }
            if(user != null)
            {
                // Validate username and password (you might want to retrieve this from a database)
                if (user.PassWord == loginDto.PassWord)
                {
                    user.Token = GenerateJwtToken(user.UserCode);
                    // Generate JWT token
                    return user;
                }
            }
            throw new AuthenticationException(ErrorMessage.LoginError, (int)ErrorCode.LoginError);
        }

        // Method to register new user (you might want to save this info to a database)
        public async Task<string> RegisterUser(RegisterDto registerDto)
        {
            var user = _mapper.Map<UserDto>(registerDto);
            var lstUser = new List<UserDto>();
            lstUser.Add(user);
            user.EditMode = EditMode.Create;
            var result = await _userService.SaveData(lstUser);
            if (result > 0)
            {
                return GenerateJwtToken(user.UserCode);
            }
            throw new DBException(ErrorMessage.RegisterError, (int)ErrorCode.RegisterError);
            // Save username and password to database
        }
    }
}
