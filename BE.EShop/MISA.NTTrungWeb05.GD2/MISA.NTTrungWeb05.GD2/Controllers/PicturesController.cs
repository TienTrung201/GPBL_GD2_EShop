using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;

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
            string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            var file = Request.Form.Files[0];
            var result = await _pictureService.UploadAndInsertAsync(uploadsPath, file); 

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var result = await _pictureService.GetFileByIdAsync(uploadsPath, id);
            return File(result, "image/jpeg"); // Đổi kiểu file tùy theo định dạng ảnh
        }
    }
}
