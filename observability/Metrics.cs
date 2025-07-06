using System.Diagnostics.Metrics;

namespace DotNetEcosystemStudy.Observability;

public static class Metrics
{
    public static readonly Meter GreeterMeter = new Meter("OtPrGrYa.Example", "1.0.0");
    public static readonly Meter CreateOrganizationMeter = new Meter("OtPrCrOr.Example", "1.0.0");

    public static readonly Counter<int> CountGreetings = GreeterMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");
    public static readonly Counter<int> CountOrganizationsCreated = CreateOrganizationMeter.CreateCounter<int>("organizations.created.count", description: "Counts the number of organizations created");
}