// $ANTLR 3.3.0.7239 C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g 2011-08-08 11:08:01

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 219
// Unreachable code detected.
#pragma warning disable 162


using System.Collections.Generic;
using Antlr.Runtime;
using Stack = System.Collections.Generic.Stack<object>;
using List = System.Collections.IList;
using ArrayList = System.Collections.Generic.List<object>;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "3.3.0.7239")]
[System.CLSCompliant(false)]
public partial class NCalcLexer : Antlr.Runtime.Lexer
{
	public const int EOF=-1;
	public const int DATETIME=4;
	public const int DIGIT=5;
	public const int E=6;
	public const int EscapeSequence=7;
	public const int FALSE=8;
	public const int FLOAT=9;
	public const int HexDigit=10;
	public const int ID=11;
	public const int INTEGER=12;
	public const int LETTER=13;
	public const int NAME=14;
	public const int STRING=15;
	public const int TRUE=16;
	public const int UnicodeEscape=17;
	public const int WS=18;
	public const int T__19=19;
	public const int T__20=20;
	public const int T__21=21;
	public const int T__22=22;
	public const int T__23=23;
	public const int T__24=24;
	public const int T__25=25;
	public const int T__26=26;
	public const int T__27=27;
	public const int T__28=28;
	public const int T__29=29;
	public const int T__30=30;
	public const int T__31=31;
	public const int T__32=32;
	public const int T__33=33;
	public const int T__34=34;
	public const int T__35=35;
	public const int T__36=36;
	public const int T__37=37;
	public const int T__38=38;
	public const int T__39=39;
	public const int T__40=40;
	public const int T__41=41;
	public const int T__42=42;
	public const int T__43=43;
	public const int T__44=44;
	public const int T__45=45;
	public const int T__46=46;
	public const int T__47=47;
	public const int T__48=48;

    // delegates
    // delegators

	public NCalcLexer()
	{
		OnCreated();
	}

	public NCalcLexer(ICharStream input )
		: this(input, new RecognizerSharedState())
	{
	}

