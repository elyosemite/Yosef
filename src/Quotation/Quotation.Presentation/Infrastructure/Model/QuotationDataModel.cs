using AutoMapper;
using Quotation.Presentation.Domain;

namespace Quotation.Presentation.Infrastructure.Model;

public class QuotationDataModel
{
    public Guid QuotationId { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class QuotationProfile : Profile
{
    public QuotationProfile()
    {
        CreateMap<QuotationDomain, QuotationDataModel>()
            .ForMember(dest => dest.QuotationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<QuotationDataModel, QuotationDomain>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuotationId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}