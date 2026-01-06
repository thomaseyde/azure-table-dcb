using Azure.Data.Tables;
using Core.Tables.Conditions;

namespace Core.Tables;

public interface IDcbTableClient
{
    Task<IEnumerable<TableEntity>> Query(ICondition condition, IEnumerable<string> select);
    Task Add(IEnumerable<TableEntity> entities);
}