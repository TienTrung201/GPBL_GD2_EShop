using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Repository
{
    public interface IOrderDetailRepository : ICodeRepository<OrderDetail, OrderDetail>
    {
        /// <summary>
        /// Xóa hết detail của order
        /// </summary>
        /// <param name="code">Mã code bản ghi</param>
        /// <returns>bản ghi được tìm thấy</returns>
        Task<int> DeleteOrderDetailByOrderID(Guid orderId);
    }
}
