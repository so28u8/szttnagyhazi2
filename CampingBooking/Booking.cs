using System;
using CampingBooking;

namespace CampingBooking
{
    public class Booking
    {
        private static int counter = 1;

        public int Id { get; private set; }
        public User User { get; set; }
        public Place Place { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TotalPrice { get; set; }

        public Booking(User user, Place place, DateTime from, DateTime to, int totalPrice)
        {
            Id = counter++;
            User = user;
            Place = place;
            From = from;
            To = to;
            TotalPrice = totalPrice;
        }
    }
}
