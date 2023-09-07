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
    public class UnitRepository : CodeRepository<Unit, UnitModel>, IUnitRepository
    {
        public UnitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// lấy mã vạch không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã vạch lõi</paran>
        /// <returns>Chuỗi mã vạch không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public async Task<string> GetNameInvalidAsync(string listNames)
        {
            var storedProcedureName = $"Proc_Unit_CheckListNames";
            var param = new DynamicParameters();
            param.Add($"@Names", listNames);
            param.Add("@ListNameInvalid", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            await _uow.Connection.QueryFirstOrDefaultAsync<string>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            var result = param.Get<string>("@ListNameInvalid");
            return result;
        }
    }
}
