#pragma warning disable

namespace Booking.API.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TravelDatesOverlapException : Exception
    {
        public TravelDatesOverlapException()
        {
        }

        public TravelDatesOverlapException(string message)
            : base(message)
        {
        }

        public TravelDatesOverlapException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
