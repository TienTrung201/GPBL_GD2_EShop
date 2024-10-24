using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Excel;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Order;
using MISA.NTTrungWeb05.GD2.Application.Interface.Excel;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Controllers.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Model;

namespace MISA.NTTrungWeb05.GD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CodeController<OrderDTO, OrderDTO, OrderModel>
    {
        #region Field
        #endregion
        public OrdersController(IOrderService iorderService) : base(iorderService)
        {
        }
    }
}
