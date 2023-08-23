using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Unit;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
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
        private readonly IUnitManager _unitManager;
        private readonly IUnitRepository _unitRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public UnitService(IUnitManager unitManager,
            IUnitRepository unitRepository,
            IInventoryRepository inventoryRepository,
            IMapper mapper, IUnitOfWork unitOfWork) : base(unitRepository, mapper, unitOfWork)
        {
            _unitManager = unitManager;
            _unitRepository = unitRepository;
            _inventoryRepository = inventoryRepository;
        }

        #region Methods
        /// <summary>
        /// Validate Nghiệp vụ
        /// </summary>
        /// <param name="createDto">Đối tượng thêm mới</param>
        /// <returns>Thực thể tìm thấy</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public override async Task<Unit> MapCreateDtoToEntityValidateAsync(UnitRequestDto createDto)
        {
            // validate nghiệp vụ
            // Kiểm tra trùng mã
            await _unitManager.CheckDublicateCode(createDto.UnitCode, null);

            var unit = _mapper.Map<Unit>(createDto);
            unit.UnitId = Guid.NewGuid();
            unit.CreatedDate = DateTime.Now;
            unit.ModifiedDate = DateTime.Now;
            return unit;
        }

        /// <summary>
        /// Validate Nghiệp vụ
        /// </summary>
        /// <param name="createDto">Đối tượng được cập nhật</param>
        /// <param name="id">Id được cập nhật</param>
        /// <returns>Thực thể tìm thấy</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public override async Task<Unit> MapUpdateDtoToEntityValidateAsync(Guid id, UnitRequestDto updateDto)
        {
            // Kiểm tra có tồn tại không
            var department = await _unitRepository.GetByIdAsync(id);

            // Kiểm tra trùng mã
            await _unitManager.CheckDublicateCode(updateDto.UnitCode, department.UnitCode);

            var updateEmployee = _mapper.Map<Unit>(updateDto);
            updateEmployee.UnitId = department.UnitId;
            updateEmployee.ModifiedDate = DateTime.Now;
            return updateEmployee;
        }
        /// <summary>
        /// Ghi đè lại hàm xóa để kiểm tra xem có phát sinh dữ liệu liên quan không
        /// </summary>
        /// <paran name="id">id của bản ghi cần xóa</paran>
        /// <returns>số bản ghi thay đổi</returns>
        /// CreatedBy: NTTrung (14/07/2023)
        public async override Task<int> DeleteAsync(Guid id)
        {
            // kiểm tra xem có phát sinh không nếu phát sinh lỗi luôn
            //await _inventoryRepository.GetEmployeeByUnitId(id);

            var model = await _unitRepository.GetByIdAsync(id);

            var entity = _mapper.Map<Unit>(model);

            var result = await _unitRepository.DeleteAsync(entity);
            return result;

        }
        #endregion
    }
}
