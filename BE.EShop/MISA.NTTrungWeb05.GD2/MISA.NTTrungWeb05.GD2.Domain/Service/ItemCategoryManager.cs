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
    public class ItemCategoryManager : IItemCategoryManager
    {
        #region Fields
        private readonly IItemCategoryRepository _itemCategoryRepository;
        #endregion
        #region Constructor
        public ItemCategoryManager(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
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
            var entity = await _itemCategoryRepository.GetByCodeAsync(newCode);
            if (entity != null && entity.ItemCategoryCode != oldCode)
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
            await _itemCategoryRepository.GetByIdAsync(id);
        }
    }
}
