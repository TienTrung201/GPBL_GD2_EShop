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
        /// Thêm nhiều
        /// </summary>
        /// <paran name="entity">Danh sách bản ghi thêm</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public async Task<int> InsertMultipleAsync(List<Inventory> listInventories, Guid? parentId)
        {
            var storedProcedureName = $"Proc_Excute";
            var parameters = new DynamicParameters();
            var query = new StringBuilder();
            query.Append($"INSERT INTO Inventory ");
            var listPropertiesToString = new List<string>();
            var properties = typeof(Inventory).GetProperties();//Lấy danh sách các thuộc tính của đối tượng

            foreach (var property in properties)
            {
                listPropertiesToString.Add(property.Name);
            }
            query.Append($"( {string.Join(", ", listPropertiesToString)} ) Values ");
            int index = 1;
            listInventories.ForEach((inventory) =>
            {
                inventory.ParentId = parentId;
                query.Append("( ");
                int indexColumn = 0;
                listPropertiesToString.ForEach(property =>
                {
                    PropertyInfo propertyInfo = inventory.GetType().GetProperty(property);
                    var value = propertyInfo?.GetValue(inventory);
                    query.Append($"@{propertyInfo}{index}");
                    parameters.Add($"@{propertyInfo}{index}", value);
                    if (indexColumn != listPropertiesToString.Count() - 1)
                    {
                        query.Append(", ");
                    }
                    indexColumn++;
                });
                query.Append("), ");
            });
            var queryString = query.ToString();
            parameters.Add("@QueryString", queryString);

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }
        /// <summary>
        /// Sửa nhiều
        /// </summary>
        /// <paran name="entity">Danh sách bản ghi sửa</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public async Task<int> UpdateMultipleAsync(List<Inventory> listInventories)
        {
            var storedProcedureName = $"Proc_Excute";
            var parameters = new DynamicParameters();
            var query = new StringBuilder();
            var listPropertiesToString = new List<string>();
            var properties = typeof(Inventory).GetProperties();//Lấy danh sách các thuộc tính của đối tượng

            foreach (var property in properties)
            {
                listPropertiesToString.Add(property.Name);
            }

            int index = 1;
            listInventories.ForEach((inventory) =>
            {
                query.Append("Update Inventory Set ");
                int indexColumn = 0;
                listPropertiesToString.ForEach(property =>
                {
                    PropertyInfo propertyInfo = inventory.GetType().GetProperty(property);
                    var value = propertyInfo?.GetValue(inventory);
                    query.Append($"{propertyInfo} = @{propertyInfo}{index} Where InventoryId = {inventory.GetKey()}");
                    parameters.Add($"@{propertyInfo}{index}", value);
                    if (indexColumn != listPropertiesToString.Count() - 1)
                    {
                        query.Append(", ");
                    }
                    indexColumn++;
                });
                query.Append(";");
            });
            var queryString = query.ToString();
            parameters.Add("@QueryString", queryString);

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
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
