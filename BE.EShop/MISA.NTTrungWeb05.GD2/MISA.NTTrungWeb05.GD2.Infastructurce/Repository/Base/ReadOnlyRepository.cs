using Dapper;
using MISA.NTTrungWeb05.GD2.Domain;
using MISA.NTTrungWeb05.GD2.Domain.Common;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base
{
    /// <summary>
    /// Lớp cơ sở cho Repository chỉ đọc (ReadOnly) dùng cho việc truy vấn dữ liệu từ cơ sở dữ liệu.
    /// Kế thừa từ lớp IReadOnlyRepository để cung cấp các phương thức truy vấn dữ liệu cơ bản.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu dữ liệu của đối tượng Entity</typeparam>
    /// <typeparam name="TModel">Kiểu dữ liệu của đối tượng Model</typeparam>
    /// CreatedBy: NTTrung (14/07/2023)
    public abstract class ReadOnlyRepository<TEntity, TModel> : IReadOnlyRepository<TEntity, TModel>
    {
        #region Fields
        protected readonly IUnitOfWork _uow;
        public virtual string TableName { get; protected set; } = typeof(TEntity).Name;
        public virtual string TableId { get; protected set; } = typeof(TEntity).Name + "Id";
        #endregion
        #region Constructor
        protected ReadOnlyRepository(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <paran name="entity">Code</paran>
        /// <returns>Danh sách Đối tượng</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var storedProcedureName = $"Proc_{TableName}_GetAll";
            var result = await _uow.Connection.QueryAsync<TModel>(storedProcedureName, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }
        /// </summary>
        /// Tìm kiếm theo Id
        /// </summary>
        /// <paran name="id">id</paran>
        /// <returns>Danh sách Đối tượng</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<TModel?> FindByIdAsync(Guid id)
        {
            var storedProcedureName = $"Proc_{TableName}_GetById";
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var result = await _uow.Connection.QueryFirstOrDefaultAsync<TModel>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }
        /// </summary>
        /// Tìm kiếm theo chuỗi ids
        /// </summary>
        /// <paran name="id">id</paran>
        /// <returns>Danh sách Đối tượng</returns>
        /// CreatedBy: NTTrung (03/08/2023)
        public async Task<IEnumerable<TModel>> GetByIdsAsync(string ids)
        {
            var storedProcedureName = $"Proc_{TableName}_GetByIds";
            var param = new DynamicParameters();
            param.Add("@Ids", ids);

            var result = await _uow.Connection.QueryAsync<TModel>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }
        /// </summary>
        /// Tìm kiếm theo Id
        /// </summary>
        /// <paran name="id">id</paran>
        /// <returns>Danh sách Đối tượng</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<TModel> GetByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            if (result == null)
            {
                throw new NotFoundException(string.Format(ErrorMessage.NotFound, id), (int)ErrorCode.NotFoundCode);
            }
            return result;
        }

        /// <summary>
        /// Phân trang tìm kiếm
        /// </summary>
        /// <returns>Số bản ghi + danh sách bản ghi</returns>
        /// CreatedBy: NTTrung (17/07/2023)
        public async Task<Pagination<TModel>> FilterAsync(int pageSize, int currentPage, string search)
        {
            var storedProcedureName = $"Proc_{TableName}_Pagination";
            var parameters = new DynamicParameters();
            parameters.Add("@Search", search);
            parameters.Add("@CurrentPage", currentPage);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var listData = await _uow.Connection.QueryAsync<TModel>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            var totalRecords = parameters.Get<int>("@TotalRecords");

            var result = new Pagination<TModel>(listData.ToList(), totalRecords);
            return result;
        }
        #endregion
    }
}
