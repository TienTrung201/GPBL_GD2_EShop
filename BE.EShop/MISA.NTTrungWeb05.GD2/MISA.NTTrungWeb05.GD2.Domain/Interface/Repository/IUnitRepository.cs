using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Interface.Repository
{
    public interface IUnitRepository : ICRUDRepository<Unit, UnitModel>
    {
        /// <summary>
        /// lấy mã lỗi không hợp lệ
        /// </summary>
        /// <paran name="entity">Chuỗi mã lỗi</paran>
        /// <returns>Chuỗi mã không hợp lệ</returns>
        /// CreatedBy: NTTrung (24/08/2023)
        Task<string> GetNameInvalidAsync(string listCodes);
    }
}
