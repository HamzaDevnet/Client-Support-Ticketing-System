using FluentValidation;
using CSTS.DAL.Models;
using CSTS.DAL.Enum;

namespace CSTS.DAL.Validation
{
    public class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(x => x.Product)
                .NotEmpty().WithMessage("Product is required.")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

            RuleFor(x => x.ProblemDescription)
                .NotEmpty().WithMessage("Problem description is required.")
                .Length(10, 1000).WithMessage("Problem description must be between 10 and 1000 characters.");

            RuleFor(x => x.Attachments)
                .NotEmpty().WithMessage("Attachments are required.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid ticket status.");

            RuleFor(x => x.CreatedDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Created date must be in the past or present.");

            RuleFor(x => x.ModifiedDate)
                .GreaterThanOrEqualTo(x => x.CreatedDate).WithMessage("Modified date must be greater than or equal to created date.");
        }
    }
}
