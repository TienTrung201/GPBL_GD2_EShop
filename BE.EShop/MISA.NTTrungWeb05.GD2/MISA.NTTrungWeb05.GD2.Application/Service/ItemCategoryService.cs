using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Dtos.ItemCategory;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using MISA.NTTrungWeb05.GD2.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service
{
    public class ItemCategoryService : CodeService<ItemCategory, ItemCategoryModel, ItemCategoryResponseDto, ItemCategoryRequestDto>, IItemCategoryService
    {
        private readonly IItemCategoryManager _itemCategoryManager;
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ItemCategoryService(IItemCategoryManager itemCategoryManager,
            IItemCategoryRepository itemCategoryRepository,
            IInventoryRepository inventoryRepository,
            IMapper mapper, IUnitOfWork unitOfWork) : base(itemCategoryRepository, mapper, unitOfWork)
        {
            _itemCategoryManager = itemCategoryManager;
            _itemCategoryRepository = itemCategoryRepository;
            _inventoryRepository = inventoryRepository;
        }

        #region Methods
        /// <summary>
        /// Validate Nghiệp vụ
        /// </summary>
        /// <param name="createDto">Đối tượng thêm mới</param>
        /// <returns>Thực thể tìm thấy</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public override async Task<ItemCategory> MapCreateDtoToEntityValidateAsync(ItemCategoryRequestDto createDto)
        {
            // validate nghiệp vụ
            // Kiểm tra trùng mã
            await _itemCategoryManager.CheckDublicateCode(createDto.ItemCategoryCode, null);

            var itemCategory = _mapper.Map<ItemCategory>(createDto);
            itemCategory.ItemCategoryId = Guid.NewGuid();
            itemCategory.CreatedDate = DateTime.Now;
            itemCategory.ModifiedDate = DateTime.Now;
            return itemCategory;
        }

        /// <summary>
        /// Validate Nghiệp vụ
        /// </summary>
        /// <param name="createDto">Đối tượng được cập nhật</param>
        /// <param name="id">Id được cập nhật</param>
        /// <returns>Thực thể tìm thấy</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public override async Task<ItemCategory> MapUpdateDtoToEntityValidateAsync(Guid id, ItemCategoryRequestDto updateDto)
        {
            // Kiểm tra có tồn tại không
            var department = await _itemCategoryRepository.GetByIdAsync(id);

            // Kiểm tra trùng mã
            await _itemCategoryManager.CheckDublicateCode(updateDto.ItemCategoryCode, department.ItemCategoryCode);

            var updateEmployee = _mapper.Map<ItemCategory>(updateDto);
            updateEmployee.ItemCategoryId = department.ItemCategoryId;
            updateEmployee.ModifiedDate = DateTime.Now;
            return updateEmployee;
        }
        /// <summary>
        /// Ghi đè lại hàm xóa để kiểm tra xem có phát sinh dữ liệu liên quan không
        /// </summary>
        /// <paran name="id">id của bản ghi cần xóa</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async override Task<int> DeleteAsync(Guid id)
        {
            // kiểm tra xem có phát sinh không nếu phát sinh lỗi luôn
            //await _inventoryRepository.GetEmployeeByItemCategoryId(id);

            var model = await _crudRepository.GetByIdAsync(id);

            var entity = _mapper.Map<ItemCategory>(model);

            var result = await _crudRepository.DeleteAsync(entity);
            return result;

        }

        /// <summary>
        /// Validate trước khi sửa list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task ValidateListUpdate(List<ItemCategoryRequestDto> data)
        {
            //Hàm validate sửa
            var listCodes = data.Where(itemCategory => itemCategory.EditMode == EditMode.Update && itemCategory.IsUpdateCode).Select((itemCategory) => itemCategory.ItemCategoryCode);

            //Kiểm tra xem trùng không
            await _itemCategoryManager.CheckDublicateListCodes(string.Join(',', listCodes));
        }
        /// <summary>
        /// Validate trước khi thêm list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task ValidateListCreate(List<ItemCategoryRequestDto> data)
        {
            //Hàm validate thêm
            var listCodesCreate = data.Where(itemCategory => itemCategory.EditMode == EditMode.Create).Select((itemCategory) => itemCategory.ItemCategoryCode);
            await _itemCategoryManager.CheckDublicateListCodes(string.Join(',', listCodesCreate));
        }
        /// <summary>
        /// Trước khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public override void PreSave(List<ItemCategoryRequestDto> listData)
        {
            listData.ForEach(data =>
            {
                if(data.EditMode == EditMode.Create)
                {
                    data.ItemCategoryId = Guid.NewGuid();
                }
            });
        }
        #endregion
    }
}
