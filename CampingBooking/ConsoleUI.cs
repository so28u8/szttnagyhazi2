using System;

namespace CampingBooking
{
    public class ConsoleUI
    {
        private readonly PlaceManager placeManager = new();
        private readonly BookingManager bookingManager = new();
        private readonly AvailabilityChecker availabilityChecker = new();
        private readonly PriceCalculator priceCalculator = new();

        public void Run()
        {
            Console.WriteLine("Üdvözöllek a Kemping-foglaló alkalmazásban!");

            while (true)
            {
                Console.WriteLine("\n1) Helyek listázása");
                Console.WriteLine("2) Foglalás létrehozása");
                Console.WriteLine("3) Foglalások listázása");
                Console.WriteLine("4) Foglalás módosítása");
                Console.WriteLine("5) Foglalás törlése");
                Console.WriteLine("6) Kilépés");
                Console.Write("Válassz: ");


                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ListPlaces();
                        break;
                    case "2":
                        MakeBooking();
                        break;
                    case "3":
                        ListBookings();
                        break;
                    case "4":
                        ModifyBooking();
                        break;
                    case "5":
                        DeleteBooking();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Rossz opció!");
                        break;
                }
            }
        }

        private void ListPlaces()
        {
            Console.WriteLine("\nElérhető helyek:");
            foreach (var p in placeManager.Places)
            {
                Console.WriteLine($"{p.Id}) {p.Name} - Kapacitás: {p.Capacity}, Ár/éj: {p.PricePerNight} Ft");
            }
        }

        private void MakeBooking()
        {
            Console.Write("Foglalási név: ");
            string username = Console.ReadLine();
            User user = new User(username, UserRole.Guest);

            Console.Write("ID: ");
            int placeId = int.Parse(Console.ReadLine());
            Place place = placeManager.GetPlaceById(placeId);

            if (place == null)
            {
                Console.WriteLine("Rossz ID!");
                return;
            }

            Console.Write("-tól (yyyy-mm-dd): ");
            DateTime from = DateTime.Parse(Console.ReadLine());
            Console.Write("-ig (yyyy-mm-dd): ");
            DateTime to = DateTime.Parse(Console.ReadLine());

            if (!availabilityChecker.IsAvailable(place, from, to, bookingManager))
            {
                Console.WriteLine("A hely nem elérhető ebben az idősávban!");
                return;
            }

            int price = priceCalculator.CalculateTotalPrice(place, from, to);

            Booking booking = new Booking(user, place, from, to, price);
            bookingManager.AddBooking(booking);

            Console.WriteLine($"Sikeres foglalás! Összesen: {price} Ft");
        }

        private void ListBookings()
        {
            Console.WriteLine("\nFoglalások:");

            foreach (var b in bookingManager.Bookings)
            {
                Console.WriteLine(
                    $"ID: {b.Id} | {b.User.Username} | {b.Place.Name} | {b.From:yyyy-MM-dd} -> {b.To:yyyy-MM-dd} | {b.TotalPrice} Ft"
                );
            }

            if (bookingManager.Bookings.Count == 0)
                Console.WriteLine("Nincs még foglalás.");
        }

        private void DeleteBooking()
        {
            ListBookings();

            Console.Write("\nAdd meg a törölni kívánt foglalás ID-ját: ");
            int id = int.Parse(Console.ReadLine());

            if (bookingManager.RemoveBooking(id))
                Console.WriteLine("Foglalás törölve.");
            else
                Console.WriteLine("Nincs ilyen foglalás!");
        }

        private void ModifyBooking()
        {
            ListBookings();

            Console.Write("\nAdd meg a módosítandó foglalás ID-ját: ");
            int id = int.Parse(Console.ReadLine());

            var booking = bookingManager.GetBookingById(id);
            if (booking == null)
            {
                Console.WriteLine("Nincs ilyen foglalás!");
                return;
            }

            Console.Write("Új kezdő dátum (yyyy-mm-dd): ");
            DateTime newFrom = DateTime.Parse(Console.ReadLine());

            Console.Write("Új vég dátum (yyyy-mm-dd): ");
            DateTime newTo = DateTime.Parse(Console.ReadLine());

            // Foglaltság ellenőrzés
            if (!availabilityChecker.IsAvailable(booking.Place, newFrom, newTo, bookingManager))
            {
                Console.WriteLine("Ebben az időszakban nem foglalható!");
                return;
            }

            int newPrice = priceCalculator.CalculateTotalPrice(booking.Place, newFrom, newTo);

            bookingManager.UpdateBooking(id, newFrom, newTo, newPrice);

            Console.WriteLine($"Foglalás módosítva. Új ár: {newPrice} Ft");
        }

    }
}
