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
        /// lấy mã lỗi không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã lỗi</paran>
        /// <returns>Chuỗi mã không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        Task<string> GetSKUCodeInvalidAsync(string listSKUCodes);
        /// <summary>
        /// lấy mã vạch không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã vạch lõi</paran>
        /// <returns>Chuỗi mã vạch không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        Task<string> GetBarcodeInvalidAsync(string listBarcodes);
        /// <summary>
        /// Hàm lấy detail từ parentid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<List<InventoryModel>> GetDetailByParentId(Guid uid);
    }
}
