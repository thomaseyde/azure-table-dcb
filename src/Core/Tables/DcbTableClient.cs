using Azure;
using Azure.Data.Tables;
using Core.Tables.Conditions;

namespace Core.Tables;

public class DcbTableClient(TableClient tableClient) : IDcbTableClient
{
    public Task<IEnumerable<TableEntity>> Query(ICondition condition, IEnumerable<string> select)
    {
        var entities = GetEntities(select);
        var result = Materialize(entities);
        return result;
    }

    public async Task Add(IEnumerable<TableEntity> entities)
    {
        var transactions = entities.Select(Add).Chunk(100);

        foreach (var transaction in transactions) await tableClient.SubmitTransactionAsync(transaction);
    }

    AsyncPageable<TableEntity> GetEntities(IEnumerable<string> select)
    {
        return tableClient.QueryAsync<TableEntity>("", select: select);
    }

    static async Task<IEnumerable<TableEntity>> Materialize(AsyncPageable<TableEntity> entities)
    {
        List<TableEntity> result = [];

        await foreach (var entity in entities) result.Add(entity);

        return [..result];
    }

    static TableTransactionAction Add(TableEntity e)
    {
        return new TableTransactionAction(TableTransactionActionType.Add, e);
    }
}