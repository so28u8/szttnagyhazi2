using System;

namespace CampingBooking
{
    public class PriceCalculator
    {
        public int CalculateTotalPrice(Place place, DateTime from, DateTime to, int guesCount)
        {
            int nights = (to - from).Days;
            if (nights <= 0) nights = 1;
            int baseprice = nights * place.PricePerNight;
            int totalprice = baseprice * guesCount;
            return totalprice;
        }
    }
}
