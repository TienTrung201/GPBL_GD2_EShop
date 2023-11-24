using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Common;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service.Base
{
    public abstract class ReadOnlyService<TEntity, TModel, TEntityResponseDto> : IReadOnlyService<TModel, TEntityResponseDto>
    {
        #region Fields
        protected readonly IReadOnlyRepository<TEntity, TModel> _readOnlyRepository;
        protected readonly IMapper _mapper;
        #endregion
        #region Constructor
        protected ReadOnlyService(IReadOnlyRepository<TEntity, TModel> readOnlyRepository, IMapper mapper)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy bản ghi trong trang và lọc
        /// </summary>
        /// <paran name="entity">List Filter</paran>
        /// <returns>Danh sách bản ghi trong trang</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        /// <summary>
        public async Task<Pagination<TModel>> FilterAsync(FilterSort listFilter)
        {
            var result = await _readOnlyRepository.FilterAsync(listFilter);
            return result;
        }
        /// <summary>
        /// Phân trang tìm kiếm
        /// </summary>
        /// <returns>Danh sác bản ghi trong trang</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        public async Task<Pagination<TModel>> FilterAsync(int? pageSize, int? currentPage, string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }
            if (!currentPage.HasValue || currentPage.Value < 1)
            {
                currentPage = 1;
            }
            var result = await _readOnlyRepository.FilterAsync(pageSize.Value, currentPage.Value, search);

            //pagedEmployees.Data = _mapper.Map<IEnumerable<EmployeeDto>>(pagedEmployees.Data);

            return result;
        }
        /// <summary>
        /// Lấy tất cả danh sách
        /// </summary>
        /// <returns>Mã code mới</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<IEnumerable<TEntityResponseDto>> GetAllAsync()
        {
            var model = await _readOnlyRepository.GetAllAsync();
            var listEntityDto = _mapper.Map<List<TEntityResponseDto>>(model);
            return listEntityDto;
        }
        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bsản ghi tìm thấy</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<TEntityResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _readOnlyRepository.GetByIdAsync(id);
            var entityDto = _mapper.Map<TEntityResponseDto>(entity);
            return entityDto;
        }
        #endregion
    }
}
