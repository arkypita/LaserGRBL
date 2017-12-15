using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Collections.ObjectModel;
using System.Text;

namespace Tools
{
	/// <summary>
	/// Evaluatable mathmatical function.
	/// </summary>
	/// <remarks><pre>
	/// 19 Jul 2004 - Jeremy Roberts
	/// Takes a string and allow a user to add variables. Evaluates the string as a mathmatical function.
	/// 11 Jan 2006 - Will Gray
	///  - Added generic collection implementation.
	/// </pre></remarks>
	[Serializable()]
	public class Expression : MarshalByRefObject
	{
		/// <summary>
		/// Dynaamic function type.
		/// </summary>
		/// <remarks><pre>
		/// 20 Dec 2005 - Jeremy Roberts
		/// </pre></remarks>
		[Serializable()]
		public abstract class DynamicFunction
		{
			public abstract double EvaluateD(Dictionary<string, double> variables);
			public abstract bool EvaluateB(Dictionary<string, double> variables);
		}
		protected DynamicFunction dynamicFunction;
		//protected AppDomain NewAppDomain;

		#region Class variable
		//class Variable
		//{
		//    public string name;
		//    public double val;
		//}
		#endregion

		#region Member data
		protected string inFunction = string.Empty; // Infix function.
		protected string postFunction = string.Empty; // Postfix function.
		protected Dictionary<string, double> variables = new Dictionary<string, double>();
		protected const double TRUE = 1;
		protected const double FALSE = 0;
		protected string[] splitPostFunction;
		private bool compilecode = false;
		#endregion

		~Expression()
		{
			//NewAppDomain.;
		}

		#region Constructors
		/// <summary>
		/// Creation constructor.
		/// </summary>
		/// <remarks><pre>
		/// 20 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public Expression()
		{
		}

		/// <summary>
		/// Creation constructor.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="function">The function to be evaluated.</param>
		public Expression(string function)
		{
			this.Function = function;
		}

		///// <summary>
		///// Copy constructor.
		///// </summary>
		///// <remarks><pre>
		///// 19 Jul 2004 - Jeremy Roberts
		///// </pre></remarks>
		///// <param name="function">The function to be evaluated.</param>
		//public Expression(Expression cloneMe)
		//{
		//    this.Function = cloneMe.Function;
		//    foreach (string key in cloneMe.variables.Keys)
		//    {
		//        this.AddSetVariable(key, (double)cloneMe.variables[key]);
		//    }
		//}
		#endregion

		#region properties
		/// <summary>
		/// InFix property
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public string Function
		{
			get { return this.inFunction; }
			set
			{
				// This will throw an error if it does not validate.
				this.Validate(value);

				// Function is valid.
				this.inFunction = value;
				this.postFunction = this.Infix2Postfix(this.inFunction);
				this.splitPostFunction = postFunction.Split(new char[] { ' ' });
				this.ClearVariables();
				if (this.compilecode)
					this.compile();
			}
		}

		/// <summary>
		/// PostFix property
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public string PostFix
		{
			get { return postFunction; }
		}

		/// <summary>
		/// PostFix property
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public string InFix
		{
			get { return this.Expand(inFunction); }
		}

		public bool Compile
		{
			get { return this.compilecode; }
			set { this.compilecode = value; }
		}
		#endregion

