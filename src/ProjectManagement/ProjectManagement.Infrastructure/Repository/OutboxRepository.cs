using System.Text.Json;
using Dapper;
using Npgsql;
using ProjectManagement.Infrastructure.Settings.Interfaces;
using Yosef.ProjectManagement.Domain.Events;
using Yosef.ProjectManagement.Domain.Outbox;

namespace ProjectManagement.Infrastructure.Repository;

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
            Payload = JsonSerializer.Serialize(domainEvent)
        };

        const string sql = @"
            INSERT INTO ""OutboxMessages"" 
            (""Id"", ""OccurredOn"", ""Type"", ""Payload"") 
            VALUES (@Id, @OccurredOn, @Type, @Payload)";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, message, cancellationToken: ct));
    }

    public async Task<IEnumerable<OutboxMessage>> GetPendingAsync(int limit, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT * FROM ""OutboxMessages""
            WHERE ""ProcessedOn"" IS NULL
            ORDER BY ""OccurredOn""
            LIMIT @Limit";

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<OutboxMessage>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
    }

    public async Task MarkProcessedAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = @"UPDATE ""OutboxMessages"" SET ""ProcessedOn"" = @Now WHERE ""Id"" = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, new { Now = DateTime.UtcNow, Id = id }, cancellationToken: ct));
    }

    public async Task MarkErrorAsync(Guid id, string error, CancellationToken ct = default)
    {
        const string sql = @"UPDATE ""OutboxMessages"" SET ""Error"" = @Error WHERE ""Id"" = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, new { Error = error, Id = id }, cancellationToken: ct));
    }
}