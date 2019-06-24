using Newtonsoft.Json;
using RentalService.Models;
using SharedComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RentalService
{
    public class OrderProcessor
    {
        private static OrderProcessor instance = null;
        private static readonly object instanceLock = new object();

        private List<InvoiceModel> invoicesHistory;
        private readonly string rawCatalogData;
        private readonly IEnumerable<EquipmentStoringModel> catalog;
        private readonly InvoiceSumCalculator invoiceSumCalculator;

        private OrderProcessor()
        {
            rawCatalogData = File.ReadAllText("EquipmentCatalog.json");
            catalog = JsonConvert.DeserializeObject<IEnumerable<EquipmentStoringModel>>(rawCatalogData);

            invoicesHistory = new List<InvoiceModel>();
            invoiceSumCalculator = new InvoiceSumCalculator();
        }

        public static OrderProcessor GetInstance()
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderProcessor();
                    }
                }
            }

            return instance;
        }

        public string GetRawCatalogData()
        {
            return rawCatalogData;
        }

        public string CalculateInvoice(IEnumerable<OrderItemModel> invoiceItems)
        {
            InvoiceModel invoice = new InvoiceModel();
            invoice.AddToInvoice(invoiceItems);
            invoicesHistory.Add(invoice);

            StringBuilder sb = new StringBuilder($"Invoice #{invoicesHistory.Count}\n\n");
            sb.AppendLine("Pos. Name                                         Qty.  Sum");
            sb.AppendLine("-----------------------------------------------------------------");

            int i = 0;
            decimal invoiceTotal = 0.00M;
            foreach (var item in invoiceItems)
            {
                var catalogItem = catalog.First(x => x.ItemId == item.ItemId);
                decimal sum = invoiceSumCalculator.CalculatePrice(catalogItem.Type, item.Quantity);
                invoiceTotal += sum;

                sb.AppendLine($"{(++i).ToString().PadLeft(4)} {catalogItem.Name.PadRight(40)} {item.Quantity.ToString().PadLeft(6)} {sum.ToString().PadLeft(12)}");
            }

            sb.AppendLine("-----------------------------------------------------------------");
            sb.AppendLine($"Total {invoiceTotal.ToString().PadLeft(59)}\n\n");

            int loyalty = invoice.CalculateLoyalty(catalog);
            int totalLoyalty = invoicesHistory.Sum(x => x.CalculateLoyalty(catalog));

            sb.Append($"Loyalty points added: {loyalty.ToString().PadLeft(3)}               Total loyalty points: {totalLoyalty.ToString().PadLeft(3)}");

            return sb.ToString();
        }
    }
}
