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
                Console.WriteLine(
                    "1) Helyek listázása\n" +
                    "2) Foglalás létrehozása\n" +
                    "3) Foglalások listázása\n" +
                    "4) Foglalás módosítása\n" +
                    "5) Foglalás törlése\n" +
                    "\n--- ADMIN FUNKCIÓK ---\n" +
                    "6) Hely hozzáadása\n" +
                    "7) Hely módosítása\n" +
                    "8) Hely törlése\n\n" +
                    "9) Kilépés\n" +
                    "Válassz: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": ListPlaces(); break;
                    case "2": MakeBooking(); break;
                    case "3": ListBookings(); break;
                    case "4": ModifyBooking(); break;
                    case "5": DeleteBooking(); break;
                    //TODO: ADMIN FUNKCIÓK HÍVÁSA
                    case "6": AdminAddPlace(); break;
                    case "7": AdminModifyPlace(); break;
                    case "8": AdminDeletePlace(); break;
                    case "9": return;
                    default:
                        Console.WriteLine("Hiba, próbáld újra!\n\n");
                        break;
                }
            }
        }

        private void ListPlaces()
        {
            Console.Clear();
            Console.WriteLine("\nElérhető helyek:");
            foreach (var p in placeManager.Places)
            {
                Console.WriteLine($"{p.Id}) {p.Name} - Kapacitás: {p.Capacity} fő, Ár/éj: {p.PricePerNight} Ft");
            }
            Console.WriteLine("\n\n");
        }

        private void MakeBooking()
        {
            Console.Clear();
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

            Console.WriteLine($"Sikeres foglalás! Összesen: {price} Ft\n\n");
        }

        private void ListBookings()
        {
            Console.Clear();
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
            Console.Clear();
            Console.WriteLine("\n--- Kölcsönzés törlése ---\n");
            ListBookings();
            if (bookingManager.Bookings.Count == 0) return;

            int id = GetValidInt("\nAdd meg a törölni kívánt foglalás ID-ját: ");

            if (bookingManager.RemoveBooking(id))
                Console.WriteLine("Foglalás törölve.\n\n");
            else
                Console.WriteLine("Nincs ilyen foglalás!\n\n");
        }

        private void ModifyBooking()
        {
            Console.Clear();
            Console.WriteLine("\n--- Kölcsönzés módosítása ---\n");
            ListBookings();
            if (bookingManager.Bookings.Count == 0) return;

            int id = GetValidInt("\nAdd meg a módosítandó foglalás ID-ját: ");

            var booking = bookingManager.GetBookingById(id);
            if (booking == null)
            {
                Console.WriteLine("Nincs ilyen foglalás!\n\n");
                return;
            }

            Console.WriteLine($"Jelenlegi adatok: {booking.Place.Name}, {booking.From:yyyy-MM-dd} -> {booking.To:yyyy-MM-dd}");

            
            DateTime newFrom = GetValidDate("Új kezdő dátum", DateTime.Today);
            DateTime newTo = GetValidDate("Új vég dátum", newFrom.AddDays(1));
            
            
            int guestCount = GetValidInt($"Vendégszám (Max {booking.Place.Capacity} fő): ", 1, booking.Place.Capacity);

            
            if (!availabilityChecker.IsAvailable(booking.Place, newFrom, newTo, bookingManager, id))
            {
                Console.WriteLine("Ebben az időszakban nem foglalható (ütközés más foglalással)!\n\n");
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

        private void AdminAddPlace()
        {
            Console.Clear();
            Console.WriteLine("\n--- Új hely hozzáadása ---\n");

            string name = GetValidString("Hely neve: ");
            int cap = GetValidInt("Kapacitás: ", 1, 1000);
            int price = GetValidInt("Ár/éj (Ft): ", 1, 1000000);

            placeManager.AddPlace(name, cap, price);
            Console.WriteLine("Hely sikeresen hozzáadva.\n\n");

        }

        private void AdminModifyPlace()
        {
            Console.Clear();
            Console.WriteLine("\n--- Hely módosítása ---\n");
            ListPlaces();

            int id = GetValidInt("Módosítandó Hely ID: ");
            var p = placeManager.GetPlaceById(id);

            if (p == null)
            {
                Console.WriteLine("Nincs ilyen hely!\n\n");
                return;
            }

            string newName = GetValidString("Új név: ");
            int newCap = GetValidInt("Új kapacitás: ", 1, 1000);
            int newPrice = GetValidInt("Új ár/éj: ", 1, 1000000);

            placeManager.ModifyPlace(id, newName, newCap, newPrice);
            Console.WriteLine("Hely módosítva.\n\n");
        }

        private void AdminDeletePlace()
        {
            Console.Clear();
            Console.WriteLine("\n--- Hely törlése ---\n");
            ListPlaces();

            int id = GetValidInt("Törlendő Hely ID: ");

            bool ok = placeManager.RemovePlace(id, bookingManager);
            if (!ok)
                Console.WriteLine("Nem törölhető: létezik hozzá aktív foglalás vagy nincs ilyen hely.\n\n");
            else
                Console.WriteLine("Hely törölve.\n\n");
        }
    }
}