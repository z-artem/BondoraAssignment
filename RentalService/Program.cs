using AutoMapper;
using RentalService.Models;
using SharedComponents.Dtos;
using System;

namespace RentalService
{
    class Program
    {
        static void Main(string[] args)
        {
            InitAutomapper();
            new MessageServer(args.Length > 0 ? args[0] : "localhost");

            Console.WriteLine("Service is up and running...");
            Console.ReadLine();
        }

        private static void InitAutomapper()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<OrderItemDto, OrderItemModel>());
        }
    }
}