		/// <summary>
		/// Convecs an infix string to a post fix string.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="func">The function to convert</param>
		/// <returns>A post fix string.</returns>
		protected string Infix2Postfix(string func)
		{
			func = this.Expand(func);

			string[] inFix = func.Split(new char[] { ' ' });

			Stack<string> postFix = new Stack<string>();

			//          inFix = evaluateLogic(inFix);

			Stack<string> operators = new Stack<string>();
			string currOperator;

			foreach (string token in inFix)
			{
				// If the token is an operand
				if (this.IsOperand(token))
				{
					//push on the postfix vector
					postFix.Push(token);
				}
				// If the token is a "("
				else if (token == "(")
				{
					//push on the operatorVector
					operators.Push(token);
				}
				// If the token is a ")"
				else if (token == ")")
				{
					// pop the operatorVector and store operator
					currOperator = operators.Pop();

					// while operator is not a "("
					while (currOperator != "(")
					{
						// push the operator on the postfixVector
						postFix.Push(currOperator);
						// pop the operatorVector and store operator
						currOperator = operators.Pop();
					}
				}
				// If the token is an operator
				else if (this.IsOperator(token))
				{
					// while precedence of the operator is <= precedence of the token
					while (operators.Count > 0)
					{
						if (this.GetPrecedence(token) <= this.GetPrecedence(operators.Peek()))
						{
							// pop the operatorVector and store operator
							currOperator = operators.Pop();
							//push operator on the postfix vector
							postFix.Push(currOperator);
						}
						else
							break;
					}
					//push token on the postfix vector
					operators.Push(token);
				}
			}
			// while operatorVector is not empty
			while (operators.Count > 0)
			{
				// pop the operatorVector and store operator
				currOperator = operators.Pop();
				//push operator on the postfix vector
				postFix.Push(currOperator);
			}

			// Build the post fix string.
			string psString = string.Empty;
			foreach (string item in postFix)
			{
				psString = item + " " + psString;
			}
			psString = psString.Trim();

			return psString;
		}

		/// <summary>
		/// Checks to see if a string is an operand.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">String to check</param>
		/// <returns></returns>
		protected bool IsOperand(string token)
		{
			if (!this.IsOperator(token) &&
				token != "(" &&
				token != ")" &&
				token != "{" &&
				token != "}")
				return true;
			else
				return false;
		}

		/// <summary>
		/// Checks to see if a string is an operator.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">String to check</param>
		/// <returns></returns>
		protected bool IsOperator(string token)
		{
			if (token == "+" ||
				token == "-" ||
				token == "*" ||
				token == "/" ||
				token == "^" ||
				token == "&&" ||
				token == "||" ||
				token == "sign" ||
				token == "abs" ||
				token == "neg" ||
				token == "==" ||
				token == ">=" ||
				token == "<=" ||
				token == ">" ||
				token == "<" ||
				token == "!=")
				return true;
			else
				return false;
		}

		/// <summary>
		/// Expandn the spaces around operators and operands.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="function">Function to expand.</param>
		/// <returns></returns>
		public string Expand(string function)
		{
			// Clean the function.
			function = Regex.Replace(function, @"[ ]+", @"");
			function = function.Replace("(", " ( ");
			function = function.Replace(")", " ) ");
			function = function.Replace("+", " + ");
			function = function.Replace("-", " - ");
			function = function.Replace("*", " * ");
			function = function.Replace("/", " / ");
			function = function.Replace("^", " ^ ");
			function = function.Replace("||", " || ");
			function = function.Replace("&&", " && ");
			function = function.Replace("==", " == ");
			function = function.Replace(">=", " >= ");
			function = function.Replace("<=", " <= ");
			//function = function.Replace("<", " < ");
			function = Regex.Replace(function, @"<([^=]|$)", @" < $1");
			//function = function.Replace(">", " > ");
			function = Regex.Replace(function, @">([^=]|$)", @" > $1");
			function = function.Replace("!=", " != ");
			function = function.Replace("sign", " sign ");
			function = function.Replace("abs", " abs ");
			function = function.Replace("neg", " neg ");
			function = function.Trim();
			function = Regex.Replace(function, @"[ ]+", @" ");

			// Find and correct for scientific notation
			function = Expression.ScientificNotationCorrection(function);

			// Fix negative real values. Ex:  "1 + - 2" = "1 + -2". "1 + - 2e-1" = "1 + -2e-1".
			function = Regex.Replace(
				function,
				@"([<>=/*+(-] -|sign -|^-) (\d+|[0-9]+[eE][+-]\d+)(\s|$)",
				@"$1$2$3");

			return function;
		}

