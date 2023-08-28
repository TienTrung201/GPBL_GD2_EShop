using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
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
        ICRUDService<TEntityResponseDto, TEntityRequestDto, TModel> where TEntityRequestDto : BaseDto where TEntity : BaseAudiEntity
    {
        #region Fields
        protected readonly ICRUDRepository<TEntity, TModel> _crudRepository;
        protected readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Constructor
        protected CRUDService(ICRUDRepository<TEntity, TModel> crudRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(crudRepository, mapper)
        {
            _unitOfWork = unitOfWork;
            _crudRepository = crudRepository;
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
            var result = await _crudRepository.InsertAsync(entity);
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
            var result = await _crudRepository.UpdateAsync(entity);
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

            var model = await _crudRepository.GetByIdAsync(id);

            var entity = _mapper.Map<TEntity>(model);

            var result = await _crudRepository.DeleteAsync(entity);
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
            var result = await _crudRepository.DeleteManyAsync(ids);
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


        /// <summary>
        /// Thêm sửa xóa data
        /// </summary>
        /// <paran name="DATA">Thông tin hàng hóa và list Item</paran>
        /// <returns>Bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        public async Task<int> SaveData(TEntityRequestDto data)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                PreSave(data);
                var result = 0;
                //Cập nhật Master

                switch (data.EditMode)
                {
                    case EditMode.Create:
                        var dataInsert = await MapCreateDtoToEntityValidateAsync(data);
                        data.SetValue($"{typeof(TEntity).Name}Id", dataInsert.GetKey());
                        result += await _crudRepository.InsertAsync(dataInsert);
                        break;
                    case EditMode.Update:
                        var dataUpdate = await MapUpdateDtoToEntityValidateAsync(data.GetKey(), data);
                        result += await _crudRepository.UpdateAsync(dataUpdate);
                        break;
                    default:
                        break;
                }
                result += await AfterSave(data);
                await _unitOfWork.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                throw;
            }
        }
        /// <summary>
        /// Hàm Thêm sửa xóa
        /// </summary>
        /// <paran name="DATA">Thông tin hàng hóa và list Item</paran>
        /// <returns>Bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        public async Task<int> CUDListService(List<TEntityRequestDto> data)
        {
            //Nếu có detail thì cập nhật Master và Detail
            int result = 0;
            var listDelete = data.Where(entity => entity.EditMode == EditMode.Delete).ToList();
            var listUpdate = data.Where(entity => entity.EditMode == EditMode.Update).ToList();
            var listCreate = data.Where(entity => entity.EditMode == EditMode.Create).ToList();

            //--------------------------------------Xóa những hàng hóa bị xóa--------------------------
            if (listDelete.Count() > 0)
            {
                //Hàm validate xóa
                await ValidateListDelete(listDelete);
                var listIdsDelete = listDelete.Select(entity => entity.GetKey().ToString());
                var listIdsToString = string.Join(",", listIdsDelete);
                result += await _crudRepository.DeleteManyAsync(listIdsToString);
            }
            //---------------------------------------Update list hàng hóa-------------------------------
            //Kiểm tra xem mã hàng hóa và mã vạch có trùng không
            if (listUpdate.Count() > 0)
            {
                await ValidateListUpdate(listUpdate);
                //Không có lỗi thì Update
                var listEntityUpdate = _mapper.Map<List<TEntity>>(listUpdate);

                result += await _crudRepository.UpdateMultipleAsync(listEntityUpdate);

            }
            //---------------------------------------Create list hàng hóa--------------------------------
            //Kiểm tra xem mã hàng hóa và mã vạch có trùng không
            if (listCreate.Count() > 0)
            {
                await ValidateListCreate(listCreate);
                //Không có lỗi thì thêm mới
                var listEntityCreate = _mapper.Map<List<TEntity>>(listCreate);

                result += await _crudRepository.InsertMultipleAsync(listEntityCreate);

            }
            return result;
        }
        /// <summary>
        /// Trước khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public virtual void PreSave(TEntityRequestDto data) { }
        /// <summary>
        /// Sau khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public virtual async Task<int> AfterSave(TEntityRequestDto data)
        {
            return 0;
        }
        /// <summary>
        /// Sau khi lưu thành công
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public virtual void AfterSaveSuccess(TEntityRequestDto data) { }
        /// <summary>
        /// Validate trước khi xóa list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public virtual async Task ValidateListDelete(List<TEntityRequestDto> data) { }
        /// <summary>
        /// Validate trước khi sửa list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public virtual async Task ValidateListUpdate(List<TEntityRequestDto> data) { }
        /// <summary>
        /// Validate trước khi thêm list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public virtual async Task ValidateListCreate(List<TEntityRequestDto> data) { }

        #endregion
    }
}
