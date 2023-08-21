using MISA.NTTrungWeb05.GD2.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Model
{
    /// <summary>
    /// Model hàng hóa
    /// </summary>
    /// CreatedBy NTTrung (21/08/2023)
    public class InventoryModel : Inventory
    {
        public string ItemCategoryName { get; set; } = string.Empty;
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>  
        public string UnitName { get; set; } = string.Empty;
    }
}
