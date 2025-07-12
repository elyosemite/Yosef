using AutoMapper;
using ProjectManagement.Domain.Aggregates;

namespace ProjectManagement.Infrastructure.Model;

public class ProjectDataModel : ITableObject<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StarsCount { get; set; }
    public int ForksCount { get; set; }
    public int ContributorsCount { get; set; }
    public Guid Identifier { get; set; }
    public Guid OrganizationIdentifier { get; set; }
    public virtual OrganizationDataModel Organization { get; set; } = null!;

    public void UpdateTableRegisterId(int id)
    {
        Id = id;
    }
}

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDataModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StarsCount, opt => opt.MapFrom(src => src.StarsCount))
            .ForMember(dest => dest.ForksCount, opt => opt.MapFrom(src => src.ForksCount))
            .ForMember(dest => dest.ContributorsCount, opt => opt.MapFrom(src => src.ContributorsCount))
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.OrganizationIdentifier, opt => opt.MapFrom(src => src.OrganizationIdentifier));
    }
}
