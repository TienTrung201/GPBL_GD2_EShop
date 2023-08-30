using Dapper;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository
{
    public class PictureRepository : IPictureRepository
    {
        #region Fields
        private readonly IUnitOfWork _uow;
        #endregion
        #region Constructor
        public PictureRepository(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        #endregion
        #region Methods
        public async Task<Picture> GetByIdAsync(Guid id)
        {
            var storedProcedureName = $"Proc_Picture_GetById";
            var parameters = new DynamicParameters();//Tạo parameter
            parameters.Add("@Id", id);

            var result = await _uow.Connection.QueryFirstOrDefaultAsync<Picture>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        }

        public async Task<int> InsertAsync(Picture picture)
        {
            var storedProcedureName = $"Proc_Picture_Insert";
            var parameters = new DynamicParameters();//Tạo parameter
            parameters.Add("@PictureID", picture.PictureId);
            parameters.Add("@PictureName", picture.PictureName);
            parameters.Add("@Extension", picture.Extension);

            var result = await _uow.Connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return result;
        } 
        #endregion
    }
}
