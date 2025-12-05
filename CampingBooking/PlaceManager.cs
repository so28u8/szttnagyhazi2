using System.Collections.Generic;
using CampingBooking;

namespace CampingBooking
{
    public class PlaceManager
    {
        public List<Place> Places { get; } = new();

        public PlaceManager()
        {
            Places.Add(new Place(1, "Lake House", 4, 20000));
            Places.Add(new Place(2, "Forest Cabin", 6, 25000));
            Places.Add(new Place(3, "Mountain Tent Spot", 2, 10000));
        }

        public Place GetPlaceById(int id)
        {
            return Places.Find(p => p.Id == id);
        }
    }
}
