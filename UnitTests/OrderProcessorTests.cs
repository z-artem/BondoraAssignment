using FluentAssertions;
using Newtonsoft.Json;
using RentalService;
using RentalService.Models;
using SharedComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class OrderProcessorTests
    {
        private readonly OrderProcessor orderProcessor;
        private readonly IEnumerable<EquipmentStoringModel> testData;

        public OrderProcessorTests()
        {
            testData = new List<EquipmentStoringModel>()
            {
                new EquipmentStoringModel()
                {
                    ItemId = 1,
                    Name = "Test 1",
                    Type = EquipmentType.Heavy
                },
                new EquipmentStoringModel()
                {
                    ItemId = 2,
                    Name = "Test 2",
                    Type = EquipmentType.Regular
                },
                new EquipmentStoringModel()
                {
                    ItemId = 3,
                    Name = "Test 3",
                    Type = EquipmentType.Specialized
                }
            };

            string jsonData = JsonConvert.SerializeObject(testData);
            File.WriteAllText("EquipmentCatalog.json", jsonData);

            orderProcessor = OrderProcessor.GetInstance();
        }

        [Fact]
        public void GetRawCatalogData_ReturnsCatalogData()
        {
            // act
            string actualResult = orderProcessor.GetRawCatalogData();

            // assert
            actualResult.Should().NotBeNullOrWhiteSpace();
            var actualData = JsonConvert.DeserializeObject<List<EquipmentStoringModel>>(actualResult);
            actualData.Should().BeEquivalentTo(testData);
        }

        [Fact]
        public void CalculateInvoice_ArgumentIsOk_ReturnsInvoice()
        {
            // arrange
            var invoiceItems = testData.Select(x => new OrderItemModel()
            {
                ItemId = x.ItemId,
                Quantity = 1001
            });

            // act
            string actualResult = orderProcessor.CalculateInvoice(invoiceItems);

            // assert
            actualResult.Should().NotBeNullOrWhiteSpace();
            actualResult.Should().Contain("1001");
            foreach(var dataItem in testData)
            {
                actualResult.Should().Contain(dataItem.Name);
            }
        }
    }
}
