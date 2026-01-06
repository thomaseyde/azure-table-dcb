using Core.Tables.Conditions;
using Core.Tables.Fields;

namespace Core.Tables;

public static class LogicalExtensions
{
    extension(PartitionKey)
    {
        public static Eq Eq(string value)
        {
            return new Eq(new PartitionKey(value));
        }
    }

    extension(RowKey)
    {
        public static Gt Gt(string value)
        {
            return new Gt(new RowKey(value));
        }
    }

    extension(ICondition condition)
    {
        public And And(ICondition other)
        {
            return new And(condition, other);
        }
    }
}