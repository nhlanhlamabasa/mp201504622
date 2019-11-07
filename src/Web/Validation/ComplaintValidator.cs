namespace Web.Validation
{
    using FluentValidation;
    using HotelSystem.SharedKernel.ViewModels;

    public class ComplaintValidator : AbstractValidator<ComplaintViewModel>
    {
        public ComplaintValidator()
        {
            RuleFor(x => x.ComplainType).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
