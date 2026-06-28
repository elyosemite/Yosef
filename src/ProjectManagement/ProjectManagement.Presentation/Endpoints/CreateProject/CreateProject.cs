using ProjectManagement.Application.Repository;
using ProjectManagement.Domain.Aggregates;

namespace ProjectManagement.Presentation.Endpoints.CreateProject;

public class CreateProject
{
    private readonly IBrokerageRepository _brokerageRepository;

    public CreateProject(IBrokerageRepository brokerageRepository)
    {
        _brokerageRepository = brokerageRepository;
    }

    public async Task<IResult> ActionAsync(CreateProjectRequest req)
    {
        var brokerage = await _brokerageRepository.GetByIdAsync(req.OrganizationIdentifier);

        if (brokerage is null)
            return TypedResults.NotFound($"Brokerage with Id {req.OrganizationIdentifier} not found.");

        var project = Project.ProjectFactory(
            req.ProjectName,
            req.Description,
            req.StarsCount,
            req.ForksCount,
            req.ContributorsCount,
            brokerage.Identifier);

        brokerage.AddProject(project);
        await _brokerageRepository.UpsertAsync(brokerage);

        var result = await _brokerageRepository.GetByIdAsync(req.OrganizationIdentifier);
        return TypedResults.Ok(result);
    }
}