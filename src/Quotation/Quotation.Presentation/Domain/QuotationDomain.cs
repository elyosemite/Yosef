namespace Quotation.Presentation.Domain;

public class QuotationDomain
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerName { get; set; }
    public string Product { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}