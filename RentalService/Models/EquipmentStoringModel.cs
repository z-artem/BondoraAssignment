using SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalService.Models
{
    public class EquipmentStoringModel
    {
        public long ItemId { get; set; }

        public string Name { get; set; }

        public EquipmentType Type { get; set; }
    }
}
