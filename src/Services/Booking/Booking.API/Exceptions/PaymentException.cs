namespace Booking.API.Exceptions
{
#pragma warning disable
    using System;

    public class PaymentException : Exception
    {
        public PaymentException()
        {
        }

        public PaymentException(string message)
            : base(message)
        {
        }

        public PaymentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
