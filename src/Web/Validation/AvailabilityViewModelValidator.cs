namespace Web.Validation
{
    using FluentValidation;
    using HotelSystem.SharedKernel.ViewModels;

    public class AvailabilityViewModelValidator : AbstractValidator<AvailabilityViewModel>
    {
        public AvailabilityViewModelValidator()
        {
            RuleFor(availabilityViewModel => availabilityViewModel.Arrival).LessThan(availabilityViewModel => availabilityViewModel.Departure);
            RuleFor(availabilityViewModel => availabilityViewModel.Departure).GreaterThan(availabilityViewModel => availabilityViewModel.Arrival);
            RuleFor(availabiltyViewModel => availabiltyViewModel.NumberOfGuests).GreaterThan(0);
            RuleFor(availabilityViewModel => availabilityViewModel.HotelId).NotEmpty();
            RuleFor(availabilityViewModel => availabilityViewModel.HotelName).NotEmpty();
        }
    }
}