		// Corrects for spaced function...
		/// <summary>
		/// Corrects scientific notation in an expression.
		/// </summary>
		/// <param name="function">The expanded function to check.</param>
		/// <returns>The corrected expanded function</returns>
		public static string ScientificNotationCorrection(string function)
		{
			char[] ops = { '-', '+' };
			int n = function.IndexOfAny(ops, 0);
			while ((n <= function.Length) && (n > -1))
			{
				// Find the previous space.
				int prevCut = -1;
				int nextCut = -1;

				if (n - 2 <= 0)
					prevCut = 0;
				else
					prevCut = function.LastIndexOf(" ", n - 2, n - 2) + 1;
				//prevCut = n-2 <= 0 ? 0 : function.LastIndexOf(" ", n-2, n-2) + 1;
				//prevSpace = function.LastIndexOf(" ", n-2, n-2);

				if (n + 2 < function.Length)
					nextCut = function.IndexOf(" ", n + 2);
				else
					nextCut = function.Length;
				nextCut = (nextCut == -1 ? function.Length : nextCut);

				string checkMeSpace = function.Substring(prevCut, nextCut - prevCut);
				string checkMe = checkMeSpace.Replace(" ", string.Empty);

				bool realValue = false;
				double val = Double.NaN;
				try
				{
					val = Double.Parse(checkMe, System.Globalization.CultureInfo.InvariantCulture);
					realValue = true;
				}
				catch { }

				if (realValue)
				{
					function = function.Replace(checkMeSpace, checkMe);
					//n = n - checkMeSpace.Length + checkMe.Length;
					n = prevCut + checkMe.Length - 1;
				}

				n = function.IndexOfAny(ops, n + 1);

			}

			return function;
		}

		/// <summary>
		/// Returns the precedance of an operator.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">Token to check.</param>
		/// <returns></returns>
		protected int GetPrecedence(string token)
		{
			if (
				token == "+" ||
				token == "-" ||
				token == "||")
				return 1;
			else if (
				token == "*" ||
				token == "/" ||
				token == "&&")
				return 2;
			else if (
				token == "==" ||
				token == ">=" ||
				token == "<=" ||
				token == ">" ||
				token == "<" ||
				token == "!=" ||
				token == "sign" ||
				token == "^")
				return 3;
			else if (
				token == "abs" ||
				token == "neg")
				return 4;
			else
				return 0;
		}

		/// <summary>
		/// Adds or sets a Variable.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="name">Variable name.</param>
		/// <param name="value">Variabale value.</param>
		public void AddSetVariable(string name, double val)
		{
			variables[name] = val;
		}

		/// <summary>
		/// Adds or sets a Variable.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="name">Variable name.</param>
		/// <param name="value">Variabale value.</param>
		public void AddSetVariable(string name, bool val)
		{
			double dval;
			if (val)
				dval = 1;
			else
				dval = 0;

			variables[name] = dval;
		}

		/// <summary>
		/// Clears the variable information.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public void ClearVariables()
		{
			variables.Clear();
		}

		/// <summary>
		/// Clears all information.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public void Clear()
		{
			inFunction = string.Empty;
			postFunction = string.Empty;
			variables.Clear();
		}

		/// <summary>
		/// Checks to see if a string is a variable.
		/// </summary>
		/// <remarks><pre>
		/// 20 Jul 2004 - Jeremy Roberts
		/// 11 Aug 2004 - Jeremy Roberts
		///  Changed to check to see if it is a number.
		/// </pre></remarks>
		/// <param name="token">String to check.</param>
		/// <returns></returns>
		protected bool IsVariable(string token)
		{
			//if (isOperator(token))
			//    return false;

			if (token == "true" || token == "false")
				return false;

			if (!this.IsOperand(token))
				return false;

			if (this.IsNumber(token))
				return false;

			return true;
		}

		public ReadOnlyCollection<string> FunctionVariables
		{
			get
			{
				// Check to see that the function is valid.
				if (this.inFunction.Equals(string.Empty) || this.inFunction == null)
					throw new Exception("Function does not exist");

				// Expand the function.
				string func = this.Expand(inFunction);

				// Tokenize the funcion.
				string[] inFix = func.Split(new char[] { ' ' });

				// The arraylist to return
				List<string> retVal = new List<string>();

				// Check each token to see if its a variable.
				foreach (string token in inFix)
				{
					if (this.IsVariable(token))
						retVal.Add(token);
				}

				return retVal.AsReadOnly();
			}
		}

		/// <summary>
		/// Returns a variable's value.
		/// </summary>
		/// <remarks><pre>
		/// 20 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">Variable to return.</param>
		/// <returns></returns>
		public double GetVariableValue(string token)
		{
			try
			{
				return variables[token];
			}
			catch
			{
				return double.NaN;
			}
		}

