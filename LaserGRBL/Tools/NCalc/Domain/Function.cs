namespace NCalc.Domain
{
	public class Function : LogicalExpression
	{
		public Function(Identifier identifier, LogicalExpression[] expressions)
		{
            Identifier = identifier;
            Expressions = expressions;
		}

	    public Identifier Identifier { get; set; }

	    public LogicalExpression[] Expressions { get; set; }

	    public override void Accept(LogicalExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
