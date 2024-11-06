using Dapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.OrderDetail;
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
    public class OrderRepository : CodeRepository<Order, OrderModel>, IOrderRepository
    {
        public OrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Hàm custtom kết quả cho master
        /// </summary>
        /// <paran name="entity">master</paran>
        /// <returns>Hàng hóa đã có detail</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        public override async Task<OrderModel> CustomResult(OrderModel order)
        {
            var result = await GetDetailByParentId(order.OrderId.Value);
            order.OrderDetails = result.ToList();
            return order;
        }
        public async Task<List<OrderDetail>> GetDetailByParentId(Guid uid)
        {
            var storedProcedureName = $"Proc_{TableName}_GetDetail";
            var param = new DynamicParameters();
            param.Add("@OrderId", uid);
            var result = await _uow.Connection.QueryAsync<OrderDetail>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result.ToList();
        }
    }
}
