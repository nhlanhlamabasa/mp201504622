namespace Booking.API.Validation
{
#pragma warning disable
    using HotelSystem.SharedKernel;
    using System;

    public static class Validate
    {

        public static Entity<Guid> EnityCannotBeNull(Entity<Guid> entity)
        {
            return entity ?? throw new ArgumentNullException("entity cannot be null");
        }


        public static decimal MustBeGreaterThanZero(decimal value)
        {
            return (value <= 0) ? throw new ArgumentOutOfRangeException("value cannot be null") : value;
        }


        public static int MustBeGreaterThanZero(int value)
        {
            return (value <= 0) ? throw new ArgumentOutOfRangeException("value cannot be null") : value;
        }


        public static Guid MustNotBeNull(Guid value)
        {
            return (value == null) ? throw new ArgumentNullException("value cannot be null") : value;
        }


        public static string MustNotBeNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("value cannot be null") : value;
        }

        public static bool SatisfiesRange(int bottom, int top, int value)
        {
            if (value < bottom)
            {
                throw new ArgumentOutOfRangeException("value", value, "below range");
            }
            else if (value > top)
            {
                throw new ArgumentOutOfRangeException("value", value, "above range");
            }
            else
            {
                return true;
            }
        }
    }
}
