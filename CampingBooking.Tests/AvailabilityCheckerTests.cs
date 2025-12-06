using Xunit;
using System;
using CampingBooking;
using Assert = Xunit.Assert;

namespace CampingBooking.Tests
{
    public class AvailabilityCheckerTests
    {
        private readonly AvailabilityChecker _checker;
        private readonly BookingManager _bookingManager;
        private readonly Place _place;

        public AvailabilityCheckerTests()
        {
            _checker = new AvailabilityChecker();
            _bookingManager = new BookingManager();
            _place = new Place(1, "Test Place", 4, 10000);
        }

        [Fact]
        public void IsAvailable_ReturnsTrue_WhenNoBookingsExist()
        {
            bool result = _checker.IsAvailable(_place, DateTime.Today, DateTime.Today.AddDays(1), _bookingManager);
            Assert.True(result);
        }

        [Fact]
        public void IsAvailable_ReturnsFalse_WhenDatesOverlapExactly()
        {
            var existingBooking = new Booking(new User("User1", UserRole.Guest), _place, 
                new DateTime(2025, 1, 1), new DateTime(2025, 1, 5), 10000);
            _bookingManager.AddBooking(existingBooking);
            
            bool result = _checker.IsAvailable(_place, new DateTime(2025, 1, 1), new DateTime(2025, 1, 5), _bookingManager);
            
            Assert.False(result, "Nem szabadna engednie ugyanarra a dátumra foglalni.");
        }

        [Fact]
        public void IsAvailable_ReturnsFalse_WhenDatesOverlapPartially()
        {
            var existingBooking = new Booking(new User("User1", UserRole.Guest), _place,
                new DateTime(2025, 1, 10), new DateTime(2025, 1, 15), 10000);
            _bookingManager.AddBooking(existingBooking);
            
            bool result = _checker.IsAvailable(_place, new DateTime(2025, 1, 12), new DateTime(2025, 1, 18), _bookingManager);

            Assert.False(result, "Részleges átfedést sem szabad engednie.");
        }

        [Fact]
        public void IsAvailable_ReturnsTrue_WhenUpdatingOwnBooking()
        {
            
            var myBooking = new Booking(new User("Me", UserRole.Guest), _place, 
                new DateTime(2025, 1, 1), new DateTime(2025, 1, 5), 10000);
            _bookingManager.AddBooking(myBooking);
            int myId = myBooking.Id;
            bool result = _checker.IsAvailable(_place, new DateTime(2025, 1, 2), new DateTime(2025, 1, 6), _bookingManager, excludeBookingId: myId);

            Assert.True(result, "Módosításkor a saját régi időpontunk nem okozhat ütközést.");
        }
    }
}