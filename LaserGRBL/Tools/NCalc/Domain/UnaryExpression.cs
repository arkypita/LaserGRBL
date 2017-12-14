namespace NCalc.Domain
{
	public class UnaryExpression : LogicalExpression
    {
		public UnaryExpression(UnaryExpressionType type, LogicalExpression expression)
		{
            Type = type;
            Expression = expression;
		}

	    public LogicalExpression Expression { get; set; }

	    public UnaryExpressionType Type { get; set; }

	    public override void Accept(LogicalExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
	}

	public enum UnaryExpressionType
	{
		Not,
        Negate,
        BitwiseNot
	}
}
