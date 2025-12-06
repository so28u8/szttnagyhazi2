using Xunit;
using System;
using CampingBooking;
using Assert = Xunit.Assert;

namespace CampingBooking.Tests
{
    public class PriceCalculatorTests
    {
        private readonly PriceCalculator _calculator = new PriceCalculator();
        private readonly Place _place = new Place(1, "Test", 2, 5000); // 5000 Ft/éj

        [Fact]
        public void CalculateTotalPrice_StandardCase()
        {
            DateTime from = new DateTime(2025, 1, 1);
            DateTime to = new DateTime(2025, 1, 4); // 3 éjszaka
            int guests = 2;
            
            int price = _calculator.CalculateTotalPrice(_place, from, to, guests);

            Assert.Equal(30000, price);
        }

        [Fact]
        public void CalculateTotalPrice_MinimumOneNight()
        {
            DateTime from = new DateTime(2025, 1, 1);
            DateTime to = new DateTime(2025, 1, 1);
            int guests = 1;

            int price = _calculator.CalculateTotalPrice(_place, from, to, guests);
            
            Assert.Equal(5000, price);
        }
    }
}