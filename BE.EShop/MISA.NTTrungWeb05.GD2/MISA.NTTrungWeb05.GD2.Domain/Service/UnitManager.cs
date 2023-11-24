using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Service
{
    public class UnitManager : IUnitManager
    {
        #region Fields
        private readonly IUnitRepository _unitRepository;
        #endregion
        #region Constructor
        public UnitManager(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        #endregion
        /// <summary>
        /// Kiểm tra có tồn tại không
        /// </summary>
        /// <paran name="id">Định danh</paran>
        /// CreatedBy: NTTrung (16/07/2023)
        public async Task CheckExistAsync(Guid? id)
        {
            if (id.HasValue)
            {
                await _unitRepository.GetByIdAsync(id.Value);
            }
        }
        public async Task CheckDublicateListNames(string listNames)
        {
            var result = await _unitRepository.GetNameInvalidAsync(listNames);
            if (result != null)
            {
                throw new DuplicateCodeDetailException(string.Format(ErrorMessage.DuplicateError, result), (int)ErrorCode.DuplicateCodeDetail);
            }
        }
    }
}