		protected bool Validate(string inFix)
		{
			string inFixClean = this.Expand(inFix);
			string[] func = inFixClean.Split(new char[] { ' ' });

			// Check parenthesis
			int parCount = 0;
			for (int i = 0; i < func.Length; i++)
			{
				// Make sure the abs function is formatted correctly.
				if (func[i] == "abs")
				{
					if (i == func.Length - 1)
						throw new Exception("Operator error! abs does not have a \"(\"" + inFix);
					else if ((string)func[i + 1] != "(")
						throw new Exception("Operator error! abs does not have a \"(\"" + inFix);
				}

				// Make sure the neg function is formatted correctly.
				if (func[i] == "neg")
					if (i != 0)
						if (this.IsOperand(func[i - 1]))
							throw new Exception("Operator error! neg used improperly." + inFix);

				if (i > 0 && i < func.Length - 1)
					if (func[i] == "(" || func[i] == ")")
						if (this.IsOperand(func[i - 1]) && this.IsOperand(func[i + 1]))
							throw new Exception("Operator error!" + func[i] + " used improperly." + inFix);

				if (func[i] == "(")
					parCount++;
				else if (func[i] == ")")
					parCount--;
				if (parCount < 0)
					// TODO: Make the exception better.
					throw new Exception("Parenthesis error! No matching opening parenthesis." + " " + inFix);
			}
			if (parCount != 0)
				// TODO: Make the exception better.
				throw new Exception("Parenthesis error! No matching closeing parenthesis." + " " + inFix);

			// Check operators

			// Create a temporary vector to hold the secondary stack.
			Stack<string> workstack = new Stack<string>();
			func = this.Infix2Postfix(inFix).Split(new char[] { ' ' });

			// loop through the postfix vector
			string token = string.Empty;
			for (int i = 0; i < func.Length; i++)
			{
				token = func[i];

				// If the current string is an operator
				if (this.IsOperator(token))
				{
					if (token == "abs" ||
						token == "neg" ||
						token == "sign")
					{
						try
						{
							workstack.Pop();
							workstack.Push("0");
						}
						catch
						{
							throw new Exception("Operator error! \"" + token + "\". " + inFix);
						}
					}
					else
					{
						try
						{
							workstack.Pop();
							workstack.Pop();
							workstack.Push("0");
						}
						catch
						{
							throw new Exception("Operator error! \"" + token + "\". " + inFix);
						}
					}
				}
				else
				{
					// push the string on the workstack
					workstack.Push(token);
				}
			}

			// Check to see if the value on the back is a variable.
			//return convertString((string)workstack.Peek());

			return true;
		}

		/// <summary>
		/// Evaluates the funcion as for a double.
		/// </summary>
		/// <remarks><pre>
		/// 19 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <returns></returns>
		public double EvaluateD()
		{
			if (dynamicFunction != null)
				return dynamicFunction.EvaluateD(variables);

			// TODO! Check to see that we have the variable that we need.

			// Create a temporary vector to hold the secondary stack.
			Stack<string> workstack = new Stack<string>();
			string sLeft;
			string sRight;
			double dLeft = 0;
			double dRight = 0;
			double dResult = 0;

			//this.splitPostFunction = postFunction.Split(new char[] { ' ' });

			// loop through the postfix vector
			string token = string.Empty;
			for (int i = 0, numCount = this.splitPostFunction.Length; i < numCount; i++)
			{
				token = this.splitPostFunction[i];

				// If the current string is an operator
				if (this.IsOperator(token))
				{
					// Single operand operators.
					if (token == "abs" ||
						token == "neg" ||
						token == "sign")
					{
						// Get right operand
						sLeft = workstack.Pop();

						// Convert the operands
						dLeft = this.ConvertString(sLeft);
					}
					// Double operand operators
					else
					{
						// Get right operand
						sRight = workstack.Pop();

						// Get left operand
						sLeft = workstack.Pop();

						// Convert the operands
						dLeft = this.ConvertString(sLeft);
						dRight = this.ConvertString(sRight);
					}

					// call the operator
					switch (token)
					{
						case "+":
							// Add the operands
							dResult = dLeft + dRight;
							break;

						case "-":
							// Add the operands
							dResult = dLeft - dRight;
							break;

						case "*":
							// Multiply the operands
							dResult = dLeft * dRight;
							break;

						case "/":
							// Divide the operands
							if (dRight == 0)
								dResult = double.NaN;
							else
								dResult = dLeft / dRight;
							break;

						case "^":
							// Raise the number to the power.
							dResult = Math.Pow(dLeft, dRight);
							break;

						case "sign":
							// Get the sign.
							dResult = dLeft >= 0 ? 1 : -1;
							break;

						case "abs":
							// Convert to postive.
							dResult = Math.Abs(dLeft);
							break;

						case "neg":
							// Change the sign.
							dResult = -1 * dLeft;
							break;
					}

					// Push the result on the stack
					workstack.Push(dResult.ToString(System.Globalization.CultureInfo.InvariantCulture));
				}
				else
				{
					// push the string on the workstack
					workstack.Push(token);
				}
			}

			// Check to see if the value on the back is a variable.
			return this.ConvertString(workstack.Peek());
		}

