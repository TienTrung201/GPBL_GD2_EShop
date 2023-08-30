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
        protected readonly IUnitOfWork _unitOfWork;
        public PictureService(IPictureRepository pictureRepository, IUnitOfWork unitOfWork)
        {
            _pictureRepository = pictureRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Thêm ảnh vào file và gọi db
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<PictureResponseDto> UploadAndInsertAsync(string uploadsPath,IFormFile file)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var picture = new Picture();
                picture.PictureId = Guid.NewGuid();
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                picture.PictureName = uniqueFileName;

                await _pictureRepository.InsertAsync(picture);

                var filePath = Path.Combine(uploadsPath, uniqueFileName);
                var imageUrl = $"https://localhost:7170/api/Pictures/{picture.PictureId}";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                await _unitOfWork.CommitAsync();
                return new PictureResponseDto()
                {
                    Picture = picture,
                    PictureUrl = imageUrl
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                throw;
            }
            
        }
        /// <summary>
        /// Lấy tên file và ảnh bằng id
        /// </summary>
        /// <param name="uploadPath"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<byte[]> GetFileByIdAsync(string uploadsPath, Guid id)
        {
            var picture = await _pictureRepository.GetByIdAsync(id);

            string imagePath = Path.Combine(uploadsPath, picture.PictureName);

            if (!System.IO.File.Exists(imagePath))
            {
                throw new NotFoundException(string.Format(ErrorMessage.NotFound, picture.PictureName), (int)ErrorCode.NotFoundCode);
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return imageBytes;
        }
    }
}
