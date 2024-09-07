using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.OrderDetail;
using MISA.NTTrungWeb05.GD2.Application.Dtos.SAInvoice;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
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
    public class OrderDetailService : CodeService<OrderDetail, OrderDetail, OrderDetailDTO, OrderDetailDTO>, IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailepository;

        public OrderDetailService(
            IOrderDetailRepository orderDetailRepository,
            IMapper mapper, IUnitOfWork unitOfWork) : base(orderDetailRepository, mapper, unitOfWork)
        {
            _orderDetailepository = orderDetailRepository;
        }
    }
}