		/// <summary>
		/// Converts a string to its value representation.
		/// </summary>
		/// <remarks><pre>
		/// 20 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">The string to check.</param>
		/// <returns></returns>
		protected double ConvertString(string token)
		{
			try
			{
				return variables[token];
			}
			catch
			{
				if (token == "true")
					return TRUE;
				else if (token == "false")
					return FALSE;
				else
					// Convert the operand
					return double.Parse(token, System.Globalization.CultureInfo.InvariantCulture);
			}

			/*
			// If operand is a variable
			if (this.IsVariable(token))
				// Get variable value
				return this.GetVariableValue(token);
			else if (token == "true")
				return TRUE;
			else if (token == "false")
				return FALSE;
			else
				// Convert the operand
				return double.Parse(token);
			*/
		}

		/// <summary>
		/// Overloads the ToString method.
		/// </summary>
		/// <remarks><pre>
		/// 20 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">The string to check.</param>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			ret.Append(inFunction);
			int count = 0;
			foreach (KeyValuePair<string, double> keyval in variables)
			{
				if (count++ == 0)
					ret.Append("; ");
				else
					ret.Append(", ");
				ret.Append(keyval.Key + "=" + keyval.Value);
			}
			return ret.ToString();
		}

		/// <summary>
		/// Evaluates the function given as a boolean expression.
		/// </summary>
		/// <remarks><pre>
		/// 20 Jul 2004 - Jeremy Roberts
		/// </pre></remarks>
		public bool EvaluateB()
		{
			if (dynamicFunction != null)
				return dynamicFunction.EvaluateB(variables);

			// TODO! Check to see that we have the variable that we need.

			// Create a temporary vector to hold the secondary stack.
			Stack<string> workstack = new Stack<string>();
			string sLeft = string.Empty;
			string sRight = string.Empty;
			string sResult = string.Empty;
			double dLeft = 0;
			double dRight = 0;
			double dResult = 0;

			string[] func = postFunction.Split(new char[] { ' ' });

			// loop through the postfix vector
			string token = string.Empty;
			for (int i = 0; i < func.Length; i++)
			{
				token = func[i];

				// If the current string is an operator
				if (this.IsOperator(token))
				{
					// Single operand operators.
					if (token == "abs" ||
						token == "neg")
					{
						// Get right operand
						sLeft = workstack.Pop();

						// Convert the operands
						dLeft = this.ConvertString(sLeft);
					}
					// Double operand operators
					else
					{
						// Get right operand
						sRight = workstack.Pop();

						// Get left operand
						sLeft = workstack.Pop();

						// Convert the operands
						dLeft = this.ConvertString(sLeft);
						dRight = this.ConvertString(sRight);
					}

					// call the operator
					switch (token)
					{
						case "<=":
							// Make the comparison.
							if (dLeft <= dRight)
								sResult = "true";
							else
								sResult = "false";
							break;

						case "<":
							// Make the comparison.
							if (dLeft < dRight)
								sResult = "true";
							else
								sResult = "false";
							break;

						case ">=":
							// Make the comparison.
							if (dLeft >= dRight)
								sResult = "true";
							else
								sResult = "false";
							break;

						case ">":
							// Make the comparison.
							if (dLeft > dRight)
								sResult = "true";
							else
								sResult = "false";
							break;

						case "==":
							// Get right operand
							// Make the comparison.
							if (dLeft == dRight)
								sResult = "true";
							else
								sResult = "false";
							break;

						case "!=":
							// Make the comparison.
							if (dLeft != dRight)
								sResult = "true";
							else
								sResult = "false";
							break;

						case "||":
							// OR the operands.
							if (dRight == TRUE || dLeft == TRUE)
								sResult = "true";
							else
								sResult = "false";
							break;

						case "&&":
							// AND the operands
							if (dRight == TRUE && dLeft == TRUE)
								sResult = "true";
							else
								sResult = "false";
							break;

						case "abs":
							// Convert to postive.
							dResult = Math.Abs(dLeft);
							sResult = dResult.ToString(System.Globalization.CultureInfo.InvariantCulture);
							break;

						case "neg":
							// Convert to postive.
							dResult = -1 * dLeft;
							sResult = dResult.ToString(System.Globalization.CultureInfo.InvariantCulture);
							break;
					}

					// Push the result on the stack
					workstack.Push(sResult);
				}
				else
				{
					// push the string on the workstack
					workstack.Push(token);
				}
			}

			//if (workstack.back() == "true")
			//    return true;
			//else
			//    return false;

			if (this.ConvertString(workstack.Peek()) == TRUE)
				return true;
			//if (this.ConvertString(workstack.Peek()) == FALSE)
			return false;
		}

