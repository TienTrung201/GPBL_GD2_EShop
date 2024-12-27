using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Order;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain;
using MISA.NTTrungWeb05.GD2.Domain.Common;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity;
using NTTRUNG_BaseWebAPI_Application.Dtos.Entity.Account;
using NTTRUNG_BaseWebAPI_Application.Interface.Service;
using NTTRUNG_baseWebAPI_Domain.Interface.Repository;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using NTTRUNG_BaseWebAPI_Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NTTRUNG_BaseWebAPI_Application.Service
{
    public class UserService : CodeService<User, UserModel, UserDto, UserDto>, IUserService
    {

        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(userRepository, mapper, unitOfWork)
        {
            _userRepository = userRepository;
        }
        public async override void PreSave(List<UserDto> listData)
        {
            foreach (var item in listData)
            {
                item.PassWord = CommonFunction.Encrypt(item.PassWord);
                item.RoleId = Guid.Parse("74f1ae97-1b08-4227-a5b8-627cfc03ee40");
            }
        }
        public async override Task ValidateListCreate(List<UserDto> data)
        {
            foreach (var item in data)
            {
                if (item.EditMode == EditMode.Create)
                {
                    var user = await _userRepository.GetUserByCodeOrEmail(userCode: item.UserCode);
                    if (user != null)
                    {
                        throw new DuplicateCodeException(string.Format(ErrorMessage.DuplicateError, item.UserCode), (int)ErrorCode.DuplicateCode);
                    }
                }

            }
        }
        /// <summary>
        /// Update mã code với tiền tố và tăng mã code lên 1
        /// </summary>
        /// <param name="data"></param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task AfterSaveSuccess(List<UserDto> listData)
        {

            foreach (var item in listData)
            {

                if (item.EditMode == EditMode.Create )
                {
                    string pattern = "^[A-Za-z]+";
                    string prefix = Regex.Match(item.UserCode, pattern).Value;

                    await _userRepository.UpdateCodeAsync(prefix);
                }

            }


        }
    }
}
