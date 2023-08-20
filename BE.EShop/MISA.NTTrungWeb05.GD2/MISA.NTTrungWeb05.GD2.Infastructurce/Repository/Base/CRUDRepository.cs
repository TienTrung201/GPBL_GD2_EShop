using Dapper;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base
{
    /// <summary>
    /// Lớp cơ sở cho Repository dùng cho các thao tác chỉ đọc (ReadOnly) và thao tác cơ bản (CRUD) với cơ sở dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu dữ liệu của đối tượng Entity</typeparam>
    /// <typeparam name="TModel">Kiểu dữ liệu của đối tượng Model</typeparam>
    /// CreatedBy: NTTrung (14/07/2023)
    public abstract class CRUDRepository<TEntity, TModel> :
        ReadOnlyRepository<TEntity, TModel>, ICRUDRepository<TEntity, TModel>
        where TEntity : BaseAudiEntity //chắc chắn TEntity phải kế thừa IHaskey thì mới .GetKey được
    {
        protected CRUDRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region Methods
        /// <summary>
        /// Thêm một bản ghi mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="entity">Thông tin chi tiết bản ghi được thêm</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<int> InsertAsync(TEntity entity)
        {
            var storedProcedureName = $"Proc_{TableName}_Insert";

            var parameters = CreateParametersFromEntity(entity);

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }
        /// <summary>
        /// Sửa thông tin một bản ghi trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="entity">Thông tin chi tiết bản ghi được sửa</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<int> UpdateAsync(TEntity entity)
        {
            var storedProcedureName = $"Proc_{TableName}_Update";

            var parameters = CreateParametersFromEntity(entity);

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }
        /// <summary>
        /// Xóa một bản ghi khỏi cơ sở dữ liệu.
        /// </summary>
        /// <param name="entity">Đối tượng cần xóa</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<int> DeleteAsync(TEntity entity)
        {
            var storedProcedureName = $"Proc_{TableName}_DeleteById";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", entity.GetKey());

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }
        /// <summary>
        /// Xóa nhiều bản ghi từ danh sách các id được cung cấp.
        /// </summary>
        /// <param name="ids">Chuỗi id của các bản ghi cần xóa</param>
        /// <returns>Số bản ghi thay đổi</returns>
        public virtual async Task<int> DeleteManyAsync(string ids)
        {
            var storedProcedureName = $"Proc_{TableName}_DeleteMany";
            var parameters = new DynamicParameters();
            parameters.Add("@Ids", ids);

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }
        /// <summary>
        /// Tạo CreateParameters từ Entity  nhiều
        /// </summary>
        /// <paran name="entity">Đối tượng</paran>
        /// <returns>parameters</returns>
        private DynamicParameters CreateParametersFromEntity(TEntity entity)
        {
            var parameters = new DynamicParameters();//Tạo parameter
            var properties = typeof(TEntity).GetProperties();//Lấy danh sách các thuộc tính của đối tượng
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                parameters.Add($"@{property.Name}", value);
            }
            return parameters;//Trả về đối tượng parameters từ đối tượng entity
        }
        #endregion
    }
}
