using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Order;
using MISA.NTTrungWeb05.GD2.Application.Dtos.OrderDetail;
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
    public class OrderProfile : Profile
    {
        /// <summary>
        /// Đăng ký Mapper
        /// </summary>
        /// CreatedBy: NTTrung (22/08/2023)
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderModel, OrderDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderModel, Order>();


            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<OrderDetailDTO, OrderDetail>();
        }
    }
}
