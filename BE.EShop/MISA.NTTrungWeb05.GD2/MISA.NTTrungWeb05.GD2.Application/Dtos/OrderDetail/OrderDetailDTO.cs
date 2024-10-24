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

        public string OrderDetailStatus { get; set; } // trạng thái chi tiết đơn hàng
        public decimal Amount { get; set; } // tổng tiền cho sản phẩm
        public decimal TotalAmount { get; set; } // tổng tiền chi tiết đơn hàng
        public int ItemType { get; set; } // kiểu sản phẩm (int)
        public Guid InventoryId { get; set; } // ID kho hàng
        public string InventoryName { get; set; } // Tên hàng hóa
        public string SKUCode { get; set; } // Mã hàng hóa
        public string SKUCodeCustom { get; set; } // Mã hàng hóa tùy chỉnh
        public decimal UnitPrice { get; set; } // Giá bán
        public string PictureId { get; set; } // Ảnh (có thể null)
        public string ParentId { get; set; } // Id của master (có thể null)
        public int Quantity { get; set; } // Số lượng
        public decimal TotalPrice { get; set; } // Tổng tiền
    }
}
