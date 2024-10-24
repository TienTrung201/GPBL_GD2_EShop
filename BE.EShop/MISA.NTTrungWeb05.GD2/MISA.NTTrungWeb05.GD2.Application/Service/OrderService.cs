using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Order;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service
{
    public class OrderService : CodeService<Order, OrderModel, OrderDTO, OrderDTO>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderDetailService _orderDetailService;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailService orderDetailService,
            IOrderDetailRepository orderDetailRepository,
            IMapper mapper, IUnitOfWork unitOfWork) : base(orderRepository, mapper, unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderDetailService = orderDetailService;
        }
        /// <summary>
        /// Trước khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public override void PreSave(List<OrderDTO> listData)
        {
            decimal totalAmountOrder = 0;
            decimal amountOrder = 0;
            foreach (var item in listData)
            {
                if(item.EditMode == EditMode.Create)
                {
                    var masterID = Guid.NewGuid();
                    item.OrderId = masterID;
                    foreach (var orderDetail in item.LstOrderDetail)
                    {
                        decimal amount = (decimal)(orderDetail.UnitPrice * orderDetail.Quantity);
                        amountOrder += amount;
                        orderDetail.OrderId = masterID;
                        orderDetail.OrderDetailId = Guid.NewGuid();
                    }

                    totalAmountOrder = amountOrder;
                    item.TotalAmount = totalAmountOrder;
                    item.Amount = totalAmountOrder;
                }
            }
        }
        public async override Task AfterSaveSuccess(List<OrderDTO> listData)
        {
            foreach (var item in listData)
            {
                if (item.EditMode == EditMode.Create)
                {
                    string pattern = "^[A-Za-z]+";
                    string prefix = Regex.Match(item.OrderNo, pattern).Value;

                    await _orderRepository.UpdateCodeAsync(prefix);
                }
            }
            // Sử dụng LINQ để lấy tất cả OrderDetail vào một biến
            var allOrderDetails = listData.SelectMany(o => o.LstOrderDetail).ToList();
            await _orderDetailService.SaveData(allOrderDetails);
        }
    }
}
