using FluentValidation;
using CSTS.DAL.AutoMapper.DTOs;

public class CreateCommentDTOValidator : AbstractValidator<CreateCommentDTO>
{
    public CreateCommentDTOValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content cannot be empty.")
            .Length(1, 1000).WithMessage("Content must be between 1 and 1000 characters.");

        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("TicketId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
