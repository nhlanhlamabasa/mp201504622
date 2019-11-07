namespace Web.Validation
{
    using FluentValidation;
    using HotelSystem.SharedKernel.ViewModels;

    public class SelectRoomsValidator : AbstractValidator<BookingViewModel>
    {
        public SelectRoomsValidator(int NumberOfBeds)
        {
            RuleFor(x => x.Arrival).LessThan(x => x.Departure);
            RuleFor(x => x.Departure).GreaterThan(x => x.Arrival);
            RuleFor(x => x.HotelId).NotEmpty();
            RuleFor(x => x.HotelName).NotEmpty();
            RuleFor(x => x.NumberOfGuests).Equal(NumberOfBeds).WithMessage("Number of guests must equal number of selected rooms.");
            RuleFor(x => x.NumSelectedRooms).LessThanOrEqualTo(0).WithMessage("At least one room is required.");
        }
    }
}
