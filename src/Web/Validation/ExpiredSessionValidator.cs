namespace Web.Validation
{
    using FluentValidation;
    using HotelSystem.SharedKernel.ViewModels;
    using System;

    public class ExpiredSessionValidator : AbstractValidator<BookingViewModel>
    {
        public ExpiredSessionValidator()
        {
            RuleFor(x => x.Arrival).GreaterThan(DateTime.MinValue);
            RuleFor(x => x.Departure).GreaterThan(DateTime.MinValue);
            RuleFor(x => x.HotelId).NotEmpty();
            RuleFor(x => x.HotelName).NotEmpty();
            RuleFor(x => x.NumberOfGuests).GreaterThan(0);
        }
    }
}
