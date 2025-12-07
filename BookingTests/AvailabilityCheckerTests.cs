using Xunit;
using System;
using CampingBooking;

namespace Tests
{
    public class AvailabilityCheckerTests
    {
        [Fact]
        public void IsAvailable_WhenNoOverlap_ReturnsTrue()
        {
            var ac = new AvailabilityChecker();
            var bm = new BookingManager();
            var place = new Place(1, "A", 2, 10000);

            bm.AddBooking(new Booking(new User("U", UserRole.Guest), place,
                DateTime.Today, DateTime.Today.AddDays(2), 200));

            bool available = ac.IsAvailable(place,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(3), bm);

            Assert.True(available);
        }

        [Fact]
        public void IsAvailable_WhenOverlaps_ReturnsFalse()
        {
            var ac = new AvailabilityChecker();
            var bm = new BookingManager();
            var place = new Place(1, "A", 2, 10000);

            bm.AddBooking(new Booking(new User("U", UserRole.Guest), place,
                DateTime.Today, DateTime.Today.AddDays(3), 300));

            bool available = ac.IsAvailable(place,
                DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), bm);

            Assert.False(available);
        }
    }
}
