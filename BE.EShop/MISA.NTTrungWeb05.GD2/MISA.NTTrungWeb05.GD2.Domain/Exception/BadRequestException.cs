﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain
{
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Mã lỗi cho ngoại lệ
        /// </summary>

        public int ErrorCode { get; set; }
        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public BadRequestException() { }
        /// <summary>
        /// Constructor với mã lỗi
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        public BadRequestException(int errorCode)
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// Constructor với thông báo và mã lỗi
        /// </summary>
        /// <param name="message">Thông báo ngoại lệ</param>
        /// <param name="errorCode">Mã lỗi</param>
        public BadRequestException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// Constructor với thông báo
        /// </summary>
        /// <param name="message">Thông báo ngoại lệ</param>
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
