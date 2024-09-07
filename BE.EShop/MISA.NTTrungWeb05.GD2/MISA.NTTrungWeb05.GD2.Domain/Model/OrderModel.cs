using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Entity
{
    public class OrderModel : Order
    {
       public List<OrderDetail> LstOrderDetail { get; set; }
    }
}
