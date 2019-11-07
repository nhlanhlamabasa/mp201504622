namespace HotelSystem.SharedKernel
{
    using System;

    /// <summary>
    /// Defines the <see cref="Guard" />
    /// </summary>
    public class Guard
    {
        /// <summary>
        /// The ForLowerBound
        /// </summary>
        /// <param name="value">The value<see cref="decimal"/></param>
        /// <param name="upperBound">The upperBound<see cref="decimal"/></param>
        /// <param name="parameterName">The parameterName<see cref="string"/></param>
        public static void ForLowerBound(decimal value, decimal upperBound, string parameterName)
        {
            if (value >= upperBound)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        /// <summary>
        /// Validates DateTimeRange
        /// </summary>
        /// <param name="value">Start Date</param>
        /// <param name="dateToPrecede">End Date</param>
        /// <param name="parameterName">Parameter Name</param>
        public static void ForPrecedesDate(DateTime value, DateTime dateToPrecede, string parameterName)
        {
            if (value >= dateToPrecede)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }
    }
}
