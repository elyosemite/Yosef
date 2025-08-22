using System.Diagnostics.Metrics;

namespace Quotation.Infrastructure.Metric;

public static class Metrics
{
    public static readonly Meter QuotationMeter = new Meter("Quotation", "1.0.0");

    public static readonly Counter<long> CountQuotation = QuotationMeter.CreateCounter<long>("quotation.count", description: "Counts the number of quotations");
}