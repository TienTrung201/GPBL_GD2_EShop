using Microsoft.AspNetCore.Http;
using MISA.NTTrungWeb05.GD2.Application.Dtos;
using MISA.NTTrungWeb05.GD2.Application.Interface.Service;
using MISA.NTTrungWeb05.GD2.Domain;
using MISA.NTTrungWeb05.GD2.Domain.Entity;
using MISA.NTTrungWeb05.GD2.Domain.Enum;
using MISA.NTTrungWeb05.GD2.Domain.Interface.Repository;
using MISA.NTTrungWeb05.GD2.Domain.Interface.UnitOfWork;
using MISA.NTTrungWeb05.GD2.Domain.Resources.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Application.Service
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        public PictureService(IPictureRepository pictureRepository, IUnitOfWork unitOfWork)
        {
            _pictureRepository = pictureRepository;
        }

        /// <summary>
        /// Thêm ảnh vào file và gọi db
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<PictureResponseDto> UploadAndInsertAsync(IFormFile file)
        {
            string uploadsPath = @"D:\Wep\learnCode\Misa\Pictures\Uploads";
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = Path.GetExtension(file.FileName);
            if (!validExtensions.Contains(fileExtension.ToLower()))
            {
                throw new BadRequestException(ErrorMessage.ImgFileErrorExtension, (int)ErrorCode.FIleImageInvalid);
            }

            // Kiểm tra dung lượng tệp tin ảnh
            long fileSizeLimit = 5 * 1024 * 1024; // 5MB
            if (file.Length > fileSizeLimit)
            {
                throw new BadRequestException(ErrorMessage.ImgFileErrorSize, (int)ErrorCode.FIleImageInvalid);
            }
            var picture = new Picture();
            picture.PictureId = Guid.NewGuid();
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            picture.PictureName = uniqueFileName;
            picture.Extension = fileExtension;

            await _pictureRepository.InsertAsync(picture);

            var filePath = Path.Combine(uploadsPath, uniqueFileName);
            var imageUrl = $"https://localhost:7170/api/Pictures/{picture.PictureId}";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return new PictureResponseDto()
            {
                Picture = picture,
                PictureUrl = imageUrl,
                Extension = fileExtension
            };
        }
        /// <summary>
        /// Lấy tên file và ảnh bằng id
        /// </summary>
        /// <param name="uploadPath"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<byte[]> GetFileByIdAsync(Guid id)
        {
            string uploadsPath = @"D:\Wep\learnCode\Misa\Pictures\Uploads";
            string pathDefault = @"D:\Wep\learnCode\Misa\Pictures\Default";
            var picture = await _pictureRepository.GetByIdAsync(id);
            string imagePath = "";
            //Nếu không tìm thấy theo id thì trả về ảnh mặc định
            if (picture == null)
            {
                imagePath += Path.Combine(pathDefault, "default.jpg");
            }
            else
            {
                //Nếu không tồn tại đường dẫn ảnh trả về ảnh mặc định
                string checkPath = Path.Combine(uploadsPath, picture.PictureName);   
                if (!System.IO.File.Exists(checkPath))
                {
                    imagePath += Path.Combine(pathDefault, "default.jpg");
                }
                else
                {
                    imagePath += checkPath;
                }
            }
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return imageBytes;
        }
    }
}
