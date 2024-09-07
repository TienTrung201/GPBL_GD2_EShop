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
        /// Trước khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public override void PreSave(List<InventoryRequestDto> listData)
        {
            //Cập nhật giá trung bình
            var master = listData.FirstOrDefault(data => string.IsNullOrEmpty(data.ColorCode) && string.IsNullOrEmpty(data.SizeCode));
            if (master?.EditMode == EditMode.Create)
            {
                master.InventoryId = Guid.NewGuid();
            }
            var detail = listData.Where(data => !string.IsNullOrEmpty(data.ColorCode) || !string.IsNullOrEmpty(data.SizeCode)).ToList();

            var dataUpdateCreate = detail.Where((data) => data.EditMode != EditMode.Delete).ToList();
            if (dataUpdateCreate.Count() > 0)
            {
                decimal? totalUnitPrice = 0;
                decimal? totalCostPrice = 0;
                dataUpdateCreate.ForEach(inventory =>
                {
                    inventory.ParentId = master?.InventoryId;
                    if (inventory.EditMode == EditMode.Create)
                    {
                        inventory.InventoryId = Guid.NewGuid();
                    }
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
                master.AvgCostPrice = newAvgCostPrice;
                master.AvgUnitPrice = newAvgUnitPrice;
            }
            else
            {
                master.AvgCostPrice = master.UnitPrice;
                master.AvgUnitPrice = master.CostPrice;
            }
        }
        /// <summary>
        /// Update mã code với tiền tố và tăng mã code lên 1
        /// </summary>
        /// <param name="data"></param>
        /// CreatedBy: NTTrung (27/08/2023)
        public async override Task AfterSaveSuccess(List<InventoryRequestDto> listData)
        {
            var master = listData.FirstOrDefault(data => data.ParentId == null);
            if (master.EditMode == EditMode.Create)
            {
                string pattern = "^[A-Za-z]+";
                string prefix = Regex.Match(master.SKUCode, pattern).Value;

                await _inventoryRepository.UpdateCodeAsync(prefix);
            }

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
