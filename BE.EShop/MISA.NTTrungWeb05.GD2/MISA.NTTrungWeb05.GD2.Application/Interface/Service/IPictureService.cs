using Microsoft.AspNetCore.Http;
using MISA.NTTrungWeb05.GD2.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Interface.Service
{
    public interface IPictureService
    {
        /// <summary>
        /// Thêm ảnh vào file và gọi db
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<PictureResponseDto> UploadAndInsertAsync(IFormFile file);
        /// <summary>
        /// Lấy tên file và ảnh bằng id
        /// </summary>
        /// <param name="uploadPath"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<byte[]> GetFileByIdAsync(Guid id);
    }
}
