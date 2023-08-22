using MISA.NTTrungWeb05.GD2.Application.Dtos.ItemCategory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Service
{
    public interface IItemCategoryService : ICodeService<ItemCategoryResponseDto,ItemCategoryRequestDto,ItemCategoryModel>
    {
    }
}
