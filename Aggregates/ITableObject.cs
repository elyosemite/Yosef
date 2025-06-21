namespace DotNetEcosystemStudy.Aggregates;

public interface ITableObject<TId> where TId : IEquatable<TId>
{
    /// <summary>
    /// This identifier represents the unique register in database table.
    /// It is used to map the aggregate root to the database record.
    /// It is not used in the Domain Model, but it is used in the Data Model.
    /// It is not the same as the AggregateRoot identifier.
    /// It is used to identify the record in the database table.
    /// </summary>
    TId Id { get; }
}