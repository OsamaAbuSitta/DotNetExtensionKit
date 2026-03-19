using System;

namespace DotNetExtensionKit
{
    /// <summary>
    /// Provides extension methods for DateTime manipulation including day, week,
    /// month, and year boundaries, age calculation, and date utility checks.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns a DateTime representing the start of the day (00:00:00.000) for the given date,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the start of the day.</param>
        /// <returns>A new <see cref="DateTime"/> set to midnight (00:00:00.000) on the same date, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime now = new DateTime(2024, 6, 15, 14, 30, 0);
        /// DateTime start = now.StartOfDay();
        /// // start == 2024-06-15 00:00:00.000
        /// </code>
        /// </example>
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);
        }

        /// <summary>
        /// Returns a DateTime representing the end of the day (23:59:59.9999999) for the given date,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the end of the day.</param>
        /// <returns>A new <see cref="DateTime"/> set to the last tick of the day (23:59:59.9999999) on the same date, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime now = new DateTime(2024, 6, 15, 14, 30, 0);
        /// DateTime end = now.EndOfDay();
        /// // end == 2024-06-15 23:59:59.9999999
        /// </code>
        /// </example>
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, dateTime.Kind)
                .AddTicks(TimeSpan.TicksPerSecond - 1);
        }

        /// <summary>
        /// Returns a DateTime representing the start of the week (00:00:00.000) for the given date,
        /// finding the most recent occurrence of the specified day of the week.
        /// Preserves the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the start of the week.</param>
        /// <param name="startOfWeek">The day considered the first day of the week. Defaults to <see cref="DayOfWeek.Monday"/>.</param>
        /// <returns>A new <see cref="DateTime"/> set to midnight on the most recent occurrence of <paramref name="startOfWeek"/>, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime wednesday = new DateTime(2024, 6, 12); // Wednesday
        /// DateTime weekStart = wednesday.StartOfWeek();
        /// // weekStart == 2024-06-10 (Monday) 00:00:00.000
        ///
        /// DateTime sundayStart = wednesday.StartOfWeek(DayOfWeek.Sunday);
        /// // sundayStart == 2024-06-09 (Sunday) 00:00:00.000
        /// </code>
        /// </example>
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = ((int)dateTime.DayOfWeek - (int)startOfWeek + 7) % 7;
            return dateTime.AddDays(-diff).StartOfDay();
        }

        /// <summary>
        /// Returns a DateTime representing the end of the week (23:59:59.9999999) for the given date,
        /// six days after the start of the week. Preserves the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the end of the week.</param>
        /// <param name="startOfWeek">The day considered the first day of the week. Defaults to <see cref="DayOfWeek.Monday"/>.</param>
        /// <returns>A new <see cref="DateTime"/> set to the last tick of the day, six days after the start of the week, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime wednesday = new DateTime(2024, 6, 12); // Wednesday
        /// DateTime weekEnd = wednesday.EndOfWeek();
        /// // weekEnd == 2024-06-16 (Sunday) 23:59:59.9999999
        /// </code>
        /// </example>
        public static DateTime EndOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dateTime.StartOfWeek(startOfWeek).AddDays(6).EndOfDay();
        }

        /// <summary>
        /// Returns a DateTime representing the first day of the month at 00:00:00.000,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the start of the month.</param>
        /// <returns>A new <see cref="DateTime"/> set to the first day of the month at midnight, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2024, 6, 15);
        /// DateTime monthStart = date.StartOfMonth();
        /// // monthStart == 2024-06-01 00:00:00.000
        /// </code>
        /// </example>
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, dateTime.Kind);
        }

        /// <summary>
        /// Returns a DateTime representing the last day of the month at 23:59:59.9999999,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the end of the month.</param>
        /// <returns>A new <see cref="DateTime"/> set to the last day of the month at the last tick of the day, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2024, 6, 15);
        /// DateTime monthEnd = date.EndOfMonth();
        /// // monthEnd == 2024-06-30 23:59:59.9999999
        /// </code>
        /// </example>
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            int lastDay = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year, dateTime.Month, lastDay, 0, 0, 0, 0, dateTime.Kind).EndOfDay();
        }

        /// <summary>
        /// Returns a DateTime representing January 1st of the year at 00:00:00.000,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the start of the year.</param>
        /// <returns>A new <see cref="DateTime"/> set to January 1st at midnight, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2024, 6, 15);
        /// DateTime yearStart = date.StartOfYear();
        /// // yearStart == 2024-01-01 00:00:00.000
        /// </code>
        /// </example>
        public static DateTime StartOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Kind);
        }

        /// <summary>
        /// Returns a DateTime representing December 31st of the year at 23:59:59.9999999,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate the end of the year.</param>
        /// <returns>A new <see cref="DateTime"/> set to December 31st at the last tick of the day, with the original <see cref="DateTime.Kind"/> preserved.</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2024, 6, 15);
        /// DateTime yearEnd = date.EndOfYear();
        /// // yearEnd == 2024-12-31 23:59:59.9999999
        /// </code>
        /// </example>
        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31, 0, 0, 0, 0, dateTime.Kind).EndOfDay();
        }

        /// <summary>
        /// Calculates the age in whole years from the birth date to today.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <returns>The age in whole years as of today's date.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the birth date is after today.</exception>
        /// <example>
        /// <code>
        /// DateTime dob = new DateTime(1990, 3, 25);
        /// int age = dob.Age();
        /// // age == number of whole years between dob and DateTime.Today
        /// </code>
        /// </example>
        public static int Age(this DateTime birthDate)
        {
            return birthDate.Age(DateTime.Today);
        }

        /// <summary>
        /// Calculates the age in whole years from the birth date to the specified reference date.
        /// Subtracts one year if the birthday has not yet occurred in the reference year.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <param name="asOf">The reference date to calculate age against.</param>
        /// <returns>The age in whole years as of the specified reference date.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the birth date is after the reference date.</exception>
        /// <example>
        /// <code>
        /// DateTime dob = new DateTime(1990, 3, 25);
        /// int age = dob.Age(new DateTime(2024, 6, 15));
        /// // age == 34
        /// </code>
        /// </example>
        public static int Age(this DateTime birthDate, DateTime asOf)
        {
            if (birthDate > asOf)
                throw new ArgumentOutOfRangeException(nameof(birthDate), "Birth date cannot be after the reference date.");

            int age = asOf.Year - birthDate.Year;

            if (asOf.Month < birthDate.Month ||
                (asOf.Month == birthDate.Month && asOf.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Returns true if the DateTime falls on a Saturday or Sunday.
        /// </summary>
        /// <param name="dateTime">The date to check.</param>
        /// <returns><c>true</c> if the date is a Saturday or Sunday; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// DateTime saturday = new DateTime(2024, 6, 15); // Saturday
        /// bool result = saturday.IsWeekend();
        /// // result == true
        /// </code>
        /// </example>
        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Returns true if the DateTime falls on a weekday (Monday through Friday).
        /// </summary>
        /// <param name="dateTime">The date to check.</param>
        /// <returns><c>true</c> if the date is Monday through Friday; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// DateTime monday = new DateTime(2024, 6, 10); // Monday
        /// bool result = monday.IsWeekday();
        /// // result == true
        /// </code>
        /// </example>
        public static bool IsWeekday(this DateTime dateTime)
        {
            return !dateTime.IsWeekend();
        }

        /// <summary>
        /// Returns true if the DateTime falls within the inclusive range [start, end].
        /// </summary>
        /// <param name="dateTime">The date to check.</param>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The inclusive end of the range.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is greater than or equal to <paramref name="start"/> and less than or equal to <paramref name="end"/>; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2024, 6, 15);
        /// bool inRange = date.IsBetween(new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        /// // inRange == true
        /// </code>
        /// </example>
        public static bool IsBetween(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime >= start && dateTime <= end;
        }
    }
}
