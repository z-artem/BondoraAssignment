using AutoMapper;
using Newtonsoft.Json;
using RentalService.Models;
using SharedComponents;
using SharedComponents.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalService
{
    public class MessageServer
    {
        private IDictionary<RPCCommandType, Func<string, string>> commandHandlers;

        private OrderProcessor orderProcessor;

        public MessageServer(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
            {
                throw new ArgumentNullException(nameof(hostname));
            }

            RegisterCommandHandlers();
            orderProcessor = OrderProcessor.GetInstance();

            IRPCServer rpcServer = new RabbitRPCServer(hostname);
            rpcServer.RegisterReceiverCallback(ReceivedCallback);
        }

        private void RegisterCommandHandlers()
        {
            commandHandlers = new Dictionary<RPCCommandType, Func<string, string>>();

            commandHandlers[RPCCommandType.ListEquipment] = EquipmentCatalog;
            commandHandlers[RPCCommandType.CreateOrder] = CreateOrder;
        }

        private string ReceivedCallback(string message)
        {
            string[] msgParts = message.Split(',', 2);

            if (!int.TryParse(msgParts[0], out int cmdCode))
            {
                return null;
            }

            RPCCommandType cmd;
            if (Enum.IsDefined(typeof(RPCCommandType), cmdCode))
            {
                cmd = (RPCCommandType)cmdCode;
            }
            else
            {
                return null;
            }

            var handler = commandHandlers[cmd];
            string result = handler(msgParts.Length > 1 ? msgParts[1] : string.Empty);

            return result;
        }

        private string EquipmentCatalog(string _)
        {
            return orderProcessor.GetRawCatalogData();
        }

        private string CreateOrder(string orderRawData)
        {
            var orderItemDtos = JsonConvert.DeserializeObject<IEnumerable<OrderItemDto>>(orderRawData);
            var invoiceItems = Mapper.Map<List<OrderItemModel>>(orderItemDtos);

            string invoiceData = orderProcessor.CalculateInvoice(invoiceItems);

            return invoiceData;
        }
    }
}
