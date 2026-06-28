using System.Text.Json.Serialization;
using AutoMapper;
using ProjectManagement.Domain.Aggregates;

namespace ProjectManagement.Infrastructure.Model;

public class BrokerageDataModel : ITableObject<int>
{
    public int Id { get; set; }
    public string BrokerageName { get; set; } = null!;
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

public class BrokerageProfile : Profile
{
    public BrokerageProfile()
    {
        CreateMap<Brokerage, BrokerageDataModel>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.BrokerageName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContributorsCount, opt => opt.MapFrom(src => src.ContributorsCount))
            .ForMember(dest => dest.Secret, opt => opt.MapFrom(src => src.Secret))
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects));

        CreateMap<BrokerageDataModel, Brokerage>()
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ConstructUsing(src => Brokerage.BrokerageFactory(src.Identifier, src.BrokerageName, src.ContributorsCount));
    }
}
