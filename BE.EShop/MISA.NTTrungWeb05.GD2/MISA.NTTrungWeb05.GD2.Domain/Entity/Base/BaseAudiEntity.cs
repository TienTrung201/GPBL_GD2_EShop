using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Entity
{
    /// <summary>
    /// Lớp chứa các thuộc tính dùng để theo dõi thông tin người tạo và người sửa
    /// </summary>
    /// <CreatedBy>NTTrung (Ngày 16/07/2023)</CreatedBy>
    public abstract class BaseAudiEntity
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        /// CreatedBy: NTTrung (16/07/2023) 
        public DateTimeOffset? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        /// CreatedBy: NTTrung (16/07/2023) 
        [MaxLength(255, ErrorMessage = "Người tạo tối đa 255 ký tự")]
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        /// CreatedBy: NTTrung (16/07/2023) 
        [MaxLength(255, ErrorMessage = "Người sửa tối đa 255 ký tự")]
        public DateTimeOffset? ModifiedDate { get; set; }
        /// <summary>
        /// Người sửa
        /// </summary>
        /// CreatedBy: NTTrung (16/07/2023) 
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Lấy tên id của đối tượng
        /// </summary>
        /// <returns>Giá trị của thuộc tính Id hoặc Guid.Empty nếu không tồn tại</returns>
        public Guid GetKey()
        {
            // Lấy tên của class hiện tại
            var nameProperty = this.GetType().Name;

            // Tìm thuộc tính có tên là ClassNameId
            var property = this.GetType().GetProperty(nameProperty + "Id");

            // Nếu thuộc tính tồn tại
            if (property != null)
            {
                var value = property.GetValue(this);

                // Kiểm tra nếu giá trị không null và có kiểu Guid
                if (value != null && value is Guid)
                {
                    return (Guid)value;
                }
            }

            // Trả về Guid.Empty nếu không tìm thấy thuộc tính hoặc giá trị là null
            return Guid.Empty;
        }
        /// <summary>
        /// hàm set giá trị cho property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetValue(string propertyName, object value)
        {
            this.GetType().GetProperty(propertyName)?.SetValue(this, value);
        }
    }
}
