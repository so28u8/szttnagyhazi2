using Xunit;
using System;
using CampingBooking;

namespace Tests
{
    public class BookingManagerTests
    {
        [Fact]
        public void AddBooking_AddsBookingSuccessfully()
        {
            var bm = new BookingManager();
            var p = new Place(1, "Test", 2, 10000);
            var b = new Booking(new User("A", UserRole.Guest), p,
                DateTime.Today, DateTime.Today.AddDays(1), 100);

            bm.AddBooking(b);

            Assert.Single(bm.Bookings);
        }

        [Fact]
        public void RemoveBooking_RemovesCorrectly()
        {
            var bm = new BookingManager();
            var p = new Place(1, "Test", 2, 10000);
            var b = new Booking(new User("A", UserRole.Guest), p,
                DateTime.Today, DateTime.Today.AddDays(1), 100);

            bm.AddBooking(b);

            bool success = bm.RemoveBooking(b.Id);

            Assert.True(success);
            Assert.Empty(bm.Bookings);
        }

        [Fact]
        public void UpdateBooking_UpdatesDateAndPrice()
        {
            var bm = new BookingManager();
            var p = new Place(1, "X", 2, 10000);
            var b = new Booking(new User("A", UserRole.Guest), p,
                DateTime.Today, DateTime.Today.AddDays(2), 200);

            bm.AddBooking(b);

            var newFrom = DateTime.Today.AddDays(1);
            var newTo = DateTime.Today.AddDays(3);

            bm.UpdateBooking(b.Id, newFrom, newTo, 500);

            Assert.Equal(newFrom, b.From);
            Assert.Equal(newTo, b.To);
            Assert.Equal(500, b.TotalPrice);
        }
    }
}
