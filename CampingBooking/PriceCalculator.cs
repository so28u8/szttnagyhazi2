using System;

namespace CampingBooking
{
    public class PriceCalculator
    {
        public int CalculateTotalPrice(Place place, DateTime from, DateTime to)
        {
            int nights = (to - from).Days;
            if (nights <= 0) nights = 1;
            return nights * place.PricePerNight;
        }
    }
}
