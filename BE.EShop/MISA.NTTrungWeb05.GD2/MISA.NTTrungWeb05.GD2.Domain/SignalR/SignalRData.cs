using MISA.NTTrungWeb05.GD2.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.SignalR
{
    public class SignalRData
    {
        public string UserId { get; set; }
        public object Data { get; set; }
        public SignalRType SignalRType { get; set; }
    }
}
