using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Base
{
    public interface ICRUDService<TEntityResponseDto, TEntityRequestDto, Tmodel> : IReadOnlyService<Tmodel, TEntityResponseDto>
    {
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
        /// <summary>
        /// Lưu data
        /// </summary>
        /// <paran name="DATA">Thông tin hàng hóa và list Item</paran>
        /// <returns>Bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (23/08/2023)
        Task<int> SaveData(List<TEntityRequestDto> listData);
    }
}
