using System;
using System.Collections;
using System.Collections.Generic;

namespace NCalc.Domain
{


    public class EvaluationVisitor : LogicalExpressionVisitor
    {
        private delegate T Func<T>();

        private readonly EvaluateOptions _options = EvaluateOptions.None;

        private bool IgnoreCase { get { return (_options & EvaluateOptions.IgnoreCase) == EvaluateOptions.IgnoreCase; } }

        public EvaluationVisitor(EvaluateOptions options)
        {
            _options = options;
        }

        public object Result { get; private set; }

        private object Evaluate(LogicalExpression expression)
        {
            expression.Accept(this);
            return Result;
        }

        public override void Visit(LogicalExpression expression)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private static Type[] CommonTypes = new[] { typeof(Int64), typeof(Double), typeof(Boolean), typeof(String), typeof(Decimal) };

    /// <summary>
        /// Gets the the most precise type.
        /// </summary>
        /// <param name="a">Type a.</param>
        /// <param name="b">Type b.</param>
        /// <returns></returns>
        private static Type GetMostPreciseType(Type a, Type b)
        {
            foreach (Type t in CommonTypes)
            {
                if (a == t || b == t)
                {
                    return t;
                }
            }

            return a;
        }

        public int CompareUsingMostPreciseType(object a, object b)
        {
            Type mpt = GetMostPreciseType(a.GetType(), b.GetType());
            return Comparer.Default.Compare(Convert.ChangeType(a, mpt), Convert.ChangeType(b, mpt));
        }

        public override void Visit(TernaryExpression expression)
        {
            // Evaluates the left expression and saves the value
            expression.LeftExpression.Accept(this);
            bool left = Convert.ToBoolean(Result);

            if (left)
            {
                expression.MiddleExpression.Accept(this);
            }
            else
            {
                expression.RightExpression.Accept(this);
            }
        }

        private static bool IsReal(object value)
        {
            var typeCode = Type.GetTypeCode(value.GetType());

            return typeCode == TypeCode.Decimal || typeCode == TypeCode.Double || typeCode == TypeCode.Single;
        }

        public override void Visit(BinaryExpression expression)
        {
            // simulate Lazy<Func<>> behavior for late evaluation
            object leftValue = null;
            Func<object> left = () =>
                                 {
                                     if (leftValue == null)
                                     {
                                         expression.LeftExpression.Accept(this);
                                         leftValue = Result;
                                     }
                                     return leftValue;
                                 };

            // simulate Lazy<Func<>> behavior for late evaluation
            object rightValue = null;
            Func<object> right = () =>
            {
                if (rightValue == null)
                {
                    expression.RightExpression.Accept(this);
                    rightValue = Result;
                }
                return rightValue;
            };

            switch (expression.Type)
            {
                case BinaryExpressionType.And:
                    Result = Convert.ToBoolean(left()) && Convert.ToBoolean(right());
                    break;

                case BinaryExpressionType.Or:
                    Result = Convert.ToBoolean(left()) || Convert.ToBoolean(right());
                    break;

                case BinaryExpressionType.Div:
                    Result = IsReal(left()) || IsReal(right())
                                 ? Numbers.Divide(left(), right())
                                 : Numbers.Divide(Convert.ToDouble(left()), right());
                    break;

                case BinaryExpressionType.Equal:
                    // Use the type of the left operand to make the comparison
                    Result = CompareUsingMostPreciseType(left(), right()) == 0;
                    break;

                case BinaryExpressionType.Greater:
                    // Use the type of the left operand to make the comparison
                    Result = CompareUsingMostPreciseType(left(), right()) > 0;
                    break;

                case BinaryExpressionType.GreaterOrEqual:
                    // Use the type of the left operand to make the comparison
                    Result = CompareUsingMostPreciseType(left(), right()) >= 0;
                    break;

                case BinaryExpressionType.Lesser:
                    // Use the type of the left operand to make the comparison
                    Result = CompareUsingMostPreciseType(left(), right()) < 0;
                    break;

                case BinaryExpressionType.LesserOrEqual:
                    // Use the type of the left operand to make the comparison
                    Result = CompareUsingMostPreciseType(left(), right()) <= 0;
                    break;

                case BinaryExpressionType.Minus:
                    Result = Numbers.Soustract(left(), right());
                    break;

                case BinaryExpressionType.Modulo:
                    Result = Numbers.Modulo(left(), right());
                    break;

                case BinaryExpressionType.NotEqual:
                    // Use the type of the left operand to make the comparison
                    Result = CompareUsingMostPreciseType(left(), right()) != 0;
                    break;

                case BinaryExpressionType.Plus:
                    if (left() is string)
                    {
                        Result = String.Concat(left(), right());
                    }
                    else
                    {
                        Result = Numbers.Add(left(), right());
                    }

                    break;

                case BinaryExpressionType.Times:
                    Result = Numbers.Multiply(left(), right());
                    break;

                case BinaryExpressionType.BitwiseAnd:
                    Result = Convert.ToUInt16(left()) & Convert.ToUInt16(right());
                    break;


                case BinaryExpressionType.BitwiseOr:
                    Result = Convert.ToUInt16(left()) | Convert.ToUInt16(right());
                    break;


                case BinaryExpressionType.BitwiseXOr:
                    Result = Convert.ToUInt16(left()) ^ Convert.ToUInt16(right());
                    break;


                case BinaryExpressionType.LeftShift:
                    Result = Convert.ToUInt16(left()) << Convert.ToUInt16(right());
                    break;


                case BinaryExpressionType.RightShift:
                    Result = Convert.ToUInt16(left()) >> Convert.ToUInt16(right());
                    break;
            }
        }

