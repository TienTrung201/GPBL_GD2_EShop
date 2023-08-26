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
        /// Tìm data bằng mã vạch 
        /// </summary>
        /// <paran name="entity">Code</paran>
        /// <returns>Đối tượng</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<Inventory?> GetByBarcodeAsync(string barcode);
        /// <summary>
        /// Thêm nhiều
        /// </summary>
        /// <paran name="entity">Danh sách bản ghi thêm</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> InsertMultipleAsync(List<Inventory> listInventories);
        /// <summary>
        /// Sửa nhiều
        /// </summary>
        /// <paran name="entity">Danh sách bản ghi sửa</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> UpdateMultipleAsync(List<Inventory> listInventories);
        /// <summary>
        /// lấy mã lỗi không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã lỗi</paran>
        /// <returns>Chuỗi mã không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        Task<string> GetSKUCodeInvalid(string listSKUCodes);
        /// <summary>
        /// lấy mã vạch không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã vạch lõi</paran>
        /// <returns>Chuỗi mã vạch không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        Task<string> GetBarcodeInvalid(string listBarcodes);
    }
}
