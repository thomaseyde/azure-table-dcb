using Azure.Data.Tables;
using Core.Tables;
using Core.Tables.Conditions;
using Core.Tables.Fields;

namespace Core.Testing;

public class DcbTableClientStub : IDcbTableClient
{
    readonly List<TableEntity> allEntities = [];

    public async Task Add(IEnumerable<TableEntity> entities)
    {
        await Task.CompletedTask;
        allEntities.AddRange(entities);
    }

    public async Task<IEnumerable<TableEntity>> Query(ICondition condition, IEnumerable<string> select)
    {
        await Task.CompletedTask;
        return allEntities.Where(e => Evaluate(e, condition));
    }

    static bool Evaluate(TableEntity e, ICondition condition)
    {
        return condition switch
        {
            All => true,
            And t => And(e, t),
            Gt t => Gt(e, t.Field),
            Eq t => Eq(e, t.Field),
            _ => throw new NotSupportedException()
        };
    }

    static bool And(TableEntity e, And t)
    {
        return Evaluate(e, t.Left) && Evaluate(e, t.Right);
    }

    static bool Eq(TableEntity e, IField f)
    {
        return f switch
        {
            PartitionKey t => e.PartitionKey.CompareTo(t.Value, StringComparison.InvariantCultureIgnoreCase) == 0,
            _ => throw new NotSupportedException()
        };
    }

    static bool Gt(TableEntity e, IField f)
    {
        return f switch
        {
            PartitionKey t => e.PartitionKey.CompareTo(t.Value, StringComparison.InvariantCultureIgnoreCase) == 1,
            RowKey t => e.RowKey.CompareTo(t.Value, StringComparison.InvariantCultureIgnoreCase) == 1,
            _ => throw new NotSupportedException()
        };
    }
}