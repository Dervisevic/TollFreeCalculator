using System;
using System.Collections.Generic;

namespace TollFreeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting tests for TollFreeCalculator.");
            var totalTests = 0;
            var successfulTests = 0;

            Dictionary<string, DateTime> times = new Dictionary<string, DateTime>
            {
                { "06:40", new DateTime(2020, 4, 29, 6, 40, 52) },
                { "06:55", new DateTime(2020, 4, 29, 6, 55, 52) },
                { "07:10", new DateTime(2020, 4, 29, 7, 10, 52) },
                { "07:41", new DateTime(2020, 4, 29, 7, 41, 52) },
                { "09:10", new DateTime(2020, 4, 29, 9, 10, 52) },
                { "08:22", new DateTime(2020, 4, 29, 8, 22, 52) },
                { "10:18", new DateTime(2020, 4, 29, 10, 18, 52) },
                { "10:30", new DateTime(2020, 4, 29, 10, 30, 52) },
                { "16:59", new DateTime(2020, 4, 29, 16, 59, 00) },
                { "17:59", new DateTime(2020, 4, 29, 17, 59, 00) },
                { "18:10", new DateTime(2020, 4, 29, 18, 10, 52) },
                { "19:22", new DateTime(2020, 4, 29, 19, 22, 00) }
            };

            Dictionary<string, DateTime> dates = new Dictionary<string, DateTime>
            {
                { "03-01", new DateTime(2020, 3, 1, 17, 59, 00) },
                { "03-02", new DateTime(2020, 3, 2, 17, 59, 00) },
                { "04-26", new DateTime(2020, 4, 26, 17, 59, 00) },
                { "04-27", new DateTime(2020, 4, 27, 17, 59, 00) },
                { "05-01", new DateTime(2020, 5, 1, 17, 59, 00) },
                { "12-24", new DateTime(2020, 12, 24, 17, 59, 00) },
                { "01-15", new DateTime(2020, 1, 15, 17, 59, 00) }
            };

            // Setting up vehicles
            var regularCar = new TollFeeCalculator.Car();
            var diplomat = new TollFeeCalculator.Car { IsDiplomat = true };
            var emergencyVehicle = new TollFeeCalculator.Car { IsEmergency = true };
            var militaryCar = new TollFeeCalculator.Car { IsMilitary = true };
            var bike = new TollFeeCalculator.Motorbike();
            var tractor = new TollFeeCalculator.Tractor();


            // Setup Toll Calculator
            var tollCalc = new TollCalculator();

            /*
             * Testing Vehice Exemption status
             */
            totalTests += 6;
            if (regularCar.GetExemptionStatus() == false) { successfulTests++; }
            if (diplomat.GetExemptionStatus() == true) { successfulTests++; }
            if (emergencyVehicle.GetExemptionStatus() == true) { successfulTests++; }
            if (militaryCar.GetExemptionStatus() == true) { successfulTests++; }
            if (bike.GetExemptionStatus() == true) { successfulTests++; }
            if (tractor.GetExemptionStatus() == true) { successfulTests++; }


            /*
             * Single Passes
             */
            totalTests += 3;
            if (tollCalc.GetSingleTollFee(regularCar, times["09:10"]) == 8) { successfulTests++; }
            if (tollCalc.GetSingleTollFee(regularCar, times["17:59"]) == 13) { successfulTests++; }
            if (tollCalc.GetSingleTollFee(bike, times["17:59"]) == 0) { successfulTests++; }

            /*
             * Multiple passes
             */
            totalTests += 2;
            DateTime[] passes = { times["09:10"], times["17:59"] };
            if (tollCalc.GetTollFee(regularCar, passes) == 21) { successfulTests++; }

            DateTime[] max = {
                times["07:10"],
                times["08:22"],
                times["10:18"],
                times["16:59"],
                times["18:10"],
            };
            if (tollCalc.GetTollFee(regularCar, max) == 60) { successfulTests++; }

            /*
             * Multiple passes with multi pass discount
             */
            totalTests += 3;
            DateTime[] higher = { times["06:40"], times["06:55"], times["07:10"] };
            if (tollCalc.GetTollFee(regularCar, higher) == 18) { successfulTests++; }

            // One at the start of the day, and then multi pass at the end
            DateTime[] separate = { times["06:55"], times["10:18"], times["10:30"] };
            if (tollCalc.GetTollFee(regularCar, separate) == 21) { successfulTests++; }
   
            // One just outside the 60 min window
            DateTime[] sixty = { times["06:40"], times["06:55"], times["07:10"], times["07:41"] };
            if (tollCalc.GetTollFee(regularCar, sixty) == 36) { successfulTests++; }

            /*
             * Multiple days (should fail)
             */
            totalTests += 1;
            DateTime[] dayPasses = { dates["04-26"], dates["04-27"] };
            try
            {
                tollCalc.GetTollFee(regularCar, dayPasses);
            }
            catch (System.ArgumentException)
            {
                successfulTests++;
            }

            /*
             * Test public holidays
             */
            totalTests += 5;
            if (PublicHolidays.IsTollFreeDate(dates["05-01"]) == true) { successfulTests++; };
            if (PublicHolidays.IsTollFreeDate(dates["03-01"]) == true) { successfulTests++; };
            if (PublicHolidays.IsTollFreeDate(dates["03-02"]) == false) { successfulTests++; };
            if (PublicHolidays.IsTollFreeDate(dates["12-24"]) == true) { successfulTests++; };
            if (PublicHolidays.IsTollFreeDate(dates["01-15"]) == false) { successfulTests++; };

            Console.WriteLine("Tests completed. Result: " + successfulTests.ToString() + "/" + totalTests.ToString()); ;
        }
    }
}
