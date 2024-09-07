using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.SAInvoice
{
    public class SAInvoiceDTO  : BaseDto
    {
        public Guid SAInvoiceId { get; set; }
        public Guid OrderID { get; set; }
        public decimal TotalAmount { get; set; }
        public int? PaymentType { get; set; }
        public int? PaymentStatus { get; set; }
    }
}
