using Microsoft.AspNetCore.SignalR;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.SignalR;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application
{
    public interface IWebSocketMPos
    {
        /// <summary>
        /// Thông báo có đơn hàng đặt từ web
        /// </summary>
        /// <returns></returns>
        public Task SendNotiOrderData();
        public Task JoinAdminGroup();
        
        /// <summary>
        /// Cập nhật trạng thái order cho người dùng
        /// </summary>
        /// <returns></returns>
        public Task UpdateStatusOrderForClient(OrderStatus status);
    }
}
