using Newtonsoft.Json;
using SharedComponents;
using SharedComponents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWeb.Services
{
    public class RemoteCommandService : IRemoteCommandService
    {
        private readonly IRPCClient rpcClient;

        public RemoteCommandService(IRPCClient rpcClient)
        {
            this.rpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));
        }

        public IEnumerable<EquipmentItemDto> GetEquipmentCatalog()
        {
            int cmd = (int)RPCCommandType.ListEquipment;

            string respone = rpcClient.SendMessage(cmd.ToString());
            var catalog = JsonConvert.DeserializeObject<IEnumerable<EquipmentItemDto>>(respone);

            return catalog;
        }

        public byte[] SendOrder(IEnumerable<OrderItemDto> orderItems)
        {
            int cmd = (int)RPCCommandType.CreateOrder;

            string jsonOrder = JsonConvert.SerializeObject(orderItems);
            string command = string.Join(',', cmd.ToString(), jsonOrder);

            string invoice = rpcClient.SendMessage(command);

            return Encoding.UTF8.GetBytes(invoice);
        }
    }
}
