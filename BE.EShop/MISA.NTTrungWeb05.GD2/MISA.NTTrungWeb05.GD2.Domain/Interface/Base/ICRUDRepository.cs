using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Base
{
    public interface ICRUDRepository<TEntity, TModel> : IReadOnlyRepository<TEntity, TModel>
    {
        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi được thêm</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> InsertAsync(TEntity entity);
        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi sửa</paran>
        /// <returns>Số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> UpdateAsync(TEntity entity);
        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi xóa</paran>
        /// <returns>Số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> DeleteAsync(TEntity entity);
        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <paran name="ids">Chuỗi các Id bản ghi</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        Task<int> DeleteManyAsync(string ids);
    }
}