	public NCalcLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state)
	{


		OnCreated();
	}
	public override string GrammarFileName { get { return "C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g"; } }

	private static readonly bool[] decisionCanBacktrack = new bool[0];


	partial void OnCreated();
	partial void EnterRule(string ruleName, int ruleIndex);
	partial void LeaveRule(string ruleName, int ruleIndex);

	partial void Enter_T__19();
	partial void Leave_T__19();

	// $ANTLR start "T__19"
	[GrammarRule("T__19")]
	private void mT__19()
	{
		Enter_T__19();
		EnterRule("T__19", 1);
		TraceIn("T__19", 1);
		try
		{
			int _type = T__19;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:7:7: ( '!' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:7:9: '!'
			{
			DebugLocation(7, 9);
			Match('!'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__19", 1);
			LeaveRule("T__19", 1);
			Leave_T__19();
		}
	}
	// $ANTLR end "T__19"

	partial void Enter_T__20();
	partial void Leave_T__20();

	// $ANTLR start "T__20"
	[GrammarRule("T__20")]
	private void mT__20()
	{
		Enter_T__20();
		EnterRule("T__20", 2);
		TraceIn("T__20", 2);
		try
		{
			int _type = T__20;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:8:7: ( '!=' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:8:9: '!='
			{
			DebugLocation(8, 9);
			Match("!="); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__20", 2);
			LeaveRule("T__20", 2);
			Leave_T__20();
		}
	}
	// $ANTLR end "T__20"

	partial void Enter_T__21();
	partial void Leave_T__21();

	// $ANTLR start "T__21"
	[GrammarRule("T__21")]
	private void mT__21()
	{
		Enter_T__21();
		EnterRule("T__21", 3);
		TraceIn("T__21", 3);
		try
		{
			int _type = T__21;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:9:7: ( '%' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:9:9: '%'
			{
			DebugLocation(9, 9);
			Match('%'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__21", 3);
			LeaveRule("T__21", 3);
			Leave_T__21();
		}
	}
	// $ANTLR end "T__21"

	partial void Enter_T__22();
	partial void Leave_T__22();

	// $ANTLR start "T__22"
	[GrammarRule("T__22")]
	private void mT__22()
	{
		Enter_T__22();
		EnterRule("T__22", 4);
		TraceIn("T__22", 4);
		try
		{
			int _type = T__22;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:10:7: ( '&&' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:10:9: '&&'
			{
			DebugLocation(10, 9);
			Match("&&"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__22", 4);
			LeaveRule("T__22", 4);
			Leave_T__22();
		}
	}
	// $ANTLR end "T__22"

	partial void Enter_T__23();
	partial void Leave_T__23();

	// $ANTLR start "T__23"
	[GrammarRule("T__23")]
	private void mT__23()
	{
		Enter_T__23();
		EnterRule("T__23", 5);
		TraceIn("T__23", 5);
		try
		{
			int _type = T__23;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:11:7: ( '&' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:11:9: '&'
			{
			DebugLocation(11, 9);
			Match('&'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__23", 5);
			LeaveRule("T__23", 5);
			Leave_T__23();
		}
	}
	// $ANTLR end "T__23"

	partial void Enter_T__24();
	partial void Leave_T__24();

	// $ANTLR start "T__24"
	[GrammarRule("T__24")]
	private void mT__24()
	{
		Enter_T__24();
		EnterRule("T__24", 6);
		TraceIn("T__24", 6);
		try
		{
			int _type = T__24;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:12:7: ( '(' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:12:9: '('
			{
			DebugLocation(12, 9);
			Match('('); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__24", 6);
			LeaveRule("T__24", 6);
			Leave_T__24();
		}
	}
	// $ANTLR end "T__24"

	partial void Enter_T__25();
	partial void Leave_T__25();

	// $ANTLR start "T__25"
	[GrammarRule("T__25")]
	private void mT__25()
	{
		Enter_T__25();
		EnterRule("T__25", 7);
		TraceIn("T__25", 7);
		try
		{
			int _type = T__25;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:13:7: ( ')' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:13:9: ')'
			{
			DebugLocation(13, 9);
			Match(')'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__25", 7);
			LeaveRule("T__25", 7);
			Leave_T__25();
		}
	}
	// $ANTLR end "T__25"

	partial void Enter_T__26();
	partial void Leave_T__26();

	// $ANTLR start "T__26"
	[GrammarRule("T__26")]
	private void mT__26()
	{
		Enter_T__26();
		EnterRule("T__26", 8);
		TraceIn("T__26", 8);
		try
		{
			int _type = T__26;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:14:7: ( '*' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:14:9: '*'
			{
			DebugLocation(14, 9);
			Match('*'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__26", 8);
			LeaveRule("T__26", 8);
			Leave_T__26();
		}
	}
	// $ANTLR end "T__26"

	partial void Enter_T__27();
	partial void Leave_T__27();

	// $ANTLR start "T__27"
	[GrammarRule("T__27")]
	private void mT__27()
	{
		Enter_T__27();
		EnterRule("T__27", 9);
		TraceIn("T__27", 9);
		try
		{
			int _type = T__27;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:15:7: ( '+' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:15:9: '+'
			{
			DebugLocation(15, 9);
			Match('+'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__27", 9);
			LeaveRule("T__27", 9);
			Leave_T__27();
		}
	}
	// $ANTLR end "T__27"

	partial void Enter_T__28();
	partial void Leave_T__28();

	// $ANTLR start "T__28"
	[GrammarRule("T__28")]
	private void mT__28()
	{
		Enter_T__28();
		EnterRule("T__28", 10);
		TraceIn("T__28", 10);
		try
		{
			int _type = T__28;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:16:7: ( ',' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:16:9: ','
			{
			DebugLocation(16, 9);
			Match(','); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__28", 10);
			LeaveRule("T__28", 10);
			Leave_T__28();
		}
	}
	// $ANTLR end "T__28"

	partial void Enter_T__29();
	partial void Leave_T__29();

	// $ANTLR start "T__29"
	[GrammarRule("T__29")]
	private void mT__29()
	{
		Enter_T__29();
		EnterRule("T__29", 11);
		TraceIn("T__29", 11);
		try
		{
			int _type = T__29;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:17:7: ( '-' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:17:9: '-'
			{
			DebugLocation(17, 9);
			Match('-'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__29", 11);
			LeaveRule("T__29", 11);
			Leave_T__29();
		}
	}
	// $ANTLR end "T__29"

	partial void Enter_T__30();
	partial void Leave_T__30();

	// $ANTLR start "T__30"
	[GrammarRule("T__30")]
	private void mT__30()
	{
		Enter_T__30();
		EnterRule("T__30", 12);
		TraceIn("T__30", 12);
		try
		{
			int _type = T__30;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:18:7: ( '/' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:18:9: '/'
			{
			DebugLocation(18, 9);
			Match('/'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__30", 12);
			LeaveRule("T__30", 12);
			Leave_T__30();
		}
	}
	// $ANTLR end "T__30"

	partial void Enter_T__31();
	partial void Leave_T__31();

	// $ANTLR start "T__31"
	[GrammarRule("T__31")]
	private void mT__31()
	{
		Enter_T__31();
		EnterRule("T__31", 13);
		TraceIn("T__31", 13);
		try
		{
			int _type = T__31;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:19:7: ( ':' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:19:9: ':'
			{
			DebugLocation(19, 9);
			Match(':'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__31", 13);
			LeaveRule("T__31", 13);
			Leave_T__31();
		}
	}
	// $ANTLR end "T__31"

	partial void Enter_T__32();
	partial void Leave_T__32();

	// $ANTLR start "T__32"
	[GrammarRule("T__32")]
	private void mT__32()
	{
		Enter_T__32();
		EnterRule("T__32", 14);
		TraceIn("T__32", 14);
		try
		{
			int _type = T__32;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:20:7: ( '<' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:20:9: '<'
			{
			DebugLocation(20, 9);
			Match('<'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__32", 14);
			LeaveRule("T__32", 14);
			Leave_T__32();
		}
	}
	// $ANTLR end "T__32"

	partial void Enter_T__33();
	partial void Leave_T__33();

	// $ANTLR start "T__33"
	[GrammarRule("T__33")]
	private void mT__33()
	{
		Enter_T__33();
		EnterRule("T__33", 15);
		TraceIn("T__33", 15);
		try
		{
			int _type = T__33;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:21:7: ( '<<' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:21:9: '<<'
			{
			DebugLocation(21, 9);
			Match("<<"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__33", 15);
			LeaveRule("T__33", 15);
			Leave_T__33();
		}
	}
	// $ANTLR end "T__33"

	partial void Enter_T__34();
	partial void Leave_T__34();

	// $ANTLR start "T__34"
	[GrammarRule("T__34")]
	private void mT__34()
	{
		Enter_T__34();
		EnterRule("T__34", 16);
		TraceIn("T__34", 16);
		try
		{
			int _type = T__34;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:22:7: ( '<=' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:22:9: '<='
			{
			DebugLocation(22, 9);
			Match("<="); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__34", 16);
			LeaveRule("T__34", 16);
			Leave_T__34();
		}
	}
	// $ANTLR end "T__34"

	partial void Enter_T__35();
	partial void Leave_T__35();

	// $ANTLR start "T__35"
	[GrammarRule("T__35")]
	private void mT__35()
	{
		Enter_T__35();
		EnterRule("T__35", 17);
		TraceIn("T__35", 17);
		try
		{
			int _type = T__35;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:23:7: ( '<>' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:23:9: '<>'
			{
			DebugLocation(23, 9);
			Match("<>"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__35", 17);
			LeaveRule("T__35", 17);
			Leave_T__35();
		}
	}
	// $ANTLR end "T__35"

	partial void Enter_T__36();
	partial void Leave_T__36();

	// $ANTLR start "T__36"
	[GrammarRule("T__36")]
	private void mT__36()
	{
		Enter_T__36();
		EnterRule("T__36", 18);
		TraceIn("T__36", 18);
		try
		{
			int _type = T__36;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:24:7: ( '=' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:24:9: '='
			{
			DebugLocation(24, 9);
			Match('='); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__36", 18);
			LeaveRule("T__36", 18);
			Leave_T__36();
		}
	}
	// $ANTLR end "T__36"

	partial void Enter_T__37();
	partial void Leave_T__37();

	// $ANTLR start "T__37"
	[GrammarRule("T__37")]
	private void mT__37()
	{
		Enter_T__37();
		EnterRule("T__37", 19);
		TraceIn("T__37", 19);
		try
		{
			int _type = T__37;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:25:7: ( '==' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:25:9: '=='
			{
			DebugLocation(25, 9);
			Match("=="); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__37", 19);
			LeaveRule("T__37", 19);
			Leave_T__37();
		}
	}
	// $ANTLR end "T__37"

	partial void Enter_T__38();
	partial void Leave_T__38();

	// $ANTLR start "T__38"
	[GrammarRule("T__38")]
	private void mT__38()
	{
		Enter_T__38();
		EnterRule("T__38", 20);
		TraceIn("T__38", 20);
		try
		{
			int _type = T__38;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:26:7: ( '>' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:26:9: '>'
			{
			DebugLocation(26, 9);
			Match('>'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__38", 20);
			LeaveRule("T__38", 20);
			Leave_T__38();
		}
	}
	// $ANTLR end "T__38"

	partial void Enter_T__39();
	partial void Leave_T__39();

	// $ANTLR start "T__39"
	[GrammarRule("T__39")]
	private void mT__39()
	{
		Enter_T__39();
		EnterRule("T__39", 21);
		TraceIn("T__39", 21);
		try
		{
			int _type = T__39;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:27:7: ( '>=' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:27:9: '>='
			{
			DebugLocation(27, 9);
			Match(">="); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__39", 21);
			LeaveRule("T__39", 21);
			Leave_T__39();
		}
	}
	// $ANTLR end "T__39"

	partial void Enter_T__40();
	partial void Leave_T__40();

	// $ANTLR start "T__40"
	[GrammarRule("T__40")]
	private void mT__40()
	{
		Enter_T__40();
		EnterRule("T__40", 22);
		TraceIn("T__40", 22);
		try
		{
			int _type = T__40;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:28:7: ( '>>' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:28:9: '>>'
			{
			DebugLocation(28, 9);
			Match(">>"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__40", 22);
			LeaveRule("T__40", 22);
			Leave_T__40();
		}
	}
	// $ANTLR end "T__40"

	partial void Enter_T__41();
	partial void Leave_T__41();

	// $ANTLR start "T__41"
	[GrammarRule("T__41")]
	private void mT__41()
	{
		Enter_T__41();
		EnterRule("T__41", 23);
		TraceIn("T__41", 23);
		try
		{
			int _type = T__41;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:29:7: ( '?' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:29:9: '?'
			{
			DebugLocation(29, 9);
			Match('?'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__41", 23);
			LeaveRule("T__41", 23);
			Leave_T__41();
		}
	}
	// $ANTLR end "T__41"

	partial void Enter_T__42();
	partial void Leave_T__42();

	// $ANTLR start "T__42"
	[GrammarRule("T__42")]
	private void mT__42()
	{
		Enter_T__42();
		EnterRule("T__42", 24);
		TraceIn("T__42", 24);
		try
		{
			int _type = T__42;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:30:7: ( '^' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:30:9: '^'
			{
			DebugLocation(30, 9);
			Match('^'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__42", 24);
			LeaveRule("T__42", 24);
			Leave_T__42();
		}
	}
	// $ANTLR end "T__42"

	partial void Enter_T__43();
	partial void Leave_T__43();

	// $ANTLR start "T__43"
	[GrammarRule("T__43")]
	private void mT__43()
	{
		Enter_T__43();
		EnterRule("T__43", 25);
		TraceIn("T__43", 25);
		try
		{
			int _type = T__43;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:31:7: ( 'and' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:31:9: 'and'
			{
			DebugLocation(31, 9);
			Match("and"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__43", 25);
			LeaveRule("T__43", 25);
			Leave_T__43();
		}
	}
	// $ANTLR end "T__43"

	partial void Enter_T__44();
	partial void Leave_T__44();

	// $ANTLR start "T__44"
	[GrammarRule("T__44")]
	private void mT__44()
	{
		Enter_T__44();
		EnterRule("T__44", 26);
		TraceIn("T__44", 26);
		try
		{
			int _type = T__44;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:32:7: ( 'not' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:32:9: 'not'
			{
			DebugLocation(32, 9);
			Match("not"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__44", 26);
			LeaveRule("T__44", 26);
			Leave_T__44();
		}
	}
	// $ANTLR end "T__44"

	partial void Enter_T__45();
	partial void Leave_T__45();

	// $ANTLR start "T__45"
	[GrammarRule("T__45")]
	private void mT__45()
	{
		Enter_T__45();
		EnterRule("T__45", 27);
		TraceIn("T__45", 27);
		try
		{
			int _type = T__45;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:33:7: ( 'or' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:33:9: 'or'
			{
			DebugLocation(33, 9);
			Match("or"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__45", 27);
			LeaveRule("T__45", 27);
			Leave_T__45();
		}
	}
	// $ANTLR end "T__45"

	partial void Enter_T__46();
	partial void Leave_T__46();

	// $ANTLR start "T__46"
	[GrammarRule("T__46")]
	private void mT__46()
	{
		Enter_T__46();
		EnterRule("T__46", 28);
		TraceIn("T__46", 28);
		try
		{
			int _type = T__46;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:34:7: ( '|' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:34:9: '|'
			{
			DebugLocation(34, 9);
			Match('|'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__46", 28);
			LeaveRule("T__46", 28);
			Leave_T__46();
		}
	}
	// $ANTLR end "T__46"

	partial void Enter_T__47();
	partial void Leave_T__47();

	// $ANTLR start "T__47"
	[GrammarRule("T__47")]
	private void mT__47()
	{
		Enter_T__47();
		EnterRule("T__47", 29);
		TraceIn("T__47", 29);
		try
		{
			int _type = T__47;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:35:7: ( '||' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:35:9: '||'
			{
			DebugLocation(35, 9);
			Match("||"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__47", 29);
			LeaveRule("T__47", 29);
			Leave_T__47();
		}
	}
	// $ANTLR end "T__47"

	partial void Enter_T__48();
	partial void Leave_T__48();

	// $ANTLR start "T__48"
	[GrammarRule("T__48")]
	private void mT__48()
	{
		Enter_T__48();
		EnterRule("T__48", 30);
		TraceIn("T__48", 30);
		try
		{
			int _type = T__48;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:36:7: ( '~' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:36:9: '~'
			{
			DebugLocation(36, 9);
			Match('~'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("T__48", 30);
			LeaveRule("T__48", 30);
			Leave_T__48();
		}
	}
	// $ANTLR end "T__48"

	partial void Enter_TRUE();
	partial void Leave_TRUE();

	// $ANTLR start "TRUE"
	[GrammarRule("TRUE")]
	private void mTRUE()
	{
		Enter_TRUE();
		EnterRule("TRUE", 31);
		TraceIn("TRUE", 31);
		try
		{
			int _type = TRUE;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:237:2: ( 'true' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:237:4: 'true'
			{
			DebugLocation(237, 4);
			Match("true"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("TRUE", 31);
			LeaveRule("TRUE", 31);
			Leave_TRUE();
		}
	}
	// $ANTLR end "TRUE"

	partial void Enter_FALSE();
	partial void Leave_FALSE();

	// $ANTLR start "FALSE"
	[GrammarRule("FALSE")]
	private void mFALSE()
	{
		Enter_FALSE();
		EnterRule("FALSE", 32);
		TraceIn("FALSE", 32);
		try
		{
			int _type = FALSE;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:241:2: ( 'false' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:241:4: 'false'
			{
			DebugLocation(241, 4);
			Match("false"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("FALSE", 32);
			LeaveRule("FALSE", 32);
			Leave_FALSE();
		}
	}
	// $ANTLR end "FALSE"

	partial void Enter_ID();
	partial void Leave_ID();

	// $ANTLR start "ID"
	[GrammarRule("ID")]
	private void mID()
	{
		Enter_ID();
		EnterRule("ID", 33);
		TraceIn("ID", 33);
		try
		{
			int _type = ID;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:245:2: ( LETTER ( LETTER | DIGIT )* )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:245:5: LETTER ( LETTER | DIGIT )*
			{
			DebugLocation(245, 5);
			mLETTER(); 
			DebugLocation(245, 12);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:245:12: ( LETTER | DIGIT )*
			try { DebugEnterSubRule(1);
			while (true)
			{
				int alt1=2;
				try { DebugEnterDecision(1, decisionCanBacktrack[1]);
				int LA1_0 = input.LA(1);

				if (((LA1_0>='0' && LA1_0<='9')||(LA1_0>='A' && LA1_0<='Z')||LA1_0=='_'||(LA1_0>='a' && LA1_0<='z')))
				{
					alt1=1;
				}


				} finally { DebugExitDecision(1); }
				switch ( alt1 )
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
					{
					DebugLocation(245, 12);
					input.Consume();


					}
					break;

				default:
					goto loop1;
				}
			}

			loop1:
				;

			} finally { DebugExitSubRule(1); }


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("ID", 33);
			LeaveRule("ID", 33);
			Leave_ID();
		}
	}
	// $ANTLR end "ID"

	partial void Enter_INTEGER();
	partial void Leave_INTEGER();

	// $ANTLR start "INTEGER"
	[GrammarRule("INTEGER")]
	private void mINTEGER()
	{
		Enter_INTEGER();
		EnterRule("INTEGER", 34);
		TraceIn("INTEGER", 34);
		try
		{
			int _type = INTEGER;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:249:2: ( ( DIGIT )+ )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:249:4: ( DIGIT )+
			{
			DebugLocation(249, 4);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:249:4: ( DIGIT )+
			int cnt2=0;
			try { DebugEnterSubRule(2);
			while (true)
			{
				int alt2=2;
				try { DebugEnterDecision(2, decisionCanBacktrack[2]);
				int LA2_0 = input.LA(1);

				if (((LA2_0>='0' && LA2_0<='9')))
				{
					alt2=1;
				}


				} finally { DebugExitDecision(2); }
				switch (alt2)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
					{
					DebugLocation(249, 4);
					input.Consume();


					}
					break;

				default:
					if (cnt2 >= 1)
						goto loop2;

					EarlyExitException eee2 = new EarlyExitException( 2, input );
					DebugRecognitionException(eee2);
					throw eee2;
				}
				cnt2++;
			}
			loop2:
				;

			} finally { DebugExitSubRule(2); }


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("INTEGER", 34);
			LeaveRule("INTEGER", 34);
			Leave_INTEGER();
		}
	}
	// $ANTLR end "INTEGER"

	partial void Enter_FLOAT();
	partial void Leave_FLOAT();

	// $ANTLR start "FLOAT"
	[GrammarRule("FLOAT")]
	private void mFLOAT()
	{
		Enter_FLOAT();
		EnterRule("FLOAT", 35);
		TraceIn("FLOAT", 35);
		try
		{
			int _type = FLOAT;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:253:2: ( ( DIGIT )* '.' ( DIGIT )+ ( E )? | ( DIGIT )+ E )
			int alt7=2;
			try { DebugEnterDecision(7, decisionCanBacktrack[7]);
			try
			{
				alt7 = dfa7.Predict(input);
			}
			catch (NoViableAltException nvae)
			{
				DebugRecognitionException(nvae);
				throw;
			}
			} finally { DebugExitDecision(7); }
			switch (alt7)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:253:4: ( DIGIT )* '.' ( DIGIT )+ ( E )?
				{
				DebugLocation(253, 4);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:253:4: ( DIGIT )*
				try { DebugEnterSubRule(3);
				while (true)
				{
					int alt3=2;
					try { DebugEnterDecision(3, decisionCanBacktrack[3]);
					int LA3_0 = input.LA(1);

					if (((LA3_0>='0' && LA3_0<='9')))
					{
						alt3=1;
					}


					} finally { DebugExitDecision(3); }
					switch ( alt3 )
					{
					case 1:
						DebugEnterAlt(1);
						// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
						{
						DebugLocation(253, 4);
						input.Consume();


						}
						break;

					default:
						goto loop3;
					}
				}

				loop3:
					;

				} finally { DebugExitSubRule(3); }

				DebugLocation(253, 11);
				Match('.'); 
				DebugLocation(253, 15);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:253:15: ( DIGIT )+
				int cnt4=0;
				try { DebugEnterSubRule(4);
				while (true)
				{
					int alt4=2;
					try { DebugEnterDecision(4, decisionCanBacktrack[4]);
					int LA4_0 = input.LA(1);

					if (((LA4_0>='0' && LA4_0<='9')))
					{
						alt4=1;
					}


					} finally { DebugExitDecision(4); }
					switch (alt4)
					{
					case 1:
						DebugEnterAlt(1);
						// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
						{
						DebugLocation(253, 15);
						input.Consume();


						}
						break;

					default:
						if (cnt4 >= 1)
							goto loop4;

						EarlyExitException eee4 = new EarlyExitException( 4, input );
						DebugRecognitionException(eee4);
						throw eee4;
					}
					cnt4++;
				}
				loop4:
					;

				} finally { DebugExitSubRule(4); }

				DebugLocation(253, 22);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:253:22: ( E )?
				int alt5=2;
				try { DebugEnterSubRule(5);
				try { DebugEnterDecision(5, decisionCanBacktrack[5]);
				int LA5_0 = input.LA(1);

				if ((LA5_0=='E'||LA5_0=='e'))
				{
					alt5=1;
				}
				} finally { DebugExitDecision(5); }
				switch (alt5)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:253:22: E
					{
					DebugLocation(253, 22);
					mE(); 

					}
					break;

				}
				} finally { DebugExitSubRule(5); }


				}
				break;
			case 2:
				DebugEnterAlt(2);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:254:4: ( DIGIT )+ E
				{
				DebugLocation(254, 4);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:254:4: ( DIGIT )+
				int cnt6=0;
				try { DebugEnterSubRule(6);
				while (true)
				{
					int alt6=2;
					try { DebugEnterDecision(6, decisionCanBacktrack[6]);
					int LA6_0 = input.LA(1);

					if (((LA6_0>='0' && LA6_0<='9')))
					{
						alt6=1;
					}


					} finally { DebugExitDecision(6); }
					switch (alt6)
					{
					case 1:
						DebugEnterAlt(1);
						// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
						{
						DebugLocation(254, 4);
						input.Consume();


						}
						break;

					default:
						if (cnt6 >= 1)
							goto loop6;

						EarlyExitException eee6 = new EarlyExitException( 6, input );
						DebugRecognitionException(eee6);
						throw eee6;
					}
					cnt6++;
				}
				loop6:
					;

				} finally { DebugExitSubRule(6); }

				DebugLocation(254, 11);
				mE(); 

				}
				break;

			}
			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("FLOAT", 35);
			LeaveRule("FLOAT", 35);
			Leave_FLOAT();
		}
	}
	// $ANTLR end "FLOAT"

	partial void Enter_STRING();
	partial void Leave_STRING();

	// $ANTLR start "STRING"
	[GrammarRule("STRING")]
	private void mSTRING()
	{
		Enter_STRING();
		EnterRule("STRING", 36);
		TraceIn("STRING", 36);
		try
		{
			int _type = STRING;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:6: ( '\\'' ( EscapeSequence | ( options {greedy=false; } :~ ( '\\u0000' .. '\\u001f' | '\\\\' | '\\'' ) ) )* '\\'' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:10: '\\'' ( EscapeSequence | ( options {greedy=false; } :~ ( '\\u0000' .. '\\u001f' | '\\\\' | '\\'' ) ) )* '\\''
			{
			DebugLocation(258, 10);
			Match('\''); 
			DebugLocation(258, 15);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:15: ( EscapeSequence | ( options {greedy=false; } :~ ( '\\u0000' .. '\\u001f' | '\\\\' | '\\'' ) ) )*
			try { DebugEnterSubRule(8);
			while (true)
			{
				int alt8=3;
				try { DebugEnterDecision(8, decisionCanBacktrack[8]);
				int LA8_0 = input.LA(1);

				if ((LA8_0=='\\'))
				{
					alt8=1;
				}
				else if (((LA8_0>=' ' && LA8_0<='&')||(LA8_0>='(' && LA8_0<='[')||(LA8_0>=']' && LA8_0<='\uFFFF')))
				{
					alt8=2;
				}


				} finally { DebugExitDecision(8); }
				switch ( alt8 )
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:17: EscapeSequence
					{
					DebugLocation(258, 17);
					mEscapeSequence(); 

					}
					break;
				case 2:
					DebugEnterAlt(2);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:34: ( options {greedy=false; } :~ ( '\\u0000' .. '\\u001f' | '\\\\' | '\\'' ) )
					{
					DebugLocation(258, 34);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:34: ( options {greedy=false; } :~ ( '\\u0000' .. '\\u001f' | '\\\\' | '\\'' ) )
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:258:61: ~ ( '\\u0000' .. '\\u001f' | '\\\\' | '\\'' )
					{
					DebugLocation(258, 61);
					input.Consume();


					}


					}
					break;

				default:
					goto loop8;
				}
			}

			loop8:
				;

			} finally { DebugExitSubRule(8); }

			DebugLocation(258, 103);
			Match('\''); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("STRING", 36);
			LeaveRule("STRING", 36);
			Leave_STRING();
		}
	}
	// $ANTLR end "STRING"

	partial void Enter_DATETIME();
	partial void Leave_DATETIME();

	// $ANTLR start "DATETIME"
	[GrammarRule("DATETIME")]
	private void mDATETIME()
	{
		Enter_DATETIME();
		EnterRule("DATETIME", 37);
		TraceIn("DATETIME", 37);
		try
		{
			int _type = DATETIME;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:262:3: ( '#' ( options {greedy=false; } : (~ ( '#' ) )* ) '#' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:262:5: '#' ( options {greedy=false; } : (~ ( '#' ) )* ) '#'
			{
			DebugLocation(262, 5);
			Match('#'); 
			DebugLocation(262, 9);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:262:9: ( options {greedy=false; } : (~ ( '#' ) )* )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:262:36: (~ ( '#' ) )*
			{
			DebugLocation(262, 36);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:262:36: (~ ( '#' ) )*
			try { DebugEnterSubRule(9);
			while (true)
			{
				int alt9=2;
				try { DebugEnterDecision(9, decisionCanBacktrack[9]);
				int LA9_0 = input.LA(1);

				if (((LA9_0>='\u0000' && LA9_0<='\"')||(LA9_0>='$' && LA9_0<='\uFFFF')))
				{
					alt9=1;
				}


				} finally { DebugExitDecision(9); }
				switch ( alt9 )
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
					{
					DebugLocation(262, 36);
					input.Consume();


					}
					break;

				default:
					goto loop9;
				}
			}

			loop9:
				;

			} finally { DebugExitSubRule(9); }


			}

			DebugLocation(262, 45);
			Match('#'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("DATETIME", 37);
			LeaveRule("DATETIME", 37);
			Leave_DATETIME();
		}
	}
	// $ANTLR end "DATETIME"

	partial void Enter_NAME();
	partial void Leave_NAME();

	// $ANTLR start "NAME"
	[GrammarRule("NAME")]
	private void mNAME()
	{
		Enter_NAME();
		EnterRule("NAME", 38);
		TraceIn("NAME", 38);
		try
		{
			int _type = NAME;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:265:6: ( '[' ( options {greedy=false; } : (~ ( ']' ) )* ) ']' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:265:8: '[' ( options {greedy=false; } : (~ ( ']' ) )* ) ']'
			{
			DebugLocation(265, 8);
			Match('['); 
			DebugLocation(265, 12);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:265:12: ( options {greedy=false; } : (~ ( ']' ) )* )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:265:39: (~ ( ']' ) )*
			{
			DebugLocation(265, 39);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:265:39: (~ ( ']' ) )*
			try { DebugEnterSubRule(10);
			while (true)
			{
				int alt10=2;
				try { DebugEnterDecision(10, decisionCanBacktrack[10]);
				int LA10_0 = input.LA(1);

				if (((LA10_0>='\u0000' && LA10_0<='\\')||(LA10_0>='^' && LA10_0<='\uFFFF')))
				{
					alt10=1;
				}


				} finally { DebugExitDecision(10); }
				switch ( alt10 )
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
					{
					DebugLocation(265, 39);
					input.Consume();


					}
					break;

				default:
					goto loop10;
				}
			}

			loop10:
				;

			} finally { DebugExitSubRule(10); }


			}

			DebugLocation(265, 48);
			Match(']'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("NAME", 38);
			LeaveRule("NAME", 38);
			Leave_NAME();
		}
	}
	// $ANTLR end "NAME"

	partial void Enter_E();
	partial void Leave_E();

	// $ANTLR start "E"
	[GrammarRule("E")]
	private void mE()
	{
		Enter_E();
		EnterRule("E", 39);
		TraceIn("E", 39);
		try
		{
			int _type = E;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:268:3: ( ( 'E' | 'e' ) ( '+' | '-' )? ( DIGIT )+ )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:268:5: ( 'E' | 'e' ) ( '+' | '-' )? ( DIGIT )+
			{
			DebugLocation(268, 5);
			if (input.LA(1)=='E'||input.LA(1)=='e')
			{
				input.Consume();

			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;}

			DebugLocation(268, 15);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:268:15: ( '+' | '-' )?
			int alt11=2;
			try { DebugEnterSubRule(11);
			try { DebugEnterDecision(11, decisionCanBacktrack[11]);
			int LA11_0 = input.LA(1);

			if ((LA11_0=='+'||LA11_0=='-'))
			{
				alt11=1;
			}
			} finally { DebugExitDecision(11); }
			switch (alt11)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
				{
				DebugLocation(268, 15);
				input.Consume();


				}
				break;

			}
			} finally { DebugExitSubRule(11); }

			DebugLocation(268, 26);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:268:26: ( DIGIT )+
			int cnt12=0;
			try { DebugEnterSubRule(12);
			while (true)
			{
				int alt12=2;
				try { DebugEnterDecision(12, decisionCanBacktrack[12]);
				int LA12_0 = input.LA(1);

				if (((LA12_0>='0' && LA12_0<='9')))
				{
					alt12=1;
				}


				} finally { DebugExitDecision(12); }
				switch (alt12)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
					{
					DebugLocation(268, 26);
					input.Consume();


					}
					break;

				default:
					if (cnt12 >= 1)
						goto loop12;

					EarlyExitException eee12 = new EarlyExitException( 12, input );
					DebugRecognitionException(eee12);
					throw eee12;
				}
				cnt12++;
			}
			loop12:
				;

			} finally { DebugExitSubRule(12); }


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("E", 39);
			LeaveRule("E", 39);
			Leave_E();
		}
	}
	// $ANTLR end "E"

	partial void Enter_LETTER();
	partial void Leave_LETTER();

	// $ANTLR start "LETTER"
	[GrammarRule("LETTER")]
	private void mLETTER()
	{
		Enter_LETTER();
		EnterRule("LETTER", 40);
		TraceIn("LETTER", 40);
		try
		{
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:272:2: ( 'a' .. 'z' | 'A' .. 'Z' | '_' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
			{
			DebugLocation(272, 2);
			if ((input.LA(1)>='A' && input.LA(1)<='Z')||input.LA(1)=='_'||(input.LA(1)>='a' && input.LA(1)<='z'))
			{
				input.Consume();

			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;}


			}

		}
		finally
		{
			TraceOut("LETTER", 40);
			LeaveRule("LETTER", 40);
			Leave_LETTER();
		}
	}
	// $ANTLR end "LETTER"

	partial void Enter_DIGIT();
	partial void Leave_DIGIT();

	// $ANTLR start "DIGIT"
	[GrammarRule("DIGIT")]
	private void mDIGIT()
	{
		Enter_DIGIT();
		EnterRule("DIGIT", 41);
		TraceIn("DIGIT", 41);
		try
		{
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:278:2: ( '0' .. '9' )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
			{
			DebugLocation(278, 2);
			if ((input.LA(1)>='0' && input.LA(1)<='9'))
			{
				input.Consume();

			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;}


			}

		}
		finally
		{
			TraceOut("DIGIT", 41);
			LeaveRule("DIGIT", 41);
			Leave_DIGIT();
		}
	}
	// $ANTLR end "DIGIT"

	partial void Enter_EscapeSequence();
	partial void Leave_EscapeSequence();

	// $ANTLR start "EscapeSequence"
	[GrammarRule("EscapeSequence")]
	private void mEscapeSequence()
	{
		Enter_EscapeSequence();
		EnterRule("EscapeSequence", 42);
		TraceIn("EscapeSequence", 42);
		try
		{
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:282:2: ( '\\\\' ( 'n' | 'r' | 't' | '\\'' | '\\\\' | UnicodeEscape ) )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:282:4: '\\\\' ( 'n' | 'r' | 't' | '\\'' | '\\\\' | UnicodeEscape )
			{
			DebugLocation(282, 4);
			Match('\\'); 
			DebugLocation(283, 4);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:283:4: ( 'n' | 'r' | 't' | '\\'' | '\\\\' | UnicodeEscape )
			int alt13=6;
			try { DebugEnterSubRule(13);
			try { DebugEnterDecision(13, decisionCanBacktrack[13]);
			switch (input.LA(1))
			{
			case 'n':
				{
				alt13=1;
				}
				break;
			case 'r':
				{
				alt13=2;
				}
				break;
			case 't':
				{
				alt13=3;
				}
				break;
			case '\'':
				{
				alt13=4;
				}
				break;
			case '\\':
				{
				alt13=5;
				}
				break;
			case 'u':
				{
				alt13=6;
				}
				break;
			default:
				{
					NoViableAltException nvae = new NoViableAltException("", 13, 0, input);

					DebugRecognitionException(nvae);
					throw nvae;
				}
			}

			} finally { DebugExitDecision(13); }
			switch (alt13)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:284:5: 'n'
				{
				DebugLocation(284, 5);
				Match('n'); 

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:285:4: 'r'
				{
				DebugLocation(285, 4);
				Match('r'); 

				}
				break;
			case 3:
				DebugEnterAlt(3);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:286:4: 't'
				{
				DebugLocation(286, 4);
				Match('t'); 

				}
				break;
			case 4:
				DebugEnterAlt(4);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:287:4: '\\''
				{
				DebugLocation(287, 4);
				Match('\''); 

				}
				break;
			case 5:
				DebugEnterAlt(5);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:288:4: '\\\\'
				{
				DebugLocation(288, 4);
				Match('\\'); 

				}
				break;
			case 6:
				DebugEnterAlt(6);
				// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:289:4: UnicodeEscape
				{
				DebugLocation(289, 4);
				mUnicodeEscape(); 

				}
				break;

			}
			} finally { DebugExitSubRule(13); }


			}

		}
		finally
		{
			TraceOut("EscapeSequence", 42);
			LeaveRule("EscapeSequence", 42);
			Leave_EscapeSequence();
		}
	}
	// $ANTLR end "EscapeSequence"

	partial void Enter_HexDigit();
	partial void Leave_HexDigit();

	// $ANTLR start "HexDigit"
	[GrammarRule("HexDigit")]
	private void mHexDigit()
	{
		Enter_HexDigit();
		EnterRule("HexDigit", 43);
		TraceIn("HexDigit", 43);
		try
		{
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:294:2: ( ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' ) )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:
			{
			DebugLocation(294, 2);
			if ((input.LA(1)>='0' && input.LA(1)<='9')||(input.LA(1)>='A' && input.LA(1)<='F')||(input.LA(1)>='a' && input.LA(1)<='f'))
			{
				input.Consume();

			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;}


			}

		}
		finally
		{
			TraceOut("HexDigit", 43);
			LeaveRule("HexDigit", 43);
			Leave_HexDigit();
		}
	}
	// $ANTLR end "HexDigit"

	partial void Enter_UnicodeEscape();
	partial void Leave_UnicodeEscape();

	// $ANTLR start "UnicodeEscape"
	[GrammarRule("UnicodeEscape")]
	private void mUnicodeEscape()
	{
		Enter_UnicodeEscape();
		EnterRule("UnicodeEscape", 44);
		TraceIn("UnicodeEscape", 44);
		try
		{
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:298:6: ( 'u' HexDigit HexDigit HexDigit HexDigit )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:298:12: 'u' HexDigit HexDigit HexDigit HexDigit
			{
			DebugLocation(298, 12);
			Match('u'); 
			DebugLocation(298, 16);
			mHexDigit(); 
			DebugLocation(298, 25);
			mHexDigit(); 
			DebugLocation(298, 34);
			mHexDigit(); 
			DebugLocation(298, 43);
			mHexDigit(); 

			}

		}
		finally
		{
			TraceOut("UnicodeEscape", 44);
			LeaveRule("UnicodeEscape", 44);
			Leave_UnicodeEscape();
		}
	}
	// $ANTLR end "UnicodeEscape"

	partial void Enter_WS();
	partial void Leave_WS();

	// $ANTLR start "WS"
	[GrammarRule("WS")]
	private void mWS()
	{
		Enter_WS();
		EnterRule("WS", 45);
		TraceIn("WS", 45);
		try
		{
			int _type = WS;
			int _channel = DefaultTokenChannel;
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:302:4: ( ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' ) )
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:302:7: ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' )
			{
			DebugLocation(302, 7);
			if ((input.LA(1)>='\t' && input.LA(1)<='\n')||(input.LA(1)>='\f' && input.LA(1)<='\r')||input.LA(1)==' ')
			{
				input.Consume();

			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;}

			DebugLocation(302, 37);
			_channel=Hidden;

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("WS", 45);
			LeaveRule("WS", 45);
			Leave_WS();
		}
	}
	// $ANTLR end "WS"

	public override void mTokens()
	{
		// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:8: ( T__19 | T__20 | T__21 | T__22 | T__23 | T__24 | T__25 | T__26 | T__27 | T__28 | T__29 | T__30 | T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | TRUE | FALSE | ID | INTEGER | FLOAT | STRING | DATETIME | NAME | E | WS )
		int alt14=40;
		try { DebugEnterDecision(14, decisionCanBacktrack[14]);
		try
		{
			alt14 = dfa14.Predict(input);
		}
		catch (NoViableAltException nvae)
		{
			DebugRecognitionException(nvae);
			throw;
		}
		} finally { DebugExitDecision(14); }
		switch (alt14)
		{
		case 1:
			DebugEnterAlt(1);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:10: T__19
			{
			DebugLocation(1, 10);
			mT__19(); 

			}
			break;
		case 2:
			DebugEnterAlt(2);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:16: T__20
			{
			DebugLocation(1, 16);
			mT__20(); 

			}
			break;
		case 3:
			DebugEnterAlt(3);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:22: T__21
			{
			DebugLocation(1, 22);
			mT__21(); 

			}
			break;
		case 4:
			DebugEnterAlt(4);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:28: T__22
			{
			DebugLocation(1, 28);
			mT__22(); 

			}
			break;
		case 5:
			DebugEnterAlt(5);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:34: T__23
			{
			DebugLocation(1, 34);
			mT__23(); 

			}
			break;
		case 6:
			DebugEnterAlt(6);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:40: T__24
			{
			DebugLocation(1, 40);
			mT__24(); 

			}
			break;
		case 7:
			DebugEnterAlt(7);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:46: T__25
			{
			DebugLocation(1, 46);
			mT__25(); 

			}
			break;
		case 8:
			DebugEnterAlt(8);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:52: T__26
			{
			DebugLocation(1, 52);
			mT__26(); 

			}
			break;
		case 9:
			DebugEnterAlt(9);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:58: T__27
			{
			DebugLocation(1, 58);
			mT__27(); 

			}
			break;
		case 10:
			DebugEnterAlt(10);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:64: T__28
			{
			DebugLocation(1, 64);
			mT__28(); 

			}
			break;
		case 11:
			DebugEnterAlt(11);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:70: T__29
			{
			DebugLocation(1, 70);
			mT__29(); 

			}
			break;
		case 12:
			DebugEnterAlt(12);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:76: T__30
			{
			DebugLocation(1, 76);
			mT__30(); 

			}
			break;
		case 13:
			DebugEnterAlt(13);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:82: T__31
			{
			DebugLocation(1, 82);
			mT__31(); 

			}
			break;
		case 14:
			DebugEnterAlt(14);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:88: T__32
			{
			DebugLocation(1, 88);
			mT__32(); 

			}
			break;
		case 15:
			DebugEnterAlt(15);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:94: T__33
			{
			DebugLocation(1, 94);
			mT__33(); 

			}
			break;
		case 16:
			DebugEnterAlt(16);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:100: T__34
			{
			DebugLocation(1, 100);
			mT__34(); 

			}
			break;
		case 17:
			DebugEnterAlt(17);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:106: T__35
			{
			DebugLocation(1, 106);
			mT__35(); 

			}
			break;
		case 18:
			DebugEnterAlt(18);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:112: T__36
			{
			DebugLocation(1, 112);
			mT__36(); 

			}
			break;
		case 19:
			DebugEnterAlt(19);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:118: T__37
			{
			DebugLocation(1, 118);
			mT__37(); 

			}
			break;
		case 20:
			DebugEnterAlt(20);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:124: T__38
			{
			DebugLocation(1, 124);
			mT__38(); 

			}
			break;
		case 21:
			DebugEnterAlt(21);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:130: T__39
			{
			DebugLocation(1, 130);
			mT__39(); 

			}
			break;
		case 22:
			DebugEnterAlt(22);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:136: T__40
			{
			DebugLocation(1, 136);
			mT__40(); 

			}
			break;
		case 23:
			DebugEnterAlt(23);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:142: T__41
			{
			DebugLocation(1, 142);
			mT__41(); 

			}
			break;
		case 24:
			DebugEnterAlt(24);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:148: T__42
			{
			DebugLocation(1, 148);
			mT__42(); 

			}
			break;
		case 25:
			DebugEnterAlt(25);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:154: T__43
			{
			DebugLocation(1, 154);
			mT__43(); 

			}
			break;
		case 26:
			DebugEnterAlt(26);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:160: T__44
			{
			DebugLocation(1, 160);
			mT__44(); 

			}
			break;
		case 27:
			DebugEnterAlt(27);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:166: T__45
			{
			DebugLocation(1, 166);
			mT__45(); 

			}
			break;
		case 28:
			DebugEnterAlt(28);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:172: T__46
			{
			DebugLocation(1, 172);
			mT__46(); 

			}
			break;
		case 29:
			DebugEnterAlt(29);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:178: T__47
			{
			DebugLocation(1, 178);
			mT__47(); 

			}
			break;
		case 30:
			DebugEnterAlt(30);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:184: T__48
			{
			DebugLocation(1, 184);
			mT__48(); 

			}
			break;
		case 31:
			DebugEnterAlt(31);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:190: TRUE
			{
			DebugLocation(1, 190);
			mTRUE(); 

			}
			break;
		case 32:
			DebugEnterAlt(32);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:195: FALSE
			{
			DebugLocation(1, 195);
			mFALSE(); 

			}
			break;
		case 33:
			DebugEnterAlt(33);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:201: ID
			{
			DebugLocation(1, 201);
			mID(); 

			}
			break;
		case 34:
			DebugEnterAlt(34);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:204: INTEGER
			{
			DebugLocation(1, 204);
			mINTEGER(); 

			}
			break;
		case 35:
			DebugEnterAlt(35);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:212: FLOAT
			{
			DebugLocation(1, 212);
			mFLOAT(); 

			}
			break;
		case 36:
			DebugEnterAlt(36);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:218: STRING
			{
			DebugLocation(1, 218);
			mSTRING(); 

			}
			break;
		case 37:
			DebugEnterAlt(37);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:225: DATETIME
			{
			DebugLocation(1, 225);
			mDATETIME(); 

			}
			break;
		case 38:
			DebugEnterAlt(38);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:234: NAME
			{
			DebugLocation(1, 234);
			mNAME(); 

			}
			break;
		case 39:
			DebugEnterAlt(39);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:239: E
			{
			DebugLocation(1, 239);
			mE(); 

			}
			break;
		case 40:
			DebugEnterAlt(40);
			// C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g:1:241: WS
			{
			DebugLocation(1, 241);
			mWS(); 

			}
			break;

		}

	}


	#region DFA
	DFA7 dfa7;
	DFA14 dfa14;

	protected override void InitDFAs()
	{
		base.InitDFAs();
		dfa7 = new DFA7(this);
		dfa14 = new DFA14(this);
	}

	private class DFA7 : DFA
	{
		private const string DFA7_eotS =
			"\x4\xFFFF";
		private const string DFA7_eofS =
			"\x4\xFFFF";
		private const string DFA7_minS =
			"\x2\x2E\x2\xFFFF";
		private const string DFA7_maxS =
			"\x1\x39\x1\x65\x2\xFFFF";
		private const string DFA7_acceptS =
			"\x2\xFFFF\x1\x1\x1\x2";
		private const string DFA7_specialS =
			"\x4\xFFFF}>";
		private static readonly string[] DFA7_transitionS =
			{
				"\x1\x2\x1\xFFFF\xA\x1",
				"\x1\x2\x1\xFFFF\xA\x1\xB\xFFFF\x1\x3\x1F\xFFFF\x1\x3",
				"",
				""
			};

		private static readonly short[] DFA7_eot = DFA.UnpackEncodedString(DFA7_eotS);
		private static readonly short[] DFA7_eof = DFA.UnpackEncodedString(DFA7_eofS);
		private static readonly char[] DFA7_min = DFA.UnpackEncodedStringToUnsignedChars(DFA7_minS);
		private static readonly char[] DFA7_max = DFA.UnpackEncodedStringToUnsignedChars(DFA7_maxS);
		private static readonly short[] DFA7_accept = DFA.UnpackEncodedString(DFA7_acceptS);
		private static readonly short[] DFA7_special = DFA.UnpackEncodedString(DFA7_specialS);
		private static readonly short[][] DFA7_transition;

		static DFA7()
		{
			int numStates = DFA7_transitionS.Length;
			DFA7_transition = new short[numStates][];
			for ( int i=0; i < numStates; i++ )
			{
				DFA7_transition[i] = DFA.UnpackEncodedString(DFA7_transitionS[i]);
			}
		}

		public DFA7( BaseRecognizer recognizer )
		{
			this.recognizer = recognizer;
			this.decisionNumber = 7;
			this.eot = DFA7_eot;
			this.eof = DFA7_eof;
			this.min = DFA7_min;
			this.max = DFA7_max;
			this.accept = DFA7_accept;
			this.special = DFA7_special;
			this.transition = DFA7_transition;
		}

		public override string Description { get { return "252:1: FLOAT : ( ( DIGIT )* '.' ( DIGIT )+ ( E )? | ( DIGIT )+ E );"; } }

		public override void Error(NoViableAltException nvae)
		{
			DebugRecognitionException(nvae);
		}
	}

	private class DFA14 : DFA
	{
		private const string DFA14_eotS =
			"\x1\xFFFF\x1\x21\x1\xFFFF\x1\x23\x8\xFFFF\x1\x27\x1\x29\x1\x2C\x2\xFFFF"+
			"\x3\x1E\x1\x31\x1\xFFFF\x3\x1E\x1\x36\x13\xFFFF\x2\x1E\x1\x39\x2\xFFFF"+
			"\x3\x1E\x2\xFFFF\x1\x3C\x1\x3D\x1\xFFFF\x2\x1E\x2\xFFFF\x1\x40\x1\x1E"+
			"\x1\xFFFF\x1\x42\x1\xFFFF";
		private const string DFA14_eofS =
			"\x43\xFFFF";
		private const string DFA14_minS =
			"\x1\x9\x1\x3D\x1\xFFFF\x1\x26\x8\xFFFF\x1\x3C\x2\x3D\x2\xFFFF\x1\x6E"+
			"\x1\x6F\x1\x72\x1\x7C\x1\xFFFF\x1\x72\x1\x61\x1\x2B\x1\x2E\x13\xFFFF"+
			"\x1\x64\x1\x74\x1\x30\x2\xFFFF\x1\x75\x1\x6C\x1\x30\x2\xFFFF\x2\x30\x1"+
			"\xFFFF\x1\x65\x1\x73\x2\xFFFF\x1\x30\x1\x65\x1\xFFFF\x1\x30\x1\xFFFF";
		private const string DFA14_maxS =
			"\x1\x7E\x1\x3D\x1\xFFFF\x1\x26\x8\xFFFF\x1\x3E\x1\x3D\x1\x3E\x2\xFFFF"+
			"\x1\x6E\x1\x6F\x1\x72\x1\x7C\x1\xFFFF\x1\x72\x1\x61\x1\x39\x1\x65\x13"+
			"\xFFFF\x1\x64\x1\x74\x1\x7A\x2\xFFFF\x1\x75\x1\x6C\x1\x39\x2\xFFFF\x2"+
			"\x7A\x1\xFFFF\x1\x65\x1\x73\x2\xFFFF\x1\x7A\x1\x65\x1\xFFFF\x1\x7A\x1"+
			"\xFFFF";
		private const string DFA14_acceptS =
			"\x2\xFFFF\x1\x3\x1\xFFFF\x1\x6\x1\x7\x1\x8\x1\x9\x1\xA\x1\xB\x1\xC\x1"+
			"\xD\x3\xFFFF\x1\x17\x1\x18\x4\xFFFF\x1\x1E\x4\xFFFF\x1\x23\x1\x24\x1"+
			"\x25\x1\x26\x1\x21\x1\x28\x1\x2\x1\x1\x1\x4\x1\x5\x1\xF\x1\x10\x1\x11"+
			"\x1\xE\x1\x13\x1\x12\x1\x15\x1\x16\x1\x14\x3\xFFFF\x1\x1D\x1\x1C\x3\xFFFF"+
			"\x1\x27\x1\x22\x2\xFFFF\x1\x1B\x2\xFFFF\x1\x19\x1\x1A\x2\xFFFF\x1\x1F"+
			"\x1\xFFFF\x1\x20";
		private const string DFA14_specialS =
			"\x43\xFFFF}>";
		private static readonly string[] DFA14_transitionS =
			{
				"\x2\x1F\x1\xFFFF\x2\x1F\x12\xFFFF\x1\x1F\x1\x1\x1\xFFFF\x1\x1C\x1\xFFFF"+
				"\x1\x2\x1\x3\x1\x1B\x1\x4\x1\x5\x1\x6\x1\x7\x1\x8\x1\x9\x1\x1A\x1\xA"+
				"\xA\x19\x1\xB\x1\xFFFF\x1\xC\x1\xD\x1\xE\x1\xF\x1\xFFFF\x4\x1E\x1\x18"+
				"\x15\x1E\x1\x1D\x2\xFFFF\x1\x10\x1\x1E\x1\xFFFF\x1\x11\x3\x1E\x1\x18"+
				"\x1\x17\x7\x1E\x1\x12\x1\x13\x4\x1E\x1\x16\x6\x1E\x1\xFFFF\x1\x14\x1"+
				"\xFFFF\x1\x15",
				"\x1\x20",
				"",
				"\x1\x22",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"\x1\x24\x1\x25\x1\x26",
				"\x1\x28",
				"\x1\x2A\x1\x2B",
				"",
				"",
				"\x1\x2D",
				"\x1\x2E",
				"\x1\x2F",
				"\x1\x30",
				"",
				"\x1\x32",
				"\x1\x33",
				"\x1\x35\x1\xFFFF\x1\x35\x2\xFFFF\xA\x34",
				"\x1\x1A\x1\xFFFF\xA\x19\xB\xFFFF\x1\x1A\x1F\xFFFF\x1\x1A",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"\x1\x37",
				"\x1\x38",
				"\xA\x1E\x7\xFFFF\x1A\x1E\x4\xFFFF\x1\x1E\x1\xFFFF\x1A\x1E",
				"",
				"",
				"\x1\x3A",
				"\x1\x3B",
				"\xA\x34",
				"",
				"",
				"\xA\x1E\x7\xFFFF\x1A\x1E\x4\xFFFF\x1\x1E\x1\xFFFF\x1A\x1E",
				"\xA\x1E\x7\xFFFF\x1A\x1E\x4\xFFFF\x1\x1E\x1\xFFFF\x1A\x1E",
				"",
				"\x1\x3E",
				"\x1\x3F",
				"",
				"",
				"\xA\x1E\x7\xFFFF\x1A\x1E\x4\xFFFF\x1\x1E\x1\xFFFF\x1A\x1E",
				"\x1\x41",
				"",
				"\xA\x1E\x7\xFFFF\x1A\x1E\x4\xFFFF\x1\x1E\x1\xFFFF\x1A\x1E",
				""
			};

		private static readonly short[] DFA14_eot = DFA.UnpackEncodedString(DFA14_eotS);
		private static readonly short[] DFA14_eof = DFA.UnpackEncodedString(DFA14_eofS);
		private static readonly char[] DFA14_min = DFA.UnpackEncodedStringToUnsignedChars(DFA14_minS);
		private static readonly char[] DFA14_max = DFA.UnpackEncodedStringToUnsignedChars(DFA14_maxS);
		private static readonly short[] DFA14_accept = DFA.UnpackEncodedString(DFA14_acceptS);
		private static readonly short[] DFA14_special = DFA.UnpackEncodedString(DFA14_specialS);
		private static readonly short[][] DFA14_transition;

		static DFA14()
		{
			int numStates = DFA14_transitionS.Length;
			DFA14_transition = new short[numStates][];
			for ( int i=0; i < numStates; i++ )
			{
				DFA14_transition[i] = DFA.UnpackEncodedString(DFA14_transitionS[i]);
			}
		}

		public DFA14( BaseRecognizer recognizer )
		{
			this.recognizer = recognizer;
			this.decisionNumber = 14;
			this.eot = DFA14_eot;
			this.eof = DFA14_eof;
			this.min = DFA14_min;
			this.max = DFA14_max;
			this.accept = DFA14_accept;
			this.special = DFA14_special;
			this.transition = DFA14_transition;
		}

		public override string Description { get { return "1:1: Tokens : ( T__19 | T__20 | T__21 | T__22 | T__23 | T__24 | T__25 | T__26 | T__27 | T__28 | T__29 | T__30 | T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | TRUE | FALSE | ID | INTEGER | FLOAT | STRING | DATETIME | NAME | E | WS );"; } }

		public override void Error(NoViableAltException nvae)
		{
			DebugRecognitionException(nvae);
		}
	}

 
	#endregion

}
