using MISA.NTTrungWeb05.GD2.Application.Dtos.OrderDetail;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.Order
{
    public class OrderDTO : BaseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }

        public decimal? TotalAmount { get; set; } // Tương ứng với TotalAmount decimal(10, 0) DEFAULT NULL
        public decimal? Amount { get; set; } // Tương ứng với TotalAmount decimal(10, 0) DEFAULT NULL

        public int? OrderStatus { get; set; } // Tương ứng với OrderStatus varchar(255) DEFAULT NULL

        public DateTime? OrderTime { get; set; }
        public DateTime? ShippingDate { get; set; } // ngày giao hàng, có thể null
        public string ShippingAddress { get; set; } // địa chỉ giao hàng
        public string District { get; set; } // Quận/Huyện
        public string Ward { get; set; } // Phố/Phường
        public string Street { get; set; } // Đường
        public string OrderType { get; set; } // Loại đơn hàng

        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
