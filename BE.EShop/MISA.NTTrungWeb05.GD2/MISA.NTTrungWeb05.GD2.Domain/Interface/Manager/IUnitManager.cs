using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Manager
{
    public interface IUnitManager
    {
        /// <summary>
        /// Kiểm tra có tồn tại không
        /// </summary>
        /// <paran name="id">Định danh</paran>
        /// CreatedBy: NTTrung (16/07/2023)
        Task CheckExistAsync(Guid? id);
        /// <summary>
        /// Kiểm tra danh sách tên trùng
        /// </summary>
        /// <param name="listCodes">Danh sách mã</param>
        /// CreatedBy: NTTrung (24/08/2023)
        Task CheckDublicateListNames(string listName);
    }
}
