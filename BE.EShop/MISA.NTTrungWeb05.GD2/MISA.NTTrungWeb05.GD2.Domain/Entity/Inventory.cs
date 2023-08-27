using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Entity
{
    /// <summary>
    /// Entity hàng hóa
    /// </summary>
    /// CreatedBy NTTrung (21/08/2023)
    public class Inventory : BaseAudiEntity
    {
        /// <summary>
        /// Định danh
        /// </summary>
        public Guid InventoryId { get; set; }
        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        public string InventoryName { get; set; } = string.Empty;
        /// <summary>
        /// Mã hàng hóa
        /// </summary>
        public string SKUCode { get; set; } = string.Empty;
        /// <summary>
        /// Mã hàng hóa
        /// </summary>
        public string SKUCodeCustom { get; set; } = string.Empty;
        /// <summary>
        /// Mã vạch
        /// </summary>
        public string BarCode { get; set; } = string.Empty;
        /// <summary>
        /// Giá mua
        /// </summary>
        public decimal? CostPrice { get; set; }
        /// <summary>
        /// Giá bán
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// Giá trung bình bán
        /// </summary>
        public decimal? AvgUnitPrice { get; set; } = decimal.Zero;
        /// <summary>
        /// Giá trung bình mua
        /// </summary>
        public decimal? AvgCostPrice { get; set; } = decimal.Zero;
        /// <summary>
        /// Màu sắc
        /// </summary>
        public string? Color { get; set; } = string.Empty;
        /// <summary>
        /// Mã màu
        /// </summary>
        public string? ColorCode { get; set; } = string.Empty;
        /// <summary>
        /// Size
        /// </summary>
        public string? Size { get; set; } = string.Empty;
        /// <summary>
        /// Mã size
        /// </summary>
        public string? SizeCode { get; set; } = string.Empty;
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// Ảnh
        /// </summary>
        public Guid? PictureId { get; set; }
        /// <summary>
        /// Trạng thái kinh doanh
        /// </summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// Trạng thái hiển thị
        /// </summary>
        public bool? IsShowMenu { get; set; }
        /// <summary>
        /// Id của master
        /// </summary>
        public Guid? ParentId { get; set; }
        /// /// <summary>
        /// Nhóm hàng hóa
        /// </summary>  
        public Guid ItemCategoryId { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>  
        public Guid UnitId { get; set; }
        /// <summary>
        /// Lấy Id Entity
        /// </summary>
        public override Guid GetKey()
        {
            return InventoryId;
        }
    }
}
