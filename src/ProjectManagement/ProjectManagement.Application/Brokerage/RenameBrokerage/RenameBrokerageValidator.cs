using FluentValidation;

namespace Yosef.ProjectManagement.Application.Brokerage.RenameBrokerage;

public class RenameBrokerageValidator : AbstractValidator<RenameBrokerageRequest>
{
    public RenameBrokerageValidator()
    {
        RuleFor(x => x.BrokerageId).NotEmpty().WithMessage("Brokerage identifier is required.");
        RuleFor(x => x.BrokerageName).NotEmpty().WithMessage("Brokerage name is required.");
    }
}
