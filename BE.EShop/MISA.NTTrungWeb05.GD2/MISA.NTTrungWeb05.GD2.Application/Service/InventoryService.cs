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
using System.Text.RegularExpressions;
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
            if (!string.IsNullOrEmpty(createDto.Barcode?.Replace(" ", "")))
            {
                await _inventoryManager.CheckDublicateBarcode(createDto.Barcode, null);
            }
            else
            {
                createDto.Barcode = null;
            }
            //// Kiểm tra UnitId và ItemCategory có hợp lệ không
            if (createDto.ItemCategoryId != null)
            {
                await _itemCategoryManager.CheckExistAsync(createDto.ItemCategoryId);
            }
            if (createDto.UnitId != null)
            {
                await _unitManager.CheckExistAsync(createDto.UnitId);
            }

            var entity = _mapper.Map<Inventory>(createDto);
            entity.InventoryId = Guid.NewGuid();
            //entity.SKUCodeCustom = entity.SKUCode;
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
            if (!string.IsNullOrEmpty(updateDto.Barcode?.Replace(" ", "")))
            {
                await _inventoryManager.CheckDublicateBarcode(updateDto.Barcode, entity.Barcode);
            }
            else
            {
                updateDto.Barcode = null;
            }
            // Kiểm tra unit và itemcategory có hợp lệ không
            if (updateDto.ItemCategoryId != null)
            {
                await _itemCategoryManager.CheckExistAsync(updateDto.ItemCategoryId);
            }
            if (updateDto.UnitId != null)
            {
                await _unitManager.CheckExistAsync(updateDto.UnitId);
            }
            var updateEntity = _mapper.Map<Inventory>(updateDto);
            updateEntity.InventoryId = entity.InventoryId;
            updateEntity.ModifiedDate = DateTime.Now;
            //updateEntity.SKUCodeCustom = updateEntity.SKUCode;
            return updateEntity;
        }
        /// <summary>
        /// Trước khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public override void PreSave(InventoryRequestDto data)
        {
            //Cập nhật giá trung bình
            var hasDetail = false;
            if (data.Detail?.Any() == true)//data.Detail?.Any() sẽ trả về giá trị true nếu danh sách có ít nhất một phần tử
            {
                var dataUpdateCreate = data.Detail.Where((data) => data.EditMode != EditMode.Delete).ToList();
                if (dataUpdateCreate.Count() > 0)
                {
                    decimal? totalUnitPrice = 0;
                    decimal? totalCostPrice = 0;
                    dataUpdateCreate.ForEach(inventory =>
                                {
                                    hasDetail = true;
                                    if (inventory.UnitPrice.HasValue)
                                    {
                                        totalUnitPrice += inventory.UnitPrice;
                                    }
                                    if (inventory.CostPrice.HasValue)
                                    {
                                        totalCostPrice += inventory.CostPrice;
                                    }

                                });
                    var newAvgUnitPrice = totalUnitPrice / dataUpdateCreate.Count();
                    var newAvgCostPrice = totalCostPrice / dataUpdateCreate.Count();
                    data.AvgCostPrice = newAvgCostPrice;
                    data.AvgUnitPrice = newAvgUnitPrice;
                }
            }
            if (!hasDetail)
            {
                data.AvgCostPrice = data.UnitPrice;
                data.AvgUnitPrice = data.CostPrice;
            }


        }
        /// <summary>
        /// Sau khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task<int> AfterSave(InventoryRequestDto data)
        {
            int result = 0;
            if (data.Detail?.Any() == true)
            {
                data.Detail.ForEach(dataDetail =>
                {
                    dataDetail.ParentId = data.InventoryId;
                });
                result += await CUDListService(data.Detail);
            }
            return result;
        }
        public async override Task AfterSaveSuccess(InventoryRequestDto data)
        {
            string pattern = "^[A-Za-z]+";
            string prefix = Regex.Match(data.SKUCode, pattern).Value;

            await _inventoryRepository.UpdateCodeAsync(prefix);
        }
        /// <summary>
        /// Validate trước khi sửa list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task ValidateListUpdate(List<InventoryRequestDto> data)
        {
            //Hàm validate sửa
            data.ForEach(data =>
            {
                if (string.IsNullOrEmpty(data.Barcode.Replace(" ", "")))
                {
                    data.Barcode = null;
                }
            });
            var listCodes = data.Where(inventory => inventory.EditMode == EditMode.Update && inventory.IsUpdateCode).Select((inventory) => inventory.SKUCodeCustom);
            var listBarcodes = data.Where(inventory => inventory.EditMode == EditMode.Update && inventory.IsUpdateBarcode).Select((inventory) => inventory.Barcode);

            //Kiểm tra xem trùng không
            await _inventoryManager.CheckDublicateListCodes(string.Join(',', listCodes));
            await _inventoryManager.CheckDublicateListBarcodes(string.Join(',', listBarcodes));
        }
        /// <summary>
        /// Validate trước khi thêm list
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task ValidateListCreate(List<InventoryRequestDto> data)
        {
            //Hàm validate thêm
            data.ForEach(data =>
            {
                if (string.IsNullOrEmpty(data.Barcode.Replace(" ", "")))
                {
                    data.Barcode = null;
                }
            });
            var listCodesCreate = data.Where(inventory => inventory.EditMode == EditMode.Create).Select((inventory) => inventory.SKUCodeCustom);
            var listBarcodesCreate = data.Where(inventory => inventory.EditMode == EditMode.Create).Select((inventory) => inventory.Barcode);
            await _inventoryManager.CheckDublicateListCodes(string.Join(',', listCodesCreate));
            await _inventoryManager.CheckDublicateListBarcodes(string.Join(',', listBarcodesCreate));
        }

    }
}
