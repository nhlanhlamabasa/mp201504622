using System;

//this base class comes from Julie Lerman and Steve Smith
//https://github.com/julielerman/DomainDrivenDesignforDatabaseDrivenMind
//note that this implementation does not take Collections into account

namespace HotelSystem.SharedKernel
{
    public class DateTimeRange : ValueObject<DateTimeRange>
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        /// <summary>
        /// DateTimeRange Constructor creates DateTimeRange
        /// </summary>
        /// <param name="start">Start Date</param>
        /// <param name="end">End Date</param>
        /// <exception cref="ArgumentOutOfRangeException">If Start date is greater or equal to End date</exception>
        public DateTimeRange(DateTime start, DateTime end)
        {
            try
            {
                Guard.ForPrecedesDate(start, end, "start");
                Start = start;
                End = end;
            }
            catch(ArgumentOutOfRangeException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public DateTimeRange(DateTime start, TimeSpan duration) : this(start, start.Add(duration))
        {
        }
        protected DateTimeRange() { }

        public int DurationInMinutes()
        {
            return (End - Start).Minutes;
        }

        public DateTimeRange NewEnd(DateTime newEnd)
        {
            return new DateTimeRange(this.Start, newEnd);
        }
        public DateTimeRange NewDuration(TimeSpan newDuration)
        {
            return new DateTimeRange(this.Start, newDuration);
        }
        public DateTimeRange NewStart(DateTime newStart)
        {
            return new DateTimeRange(newStart, this.End);
        }

        public static DateTimeRange CreateOneDayRange(DateTime day)
        {
            return new DateTimeRange(day, day.AddDays(1));
        }

        public static DateTimeRange CreateOneWeekRange(DateTime startDay)
        {
            return new DateTimeRange(startDay, startDay.AddDays(7));
        }

        public bool Overlaps(DateTimeRange dateTimeRange)
        {
            return this.Start < dateTimeRange.End &&
                this.End > dateTimeRange.Start;
        }

        public static bool Overlaps(DateTimeRange inMemoryInstance, DateTimeRange dateTimeRange)
        {
            return inMemoryInstance.Start < dateTimeRange.End &&
                inMemoryInstance.End > dateTimeRange.Start;
        }

        public override string ToString()
        {
            return Start.ToShortTimeString() + "-" + End.ToShortTimeString();
        }
    }
}
