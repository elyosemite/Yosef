using System.Text.Json.Serialization;
using AutoMapper;
using ProjectManagement.Domain.Aggregates;

namespace ProjectManagement.Infrastructure.Model;

public class OrganizationDataModel : ITableObject<int>
{
    public int Id { get; set; }
    public string OrganizationName { get; set; } = null!;
    public int ContributorsCount { get; set; }
    public string? Secret { get; set; }
    public Guid Identifier { get; set; }

    [JsonIgnore]
    public virtual ICollection<ProjectDataModel> Projects { get; set; } = new List<ProjectDataModel>();

    public void UpdateTableRegisterId(int id)
    {
        Id = id;
    }
}

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, OrganizationDataModel>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContributorsCount, opt => opt.MapFrom(src => src.ContributorsCount))
            .ForMember(dest => dest.Secret, opt => opt.MapFrom(src => src.Secret))
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects));

        CreateMap<OrganizationDataModel, Organization>()
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ConstructUsing(src => Organization.OrganizationFactory(src.Identifier, src.OrganizationName, src.ContributorsCount));
    }
}