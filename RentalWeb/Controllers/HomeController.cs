using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentalWeb.Models;
using RentalWeb.Services;
using SharedComponents.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RentalWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRemoteCommandService commandService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public HomeController(IRemoteCommandService commandService, IMapper mapper, ILogger logger)
        {
            this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            var catalogDto = commandService.GetEquipmentCatalog();
            var viewModel = mapper.Map<List<EquipmentItemViewModel>>(catalogDto);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SendOrder(List<EquipmentItemViewModel> viewModel)
        {
            var orderDto = mapper.Map<List<OrderItemDto>>(viewModel);
            orderDto.RemoveAll(x => x.Quantity == 0);
            byte[] responseFile = commandService.SendOrder(orderDto);

            logger.LogInformation("The invoice has been created");

            return File(responseFile, "text/plain");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
