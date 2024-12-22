using Microsoft.AspNetCore.SignalR;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.SignalR;
using NTTRUNG_BaseWebAPI_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service.SignalR
{
    public class WebSocketMPos : Hub, IWebSocketMPos
    {
        private readonly IHubContext<WebSocketMPos> _hubContext; 
        public WebSocketMPos(IHubContext<WebSocketMPos> hubContext) { _hubContext = hubContext; }
        /// <summary>
        /// Thông báo có đơn hàng đặt từ web
        /// </summary>
        /// <returns></returns>
        public async Task SendNotiOrderData()
        {
            var data = new SignalRData()
            {
                SignalRType = SignalRType.NotiOrder,
            };
            await _hubContext.Clients.All.SendAsync("NotiOrderData", data);
        }
        /// <summary>
        /// Cập nhật trạng thái order cho người dùng
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStatusOrderForClient(OrderStatus status)
        {
            var data = new SignalRData()
            {
                SignalRType = SignalRType.UpdateOrderStatus,
            };
            await _hubContext.Clients.All.SendAsync("NotiOrderData", data);
        }
    }
}
