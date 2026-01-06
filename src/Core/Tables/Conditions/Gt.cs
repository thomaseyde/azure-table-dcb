using Core.Tables.Fields;

namespace Core.Tables.Conditions;

public record Gt(IField Field) : ICondition;