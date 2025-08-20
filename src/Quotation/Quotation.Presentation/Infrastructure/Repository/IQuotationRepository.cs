using AutoMapper;
using Quotation.Infrastructure.Context;
using Quotation.Presentation.Domain;
using Quotation.Presentation.Infrastructure.Model;


namespace Quotation.Presentation.Infrastructure;

public interface IQuotationRepository
{
    Task<QuotationDomain> CreateAsync(QuotationDomain quotation);
    Task<QuotationDomain?> GetByIdAsync(Guid id);
    Task UpdateAsync(QuotationDomain quotation);
    Task DeleteAsync(Guid id);
}

public class QuotationRepository : IQuotationRepository
    {
        private readonly QuotationContext _context;
        private readonly IMapper _mapper;

    public QuotationRepository(QuotationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

        public async Task<QuotationDomain> CreateAsync(QuotationDomain quotation)
    {
        // converte aqui ...
        var dataModel = _mapper.Map<QuotationDataModel>(quotation);
        _context.Quotation.Add(dataModel);
        await _context.SaveChangesAsync();
        return quotation;
    }

        public async Task<QuotationDomain?> GetByIdAsync(Guid id)
        {
            var dataModel = await _context.Quotation.FindAsync(id);
            var domain = _mapper.Map<QuotationDomain>(dataModel);
            return domain;
        }

        public async Task UpdateAsync(QuotationDomain quotation)
        {
            var dataModel = _mapper.Map<QuotationDataModel>(quotation);
            _context.Quotation.Update(dataModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var quotation = await _context.Quotation.FindAsync(id);
            if (quotation != null)
            {
                _context.Quotation.Remove(quotation);
                await _context.SaveChangesAsync();
            }
        }
    }