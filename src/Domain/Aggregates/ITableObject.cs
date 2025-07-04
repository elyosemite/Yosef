namespace DotNetEcosystemStudy.src.Domain.Aggregates;

public interface ITableObject<T> where T : IEquatable<T>
{
    /// <summary>
    /// This identifier represents the unique register in database table.
    /// It is used to map the aggregate root to the database record.
    /// It is not used in the Domain Model, but it is used in the Data Model.
    /// It is not the same as the AggregateRoot identifier.
    /// It is used to identify the record in the database table.
    /// </summary>
    T Id { get; }

    /// <summary>
    /// Updates the table register identifier.
    /// This method is used to set the Id property after the aggregate root has been created.
    /// It is typically called by the repository after the aggregate root has been saved to the database.
    /// </summary>
    void UpdateTableRegisterId(T id);
}