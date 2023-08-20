using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Base
{
    public interface ICRUDService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, Tmodel> : IReadOnlyService<Tmodel, TEntityDto>
    {

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi được gửi đến</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> CreatetAsync(TEntityCreateDto entityCreateDto);
        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <paran name="entity">Thông tin chi tiết bản ghi được gửi đến</paran>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> UpdateAsync(Guid id, TEntityUpdateDto entityUpdateDto);
        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <paran name="id">id bản ghi</paran>
        /// <returns</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<int> DeleteAsync(Guid id);
        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <paran name="ids">Chuỗi các Id bản ghi</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        Task<int> DeleteManyAsync(string ids);
    }
}
