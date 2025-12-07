using Xunit;
using System;
using CampingBooking;

namespace Tests
{
    public class BookingConflictTests
    {
        [Fact]
        public void AddingOverlappingBooking_FailsAvailabilityCheck()
        {
            var bm = new BookingManager();
            var ac = new AvailabilityChecker();
            var place = new Place(1, "Y", 3, 10000);

            bm.AddBooking(new Booking(new User("A", UserRole.Guest), place,
                DateTime.Today, DateTime.Today.AddDays(5), 50000));

            bool available = ac.IsAvailable(place,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), bm);

            Assert.False(available);
        }
    }
}
