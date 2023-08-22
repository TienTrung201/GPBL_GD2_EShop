using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service.Base
{
    public abstract class CRUDService<TEntity, TModel, TEntityResponseDto, TEntityRequestDto>
        : ReadOnlyService<TEntity, TModel, TEntityResponseDto>,
        ICRUDService<TEntityResponseDto, TEntityRequestDto, TModel>
    {
        #region Fields
        protected readonly ICRUDRepository<TEntity, TModel> _baseRepository;
        protected readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Constructor
        protected CRUDService(ICRUDRepository<TEntity, TModel> baseRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(baseRepository, mapper)
        {
            _unitOfWork = unitOfWork;
            _baseRepository = baseRepository;
        }

        #endregion
        #region Methods
        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi được thêm</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public virtual async Task<int> CreatetAsync(TEntityRequestDto entityCreateDto)
        {
            //validate nghiệp vụ
            var entity = await MapCreateDtoToEntityValidateAsync(entityCreateDto);
            var result = await _baseRepository.InsertAsync(entity);
            return result;
        }
        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi sửa</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public virtual async Task<int> UpdateAsync(Guid id, TEntityRequestDto entityUpdateDto)
        {
            //validate nghiệp vụ
            var entity = await MapUpdateDtoToEntityValidateAsync(id, entityUpdateDto);
            var result = await _baseRepository.UpdateAsync(entity);
            return result;
        }
        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <paran name="id">id của bản ghi cần xóa</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public virtual async Task<int> DeleteAsync(Guid id)
        {

            var model = await _baseRepository.GetByIdAsync(id);

            var entity = _mapper.Map<TEntity>(model);

            var result = await _baseRepository.DeleteAsync(entity);
            return result;

        }
        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <paran name="ids">Chuỗi id của bản ghi cần xóa</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<int> DeleteManyAsync(string ids)
        {
            //try
            //{
            //    await _unitOfWork.BeginTransactionAsync();
            var result = await _baseRepository.DeleteManyAsync(ids);
            //await _unitOfWork.CommitAsync();
            return result;
            //}
            //catch (Exception)
            //{
            //    await _unitOfWork.RollBackAsync();
            //    throw;
            //}

        }
        /// <summary>
        /// Validate nghiệp vụ
        /// </summary>
        /// <paran name="createDto">thông tin bản ghi gửi đến</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public abstract Task<TEntity> MapCreateDtoToEntityValidateAsync(TEntityRequestDto createDto);
        /// <summary>
        /// Validate nghiệp vụ
        /// </summary>
        /// <paran name="ids">Id bản ghi gửi đến</paran>
        /// <paran name="createDto">thông tin bản ghi gửi đến</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public abstract Task<TEntity> MapUpdateDtoToEntityValidateAsync(Guid id, TEntityRequestDto updateDto);
        #endregion
    }
}
