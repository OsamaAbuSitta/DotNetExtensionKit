using System;
using Xunit;
using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests
{
    public class DateTimeExtensions_WeekendAndRangeTests
    {
        [Theory]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void IsWeekend_ReturnsTrue_ForWeekendDays(DayOfWeek day)
        {
            // Find the next occurrence of the given day
            var date = GetNextDayOfWeek(new DateTime(2024, 1, 1), day);
            Assert.True(date.IsWeekend());
        }

        [Theory]
        [InlineData(DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Friday)]
        public void IsWeekend_ReturnsFalse_ForWeekdays(DayOfWeek day)
        {
            var date = GetNextDayOfWeek(new DateTime(2024, 1, 1), day);
            Assert.False(date.IsWeekend());
        }

        [Theory]
        [InlineData(DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Friday)]
        public void IsWeekday_ReturnsTrue_ForWeekdays(DayOfWeek day)
        {
            var date = GetNextDayOfWeek(new DateTime(2024, 1, 1), day);
            Assert.True(date.IsWeekday());
        }

        [Theory]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void IsWeekday_ReturnsFalse_ForWeekendDays(DayOfWeek day)
        {
            var date = GetNextDayOfWeek(new DateTime(2024, 1, 1), day);
            Assert.False(date.IsWeekday());
        }

        [Fact]
        public void IsWeekend_And_IsWeekday_AreComplementary()
        {
            // Check all 7 days of a week
            var monday = new DateTime(2024, 1, 1); // Monday
            for (int i = 0; i < 7; i++)
            {
                var date = monday.AddDays(i);
                Assert.NotEqual(date.IsWeekend(), date.IsWeekday());
            }
        }

        [Fact]
        public void IsBetween_ReturnsTrue_WhenDateIsInRange()
        {
            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 1, 31);
            var date = new DateTime(2024, 1, 15);

            Assert.True(date.IsBetween(start, end));
        }

        [Fact]
        public void IsBetween_ReturnsTrue_WhenDateEqualsStart()
        {
            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 1, 31);

            Assert.True(start.IsBetween(start, end));
        }

        [Fact]
        public void IsBetween_ReturnsTrue_WhenDateEqualsEnd()
        {
            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 1, 31);

            Assert.True(end.IsBetween(start, end));
        }

        [Fact]
        public void IsBetween_ReturnsFalse_WhenDateIsBeforeRange()
        {
            var start = new DateTime(2024, 1, 10);
            var end = new DateTime(2024, 1, 31);
            var date = new DateTime(2024, 1, 5);

            Assert.False(date.IsBetween(start, end));
        }

        [Fact]
        public void IsBetween_ReturnsFalse_WhenDateIsAfterRange()
        {
            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 1, 31);
            var date = new DateTime(2024, 2, 5);

            Assert.False(date.IsBetween(start, end));
        }

        private static DateTime GetNextDayOfWeek(DateTime from, DayOfWeek day)
        {
            int daysUntil = ((int)day - (int)from.DayOfWeek + 7) % 7;
            return from.AddDays(daysUntil);
        }
    }
}
