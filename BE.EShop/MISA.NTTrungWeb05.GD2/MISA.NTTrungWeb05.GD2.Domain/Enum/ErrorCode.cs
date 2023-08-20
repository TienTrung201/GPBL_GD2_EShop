using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Enum
{
    /// <summary>
    /// Enum mã lỗi
    /// </summary>
    /// CreatedBy: NTTrung (11/07/2023)
    public enum ErrorCode
    {
        [Description("Có phát sinh")]
        ExistedConstrain = 1003,
        [Description("Trùng mã")]
        DuplicateCode = 1002,
        [Description("Không tìm thấy")]
        NotFoundCode = 1001,
        [Description("Lỗi server")]
        ErrorServerCode = 1000,

    }
}
