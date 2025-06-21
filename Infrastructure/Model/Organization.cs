using AutoMapper;
using DotNetEcosystemStudy.Aggregates;

namespace DotNetEcosystemStudy.Infrastructure.Model;

public class Organization : ITableObject<Guid>
{
    public Guid Id { get; set; }
    public string OrganizationName { get; set; } = null!;
    public int ContributorsCount { get; set; }
    public string? Secret { get; set; }
    public Guid Identifier { get; set; }
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Aggregates.Organization, Organization>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContributorsCount, opt => opt.MapFrom(src => src.ContributorsCount))
            .ForMember(dest => dest.Secret, opt => opt.MapFrom(src => src.Secret));
    }
}