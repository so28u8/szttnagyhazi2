using Xunit;
using System;
using System.IO;
using CampingBooking;
using Assert = Xunit.Assert;

namespace CampingBooking.Tests
{
    public class InputValidationTests
    {
        [Fact]
        public void ConsoleUI_ResistantToBadInput_Numeric()
        {
            // Sorrend:
            // 1. "2"           (Főmenü: Foglalás)
            // 2. "Béla"        (Név)
            // 3. "bla"         (Rossz ID -> Hiba)
            // 4. "1"           (Jó ID - Lake House)
            // 5. "2020-01-01"  (Múltbéli dátum -> Hiba)
            // 6. "2030-05-20"  (Jó kezdés - BIZTOSAN JÖVŐBELI!)
            // 7. "2030-05-25"  (Jó vég)
            // 8. "száz"        (Rossz vendégszám -> Hiba)
            // 9. "2"           (Jó vendégszám)
            // 10. "6"          (Főmenü: Kilépés)

            var fullInput = new StringReader("2\nBéla\nbla\n1\n2020-01-01\n2030-05-20\n2030-05-25\nszáz\n2\n6\n");
            Console.SetIn(fullInput);

            var output = new StringWriter();
            Console.SetOut(output); 

            var ui = new ConsoleUI();
            
            Exception ex = Record.Exception(() => ui.Run());


            if (ex != null)
            {
                 throw new Exception($"A teszt elszállt ezzel a hibával: {ex.Message}\n\nKonzol kimenet:\n{output}");
            }

            Assert.Null(ex); 
            
            string consoleOutput = output.ToString();
            Assert.Contains("Hibás adat!", consoleOutput); 
            Assert.Contains("Sikeres foglalás", consoleOutput); 
        }
    }
}