        public override void Visit(UnaryExpression expression)
        {
            // Recursively evaluates the underlying expression
            expression.Expression.Accept(this);

            switch (expression.Type)
            {
                case UnaryExpressionType.Not:
                    Result = !Convert.ToBoolean(Result);
                    break;

                case UnaryExpressionType.Negate:
                    Result = Numbers.Soustract(0, Result);
                    break;

                case UnaryExpressionType.BitwiseNot:
                    Result = ~Convert.ToUInt16(Result);
                    break;
            }
        }

        public override void Visit(ValueExpression expression)
        {
            Result = expression.Value;
        }

        public override void Visit(Function function)
        {
            var args = new FunctionArgs
                           {
                               Parameters = new Expression[function.Expressions.Length]
                           };

            // Don't call parameters right now, instead let the function do it as needed.
            // Some parameters shouldn't be called, for instance, in a if(), the "not" value might be a division by zero
            // Evaluating every value could produce unexpected behaviour
            for (int i = 0; i < function.Expressions.Length; i++ )
            {
                args.Parameters[i] =  new Expression(function.Expressions[i], _options);
                args.Parameters[i].EvaluateFunction += EvaluateFunction;
                args.Parameters[i].EvaluateParameter += EvaluateParameter;

                // Assign the parameters of the Expression to the arguments so that custom Functions and Parameters can use them
                args.Parameters[i].Parameters = Parameters;
            }            

            // Calls external implementation
            OnEvaluateFunction(IgnoreCase ? function.Identifier.Name.ToLower() : function.Identifier.Name, args);

            // If an external implementation was found get the result back
            if (args.HasResult)
            {
                Result = args.Result;
                return;
            }

            switch (function.Identifier.Name.ToLower())
            {
                #region Abs
                case "abs":

                    CheckCase("Abs", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Abs() takes exactly 1 argument");

                    Result = Math.Abs(Convert.ToDecimal(
                        Evaluate(function.Expressions[0]))
                        );

                    break;

                #endregion

                #region Acos
                case "acos":

                    CheckCase("Acos", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Acos() takes exactly 1 argument");

                    Result = Math.Acos(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Asin
                case "asin":

                    CheckCase("Asin", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Asin() takes exactly 1 argument");

                    Result = Math.Asin(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Atan
                case "atan":

                    CheckCase("Atan", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Atan() takes exactly 1 argument");

                    Result = Math.Atan(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Ceiling
                case "ceiling":

                    CheckCase("Ceiling", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Ceiling() takes exactly 1 argument");

                    Result = Math.Ceiling(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Cos

                case "cos":

                    CheckCase("Cos", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Cos() takes exactly 1 argument");

                    Result = Math.Cos(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Exp
                case "exp":

                    CheckCase("Exp", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Exp() takes exactly 1 argument");

                    Result = Math.Exp(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Floor
                case "floor":

                    CheckCase("Floor", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Floor() takes exactly 1 argument");

                    Result = Math.Floor(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region IEEERemainder
                case "ieeeremainder":

                    CheckCase("IEEERemainder", function.Identifier.Name);

                    if (function.Expressions.Length != 2)
                        throw new ArgumentException("IEEERemainder() takes exactly 2 arguments");

                    Result = Math.IEEERemainder(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToDouble(Evaluate(function.Expressions[1])));

                    break;

                #endregion

                #region Log
                case "log":

                    CheckCase("Log", function.Identifier.Name);

                    if (function.Expressions.Length != 2)
                        throw new ArgumentException("Log() takes exactly 2 arguments");

                    Result = Math.Log(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToDouble(Evaluate(function.Expressions[1])));

                    break;

                #endregion

                #region Log10
                case "log10":

                    CheckCase("Log10", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Log10() takes exactly 1 argument");

                    Result = Math.Log10(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Pow
                case "pow":

                    CheckCase("Pow", function.Identifier.Name);

                    if (function.Expressions.Length != 2)
                        throw new ArgumentException("Pow() takes exactly 2 arguments");

                    Result = Math.Pow(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToDouble(Evaluate(function.Expressions[1])));

                    break;

                #endregion

                #region Round
                case "round":

                    CheckCase("Round", function.Identifier.Name);

                    if (function.Expressions.Length != 2)
                        throw new ArgumentException("Round() takes exactly 2 arguments");

                    MidpointRounding rounding = (_options & EvaluateOptions.RoundAwayFromZero) == EvaluateOptions.RoundAwayFromZero ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven;

                    Result = Math.Round(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToInt16(Evaluate(function.Expressions[1])), rounding);

                    break;

                #endregion

                #region Sign
                case "sign":

                    CheckCase("Sign", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Sign() takes exactly 1 argument");

                    Result = Math.Sign(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Sin
                case "sin":

                    CheckCase("Sin", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Sin() takes exactly 1 argument");

                    Result = Math.Sin(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Sqrt
                case "sqrt":

                    CheckCase("Sqrt", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Sqrt() takes exactly 1 argument");

                    Result = Math.Sqrt(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Tan
                case "tan":

                    CheckCase("Tan", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Tan() takes exactly 1 argument");

                    Result = Math.Tan(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion

                #region Truncate
                case "truncate":

                    CheckCase("Truncate", function.Identifier.Name);

                    if (function.Expressions.Length != 1)
                        throw new ArgumentException("Truncate() takes exactly 1 argument");

                    Result = Math.Truncate(Convert.ToDouble(Evaluate(function.Expressions[0])));

                    break;

                #endregion
                
                #region Max
                case "max":

                    CheckCase("Max", function.Identifier.Name);

                    if (function.Expressions.Length != 2)
                        throw new ArgumentException("Max() takes exactly 2 arguments");

                    object maxleft = Evaluate(function.Expressions[0]);
                    object maxright = Evaluate(function.Expressions[1]);

                    Result = Numbers.Max(maxleft, maxright);
                    break;

                #endregion

                #region Min
                case "min":

                    CheckCase("Min", function.Identifier.Name);

                    if (function.Expressions.Length != 2)
                        throw new ArgumentException("Min() takes exactly 2 arguments");

                    object minleft = Evaluate(function.Expressions[0]);
                    object minright = Evaluate(function.Expressions[1]);

                    Result = Numbers.Min(minleft, minright);
                    break;

                #endregion

                #region if
                case "if":

                    CheckCase("if", function.Identifier.Name);

                    if (function.Expressions.Length != 3)
                        throw new ArgumentException("if() takes exactly 3 arguments");

                    bool cond = Convert.ToBoolean(Evaluate(function.Expressions[0]));

                    Result = cond ? Evaluate(function.Expressions[1]) : Evaluate(function.Expressions[2]);
                    break;

                #endregion

                #region in
                case "in":

                    CheckCase("in", function.Identifier.Name);

                    if (function.Expressions.Length < 2)
                        throw new ArgumentException("in() takes at least 2 arguments");

                    object parameter = Evaluate(function.Expressions[0]);

                    bool evaluation = false;

                    // Goes through any values, and stop whe one is found
                    for (int i = 1; i < function.Expressions.Length; i++)
                    {
                        object argument = Evaluate(function.Expressions[i]);
                        if (CompareUsingMostPreciseType(parameter, argument) == 0)
                        {
                            evaluation = true;
                            break;
                        }
                    }

                    Result = evaluation;
                    break;

                #endregion

                default:
                    throw new ArgumentException("Function not found", 
                        function.Identifier.Name);
            }
        }

        private void CheckCase(string function, string called)
        {
            if (IgnoreCase)
            {
                if (function.ToLower() == called.ToLower())
                {
                    return;
                }

                throw new ArgumentException("Function not found", called);
            }

            if (function != called)
            {
                throw new ArgumentException(String.Format("Function not found {0}. Try {1} instead.", called, function));
            }
        }

        public event EvaluateFunctionHandler EvaluateFunction;

        private void OnEvaluateFunction(string name, FunctionArgs args)
        {
            if (EvaluateFunction != null)
                EvaluateFunction(name, args);
        }

        public override void Visit(Identifier parameter)
        {
            if (Parameters.ContainsKey(parameter.Name))
            {
                // The parameter is defined in the hashtable
                if (Parameters[parameter.Name] is Expression)
                {
                    // The parameter is itself another Expression
                    var expression = (Expression)Parameters[parameter.Name];

                    // Overloads parameters 
                    foreach (var p in Parameters)
                    {
                        expression.Parameters[p.Key] = p.Value;
                    }

                    expression.EvaluateFunction += EvaluateFunction;
                    expression.EvaluateParameter += EvaluateParameter;

                    Result = ((Expression)Parameters[parameter.Name]).Evaluate();
                }
                else
                    Result = Parameters[parameter.Name];
            }
            else
            {
                // The parameter should be defined in a call back method
                var args = new ParameterArgs();

                // Calls external implementation
                OnEvaluateParameter(parameter.Name, args);

                if (!args.HasResult)
                    throw new ArgumentException("Parameter was not defined", parameter.Name);

                Result = args.Result;
            }
        }

        public event EvaluateParameterHandler EvaluateParameter;

        private void OnEvaluateParameter(string name, ParameterArgs args)
        {
            if (EvaluateParameter != null)
                EvaluateParameter(name, args);
        }

        public Dictionary<string, object> Parameters { get; set; }

    }
}
