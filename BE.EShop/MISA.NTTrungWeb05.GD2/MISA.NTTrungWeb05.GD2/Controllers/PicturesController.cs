using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MISA.NTTrungWeb05.GD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly string _uploadsPath;
        public PicturesController()
        {
            // Đường dẫn đến thư mục lưu trữ tệp tải lên
            _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {

            if (!Directory.Exists(_uploadsPath))
            {
                Directory.CreateDirectory(_uploadsPath);
            }

            var file = Request.Form.Files[0];
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadsPath, uniqueFileName);
            var imageUrl = $"https://localhost:7170/api/Pictures/{uniqueFileName}";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "File uploaded successfully.", fileName = imageUrl });
        }

        [HttpGet("{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            string imagePath = Path.Combine(uploadsPath, fileName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg"); // Đổi kiểu file tùy theo định dạng ảnh
        }
    }
}
