using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentalWeb.Models
{
    public class EquipmentItemViewModel
    {
        public long ItemId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive integer")]
        public int Quantity { get; set; }
    }
}
