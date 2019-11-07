namespace Web.Validation
{
    using FluentValidation;
    using HotelSystem.SharedKernel.ViewModels;

    public class BookingSubmitValidator : AbstractValidator<BookingViewModel>
    {
        public BookingSubmitValidator()
        {
            RuleFor(x => x.Arrival).LessThan(x => x.Departure);
            RuleFor(x => x.Departure).GreaterThan(x => x.Arrival);
            RuleFor(x => x.BookerId).NotEmpty();
            RuleFor(x => x.HotelId).NotEmpty();
            RuleFor(x => x.HotelName).NotEmpty();
            RuleFor(x => x.NumberOfGuests).GreaterThan(0);
            RuleFor(x => x.SelectedRooms.Count).GreaterThan(0);
            RuleFor(x => x.NumSelectedRooms).Equal(x => x.SelectedRooms.Count);
            RuleFor(x => x.Status).NotEmpty();
        }
        
    }
}
