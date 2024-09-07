using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.OrderDetail
{
    public class OrderDetailDTO : BaseDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }

        public Guid? ItemId { get; set; } // Tương ứng với ItemId varchar(255) DEFAULT NULL

        public decimal? Amount { get; set; } // Tương ứng với Amount decimal(8, 2) DEFAULT NULL

        public int? Quantity { get; set; } // Tương ứng với Quantity smallint(6) DEFAULT NULL
    }
}
