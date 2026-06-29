using FluentValidation;

namespace ProjectManagement.Presentation.Endpoints.CreateBrokerage;

public class CreateBrokerageValidator : AbstractValidator<BrokerageRequest>
{
    public CreateBrokerageValidator()
    {
        RuleFor(x => x.BrokerageName)
            .NotEmpty().WithMessage("Brokerage name cannot be empty.")
            .MaximumLength(100).WithMessage("Brokerage name cannot exceed 100 characters.");

        RuleFor(x => x.CNPJ)
            .NotEmpty().WithMessage("CNPJ cannot be empty.")
            .Length(14).WithMessage("CNPJ must be exactly 14 digits.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
    }
}
