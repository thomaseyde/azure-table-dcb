using Core.Tables.Fields;

namespace Core.Tables.Conditions;

public record Eq(IField Field) : ICondition;