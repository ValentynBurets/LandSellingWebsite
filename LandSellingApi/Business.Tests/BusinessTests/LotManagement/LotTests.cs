﻿using AutoMapper;
using Business.Contract.Model.LotManagement;
using Business.Contract.Model.LotManagement.Lot;
using Business.Services.LotManagement;
using Data.Contract.Repository.LotManagement;
using Data.Contract.UnitOfWork;
using Domain.Entity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Configurations;

namespace Business.Tests.BusinessTests.LotManagement
{
    [TestFixture]
    public class LotTests
    {
        static LocationDTO locationDTO = new LocationDTO()
        {
            Latitude = "20",
            Longitude = "30",
            Country = " Ukraine",
            Region = "LvivRegion",
            City = "Lviv",
            Street = "Bandery"
        };

        static LotDTO lotDTO = new LotDTO()
        {
            OwnerId = new Guid("09bb8262-8971-48f4-a"),
            ManagerId = new Guid("fe24830d-cb22-4a05-92b7-c9483109f5e8"),
            Status = "Open",
            Description = "description",
            BuyPrice = 100,
            MinBidPrice = 90,
            IsRent = false,
            IsAuction = false,
            Location = locationDTO
        };

        static Lot lot = new Lot()
        {
            Id = new Guid("b8e1f1c3-a156-4db7-9"),
            OwnerId = new Guid("2bef5669-55ee-4261-91ab-bd4281cfba3c"),
            ManagerId = new Guid("81929761-10bf-4e5c-9eeb-4d3c91a05683"),
            LocationId = new Guid("928bd058-ef62-411f-a214-05f5541dac5d"),
            Status = Domain.Entity.Constants.State.Open,
            PublicationDate = DateTime.Now,
            Description = "Description",
            BuyPrice = 120,
            MinBidPrice = 100,
            IsRent = false,
            IsAuction = true
        };

        static ReturnLotDTO returnLotDTO = new ReturnLotDTO()
        {
            Id = new Guid("b8e1f1c3-a156-4db7-9"),
            OwnerId = new Guid("2bef5669-55ee-4261-91ab-bd4281cfba3c"),
            ManagerId = new Guid("81929761-10bf-4e5c-9eeb-4d3c91a05683"),
            Status = "Open",
            PublicationDate = DateTime.Now,
            Description = "Description",
            BuyPrice = 120,
            MinBidPrice = 100,
            IsRent = false,
            IsAuction = true,
            Location = locationDTO
        };

        Guid locationId = new Guid("3617f605-4f80-4a06-bac9-ae2b73c1f805");

        IEnumerable<LotDTO> lotDTOs = new List<LotDTO>()
        {
            lotDTO
        };

        IEnumerable<Lot> lots = new List<Lot>()
        {
            lot
        };

        private static IMapper mapper = new Mapper(new MapperConfiguration(cnf => cnf.AddProfile(new MapperInitializer())));
        LotService lotService;

        [SetUp]
        public void SetUp(string longitude, string latitude)
        {
            ILocationRepository stubLocationRepository = 
                Mock.Of<ILocationRepository>(lR => lR.Add(It.IsAny<Location>()) == Task.CompletedTask &&
                                                   lR.GetByLongitudeAndLatitude(longitude, latitude) == Task.FromResult(locationId));

            ILotRepository stubLotRepository =
                Mock.Of<ILotRepository>(lR => lR.Add(It.IsAny<Lot>()) == Task.CompletedTask &&
                                              lR.GetById(It.IsAny<Guid>()) == Task.FromResult(lot) &&
                                              lR.GetAll() == Task.FromResult(lots));

            ILotUnitOfWork unitOfWork =
                Mock.Of<ILotUnitOfWork>(of => of.ImageRepository == null &&
                                              of.LotRepository == stubLotRepository &&
                                              of.LocationRepository == stubLocationRepository &&
                                              of.UserRepository == null &&
                                              of.AdminRepository == null &&
                                              of.BidRepository == null &&
                                              of.PriceCoefRepository == null &&
                                              of.AgreementRepository == null);

            lotService = new LotService(mapper, unitOfWork);
        }

        [Test]
        public void CreateNewLot()
        {
            // Act
            var result = lotService.Create(lotDTO, new Guid("4a08f0b7-4268-43d8-8092-4f2dfc17635e"));

            // Assert
            Assert.That(result.Exception, Is.Null);
        }

        [Test]
        public void GetAllImagesByLotId()
        {
            // Act
            var result = lotService.GetById(lot.Id);

            // Assert
            Assert.That(result.Result as IEnumerable<LotDTO>, Is.EqualTo(returnLotDTO));
        }

        [Test]
        public void GetAll()
        {
            // Act
            var result = lotService.GetAll();

            // Assert
            Assert.That(result.Result as IEnumerable<LotDTO>, Is.EqualTo(lotDTOs));
        }
    }
}
