using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
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
    public class UnitService : CodeService<Unit, UnitModel, UnitResponseDto, UnitRequestDto>, IUnitService
    {
        public UnitService(IUnitRepository unitRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitRepository, mapper, unitOfWork)
        {
        }

        public override Task<Unit> MapCreateDtoToEntityValidateAsync(UnitRequestDto createDto)
        {
            throw new NotImplementedException();
        }

        public override Task<Unit> MapUpdateDtoToEntityValidateAsync(Guid id, UnitRequestDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
