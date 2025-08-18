using System.Text.Json;
using Dapper;
using EventProcessor.Settings.Interfaces;
using Npgsql;

namespace EventProcessor.Repository;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly string _connectionString;

    public OutboxRepository(IGlobalSettings globalSettings)
    {
        _connectionString = globalSettings.PostgreSql.ConnectionString
                            ?? throw new ArgumentNullException(nameof(globalSettings.PostgreSql.ConnectionString), "Outbox connection string cannot be null.");
    }

    public async Task AddAsync(DomainEventBase domainEvent, CancellationToken ct = default)
    {
        var message = new OutboxMessage
        {
            OccurredOn = DateTime.UtcNow,
            Type = domainEvent.GetType().AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
        };

        const string sql = @"
            INSERT INTO ""OutboxMessage"" 
            (""Id"", ""OccurredOn"", ""Type"", ""Payload"") 
            VALUES (@Id, @OccurredOn, @Type, @Payload)";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, message, cancellationToken: ct));
    }

    public async Task<IEnumerable<OutboxMessage>> GetPendingAsync(int limit, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT * FROM ""OutboxMessage""
            WHERE ""ProcessedOn"" IS NULL
            ORDER BY ""OccurredOn""
            LIMIT @Limit";

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<OutboxMessage>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
    }

    public async Task MarkProcessedAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = @"UPDATE ""OutboxMessage"" SET ""ProcessedOn"" = @Now WHERE ""Id"" = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, new { Now = DateTime.UtcNow, Id = id }, cancellationToken: ct));
    }

    public async Task MarkOccurredAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = @"UPDATE ""OutboxMessage"" SET ""Occured"" = @Now WHERE ""Id"" = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, new { Now = DateTime.UtcNow, Id = id }, cancellationToken: ct));
    }

    public async Task MarkErrorAsync(Guid id, string error, CancellationToken ct = default)
    {
        const string sql = @"UPDATE ""OutboxMessage"" SET ""Error"" = @Error WHERE ""Id"" = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, new { Error = error, Id = id }, cancellationToken: ct));
    }
}