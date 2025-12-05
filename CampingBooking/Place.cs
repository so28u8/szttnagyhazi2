namespace CampingBooking
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int PricePerNight { get; set; }

        public Place(int id, string name, int capacity, int pricePerNight)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
            PricePerNight = pricePerNight;
        }
    }
}
