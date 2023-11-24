using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service.Base
{
    public abstract class CodeService<TEntity, TModel, TEntityResponseDto, TEntityRequestDto>
    : CRUDService<TEntity, TModel, TEntityResponseDto, TEntityRequestDto>,
        ICodeService<TEntityResponseDto, TEntityRequestDto, TModel> where TEntityRequestDto :BaseDto where TEntity : BaseAudiEntity

    {
        #region Fields
        protected readonly ICodeRepository<TEntity, TModel> _codeRepository;
        #endregion
        #region Constructor
        protected CodeService(ICodeRepository<TEntity, TModel> codeRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(codeRepository, mapper, unitOfWork)
        {
            _codeRepository = codeRepository;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Get code mới nhất
        /// </summary>
        /// <returns>Mã code mới</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<string> GetNewCodeAsync(string prefix)
        {
            var newCode = await _codeRepository.GetNewCodeAsync(prefix);
            return newCode;
        }
        #endregion
    }
}