		/// <summary>
		/// Checks to see if a string is a number.
		/// </summary>
		/// <remarks><pre>
		/// 11 Aug 2004 - Jeremy Roberts
		/// </pre></remarks>
		/// <param name="token">The string to check.</param>
		/// <returns>True if is a number, false otherwise.</returns>
		protected bool IsNumber(string token)
		{
			try
			{
				double.Parse(token, System.Globalization.CultureInfo.InvariantCulture);
				return true;
			}
			catch
			{
				return false;
			}

		}

		/// <summary>
		/// Compiles the functions.
		/// </summary>
		/// <remarks><pre>
		/// 20 Dec 2005 - Jeremy Roberts
		/// </pre></remarks>
		protected void compile()
		{
			// Code to set up the object.

			// Create a new AppDomain.
			// Set up assembly.
			//
			//NewAppDomain = System.AppDomain.CreateDomain("NewApplicationDomain");
			//NewAppDomain = appDomain;

			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "EmittedAssembly";
			AssemblyBuilder assembly = Thread.GetDomain().DefineDynamicAssembly(
				//AssemblyBuilder assembly = NewAppDomain.DefineDynamicAssembly(
				assemblyName,
				//AssemblyBuilderAccess.Save);
				AssemblyBuilderAccess.Run);
			//AssemblyBuilderAccess.RunAndSave);

			// Add Dynamic Module
			//
			ModuleBuilder module;
			module = assembly.DefineDynamicModule("EmittedModule");
			TypeBuilder dynamicFunctionClass = module.DefineType(
				"DynamicFunction",
				TypeAttributes.Public,
				typeof(DynamicFunction));

			// Define class constructor
			//
			Type objType = Type.GetType("System.Object");
			ConstructorInfo objConstructor = objType.GetConstructor(new Type[0]);
			Type[] constructorParams = { };
			ConstructorBuilder constructor = dynamicFunctionClass.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				constructorParams);

			// Emit the class constructor.
			//
			ILGenerator constructorIL = constructor.GetILGenerator();
			constructorIL.Emit(OpCodes.Ldarg_0);
			constructorIL.Emit(OpCodes.Call, objConstructor);
			constructorIL.Emit(OpCodes.Ret);

			// Define "EvaluateD" function.
			//
			Type[] args = { typeof(Dictionary<string, double>) };
			MethodBuilder evalMethodD = dynamicFunctionClass.DefineMethod(
				"EvaluateD",
				MethodAttributes.Public | MethodAttributes.Virtual,
				typeof(double),
				args);
			ILGenerator methodILD;
			methodILD = evalMethodD.GetILGenerator();
			emitFunction(this.PostFix, methodILD);

