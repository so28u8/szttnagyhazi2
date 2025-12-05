using System;

namespace CampingBooking
{
    public class AvailabilityChecker
    {
        public bool IsAvailable(Place place, DateTime from, DateTime to, BookingManager bookingManager)
        {
            foreach (var b in bookingManager.Bookings)
            {
                if (b.Place.Id == place.Id)
                {
                    bool overlaps = from < b.To && to > b.From;
                    if (overlaps) return false;
                }
            }
            return true;
        }
    }
}
