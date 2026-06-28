using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using ProjectManagement.Applciation.Repository;
using ProjectManagement.Application.Repository;
using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Domain.Events;

namespace Yosef.ProjectManagement.Application.Brokerage.RenameBrokerage;

public class RenameBrokerageHandler : IRequestHandler<RenameBrokerageRequest, RenameBrokerageResponse>
{
    private readonly ILogger<RenameBrokerageHandler> _logger;
    private readonly IValidator<RenameBrokerageRequest> _validator;
    private readonly IBrokerageRepository _brokerageRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public RenameBrokerageHandler(
        ILogger<RenameBrokerageHandler> logger,
        IValidator<RenameBrokerageRequest> validator,
        IBrokerageRepository brokerageRepository,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _logger = logger;
        _validator = validator;
        _brokerageRepository = brokerageRepository;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async ValueTask<RenameBrokerageResponse> Handle(RenameBrokerageRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Renaming brokerage {BrokerageId} to '{BrokerageName}'", request.BrokerageId, request.BrokerageName);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _logger.LogWarning("Validation failed for {Property}: {Error}", error.PropertyName, error.ErrorMessage);

            throw new ValidationException(validationResult.Errors);
        }

        var brokerage = await _brokerageRepository.GetByIdAsync(request.BrokerageId);
        if (brokerage == null)
        {
            _logger.LogWarning("Brokerage {BrokerageId} not found", request.BrokerageId);
            throw new KeyNotFoundException($"Brokerage with ID {request.BrokerageId} not found");
        }

        brokerage.UpdateName(request.BrokerageName);
        await _brokerageRepository.UpsertAsync(brokerage);

        await _domainEventDispatcher.DispatchEventAsync(new List<EntityBase<Guid>> { brokerage });

        _logger.LogInformation("Brokerage {BrokerageId} renamed to '{BrokerageName}'", brokerage.Identifier, brokerage.Name);

        return new RenameBrokerageResponse
        {
            BrokerageId = brokerage.Identifier,
            BrokerageName = brokerage.Name
        };
    }
}
