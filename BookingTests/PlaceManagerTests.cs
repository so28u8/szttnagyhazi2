using Xunit;
using CampingBooking;

namespace Tests
{
    public class PlaceManagerTests
    {
        [Fact]
        public void UpdatePrice_ChangesPriceSuccessfully()
        {
            var pm = new PlaceManager();
            var place = pm.GetPlaceById(1);
            int oldPrice = place.PricePerNight;

            pm.UpdatePrice(1, oldPrice + 5000);

            Assert.Equal(oldPrice + 5000, place.PricePerNight);
        }
    }
}
