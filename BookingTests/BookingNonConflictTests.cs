using Xunit;
using System;
using CampingBooking;

namespace Tests
{
    public class BookingNonConflictTests
    {
        [Fact]
        public void AddingNonOverlappingBooking_PassesAvailabilityCheck()
        {
            var bm = new BookingManager();
            var ac = new AvailabilityChecker();
            var place = new Place(1, "Y", 3, 10000);

            bm.AddBooking(new Booking(new User("A", UserRole.Guest), place,
                DateTime.Today, DateTime.Today.AddDays(3), 30000));

            bool available = ac.IsAvailable(place,
                DateTime.Today.AddDays(3), DateTime.Today.AddDays(5), bm);

            Assert.True(available);
        }
    }
}
