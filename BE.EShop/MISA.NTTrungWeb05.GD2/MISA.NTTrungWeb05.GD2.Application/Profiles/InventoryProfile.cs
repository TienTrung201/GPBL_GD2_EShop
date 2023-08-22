using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Profiles
{
    public class InventoryProfile : Profile
    {
        /// <summary>
        /// Đăng ký Mapper
        /// </summary>
        /// CreatedBy: NTTrung (22/08/2023)
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryResponseDto>();
            CreateMap<InventoryModel, InventoryResponseDto>();
            CreateMap<InventoryRequestDto, Inventory>();
            CreateMap<InventoryModel, Unit>();
        }
    }
}
