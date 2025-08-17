using Mediator;

namespace Yosef.ProjectManagement.Application.UpdateOrganizationName.Organization;

public class UpdateOrganizationNameRequest : IRequest<UpdateOrganizationNameResponse>
{
    public Guid OrganizationId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
}
