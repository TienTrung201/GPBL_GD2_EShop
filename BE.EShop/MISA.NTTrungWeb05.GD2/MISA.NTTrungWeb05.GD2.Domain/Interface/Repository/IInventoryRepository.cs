using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Repository
{
    public interface IInventoryRepository : ICodeRepository<Inventory, InventoryModel>
    {
        /// <summary>
        /// Lấy bản ghi theo parentId
        /// Không thấy bắn ra lỗi luôn
        /// </summary>
        /// <paran name="id">Id bản ghi</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        //Task<InventoryModel> GetByParentIdAsync(Guid parentId);
    }
}
