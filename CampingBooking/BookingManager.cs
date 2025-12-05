using System.Collections.Generic;
using System.Linq;
using CampingBooking;

namespace CampingBooking
{
    public class BookingManager
    {
        public List<Booking> Bookings { get; } = new();

        public void AddBooking(Booking booking)
        {
            Bookings.Add(booking);
        }

        public Booking GetBookingById(int id)
        {
            return Bookings.FirstOrDefault(b => b.Id == id);
        }

        public bool RemoveBooking(int id)
        {
            var b = GetBookingById(id);
            if (b == null) return false;

            Bookings.Remove(b);
            return true;
        }

        public bool UpdateBooking(int id, DateTime newFrom, DateTime newTo, int newPrice)
        {
            var b = GetBookingById(id);
            if (b == null) return false;

            b.From = newFrom;
            b.To = newTo;
            b.TotalPrice = newPrice;
            return true;
        }
    }
}
