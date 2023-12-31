﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos.ItemCategory
{
    /// <summary>
    /// Dữ liệu trả về
    /// </summary>
    /// CreatedBy NTTrung (21/08/2023)
    public class ItemCategoryResponseDto
    {
        /// <summary>
        /// Định danh
        /// </summary>
        public Guid ItemCategoryId { get; set; }
        /// <summary>
        /// Tên nhóm hàng hóa
        /// </summary>
        public string ItemCategoryName { get; set; } = string.Empty;
        /// <summary>
        /// Mã nhóm hàng hóa
        /// </summary>
        public string ItemCategoryCode { get; set; } = string.Empty;
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; } = string.Empty;
        /// <summary>
        /// Ngày tạo
        /// </summary>
        /// CreatedBy: NTTrung (16/07/2023) 
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
