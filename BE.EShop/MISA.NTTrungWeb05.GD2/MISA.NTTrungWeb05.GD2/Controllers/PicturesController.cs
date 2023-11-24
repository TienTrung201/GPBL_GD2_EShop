using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using MISA.NTTrungWeb05.GD2.Domain;

namespace MISA.NTTrungWeb05.GD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        //private readonly string _uploadsPath;
        private readonly IPictureService _pictureService;
        public PicturesController(IPictureService pictureService)
        {
            _pictureService = pictureService;
            // Đường dẫn đến thư mục lưu trữ tệp tải lên
            //_uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files[0];
            string uploadsPath = @"D:\Wep\learnCode\Misa\Uploads";
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
         
            var result = await _pictureService.UploadAndInsertAsync(file); 

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var result = await _pictureService.GetFileByIdAsync(id);
            return File(result, "image/jpeg"); // Đổi kiểu file tùy theo định dạng ảnh
        }
    }
}
