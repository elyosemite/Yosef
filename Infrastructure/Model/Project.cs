using AutoMapper;
using DotNetEcosystemStudy.Aggregates;

namespace DotNetEcosystemStudy.Infrastructure.Model;

public class Project : ITableObject<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StarsCount { get; set; }
    public int ForksCount { get; set; }
    public int ContributorsCount { get; set; }
    public Guid Identifier { get; set; }
    public Guid OrganizationIdentifier { get; set; }
    public virtual Organization Organization { get; set; } = null!;
}

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Aggregates.Project, Project>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StarsCount, opt => opt.MapFrom(src => src.StarsCount))
            .ForMember(dest => dest.ForksCount, opt => opt.MapFrom(src => src.ForksCount))
            .ForMember(dest => dest.ContributorsCount, opt => opt.MapFrom(src => src.ContributorsCount))
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier));
    }
}
