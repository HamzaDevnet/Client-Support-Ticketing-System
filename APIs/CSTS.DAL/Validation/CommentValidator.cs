using FluentValidation;
using CSTS.DAL.Models;

namespace CSTS.DAL.Validation
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .Length(1, 1000).WithMessage("Content must be between 1 and 1000 characters.");

            RuleFor(x => x.CreatedDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Created date must be in the past or present.");

            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("TicketId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
