using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Enum
{
    /// <summary>
    /// Enum Eddit mode
    /// </summary>
    /// CreatedBy: NTTrung (23/08/2023)
    public enum OrderStatus : int
    {
        [Description("Chờ xác nhận")]
        None = 0,
        [Description("Tạo order")]
        Order = 1,
        [Description("Giao hàng")]
        Delivery = 2,
        [Description("Hoàn thành đơn hàng")]
        Done = 3,
    }
    public enum OrderType: int
    {
        [Description("Order tại cửa hàng")]
        Now = 1,
        [Description("Order giao hàng")]
        Delivery = 2
    }

    public enum PaymentType : int
    {
        [Description("Tiền mặt")]
        Cash = 1,
        [Description("Chuyển khoản")]
        Card = 2,
        Debit = 3,  
    }

    public enum PaymentStatus : int
    {
        [Description("Chưa thu tiền")]
        None = 1,
        [Description("Đã thu tiền")]
        Done = 2,
        /// <summary>
        ///  nợ
        /// </summary>
        Debit = 3,
    }
}
