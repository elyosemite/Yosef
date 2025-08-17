using FluentValidation;

namespace Yosef.ProjectManagement.Application.UpdateOrganizationName.Organization;

public class UpdateOrganizationNameValidator : AbstractValidator<UpdateOrganizationNameRequest>
{
    public UpdateOrganizationNameValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("Organization identifier is required.");
        RuleFor(x => x.OrganizationName).NotEmpty().WithMessage("Organization name is required.");
    }
}
