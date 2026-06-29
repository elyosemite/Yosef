using System.Text.Json.Serialization;
using AutoMapper;
using ProjectManagement.Domain.Aggregates;

namespace ProjectManagement.Infrastructure.Model;

public class BrokerageDataModel : ITableObject<int>
{
    public int Id { get; set; }
    public string BrokerageName { get; set; } = null!;
    public string CNPJ { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
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
            .ForMember(dest => dest.CNPJ, opt => opt.MapFrom(src => src.CNPJ))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects));

        CreateMap<BrokerageDataModel, Brokerage>()
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ConstructUsing(src => Brokerage.BrokerageFactory(src.Identifier, src.BrokerageName, src.CNPJ, src.Email, src.Phone));
    }
}
