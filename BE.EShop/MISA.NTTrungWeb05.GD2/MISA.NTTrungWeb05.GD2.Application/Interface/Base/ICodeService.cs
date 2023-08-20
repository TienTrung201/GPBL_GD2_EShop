﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Base
{
    public interface ICodeService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, Tmodel> : ICRUDService<TEntityDto, TEntityCreateDto, TEntityUpdateDto, Tmodel>
    {
        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        Task<string> GetNewCodeAsync();
    }
}
