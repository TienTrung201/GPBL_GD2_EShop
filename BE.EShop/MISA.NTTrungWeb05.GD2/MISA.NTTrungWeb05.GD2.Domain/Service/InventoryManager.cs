using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Service
{
    public class InventoryManager : IInventoryManager
    {
        #region Fields
        private readonly IInventoryRepository _inventoryRepository;
        #endregion
        #region Constructor
        public InventoryManager(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        #endregion
        /// <summary>
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="newCode">Mã mới</param>
        /// <param name="oldCode">Mã cũ</param>
        /// CreatedBy: NTTrung (16/07/2023)
        public async Task CheckDublicateCode(string newCode, string? oldCode)
        {
            var entity = await _inventoryRepository.GetByCodeAsync(newCode);
            if (entity != null && entity.SKUCode != oldCode)
            {
                throw new DuplicateCodeException(string.Format(ErrorMessage.DuplicateError, newCode), (int)ErrorCode.DuplicateCode);
            }
        }
        /// <summary>
        /// Kiểm tra có tồn tại không
        /// </summary>
        /// <paran name="id">Định danh</paran>
        /// CreatedBy: NTTrung (16/07/2023)
        public async Task CheckExistAsync(Guid id)
        {
            await _inventoryRepository.GetByIdAsync(id);
        }
    }
}
