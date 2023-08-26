using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Common;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service
{
    public class InventoryService : CodeService<Inventory, InventoryModel, InventoryResponseDto, InventoryRequestDto>, IInventoryService
    {
        private readonly IInventoryManager _inventoryManager;
        private readonly IItemCategoryManager _itemCategoryManager;
        private readonly IUnitManager _unitManager;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository,
            IInventoryManager inventoryManager,
            IItemCategoryManager itemCategoryManager,
            IUnitManager unitManager,
            IMapper mapper, IUnitOfWork unitOfWork) : base(inventoryRepository, mapper, unitOfWork)
        {
            _inventoryManager = inventoryManager;
            _itemCategoryManager = itemCategoryManager;
            _unitManager = unitManager;
            _inventoryRepository = inventoryRepository;
        }
        /// <summary>
        /// Validate Nghiệp vụ
        /// </summary>
        /// <param name="createDto">Bản ghi được gửi đến</param>
        /// <returns>Bản ghi tìm thấy</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        public override async Task<Inventory> MapCreateDtoToEntityValidateAsync(InventoryRequestDto createDto)
        {
            // validate nghiệp vụ
            // Kiểm tra trùng mã
            await _inventoryManager.CheckDublicateCode(createDto.SKUCode, null);
            // Kiểm tra trùng mã vạch
            await _inventoryManager.CheckDublicateBarcode(createDto.BarCode, null);
            //// Kiểm tra UnitId và ItemCategory có hợp lệ không
            await _itemCategoryManager.CheckExistAsync(createDto.ItemCategoryId);
            await _unitManager.CheckExistAsync(createDto.UnitId);

            var entity = _mapper.Map<Inventory>(createDto);
            entity.InventoryId = Guid.NewGuid();
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            return entity;
        }
        /// <summary>
        /// Validate Nghiệp vụ
        /// </summary>
        /// <param name="updateDto">Bản ghi được gửi đến</param>
        /// <returns>Bản ghi tìm thấy</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        public async override Task<Inventory> MapUpdateDtoToEntityValidateAsync(Guid id, InventoryRequestDto updateDto)
        {
            // Kiểm tra hàng hóa có tồn tại không
            var entity = await _inventoryRepository.GetByIdAsync(id);

            // Kiểm tra trùng mã
            await _inventoryManager.CheckDublicateCode(updateDto.SKUCode, entity.SKUCode);
            // Kiểm tra trùng mã vạch
            await _inventoryManager.CheckDublicateBarcode(updateDto.BarCode, entity.BarCode);
            // Kiểm tra PositionId và DepartmentId có hợp lệ không
            await _itemCategoryManager.CheckExistAsync(updateDto.ItemCategoryId);
            await _unitManager.CheckExistAsync(updateDto.UnitId);

            var updateEntity = _mapper.Map<Inventory>(updateDto);
            updateEntity.InventoryId = entity.InventoryId;
            updateEntity.ModifiedDate = DateTime.Now;
            return updateEntity;
        }

        /// <summary>
        /// Thêm sửa xóa data
        /// </summary>
        /// <paran name="DATA">Thông tin hàng hóa và list Item</paran>
        /// <returns>Bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        public async Task<int> SaveData(InventoryRequestDto data)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = 0;
                //Cập nhật Master
                var master = new Inventory();
                switch (data.EditMode)
                {
                    case EditMode.Create:
                        master = await MapCreateDtoToEntityValidateAsync(data);
                        result += await _inventoryRepository.InsertAsync(master);
                        break;
                    case EditMode.Update:
                        master = await MapUpdateDtoToEntityValidateAsync(data.InventoryId, data);
                        result += await _inventoryRepository.UpdateAsync(master);
                        break;
                    default:
                        break;
                }
                if (data.Detail == null)
                {
                    return result;
                }
                else
                {
                    //Nếu có detail thì cập nhật Master và Detail

                    var listDelete = data.Detail.Where(inventory => inventory.EditMode == EditMode.Delete).ToArray();
                    var listUpdate = data.Detail.Where(inventory => inventory.EditMode == EditMode.Update).ToArray();
                    var listCreate = data.Detail.Where(inventory => inventory.EditMode == EditMode.Create).ToArray();

                    //--------------------------------------Xóa những hàng hóa bị xóa--------------------------
                    if (listDelete.Length > 0)
                    {
                        var listIdsDelete = listDelete.Select(inventory => inventory.InventoryId.ToString());
                        var listIdsToString = string.Join(", ", listIdsDelete);
                        result += await _inventoryRepository.DeleteManyAsync(listIdsToString);
                    }

                    //Cập nhật giá trung bình
                    decimal? totalUnitPrice = 0;
                    decimal? totalCostPrice = 0;
                    data.Detail.ForEach(inventory =>
                    {
                        if (inventory.EditMode == EditMode.Create || inventory.EditMode == EditMode.Update)
                        {
                            if (inventory.UnitPrice.HasValue)
                            {
                                totalUnitPrice += inventory.UnitPrice;
                            }
                            if (inventory.CostPrice.HasValue)
                            {
                                totalCostPrice += inventory.CostPrice;
                            }
                        }
                    });
                    var newAvgUnitPrice = totalUnitPrice / data.Detail.Count();
                    var newAvgCostPrice = totalCostPrice / data.Detail.Count();
                    data.AvgCostPrice = newAvgCostPrice;
                    data.AvgUnitPrice = newAvgUnitPrice;
                    //---------------------------------------Update list hàng hóa-------------------------------
                    //Kiểm tra xem mã hàng hóa và mã vạch có trùng không
                    if (listUpdate.Length > 0)
                    {
                        var listCodes = data.Detail.Where(inventory => inventory.EditMode == EditMode.Update && inventory.IsUpdateCode).Select((inventory) => inventory.SKUCode);
                        var listBarcodes = data.Detail.Where(inventory => inventory.EditMode != EditMode.Update && inventory.IsUpdateBarcode).Select((inventory) => inventory.BarCode);
                        await _inventoryManager.CheckDublicateListCodes(string.Join(',', listCodes));
                        await _inventoryManager.CheckDublicateListBarcodes(string.Join(',', listBarcodes));
                        //Không có lỗi thì Update
                        var listEntityUpdate = _mapper.Map<List<Inventory>>(listUpdate);

                        result += await _inventoryRepository.UpdateMultipleAsync(listEntityUpdate);

                    }


                    //---------------------------------------Create list hàng hóa--------------------------------
                    //Kiểm tra xem mã hàng hóa và mã vạch có trùng không
                    if (listCreate.Length > 0)
                    {
                        var listCodesCreate = data.Detail.Where(inventory => inventory.EditMode == EditMode.Create).Select((inventory) => inventory.SKUCode);
                        var listBarcodesCreate = data.Detail.Where(inventory => inventory.EditMode != EditMode.Create).Select((inventory) => inventory.BarCode);
                        await _inventoryManager.CheckDublicateListCodes(string.Join(',', listCodesCreate));
                        await _inventoryManager.CheckDublicateListBarcodes(string.Join(',', listBarcodesCreate));
                        //Không có lỗi thì thêm mới
                        var listEntityCreate = _mapper.Map<List<Inventory>>(listCreate);

                        result += await _inventoryRepository.InsertMultipleAsync(listEntityCreate);
                        await _unitOfWork.CommitAsync();
                    }
                }
                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                throw;
            }
        }
    }
}
