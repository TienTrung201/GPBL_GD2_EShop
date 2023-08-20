﻿using Dapper;
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
    /// Lớp cơ sở cho Repository dùng cho việc thao tác với dữ liệu mã code (Code).
    /// Kế thừa từ lớp BaseRepository để thực hiện các thao tác CRUD và ReadOnlyRepository để thực hiện thao tác chỉ đọc.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu dữ liệu của đối tượng Entity</typeparam>
    /// <typeparam name="TModel">Kiểu dữ liệu của đối tượng Model</typeparam>
    /// CreatedBy: NTTrung (14/07/2023)
    public abstract class CodeRepository<TEntity, TModel> : CRUDRepository<TEntity, TModel>, ICodeRepository<TEntity, TModel> where TEntity : BaseAudiEntity
    {
        #region Methods
        protected CodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Tìm data bằng Code 
        /// </summary>
        /// <paran name="entity">Code</paran>
        /// <returns>Đối tượng</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<TEntity?> GetByCodeAsync(string code)
        {
            var storedProcedureName = $"Proc_{TableName}_GetByCode";
            var param = new DynamicParameters();
            param.Add($"@{TableName}Code", code);
            var result = await _uow.Connection.QueryFirstOrDefaultAsync<TEntity>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }
        /// <summary>
        /// lấy mã code mới
        /// </summary>
        /// <returns>mã mới</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async Task<string> GetNewCodeAsync()
        {
            var storedProcedureName = $"Proc_{TableName}_GetNewCode";
            var parameters = new DynamicParameters();
            parameters.Add("NewCodeOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);

            await _uow.Connection.QueryFirstOrDefaultAsync<string>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            var result = parameters.Get<string>("NewCodeOut");
            return result;
        }
        #endregion

    }
}
