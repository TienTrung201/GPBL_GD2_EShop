using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.ItemCategory;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service
{
    public class ItemCategoryService : CodeService<ItemCategory, ItemCategoryModel, ItemCategoryResponseDto, ItemCategoryRequestDto>, IItemCategoryService
    {
        public ItemCategoryService(IItemCategoryRepository itemRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(itemRepository, mapper, unitOfWork)
        {
        }

        public override Task<ItemCategory> MapCreateDtoToEntityValidateAsync(ItemCategoryRequestDto createDto)
        {
            throw new NotImplementedException();
        }

        public override Task<ItemCategory> MapUpdateDtoToEntityValidateAsync(Guid id, ItemCategoryRequestDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
