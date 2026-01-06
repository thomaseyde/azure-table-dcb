namespace Core.Tables.Conditions;

public record And(ICondition Left, ICondition Right) : ICondition;