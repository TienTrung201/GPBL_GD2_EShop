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

        public decimal? TotalAmount { get; set; } // Tương ứng với TotalAmount decimal(10, 0) DEFAULT NULL
        public decimal? Amount { get; set; } // Tương ứng với TotalAmount decimal(10, 0) DEFAULT NULL

        public int? OrderStatus { get; set; } // Tương ứng với OrderStatus varchar(255) DEFAULT NULL

        public DateTime? OrderTime { get; set; }

        public List<OrderDetailDTO> LstOrderDetail { get; set; }
    }
}
