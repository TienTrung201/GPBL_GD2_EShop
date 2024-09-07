using AutoMapper;
using MISA.NTTrungWeb05.GD2.Application.Dtos.Order;
using MISA.NTTrungWeb05.GD2.Application.Dtos.SAInvoice;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Application.Service.Base;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
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
    public class SAInvoiceService : CodeService<SAInvoice, SAInvoiceModel, SAInvoiceDTO, SAInvoiceDTO>, ISAInvoiceService
    {
        private readonly ISAInvoiceRepository _sainvoiceRepository;

        public SAInvoiceService(
            ISAInvoiceRepository sainvoiceRepository,
            IMapper mapper, IUnitOfWork unitOfWork) : base(sainvoiceRepository, mapper, unitOfWork)
        {
            _sainvoiceRepository = sainvoiceRepository;
        }
    }
}
