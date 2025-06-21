using DotNetEcosystemStudy.Aggregates;

namespace DotNetEcosystemStudy.Infrastructure.Model;

internal sealed class Organization : ITableObject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ContributorsCount { get; set; }
    public string? Secret { get; set; }
    public Guid Identifier { get; private set; }
}