using System;
using System.Collections.Generic;
using System.Text;

namespace SharedComponents.Dtos
{
    public class EquipmentItemDto
    {
        public long ItemId { get; set; }

        public string Name { get; set; }

        public EquipmentType Type { get; set; }

        public int Quantity { get; set; }
    }
}
