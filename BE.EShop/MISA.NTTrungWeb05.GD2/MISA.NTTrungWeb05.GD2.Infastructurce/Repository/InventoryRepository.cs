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
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository
{
    public class InventoryRepository : CodeRepository<Inventory, InventoryModel>, IInventoryRepository
    {
        public InventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override async Task<InventoryModel> CustomResult(InventoryModel inventory)
        {
            var storedProcedureName = $"Proc_{TableName}_GetDetail";
            var param = new DynamicParameters();
            param.Add("@ParentId", inventory.InventoryId);

            var result = await _uow.Connection.QueryAsync<InventoryModel>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            inventory.Detail = result.ToList();
            return inventory;
        }

        public Task<InventoryModel> GetByParentIdAsync(Guid parentId)
        {
            throw new NotImplementedException();
        }
    }
}
