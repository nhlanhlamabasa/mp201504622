namespace Web.Validation
{
    using FluentValidation;
    using HotelSystem.SharedKernel.ViewModels;
    using System;

    public class ComplaintRatingValidator : AbstractValidator<ComplaintViewModel>
    {
        public ComplaintRatingValidator()
        {
            RuleFor(x => x.BookingId).NotEqual(Guid.Empty);
            RuleFor(x => x.LevelOfSatisfaction).NotEmpty();
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}
