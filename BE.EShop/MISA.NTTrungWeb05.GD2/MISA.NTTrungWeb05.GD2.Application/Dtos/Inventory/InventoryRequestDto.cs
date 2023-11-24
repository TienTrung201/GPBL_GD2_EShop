using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ValidateInput;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory
{
    /// <summary>
    /// Dữ liệu yêu cầu
    /// </summary>
    /// CreatedBy NTTrung (21/08/2023)
    public class InventoryRequestDto : BaseDto
    {
        /// <summary>
        /// Định danh hàng hóa
        /// </summary>
        public Guid InventoryId { get; set; }
        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(ValidateInput.Required), ErrorMessageResourceType = typeof(ValidateInput))]
        [StringLength(100, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string InventoryName { get; set; } = string.Empty;
        /// <summary>
        /// Mã hàng hóa
        /// </summary>
        public string SKUCode { get; set; } = string.Empty;
        /// <summary>
        /// Mã hàng hóa
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(ValidateInput.Required), ErrorMessageResourceType = typeof(ValidateInput))]
        [StringLength(50, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string SKUCodeCustom { get; set; } = string.Empty;
        /// <summary>
        /// Mã vạch
        /// </summary>
        [StringLength(255, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string? Barcode { get; set; } = string.Empty;
        /// <summary>
        /// Giá mua
        /// </summary>
        [RegularExpression(@"^\d{0,14}(\.\d{1,4})?$", ErrorMessageResourceName = nameof(ValidateInput.MaxPrice), ErrorMessageResourceType = typeof(ValidateInput))]
        public decimal? CostPrice { get; set; }
        /// <summary>
        /// Giá bán
        /// </summary>
        [RegularExpression(@"^\d{0,14}(\.\d{1,4})?$", ErrorMessageResourceName = nameof(ValidateInput.MaxPrice), ErrorMessageResourceType = typeof(ValidateInput))]
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
        [StringLength(50, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string? Color { get; set; } = string.Empty;
        /// <summary>
        /// Mã màu
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string? ColorCode { get; set; } = string.Empty;
        /// <summary>
        /// Size
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string? Size { get; set; } = string.Empty;
        /// <summary>
        /// Mã size
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string? SizeCode { get; set; } = string.Empty;
        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(255, ErrorMessageResourceName = nameof(ValidateInput.MaxLength), ErrorMessageResourceType = typeof(ValidateInput))]
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// Ảnh
        /// </summary>
        public Guid? PictureId { get; set; }
        /// <summary>
        /// base 64
        /// </summary>
        public string PictureContent { get; set; } = string.Empty;
        /// <summary>
        /// Có thay đổi ảnh mới hay không
        /// </summary>
        public bool IsChangePicture { get; set; }
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
        /// Nhóm hàng hóa
        /// </summary>  
        public Guid? ItemCategoryId { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>  
        public Guid? UnitId { get; set; }
        /// <summary>
        /// Code thay đổi
        /// </summary>  
        public bool IsUpdateCode { get; set; } = false;
        /// <summary>
        /// Mã vạch thay đổi
        /// </summary>  
        public bool IsUpdateBarcode { get; set; } = false;
        /// <summary>
        /// Ngày tạo
        /// </summary>
        /// CreatedBy: NTTrung (16/07/2023) 
        public DateTimeOffset? CreatedDate { get; set; }
        /// <summary>
        /// Danh sách detail
        /// </summary>  
        public List<InventoryRequestDto>? Detail { get; set; }

        public override Guid GetKey()
        {
            return InventoryId;
        }
    }
}
