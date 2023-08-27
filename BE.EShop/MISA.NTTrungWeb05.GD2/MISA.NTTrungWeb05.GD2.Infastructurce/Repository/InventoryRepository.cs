using Dapper;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static OfficeOpenXml.ExcelErrorValue;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository
{
    public class InventoryRepository : CodeRepository<Inventory, InventoryModel>, IInventoryRepository
    {
        public InventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Tìm data bằng mã vạch 
        /// </summary>
        /// <paran name="entity">Code</paran>
        /// <returns>Đối tượng</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<Inventory?> GetByBarcodeAsync(string barcode)
        {
            var storedProcedureName = $"Proc_{TableName}_GetByBarcode";
            var param = new DynamicParameters();
            param.Add($"@Barcode", barcode);
            var result = await _uow.Connection.QueryFirstOrDefaultAsync<Inventory>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }
        /// <summary>
        /// Hàm custtom kết quả cho master
        /// </summary>
        /// <paran name="entity">master</paran>
        /// <returns>Hàng hóa đã có detail</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public override async Task<InventoryModel> CustomResult(InventoryModel inventory)
        {
            var storedProcedureName = $"Proc_{TableName}_GetDetail";
            var param = new DynamicParameters();
            param.Add("@ParentId", inventory.InventoryId);
            var result = await _uow.Connection.QueryAsync<InventoryModel>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            inventory.Detail = result.ToList();
            return inventory;
        }
        /// <summary>
        /// lấy mã lỗi không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã lỗi</paran>
        /// <returns>Chuỗi mã không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public async Task<string> GetSKUCodeInvalid(string listSKUCodes)
        {
            var storedProcedureName = $"Proc_Inventory_CheckListCodes";
            var param = new DynamicParameters();
            param.Add($"@Codes", listSKUCodes);
            param.Add("@ListCodeInvalid", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            await _uow.Connection.QueryFirstOrDefaultAsync<string>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            var result = param.Get<string>("@ListCodeInvalid");
            return result;
        }
        /// <summary>
        /// lấy mã vạch không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã vạch lõi</paran>
        /// <returns>Chuỗi mã vạch không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public async Task<string> GetBarcodeInvalid(string listBarcodes)
        {
            var storedProcedureName = $"Proc_Inventory_CheckListBarcodes";
            var param = new DynamicParameters();
            param.Add($"@Barcodes", listBarcodes);
            param.Add("@ListBarcodeInvalid", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            await _uow.Connection.QueryFirstOrDefaultAsync<string>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            var result = param.Get<string>("@ListBarcodeInvalid");
            return result;
        }
    }
}