			// Define "EvaluateB" function.
			//
			MethodBuilder evalMethodB = dynamicFunctionClass.DefineMethod(
				"EvaluateB",
				MethodAttributes.Public | MethodAttributes.Virtual,
				typeof(bool),
				args);
			ILGenerator methodILB;
			methodILB = evalMethodB.GetILGenerator();
			emitFunction(this.PostFix, methodILB);

			// Create an object to use.
			//
			Type dt = dynamicFunctionClass.CreateType();
			//assembly.Save("assem.dll");
			//assembly.Save("x.exe");
			//return (function)Activator.CreateInstance(dt, new Object[] { });
			this.dynamicFunction = (DynamicFunction)Activator.CreateInstance(dt, new Object[] { });
		}

		protected void emitFunction(string function, ILGenerator ilGen)
		{
			string[] splitFunction = function.Split(new char[] { ' ' });

			// Set up two double variables.
			ilGen.DeclareLocal(typeof(System.Double));
			ilGen.DeclareLocal(typeof(System.Double));

			foreach (string token in splitFunction)
			{
				// If the current string is an operator
				if (this.IsOperator(token))
				{
					// call the operator
					switch (token)
					{
						case "+":
							{
								// Add the operands
								ilGen.Emit(OpCodes.Add);
								break;
							}

						case "-":
							{
								// Subtract the operands
								ilGen.Emit(OpCodes.Sub);
								break;
							}

						case "*":
							{
								// Multiply the operands
								ilGen.Emit(OpCodes.Mul);
								break;
							}

						case "/":
							{
								// Divide the operands
								System.Reflection.Emit.Label pushNaN = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Store the two variables.
								ilGen.Emit(OpCodes.Stloc_0); // store b in 0
								ilGen.Emit(OpCodes.Stloc_1); // store a in 1

								// Load the denominator and see if its 0.
								ilGen.Emit(OpCodes.Ldloc_0);
								ilGen.Emit(OpCodes.Ldc_R8, 0.0);
								ilGen.Emit(OpCodes.Ceq);
								ilGen.Emit(OpCodes.Brtrue_S, pushNaN);

								// It is not zero, do the division.
								ilGen.Emit(OpCodes.Ldloc_1);
								ilGen.Emit(OpCodes.Ldloc_0);
								ilGen.Emit(OpCodes.Div);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push NaN
								ilGen.MarkLabel(pushNaN);
								ilGen.Emit(OpCodes.Ldc_R8, double.NaN);

								ilGen.MarkLabel(exit);

								break;
							}

						case "^":
							{
								// Raise the number to the power.
								ilGen.EmitCall(OpCodes.Callvirt,
									typeof(System.Math).GetMethod("Pow"),
									null);
								break;
							}

						case "sign":
							{
								// Get the sign.
								System.Reflection.Emit.Label pushNeg = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Compare to see if the value is less then 0
								ilGen.Emit(OpCodes.Stloc_0); // store
								ilGen.Emit(OpCodes.Ldloc_0);
								ilGen.Emit(OpCodes.Ldc_R8, 0.0);
								ilGen.Emit(OpCodes.Clt);
								ilGen.Emit(OpCodes.Brtrue_S, pushNeg);

								// Push 1
								ilGen.Emit(OpCodes.Ldc_R8, 1.0);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push Neg
								ilGen.MarkLabel(pushNeg);
								ilGen.Emit(OpCodes.Ldc_R8, -1.0);

								ilGen.MarkLabel(exit);

								break;
							}

						case "abs":
							{
								// Convert to postive.
								Type[] absArgs = { typeof(System.Double) };
								ilGen.EmitCall(OpCodes.Callvirt,
									typeof(System.Math).GetMethod("Abs", absArgs),
									null);
								break;
							}

						case "neg":
							{
								// Change the sign.
								ilGen.Emit(OpCodes.Ldc_R8, -1.0);
								ilGen.Emit(OpCodes.Mul);
								break;
							}

						case "<=":
							{
								// Make the comparison.
								System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Compare the two values.
								ilGen.Emit(OpCodes.Cgt);
								ilGen.Emit(OpCodes.Brtrue_S, pushFalse);

								// Otherwise its true
								ilGen.Emit(OpCodes.Ldc_R8, TRUE);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push NaN
								ilGen.MarkLabel(pushFalse);
								ilGen.Emit(OpCodes.Ldc_R8, FALSE);

								ilGen.MarkLabel(exit);
								break;
							}

						case "<":
							{
								// Make the comparison.
								// Compare the two values.
								ilGen.Emit(OpCodes.Clt);
								break;
							}

						case ">=":
							{
								// Make the comparison.
								System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Compare the two values.
								ilGen.Emit(OpCodes.Clt);
								ilGen.Emit(OpCodes.Brtrue_S, pushFalse);

								// Otherwise its true
								ilGen.Emit(OpCodes.Ldc_R8, TRUE);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push NaN
								ilGen.MarkLabel(pushFalse);
								ilGen.Emit(OpCodes.Ldc_R8, FALSE);

								ilGen.MarkLabel(exit);
								break;
							}

						case ">":
							{
								// Make the comparison.
								ilGen.Emit(OpCodes.Cgt);
								break;
							}

						case "==":
							{
								// Make the comparison.
								ilGen.Emit(OpCodes.Ceq);
								break;
							}

						case "!=":
							{
								// Make the comparison.
								System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Compare the two values.
								ilGen.Emit(OpCodes.Ceq);
								ilGen.Emit(OpCodes.Brtrue_S, pushFalse);

								// Otherwise its true
								ilGen.Emit(OpCodes.Ldc_R8, TRUE);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push NaN
								ilGen.MarkLabel(pushFalse);
								ilGen.Emit(OpCodes.Ldc_R8, FALSE);

								ilGen.MarkLabel(exit);

								break;
							}

						case "||":
							{
								// Make the comparison.
								System.Reflection.Emit.Label pushTrue = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Store the two variables.
								ilGen.Emit(OpCodes.Stloc_0);
								ilGen.Emit(OpCodes.Stloc_1);

								// Compare the two values.
								ilGen.Emit(OpCodes.Ldloc_0);
								ilGen.Emit(OpCodes.Brtrue_S, pushTrue);
								ilGen.Emit(OpCodes.Ldloc_1);
								ilGen.Emit(OpCodes.Brtrue_S, pushTrue);

								// Otherwise its false
								ilGen.Emit(OpCodes.Ldc_R8, FALSE);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push NaN
								ilGen.MarkLabel(pushTrue);
								ilGen.Emit(OpCodes.Ldc_R8, TRUE);

								ilGen.MarkLabel(exit);
								break;
							}

						case "&&":
							{
								// Make the comparison.
								System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
								System.Reflection.Emit.Label exit = ilGen.DefineLabel();

								// Store the two variables.
								ilGen.Emit(OpCodes.Stloc_0);
								ilGen.Emit(OpCodes.Stloc_1);

								// Compare the two values.
								ilGen.Emit(OpCodes.Ldloc_0);
								ilGen.Emit(OpCodes.Brfalse_S, pushFalse);
								ilGen.Emit(OpCodes.Ldloc_1);
								ilGen.Emit(OpCodes.Brfalse_S, pushFalse);

								// Otherwise its true
								ilGen.Emit(OpCodes.Ldc_R8, TRUE);
								ilGen.Emit(OpCodes.Br_S, exit);

								// Push NaN
								ilGen.MarkLabel(pushFalse);
								ilGen.Emit(OpCodes.Ldc_R8, FALSE);

								ilGen.MarkLabel(exit);
								break;
							}
					}

				}
				else if (IsVariable(token))
				{
					// push the string on the workstack
					ilGen.Emit(OpCodes.Ldarg_1);
					ilGen.Emit(OpCodes.Ldstr, token);
					ilGen.EmitCall(OpCodes.Callvirt,
						typeof(System.Collections.Generic.Dictionary<string, double>).GetMethod("get_Item"),
						null);
					//ilGen.Emit(OpCodes.Unbox_Any, typeof(System.Double));
				}
				else if (token.Equals("true"))
				{
					ilGen.Emit(OpCodes.Ldc_R8, TRUE);
				}
				else if (token.Equals("false"))
				{
					ilGen.Emit(OpCodes.Ldc_R8, FALSE);
				}
				else
				{
					// Parse the number.
					ilGen.Emit(OpCodes.Ldc_R8, double.Parse(token, System.Globalization.CultureInfo.InvariantCulture));
				}
			}
			ilGen.Emit(OpCodes.Ret);
		}

	}
}