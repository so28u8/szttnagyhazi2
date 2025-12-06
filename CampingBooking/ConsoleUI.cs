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
                        Console.WriteLine("Hiba, próbáld újra!");
                        break;
                }
            }
        }

        private void ListPlaces()
        {
            Console.WriteLine("\nElérhető helyek:");
            foreach (var p in placeManager.Places)
            {
                Console.WriteLine($"{p.Id}) {p.Name} - Kapacitás: {p.Capacity} fő, Ár/éj: {p.PricePerNight} Ft");
            }
        }

        private void MakeBooking()
        {
            Console.WriteLine("\n--- Új foglalás ---");
            
            string username = GetValidString("Foglalási név: ");
            User user = new User(username, UserRole.Guest);
            
            ListPlaces();
            int placeId = GetValidInt("Válassz Hely ID-t: ");
            Place place = placeManager.GetPlaceById(placeId);

            if (place == null)
            {
                Console.WriteLine("Nincs ilyen ID-jú hely!");
                return;
            }
            
            DateTime from = GetValidDate("-tól", DateTime.Today); 
            DateTime to = GetValidDate("-ig", from.AddDays(1));   

            
            if (!availabilityChecker.IsAvailable(place, from, to, bookingManager))
            {
                Console.WriteLine("A hely nem elérhető ebben az idősávban!");
                return;
            }

            
            int guestCount = GetValidInt($"Vendégek száma (Max {place.Capacity} fő): ", 1, place.Capacity);

            
            int price = priceCalculator.CalculateTotalPrice(place, from, to, guestCount);
            Booking booking = new Booking(user, place, from, to, price);
            bookingManager.AddBooking(booking);

            Console.WriteLine($"Sikeres foglalás! Összesen: {price} Ft");
        }

        private void ListBookings()
        {
            Console.WriteLine("\nFoglalások:");

            if (bookingManager.Bookings.Count == 0)
            {
                Console.WriteLine("Nincs még foglalás.");
                return;
            }

            foreach (var b in bookingManager.Bookings)
            {
                Console.WriteLine(
                    $"ID: {b.Id} | {b.User.Username} | {b.Place.Name} | {b.From:yyyy-MM-dd} -> {b.To:yyyy-MM-dd} | {b.TotalPrice} Ft"
                );
            }
        }

        private void DeleteBooking()
        {
            ListBookings();
            if (bookingManager.Bookings.Count == 0) return;

            int id = GetValidInt("\nAdd meg a törölni kívánt foglalás ID-ját: ");

            if (bookingManager.RemoveBooking(id))
                Console.WriteLine("Foglalás törölve.");
            else
                Console.WriteLine("Nincs ilyen foglalás!");
        }

        private void ModifyBooking()
        {
            ListBookings();
            if (bookingManager.Bookings.Count == 0) return;

            int id = GetValidInt("\nAdd meg a módosítandó foglalás ID-ját: ");

            var booking = bookingManager.GetBookingById(id);
            if (booking == null)
            {
                Console.WriteLine("Nincs ilyen foglalás!");
                return;
            }

            Console.WriteLine($"Jelenlegi adatok: {booking.Place.Name}, {booking.From:yyyy-MM-dd} -> {booking.To:yyyy-MM-dd}");

            
            DateTime newFrom = GetValidDate("Új kezdő dátum", DateTime.Today);
            DateTime newTo = GetValidDate("Új vég dátum", newFrom.AddDays(1));
            
            
            int guestCount = GetValidInt($"Vendégszám (Max {booking.Place.Capacity} fő): ", 1, booking.Place.Capacity);

            
            if (!availabilityChecker.IsAvailable(booking.Place, newFrom, newTo, bookingManager, id))
            {
                Console.WriteLine("Ebben az időszakban nem foglalható (ütközés más foglalással)!");
                return;
            }

            int newPrice = priceCalculator.CalculateTotalPrice(booking.Place, newFrom, newTo, guestCount);

            bookingManager.UpdateBooking(id, newFrom, newTo, newPrice);

            Console.WriteLine($"Foglalás módosítva. Új ár: {newPrice} Ft");
        }

        private string GetValidString(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                
                if (input == null) throw new InvalidOperationException("TESZT HIBA: Elfogyott a bemeneti szöveg!");

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("A mező nem lehet üres! Próbáld újra.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        private int GetValidInt(string prompt, int min = 1, int max = int.MaxValue)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (input == null) throw new InvalidOperationException("TESZT HIBA: Elfogyott a bemeneti szöveg!");

                if (int.TryParse(input, out value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Hibás adat! Kérlek adj meg egy számot {min} és {max} között.");
            }
        }

        private DateTime GetValidDate(string prompt, DateTime minDate)
        {
            DateTime date;
            while (true)
            {
                Console.Write($"{prompt} (yyyy-MM-dd): ");
                string input = Console.ReadLine();
                
                if (input == null) throw new InvalidOperationException("TESZT HIBA: Elfogyott a bemeneti szöveg!");

                if (DateTime.TryParseExact(input, "yyyy-MM-dd", 
                    System.Globalization.CultureInfo.InvariantCulture, 
                    System.Globalization.DateTimeStyles.None, out date))
                {
                    if (date >= minDate)
                    {
                        return date;
                    }
                    Console.WriteLine("A dátum nem lehet korábbi a megengedettnél!");
                }
                else
                {
                    Console.WriteLine("Hibás formátum! Helyes formátum: év-hó-nap (pl. 2025-07-20)");
                }
            }
        }
    }
}