using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RentalWeb.Controllers;
using RentalWeb.Mapping;
using RentalWeb.Models;
using RentalWeb.Services;
using SharedComponents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class HomeControllerTests
    {
        private readonly HomeController controller;

        private readonly IRemoteCommandService commandService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public HomeControllerTests()
        {
            commandService = Substitute.For<IRemoteCommandService>();
            logger = Substitute.For<ILogger>();
            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));

            controller = new HomeController(commandService, mapper, logger);
        }

        [Fact]
        public void Constructor_ArgumentIsNull_ThrowsException()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new HomeController(null, mapper, logger));
            Assert.Throws<ArgumentNullException>(() => new HomeController(commandService, null, logger));
            Assert.Throws<ArgumentNullException>(() => new HomeController(commandService, mapper, null));
        }

        [Fact]
        public void Index_ReturnsIndexPage()
        {
            // act
            controller.Index();

            // assert
            commandService.Received(1).GetEquipmentCatalog();
        }

        [Fact]
        public void SendOrder_ArgumentIsOk_ReturnsInvoiceFile()
        {
            // arrange
            var viewModel = new List<EquipmentItemViewModel>()
            {
                new EquipmentItemViewModel()
                {
                    ItemId = 1,
                    Quantity = 11
                },
                new EquipmentItemViewModel()
                {
                    ItemId = 2,
                    Quantity = 12
                },
                new EquipmentItemViewModel()
                {
                    ItemId = 3,
                    Quantity = 13
                }
            };

            string testFileContents = "test file contents";
            commandService.SendOrder(null).ReturnsForAnyArgs(Encoding.UTF8.GetBytes(testFileContents));

            // act
            var actualResult = controller.SendOrder(viewModel);

            // assert
            commandService.Received(1).SendOrder(Arg.Is<IEnumerable<OrderItemDto>>(x => CollectionsAreEquivalent(viewModel, x)));
            logger.ReceivedWithAnyArgs(1).LogInformation(null);
            Encoding.UTF8.GetString((actualResult as FileContentResult).FileContents).Should().Be(testFileContents);
        }

        private bool CollectionsAreEquivalent(List<EquipmentItemViewModel> viewModel, IEnumerable<OrderItemDto> itemDtos)
        {
            if (viewModel.Count() != itemDtos.Count())
            {
                return false;
            }

            return viewModel.All(x => itemDtos.First(y => y.ItemId == x.ItemId).Quantity == x.Quantity);
        }
    }
}
