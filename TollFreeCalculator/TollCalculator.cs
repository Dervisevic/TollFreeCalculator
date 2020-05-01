using System;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        DateTime intervalEnd = dates[dates.Length - 1];
        if (intervalStart.Day != intervalEnd.Day)
        {
            throw new System.ArgumentException("Dates must be within the same 24 hours.", "dates");
        }

        
        int totalFee = 0;
        DateTime periodStart = intervalStart;
        foreach (DateTime date in dates)
        {
            int nextFee = GetSingleTollFee(vehicle, date);
            int prevFee = GetSingleTollFee(vehicle, periodStart);

            TimeSpan betweenPasses = date - periodStart;

            if (betweenPasses.TotalMinutes < 60)
            {
                if (totalFee > 0) totalFee -= prevFee;
                if (nextFee >= prevFee) prevFee = nextFee;
                totalFee += prevFee;
            }
            else
            {
                totalFee += nextFee;
                periodStart = date;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    /**
     * Calculate the toll for a single pass
     *
     * @param vehicle - the vehicle
     * @param date   - date and time of the pass
     * @return - the fee to be paid.
     */
    public int GetSingleTollFee(IVehicle vehicle, DateTime date)
    {
        if (PublicHolidays.IsTollFreeDate(date) || vehicle.GetExemptionStatus() == true) return 0;

        TimeSpan tollTime = date.TimeOfDay;

        if ((tollTime >= new TimeSpan(6, 0, 0) && tollTime <= new TimeSpan(6, 29, 0)) ||
            (tollTime >= new TimeSpan(8, 30, 0) && tollTime <= new TimeSpan(14, 59, 0)) ||
            (tollTime >= new TimeSpan(18, 0, 0) && tollTime <= new TimeSpan(18, 29, 0))) { return 8; }

        if ((tollTime >= new TimeSpan(6, 30, 0) && tollTime <= new TimeSpan(6, 59, 0)) ||
            (tollTime >= new TimeSpan(8, 0, 0) && tollTime <= new TimeSpan(8, 29, 0)) ||
            (tollTime >= new TimeSpan(15, 0, 0) && tollTime <= new TimeSpan(15, 29, 0)) ||
            (tollTime >= new TimeSpan(17, 0, 0) && tollTime <= new TimeSpan(17, 59, 0))) { return 13; }

        if ((tollTime >= new TimeSpan(7, 0, 0) && tollTime <= new TimeSpan(7, 59, 0)) ||
            (tollTime >= new TimeSpan(15, 30, 0) && tollTime <= new TimeSpan(16, 59, 0))) { return 18; }

        return 0;
    }
}