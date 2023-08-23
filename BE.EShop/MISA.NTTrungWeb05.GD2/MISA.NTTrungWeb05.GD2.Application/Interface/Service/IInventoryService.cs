using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Service
{
    public interface IInventoryService : ICodeService<InventoryResponseDto, InventoryRequestDto, InventoryModel>
    {
        /// <summary>
        /// Thêm sửa xóa data
        /// </summary>
        /// <paran name="DATA">Thông tin hàng hóa và list Item</paran>
        /// <returns>Bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        Task<int> SaveData(InventoryRequestDto data);
    }
}
