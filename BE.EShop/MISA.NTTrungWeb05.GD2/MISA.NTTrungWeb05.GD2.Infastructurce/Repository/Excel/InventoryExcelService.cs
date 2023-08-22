using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Base;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Infastructurce.Repository.Excel
{
    public class InventoryExcelService : ExcelService<InventoryModel, Inventory>, IInventoryExcelService
    {
        public InventoryExcelService(IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
        }
    }
}
