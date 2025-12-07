using Xunit;
using System;
using CampingBooking;

namespace Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_CreatesUniqueId()
        {
            var b1 = new Booking(new User("A", UserRole.Guest),
                                 new Place(1, "P", 2, 10000),
                                 DateTime.Today, DateTime.Today.AddDays(1), 100);

            var b2 = new Booking(new User("B", UserRole.Guest),
                                 new Place(1, "P", 2, 10000),
                                 DateTime.Today, DateTime.Today.AddDays(1), 100);

            Assert.NotEqual(b1.Id, b2.Id);
        }
    }
}
