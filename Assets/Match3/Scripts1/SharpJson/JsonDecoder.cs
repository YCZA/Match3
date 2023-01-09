using System.Collections.Generic;

namespace Match3.Scripts1.SharpJson
{
	// Token: 0x02000217 RID: 535
	public class JsonDecoder
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x00027FFE File Offset: 0x000263FE
		public JsonDecoder()
		{
			this.errorMessage = null;
			this.parseNumbersAsFloat = false;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00028014 File Offset: 0x00026414
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x0002801C File Offset: 0x0002641C
		public string errorMessage { get; private set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00028025 File Offset: 0x00026425
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x0002802D File Offset: 0x0002642D
		public bool parseNumbersAsFloat { get; set; }

		// Token: 0x06001061 RID: 4193 RVA: 0x00028036 File Offset: 0x00026436
		public object Decode(string text)
		{
			this.errorMessage = null;
			this.lexer = new Lexer(text);
			this.lexer.parseNumbersAsFloat = this.parseNumbersAsFloat;
			return this.ParseValue();
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00028064 File Offset: 0x00026464
		public static object DecodeText(string text)
		{
			JsonDecoder jsonDecoder = new JsonDecoder();
			return jsonDecoder.Decode(text);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00028080 File Offset: 0x00026480
		private IDictionary<string, object> ParseObject()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			this.lexer.NextToken();
			for (;;)
			{
				Lexer.Token token = this.lexer.LookAhead();
				if (token == Lexer.Token.None)
				{
					break;
				}
				if (token != Lexer.Token.Comma)
				{
					if (token == Lexer.Token.CurlyClose)
					{
						goto IL_56;
					}
					string key = this.EvalLexer<string>(this.lexer.ParseString());
					if (this.errorMessage != null)
					{
						goto Block_4;
					}
					token = this.lexer.NextToken();
					if (token != Lexer.Token.Colon)
					{
						goto Block_5;
					}
					object value = this.ParseValue();
					if (this.errorMessage != null)
					{
						goto Block_6;
					}
					dictionary[key] = value;
				}
				else
				{
					this.lexer.NextToken();
				}
			}
			this.TriggerError("Invalid token");
			return null;
			IL_56:
			this.lexer.NextToken();
			return dictionary;
			Block_4:
			return null;
			Block_5:
			this.TriggerError("Invalid token; expected ':'");
			return null;
			Block_6:
			return null;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00028158 File Offset: 0x00026558
		private IList<object> ParseArray()
		{
			List<object> list = new List<object>();
			this.lexer.NextToken();
			for (;;)
			{
				Lexer.Token token = this.lexer.LookAhead();
				if (token == Lexer.Token.None)
				{
					break;
				}
				if (token != Lexer.Token.Comma)
				{
					if (token == Lexer.Token.SquaredClose)
					{
						goto IL_56;
					}
					object item = this.ParseValue();
					if (this.errorMessage != null)
					{
						goto Block_4;
					}
					list.Add(item);
				}
				else
				{
					this.lexer.NextToken();
				}
			}
			this.TriggerError("Invalid token");
			return null;
			IL_56:
			this.lexer.NextToken();
			return list;
			Block_4:
			return null;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000281F0 File Offset: 0x000265F0
		private object ParseValue()
		{
			switch (this.lexer.LookAhead())
			{
			case Lexer.Token.Null:
				this.lexer.NextToken();
				return null;
			case Lexer.Token.True:
				this.lexer.NextToken();
				return true;
			case Lexer.Token.False:
				this.lexer.NextToken();
				return false;
			case Lexer.Token.String:
				return this.EvalLexer<string>(this.lexer.ParseString());
			case Lexer.Token.Number:
				if (this.parseNumbersAsFloat)
				{
					return this.EvalLexer<float>(this.lexer.ParseFloatNumber());
				}
				return this.EvalLexer<double>(this.lexer.ParseDoubleNumber());
			case Lexer.Token.CurlyOpen:
				return this.ParseObject();
			case Lexer.Token.SquaredOpen:
				return this.ParseArray();
			}
			this.TriggerError("Unable to parse value");
			return null;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000282DE File Offset: 0x000266DE
		private void TriggerError(string message)
		{
			this.errorMessage = string.Format("Error: '{0}' at line {1}", message, this.lexer.lineNumber);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00028301 File Offset: 0x00026701
		private T EvalLexer<T>(T value)
		{
			if (this.lexer.hasError)
			{
				this.TriggerError("Lexical error ocurred");
			}
			return value;
		}

		// Token: 0x04004131 RID: 16689
		private Lexer lexer;
	}
}
