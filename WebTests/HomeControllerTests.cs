using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RentalWeb.Controllers;
using RentalWeb.Mapping;
using RentalWeb.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WebTests
{
    public class HomeControllerTests
    {
        private HomeController controller;

        private IRemoteCommandService remoteCommandService;
        private IMapper mapper;
        private ILogger logger;

        public HomeControllerTests()
        {
            remoteCommandService = Substitute.For<IRemoteCommandService>();
            logger = Substitute.For<ILogger>();

            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));

            controller = new HomeController(remoteCommandService, mapper, logger);

            
        }

        [Fact]
        public void Constructor_NullParam_ThrowsException()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new HomeController(null, mapper, logger));
            Assert.Throws<ArgumentNullException>(() => new HomeController(remoteCommandService, null, logger));
            Assert.Throws<ArgumentNullException>(() => new HomeController(remoteCommandService, mapper, null));
        }
    }
}
