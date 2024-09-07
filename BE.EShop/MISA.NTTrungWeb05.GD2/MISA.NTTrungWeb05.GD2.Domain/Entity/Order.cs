using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Entity
{
    public class Order : BaseAudiEntity
    {
        public Guid OrderId { get; set; }

        public decimal? TotalAmount { get; set; } // Tương ứng với TotalAmount decimal(10, 0) DEFAULT NULL

        public int? OrderStatus { get; set; } // Tương ứng với OrderStatus varchar(255) DEFAULT NULL

        public DateTime? OrderTime { get; set; } // Tương ứng với OrderTime datetime DEFAULT NULL
    }
}
