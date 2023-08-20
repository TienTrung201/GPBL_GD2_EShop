﻿using MISA.NTTrungWeb05.GD2.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Base
{
    public interface IReadOnlyRepository<TEntity, TModel>
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<IEnumerable<TModel>> GetAllAsync();

        /// <summary>
        /// Lấy bản ghi theo Id
        /// Không thấy bắn ra lỗi luôn
        /// </summary>
        /// <paran name="id">Id bản ghi</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<TModel> GetByIdAsync(Guid id);
        /// </summary>
        /// Tìm kiếm theo chuỗi ids
        /// </summary>
        /// <paran name="id">id</paran>
        /// <returns>Danh sách Đối tượng</returns>
        /// CreatedBy: NTTrung (03/08/2023)
        Task<IEnumerable<TModel>> GetByIdsAsync(string ids);
        /// <summary>
        /// Lấy bản ghi theo Id
        /// Không tìm thấy trả về null
        /// </summary>
        /// <paran name="id">Id bản ghi</paran>
        /// <returns>Bản ghi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<TModel?> FindByIdAsync(Guid id);
        /// <summary>
        /// Phân trang tìm kiếm
        /// </summary>
        /// <returns>Số bản ghi + danh sách bản ghi</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        Task<Pagination<TModel>> FilterAsync(int pageSize, int currentPage, string search);
    }
}
