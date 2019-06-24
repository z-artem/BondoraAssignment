using FluentAssertions;
using RentalService;
using SharedComponents;
using Xunit;

namespace UnitTests
{
    public class InvoiceSumCalculatorTests
    {
        private readonly InvoiceSumCalculator calculator;

        public InvoiceSumCalculatorTests()
        {
            calculator = new InvoiceSumCalculator();
        }

        [Theory]
        [InlineData(EquipmentType.Heavy, 1, 160)]
        [InlineData(EquipmentType.Regular, 1, 160)]
        [InlineData(EquipmentType.Specialized, 1, 60)]
        [InlineData(EquipmentType.Heavy, 5, 400)]
        [InlineData(EquipmentType.Regular, 5, 340)]
        [InlineData(EquipmentType.Specialized, 5, 260)]
        public void CalculatePrice_ArgumentIsOK_ReturnsPrice(EquipmentType equipmentType, int quantity, decimal expectedResult)
        {
            // act
            decimal actualResult = calculator.CalculatePrice(equipmentType, quantity);

            // assert
            actualResult.Should().Be(expectedResult);
        }
    }
}
