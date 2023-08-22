using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.ItemCategory;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Profiles
{
    internal class ItemCategoryProfile : Profile
    {
        /// <summary>
        /// Đăng ký Mapper
        /// </summary>
        /// CreatedBy: NTTrung (22/08/2023)
        public ItemCategoryProfile()
        {
            CreateMap<ItemCategory, ItemCategoryResponseDto>();
            CreateMap<ItemCategoryModel, ItemCategoryResponseDto>();
            CreateMap<ItemCategoryRequestDto, ItemCategory>();
            CreateMap<ItemCategoryModel, Unit>();
        }
    }
}
