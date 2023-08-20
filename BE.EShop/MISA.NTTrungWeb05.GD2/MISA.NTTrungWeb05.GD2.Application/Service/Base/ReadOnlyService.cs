using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Common;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service.Base
{
    public abstract class ReadOnlyService<TEntity, TModel, TEntityDto> : IReadOnlyService<TModel, TEntityDto>
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
            if (!currentPage.HasValue)
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
        public async Task<IEnumerable<TEntityDto>> GetAllAsync()
        {
            var model = await _readOnlyRepository.GetAllAsync();
            var listEntityDto = _mapper.Map<List<TEntityDto>>(model);
            return listEntityDto;
        }
        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bsản ghi tìm thấy</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<TEntityDto> GetByIdAsync(Guid id)
        {
            var entity = await _readOnlyRepository.GetByIdAsync(id);
            var entityDto = _mapper.Map<TEntityDto>(entity);
            return entityDto;
        }
        #endregion
    }
}
