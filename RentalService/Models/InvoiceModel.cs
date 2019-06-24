using SharedComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentalService.Models
{
    public class InvoiceModel
    {
        private List<OrderItemModel> invoiceItems = new List<OrderItemModel>();

        public void AddToInvoice(IEnumerable<OrderItemModel> items)
        {
            invoiceItems.AddRange(items);
        }

        public int CalculateLoyalty(IEnumerable<EquipmentStoringModel> catalog)
        {
            int points = 0;
            foreach(var item in invoiceItems)
            {
                EquipmentType equipmentType = catalog.First(x => x.ItemId == item.ItemId).Type;
                switch (equipmentType)
                {
                    case EquipmentType.Heavy:
                        points += 2;
                        break;
                    case EquipmentType.Regular:
                        points++;
                        break;
                }
            }

            return points;
        }
    }
}
