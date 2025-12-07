using Xunit;
using System;
using CampingBooking;

namespace Tests
{
    public class PriceCalculatorTests
    {
        [Fact]
        public void CalculateTotalPrice_CorrectFor2Nights2Guests()
        {
            var calc = new PriceCalculator();
            var place = new Place(1, "X", 4, 10000);

            int total = calc.CalculateTotalPrice(
                place,
                DateTime.Today,
                DateTime.Today.AddDays(2),
                2
            );

            Assert.Equal(40000, total);
        }
    }
}
