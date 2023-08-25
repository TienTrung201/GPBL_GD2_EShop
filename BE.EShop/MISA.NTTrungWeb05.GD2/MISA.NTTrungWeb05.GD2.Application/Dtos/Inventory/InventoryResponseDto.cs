using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory
{
    /// <summary>
    /// Dữ liệu trả về
    /// </summary>
    /// CreatedBy NTTrung (21/08/2023)
    public class InventoryResponseDto
    {
        /// <summary>
        /// Định danh
        /// </summary>
        public string InventoryId { get; set; } = string.Empty;
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
        public string? BarCode { get; set; } = string.Empty;
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
        public decimal? AvgUnitPrice { get; set; }
        /// <summary>
        /// Giá trung bình mua
        /// </summary>
        public decimal? AvgCostPrice { get; set; }
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
        /// <summary>
        /// Id nhóm hàng hóa
        /// </summary>  
        public Guid ItemCategoryId { get; set; }
        /// <summary>
        /// Id đơn vị tính
        /// </summary>  
        public Guid UnitId { get; set; }
        /// <summary>
        /// Nhóm hàng hóa
        /// </summary>  
        public string ItemCategoryName { get; set; } = string.Empty;
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>  
        public string UnitName { get; set; } = string.Empty;
        /// <summary>
        /// Danh sách detail
        /// </summary>  
        public List<InventoryResponseDto>? Detail { get; set; }
        /// <summary>
        /// Chế độ
        /// </summary>  
        public EditMode EditMode { get; set; } = EditMode.None;
    }
}
