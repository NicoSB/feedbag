//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5-SNAPSHOT
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\Jonas\Documents\Visual Studio 2013\Projects\Grammar\Grammar\TypeNaming.g4 by ANTLR 4.5-SNAPSHOT

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace KaVE.Commons.Model.Names.CSharp.Parser
{

/**
 * Copyright 2016 Sebastian Proksch
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5-SNAPSHOT")]
[System.CLSCompliant(false)]
public partial class TypeNamingLexer : Lexer {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, UNKNOWN=27, POSNUM=28, LETTER=29, SIGN=30, WS=31, 
		EOL=32;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "UNKNOWN", "POSNUM", "LETTER", "SIGN", "DIGIT", "DIGIT_NON_ZERO", 
		"WS", "EOL"
	};


	public TypeNamingLexer(ICharStream input)
		: base(input)
	{
		_interp = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'->'", "','", "'d:'", "'arr('", "'):'", "'n:'", "'+'", "'.'", "'e:'", 
		"'i:'", "'s:'", "'''", "'['", "']'", "'('", "')'", "']..ctor'", "']..cctor'", 
		"'].'", "'params '", "'opt '", "'ref '", "'out '", "'this '", "'static'", 
		"'0'", "'?'", null, null, null, null, "'\n'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, "UNKNOWN", "POSNUM", "LETTER", "SIGN", "WS", "EOL"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[System.Obsolete("Use Vocabulary instead.")]
	public static readonly string[] tokenNames = GenerateTokenNames(DefaultVocabulary, _SymbolicNames.Length);

	private static string[] GenerateTokenNames(IVocabulary vocabulary, int length) {
		string[] tokenNames = new string[length];
		for (int i = 0; i < tokenNames.Length; i++) {
			tokenNames[i] = vocabulary.GetLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = vocabulary.GetSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}

		return tokenNames;
	}

	[System.Obsolete]
	public override string[] TokenNames
	{
		get
		{
			return tokenNames;
		}
	}

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "TypeNaming.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x2\"\xC5\b\x1\x4\x2"+
		"\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b\x4"+
		"\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4\x10"+
		"\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15\t\x15"+
		"\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A\x4\x1B"+
		"\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 \t \x4!"+
		"\t!\x4\"\t\"\x4#\t#\x3\x2\x3\x2\x3\x2\x3\x3\x3\x3\x3\x4\x3\x4\x3\x4\x3"+
		"\x5\x3\x5\x3\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\a\x3\a\x3\a\x3\b\x3\b"+
		"\x3\t\x3\t\x3\n\x3\n\x3\n\x3\v\x3\v\x3\v\x3\f\x3\f\x3\f\x3\r\x3\r\x3\xE"+
		"\x3\xE\x3\xF\x3\xF\x3\x10\x3\x10\x3\x11\x3\x11\x3\x12\x3\x12\x3\x12\x3"+
		"\x12\x3\x12\x3\x12\x3\x12\x3\x12\x3\x13\x3\x13\x3\x13\x3\x13\x3\x13\x3"+
		"\x13\x3\x13\x3\x13\x3\x13\x3\x14\x3\x14\x3\x14\x3\x15\x3\x15\x3\x15\x3"+
		"\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3"+
		"\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3"+
		"\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3"+
		"\x1A\x3\x1A\x3\x1A\x3\x1B\x3\x1B\x3\x1C\x3\x1C\x3\x1D\x3\x1D\a\x1D\xB0"+
		"\n\x1D\f\x1D\xE\x1D\xB3\v\x1D\x3\x1E\x3\x1E\x3\x1F\x3\x1F\x3 \x3 \x5 "+
		"\xBB\n \x3!\x3!\x3\"\x6\"\xC0\n\"\r\"\xE\"\xC1\x3#\x3#\x2\x2\x2$\x3\x2"+
		"\x3\x5\x2\x4\a\x2\x5\t\x2\x6\v\x2\a\r\x2\b\xF\x2\t\x11\x2\n\x13\x2\v\x15"+
		"\x2\f\x17\x2\r\x19\x2\xE\x1B\x2\xF\x1D\x2\x10\x1F\x2\x11!\x2\x12#\x2\x13"+
		"%\x2\x14\'\x2\x15)\x2\x16+\x2\x17-\x2\x18/\x2\x19\x31\x2\x1A\x33\x2\x1B"+
		"\x35\x2\x1C\x37\x2\x1D\x39\x2\x1E;\x2\x1F=\x2 ?\x2\x2\x41\x2\x2\x43\x2"+
		"!\x45\x2\"\x3\x2\x5\x4\x2\x43\\\x63|\v\x2##%&,-//\x31\x31<=??\x42\x42"+
		"\x61\x61\x4\x2\v\v\"\"\xC5\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3"+
		"\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3"+
		"\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2"+
		"\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2"+
		"\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2"+
		"\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2/"+
		"\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33\x3\x2\x2\x2\x2\x35\x3\x2\x2\x2"+
		"\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2\x2\x2\x2;\x3\x2\x2\x2\x2=\x3\x2\x2\x2"+
		"\x2\x43\x3\x2\x2\x2\x2\x45\x3\x2\x2\x2\x3G\x3\x2\x2\x2\x5J\x3\x2\x2\x2"+
		"\aL\x3\x2\x2\x2\tO\x3\x2\x2\x2\vT\x3\x2\x2\x2\rW\x3\x2\x2\x2\xFZ\x3\x2"+
		"\x2\x2\x11\\\x3\x2\x2\x2\x13^\x3\x2\x2\x2\x15\x61\x3\x2\x2\x2\x17\x64"+
		"\x3\x2\x2\x2\x19g\x3\x2\x2\x2\x1Bi\x3\x2\x2\x2\x1Dk\x3\x2\x2\x2\x1Fm\x3"+
		"\x2\x2\x2!o\x3\x2\x2\x2#q\x3\x2\x2\x2%y\x3\x2\x2\x2\'\x82\x3\x2\x2\x2"+
		")\x85\x3\x2\x2\x2+\x8D\x3\x2\x2\x2-\x92\x3\x2\x2\x2/\x97\x3\x2\x2\x2\x31"+
		"\x9C\x3\x2\x2\x2\x33\xA2\x3\x2\x2\x2\x35\xA9\x3\x2\x2\x2\x37\xAB\x3\x2"+
		"\x2\x2\x39\xAD\x3\x2\x2\x2;\xB4\x3\x2\x2\x2=\xB6\x3\x2\x2\x2?\xBA\x3\x2"+
		"\x2\x2\x41\xBC\x3\x2\x2\x2\x43\xBF\x3\x2\x2\x2\x45\xC3\x3\x2\x2\x2GH\a"+
		"/\x2\x2HI\a@\x2\x2I\x4\x3\x2\x2\x2JK\a.\x2\x2K\x6\x3\x2\x2\x2LM\a\x66"+
		"\x2\x2MN\a<\x2\x2N\b\x3\x2\x2\x2OP\a\x63\x2\x2PQ\at\x2\x2QR\at\x2\x2R"+
		"S\a*\x2\x2S\n\x3\x2\x2\x2TU\a+\x2\x2UV\a<\x2\x2V\f\x3\x2\x2\x2WX\ap\x2"+
		"\x2XY\a<\x2\x2Y\xE\x3\x2\x2\x2Z[\a-\x2\x2[\x10\x3\x2\x2\x2\\]\a\x30\x2"+
		"\x2]\x12\x3\x2\x2\x2^_\ag\x2\x2_`\a<\x2\x2`\x14\x3\x2\x2\x2\x61\x62\a"+
		"k\x2\x2\x62\x63\a<\x2\x2\x63\x16\x3\x2\x2\x2\x64\x65\au\x2\x2\x65\x66"+
		"\a<\x2\x2\x66\x18\x3\x2\x2\x2gh\a)\x2\x2h\x1A\x3\x2\x2\x2ij\a]\x2\x2j"+
		"\x1C\x3\x2\x2\x2kl\a_\x2\x2l\x1E\x3\x2\x2\x2mn\a*\x2\x2n \x3\x2\x2\x2"+
		"op\a+\x2\x2p\"\x3\x2\x2\x2qr\a_\x2\x2rs\a\x30\x2\x2st\a\x30\x2\x2tu\a"+
		"\x65\x2\x2uv\av\x2\x2vw\aq\x2\x2wx\at\x2\x2x$\x3\x2\x2\x2yz\a_\x2\x2z"+
		"{\a\x30\x2\x2{|\a\x30\x2\x2|}\a\x65\x2\x2}~\a\x65\x2\x2~\x7F\av\x2\x2"+
		"\x7F\x80\aq\x2\x2\x80\x81\at\x2\x2\x81&\x3\x2\x2\x2\x82\x83\a_\x2\x2\x83"+
		"\x84\a\x30\x2\x2\x84(\x3\x2\x2\x2\x85\x86\ar\x2\x2\x86\x87\a\x63\x2\x2"+
		"\x87\x88\at\x2\x2\x88\x89\a\x63\x2\x2\x89\x8A\ao\x2\x2\x8A\x8B\au\x2\x2"+
		"\x8B\x8C\a\"\x2\x2\x8C*\x3\x2\x2\x2\x8D\x8E\aq\x2\x2\x8E\x8F\ar\x2\x2"+
		"\x8F\x90\av\x2\x2\x90\x91\a\"\x2\x2\x91,\x3\x2\x2\x2\x92\x93\at\x2\x2"+
		"\x93\x94\ag\x2\x2\x94\x95\ah\x2\x2\x95\x96\a\"\x2\x2\x96.\x3\x2\x2\x2"+
		"\x97\x98\aq\x2\x2\x98\x99\aw\x2\x2\x99\x9A\av\x2\x2\x9A\x9B\a\"\x2\x2"+
		"\x9B\x30\x3\x2\x2\x2\x9C\x9D\av\x2\x2\x9D\x9E\aj\x2\x2\x9E\x9F\ak\x2\x2"+
		"\x9F\xA0\au\x2\x2\xA0\xA1\a\"\x2\x2\xA1\x32\x3\x2\x2\x2\xA2\xA3\au\x2"+
		"\x2\xA3\xA4\av\x2\x2\xA4\xA5\a\x63\x2\x2\xA5\xA6\av\x2\x2\xA6\xA7\ak\x2"+
		"\x2\xA7\xA8\a\x65\x2\x2\xA8\x34\x3\x2\x2\x2\xA9\xAA\a\x32\x2\x2\xAA\x36"+
		"\x3\x2\x2\x2\xAB\xAC\a\x41\x2\x2\xAC\x38\x3\x2\x2\x2\xAD\xB1\x5\x41!\x2"+
		"\xAE\xB0\x5? \x2\xAF\xAE\x3\x2\x2\x2\xB0\xB3\x3\x2\x2\x2\xB1\xAF\x3\x2"+
		"\x2\x2\xB1\xB2\x3\x2\x2\x2\xB2:\x3\x2\x2\x2\xB3\xB1\x3\x2\x2\x2\xB4\xB5"+
		"\t\x2\x2\x2\xB5<\x3\x2\x2\x2\xB6\xB7\t\x3\x2\x2\xB7>\x3\x2\x2\x2\xB8\xBB"+
		"\a\x32\x2\x2\xB9\xBB\x5\x41!\x2\xBA\xB8\x3\x2\x2\x2\xBA\xB9\x3\x2\x2\x2"+
		"\xBB@\x3\x2\x2\x2\xBC\xBD\x4\x33;\x2\xBD\x42\x3\x2\x2\x2\xBE\xC0\t\x4"+
		"\x2\x2\xBF\xBE\x3\x2\x2\x2\xC0\xC1\x3\x2\x2\x2\xC1\xBF\x3\x2\x2\x2\xC1"+
		"\xC2\x3\x2\x2\x2\xC2\x44\x3\x2\x2\x2\xC3\xC4\a\f\x2\x2\xC4\x46\x3\x2\x2"+
		"\x2\x6\x2\xB1\xBA\xC1\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace KaVE.Commons.Model.Names.CSharp.Parser
