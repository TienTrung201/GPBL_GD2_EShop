using MISA.NTTrungWeb05.GD2.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Base
{
    public interface IReadOnlyService<TModel, TEntityResponseDto>
    {
        /// <summary>
        /// Lấy bản ghi trong trang và lọc
        /// </summary>
        /// <paran name="entity">List Filter</paran>
        /// <returns>Danh sách bản ghi trong trang</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        /// <summary>
        Task<Pagination<TModel>> FilterAsync(FilterSort listFilter);
        /// <summary>
        /// Lấy bản ghi trong trang và lọc
        /// </summary>
        /// <paran name="entity">List Filter</paran>
        /// <returns>Danh sách bản ghi trong trang</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        /// <summary>
        Task<Pagination<TModel>> FilterAsync(int? pageSize, int? currentPage, string? search);
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)

        Task<IEnumerable<TEntityResponseDto>> GetAllAsync();
        /// <summary>
        /// Lấy bản ghi theo Id
        /// Không thấy bắn ra lỗi luôn
        /// </summary>
        /// <paran name="id">Định danh bản ghi</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<TEntityResponseDto> GetByIdAsync(Guid id);
    }
}
