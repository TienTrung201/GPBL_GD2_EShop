using MISA.NTTrungWeb05.GD2.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Repository
{
    public interface IPictureRepository
    {
        /// <summary>
        /// Thêm thông tin ảnh vào db
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        /// CreatedBy: NTTrung (31/08/2023)
        Task<int> InsertAsync(Picture picture);
        /// <summary>
        /// Lấy thông tin ảnh bằng Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: NTTrung (31/08/2023)
        Task<Picture> GetByIdAsync(Guid id);
    }
}
