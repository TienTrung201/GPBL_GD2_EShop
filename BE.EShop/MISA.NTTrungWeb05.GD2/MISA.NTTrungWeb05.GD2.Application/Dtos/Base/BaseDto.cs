using MISA.NTTrungWeb05.GD2.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Dtos
{
    public abstract class BaseDto
    {
        /// <summary>
        /// Chế độ chỉnh sửa
        /// </summary>  
        public EditMode EditMode { get; set; } = EditMode.None;
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
