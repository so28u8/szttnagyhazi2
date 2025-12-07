using System.Collections.Generic;

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

        public bool AddPlace(string name, int capacity, int price)
        {
            int newId = Places.Count == 0 ? 1 : Places[^1].Id + 1;
            Places.Add(new Place(newId, name, capacity, price));
            return true;
        }

        public bool ModifyPlace(int id, string newName, int newCapacity, int newPrice)
        {
            var p = GetPlaceById(id);
            if (p == null) return false;

            p.Name = newName;
            p.Capacity = newCapacity;
            p.PricePerNight = newPrice;
            return true;
        }

        public bool RemovePlace(int id, BookingManager bookingManager)
        {
            // Aktív foglalás ellenőrzés
            foreach (var b in bookingManager.Bookings)
            {
                if (b.Place.Id == id)
                    return false; // nem törölhető
            }

            var p = GetPlaceById(id);
            if (p == null) return false;

            Places.Remove(p);
            return true;
        }
    }
}
