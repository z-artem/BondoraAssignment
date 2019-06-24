using SharedComponents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalWeb.Services
{
    public interface IRemoteCommandService
    {
        IEnumerable<EquipmentItemDto> GetEquipmentCatalog();

        byte[] SendOrder(IEnumerable<OrderItemDto> orderItems);
    }
}
