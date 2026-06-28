using FluentValidation;

namespace ProjectManagement.Presentation.Endpoints.CreateBrokerage;

public class CreateBrokerageValidator : AbstractValidator<BrokerageRequest>
{
    public CreateBrokerageValidator()
    {
        RuleFor(x => x.BrokerageName)
            .NotEmpty().WithMessage("Brokerage name cannot be empty.")
            .MaximumLength(50).WithMessage("Brokerage name cannot exceed 50 characters.");

        RuleFor(x => x.ContributorsCount)
            .GreaterThanOrEqualTo(0).WithMessage("Contributors count must be a non-negative integer.");

        RuleFor(x => x.Secret)
            .NotEmpty().WithMessage("Secret cannot be null or empty.");
    }
}
