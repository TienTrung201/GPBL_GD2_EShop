using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Profiles
{
    public class UnitProfile : Profile
    {
        /// <summary>
        /// Đăng ký Mapper
        /// </summary>
        /// CreatedBy: NTTrung (22/08/2023)
        public UnitProfile()
        {
            CreateMap<Unit, UnitResponseDto>();
            CreateMap<UnitModel, UnitResponseDto>();
            CreateMap<UnitRequestDto, Unit>();
            CreateMap<UnitModel, Unit>();
        }
    }
}
