using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Inventory;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Order;
using MISA.NTTrungWeb05.GD2.Application.Dtos.OrderDetail;
using MISA.NTTrungWeb05.GD2.Application.Dtos.SAInvoice;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Manager;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Model;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
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
        private readonly ISAInvoiceService _invoiceService;
        private readonly IWebSocketMPos _webSocketMPos;
        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailService orderDetailService,
            IOrderDetailRepository orderDetailRepository,
            ISAInvoiceService sAInvoiceService,
            IWebSocketMPos webSocketMPos,
            IMapper mapper, IUnitOfWork unitOfWork) : base(orderRepository, mapper, unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderDetailService = orderDetailService;
            _invoiceService = sAInvoiceService;
            _webSocketMPos = webSocketMPos;
        }
        private SAInvoiceDTO CreateSAInvoice(OrderDTO order)
        {
            var saInvoice = new SAInvoiceDTO();
            saInvoice.OrderID = order.OrderId;
            saInvoice.TotalAmount = order.TotalAmount;
            saInvoice.PaymentStatus = (int)PaymentStatus.Done;
            saInvoice.SAInvoiceId = Guid.NewGuid();
            saInvoice.SAInvoiceType = order.OrderType;
            saInvoice.EditMode = EditMode.Create;
            saInvoice.PaymentType = 1;
            saInvoice.RefNo = order.OrderNo;
            return saInvoice;
        }
        /// <summary>
        /// Trước khi lưu
        /// </summary>
        /// <param name="data">Bản ghi được gửi đến</param>
        /// CreatedBy: NTTrung (27/08/2023)
        public override async void PreSave(List<OrderDTO> listData)
        {
            foreach (var item in listData)
            {
                if(item.EditMode == EditMode.Update)
                {
                    var isDeleteDetail = await _orderDetailRepository.DeleteOrderDetailByOrderID(item.OrderId);
                }
                if (item.EditMode == EditMode.Create)
                {
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    // Lấy thời gian hiện tại theo UTC
                    DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                    DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
                    item.OrderTime = localDateTime;
                }
                decimal totalAmountOrder = 0;
                decimal amountOrder = 0;
                if (item.EditMode == EditMode.Create || item.EditMode == EditMode.Update)
                {
                    var masterID = item.EditMode == EditMode.Create ? Guid.NewGuid() : item.OrderId;
                    item.OrderId = masterID;
                    foreach (var orderDetail in item.OrderDetails)
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
                    if(item.OrderStatus == (int)OrderStatus.None)
                    {
                        await _webSocketMPos.SendNotiOrderData();
                    }
                    string pattern = "^[A-Za-z]+";
                    string prefix = Regex.Match(item.OrderNo, pattern).Value;
                    await _orderRepository.UpdateCodeAsync(prefix);
                }
                if(item.EditMode != EditMode.Delete)
                {
                    // Sử dụng LINQ để lấy tất cả OrderDetail vào một biến
                    await _orderDetailService.SaveData(item.OrderDetails.ToList());
                    if (item.OrderStatus == (int)OrderStatus.Done)
                    {
                        var saInvoice = CreateSAInvoice(item);
                        var lstSAInvoice = new List<SAInvoiceDTO>();
                        lstSAInvoice.Add(saInvoice);
                        await _invoiceService.SaveData(lstSAInvoice);
                    }
                }
                else
                {
                    await _orderDetailRepository.DeleteOrderDetailByOrderID(item.OrderId);
                }
            }
        }
    }
}
