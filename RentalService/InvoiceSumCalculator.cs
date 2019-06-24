using SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalService
{
    public class InvoiceSumCalculator
    {
        private const decimal FEE_RENTAL = 100.00M;
        private const decimal FEE_PREMIUM = 60.00M;
        private const decimal FEE_REGULAR = 40.00M;

        private readonly IDictionary<EquipmentType, Func<int, decimal>> calculators;

        public InvoiceSumCalculator()
        {
            calculators = new Dictionary<EquipmentType, Func<int, decimal>>();

            calculators[EquipmentType.Heavy] = CalculateForHeavy;
            calculators[EquipmentType.Regular] = CalculateForRegular;
            calculators[EquipmentType.Specialized] = CalculateForSpecialized;
        }

        public decimal CalculatePrice(EquipmentType equipmentType, int quantity)
        {
            return calculators[equipmentType](quantity);
        }

        private decimal CalculateForHeavy(int quantity)
        {
            return FEE_RENTAL + FEE_PREMIUM * quantity;
        }

        private decimal CalculateForRegular(int quantity)
        {
            decimal price = FEE_RENTAL + FEE_PREMIUM;

            if (quantity > 1)
            {
                price += FEE_PREMIUM;
                quantity -= 2;
                price += FEE_REGULAR * quantity;
            }

            return price;
        }

        private decimal CalculateForSpecialized(int quantity)
        {
            decimal price = 0.00M;

            if (quantity <= 3)
            {
                price = FEE_PREMIUM * quantity;
            }
            else
            {
                price = FEE_PREMIUM * 3;
                quantity -= 3;
                price += FEE_REGULAR * quantity;
            }

            return price;
        }
    }
}
