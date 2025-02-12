﻿using AutoMapper;
using Business.Contract.Model.LotManagement;
using Business.Contract.Model.LotManagement.Image;
using Business.Contract.Services.LotManagement;
using Data.Contract.UnitOfWork;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services.LotManagement
{
    public class ImageService: IImageService
    {
        private IMapper _mapper;
        private readonly ILotUnitOfWork _unitOfWork;
        public ImageService(IMapper mapper, ILotUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(ImageDTO createImage)
        {
            Image newImage = new Image();

            string imageDataString = createImage.ImageData;

            if (imageDataString.Contains("base64,"))
            {
                imageDataString = imageDataString.Split("base64,")[1];
            }

            newImage.LotId = createImage.LotId;
            newImage.ImageData = Convert.FromBase64String(imageDataString);
            await _unitOfWork.ImageRepository.Add(newImage);
            await _unitOfWork.Save();
        }

        public async Task Delete(Guid id)
        {
            var image = await _unitOfWork.ImageRepository.GetById(id);
            await _unitOfWork.ImageRepository.Remove(image);
            await _unitOfWork.Save();
        }

        public async Task Update(ImageDTO updateImage, Guid imageId)
        {
            Image newImage = _mapper.Map<Image>(updateImage);
            await _unitOfWork.ImageRepository.Update(newImage);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<ReturnImageDTO>> GetAllByLotId(Guid lotId)
        {
            IEnumerable<Image> images = await _unitOfWork.ImageRepository.GetByLotId(lotId);

            List<ReturnImageDTO> imagesData = new List<ReturnImageDTO>();

            foreach (Image image in images)
            {
                ReturnImageDTO returnImageDTO = _mapper.Map<ReturnImageDTO>(image);
                returnImageDTO.Picture = Convert.ToBase64String(image.ImageData);
                imagesData.Add(returnImageDTO);
            }

            return imagesData;
        }
    }
}
