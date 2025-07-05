using FluentValidation;

namespace DotNetEcosystemStudy.src.Presentation.Endpoints.CreateOrganization;

public class CreateOrganizationValidator : AbstractValidator<OrganizationRequest>
{
    public CreateOrganizationValidator()
    {
        RuleFor(x => x.OrganizationName)
            .NotEmpty().WithMessage("Organization name cannot be empty.")
            .MaximumLength(50).WithMessage("Organization name cannot exceed 50 characters.");

        RuleFor(x => x.ContributorsCount)
            .GreaterThanOrEqualTo(0).WithMessage("Contributors count must be a non-negative integer.");

        RuleFor(x => x.Secret)
            .NotEmpty().WithMessage("Secret cannot be null or empty.");
    }
}