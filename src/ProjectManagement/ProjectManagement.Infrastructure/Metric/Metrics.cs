using System.Diagnostics.Metrics;

namespace ProjectManagement.Infrastructure.Metric;

public static class Metrics
{
    public static readonly Meter GreeterMeter = new Meter("OtPrGrYa.Example", "1.0.0");
    public static readonly Meter CreateOrganizationMeter = new Meter("OtPrCrOr.Example", "1.0.0");

    public static readonly Counter<long> CountGreetings = GreeterMeter.CreateCounter<long>("greetings.count", description: "Counts the number of greetings");
    public static readonly Counter<long> CountOrganizationsCreated = CreateOrganizationMeter.CreateCounter<long>("organizations.created.count", description: "Counts the number of organizations created");
}