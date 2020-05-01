using System;

public class PublicHolidays
{
    /**
     * Returns wether a given date is exempt from toll duties. Only supports 2020.
     *
     * @param date   - date and time of all passes on one day
     * @return - if tolls need to be applied.
     */
    public static bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
        if (date.Month == 7) return true;

        if (year == 2020)
        {
            if ((month == 1 && (day == 1 || day == 5 || day == 6)) ||
                (month == 4 && (day == 9 || day == 10 || day == 13 || day == 30)) ||
                (month == 5 && (day == 1 || day == 20 || day == 21)) ||
                (month == 5 && (day == 1 || day == 8 || day == 9)) ||
                (month == 6 && (day == 5 || day == 6 || day == 20)) ||
                (month == 10 && day == 30) ||
                (month == 12 && (day == 24 || day == 25 || day == 26 || day == 31)))
            {
                return true;
            }
        }
        
        return false;
    }
}