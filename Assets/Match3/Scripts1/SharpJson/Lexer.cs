using System;
using System.Globalization;
using System.Text;

namespace Match3.Scripts1.SharpJson
{
	// Token: 0x02000215 RID: 533
	internal class Lexer
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x000279A6 File Offset: 0x00025DA6
		public Lexer(string text)
		{
			this.Reset();
			this.json = text.ToCharArray();
			this.parseNumbersAsFloat = false;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x000279DE File Offset: 0x00025DDE
		public bool hasError
		{
			get
			{
				return !this.success;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x000279E9 File Offset: 0x00025DE9
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x000279F1 File Offset: 0x00025DF1
		public int lineNumber { get; private set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x000279FA File Offset: 0x00025DFA
		// (set) Token: 0x06001051 RID: 4177 RVA: 0x00027A02 File Offset: 0x00025E02
		public bool parseNumbersAsFloat { get; set; }

		// Token: 0x06001052 RID: 4178 RVA: 0x00027A0B File Offset: 0x00025E0B
		public void Reset()
		{
			this.index = 0;
			this.lineNumber = 1;
			this.success = true;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00027A24 File Offset: 0x00025E24
		public string ParseString()
		{
			int num = 0;
			StringBuilder stringBuilder = null;
			this.SkipWhiteSpaces();
			char c = this.json[this.index++];
			bool flag = false;
			bool flag2 = false;
			while (!flag2 && !flag)
			{
				if (this.index == this.json.Length)
				{
					break;
				}
				c = this.json[this.index++];
				if (c == '"')
				{
					flag2 = true;
					break;
				}
				if (c == '\\')
				{
					if (this.index == this.json.Length)
					{
						break;
					}
					c = this.json[this.index++];
					switch (c)
					{
					case 'r':
						this.stringBuffer[num++] = '\r';
						break;
					default:
						if (c != '"')
						{
							if (c != '/')
							{
								if (c != '\\')
								{
									if (c != 'b')
									{
										if (c != 'f')
										{
											if (c == 'n')
											{
												this.stringBuffer[num++] = '\n';
											}
										}
										else
										{
											this.stringBuffer[num++] = '\f';
										}
									}
									else
									{
										this.stringBuffer[num++] = '\b';
									}
								}
								else
								{
									this.stringBuffer[num++] = '\\';
								}
							}
							else
							{
								this.stringBuffer[num++] = '/';
							}
						}
						else
						{
							this.stringBuffer[num++] = '"';
						}
						break;
					case 't':
						this.stringBuffer[num++] = '\t';
						break;
					case 'u':
					{
						int num2 = this.json.Length - this.index;
						if (num2 >= 4)
						{
							string value = new string(this.json, this.index, 4);
							this.stringBuffer[num++] = (char)Convert.ToInt32(value, 16);
							this.index += 4;
						}
						else
						{
							flag = true;
						}
						break;
					}
					}
				}
				else
				{
					this.stringBuffer[num++] = c;
				}
				if (num >= this.stringBuffer.Length)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					stringBuilder.Append(this.stringBuffer, 0, num);
					num = 0;
				}
			}
			if (!flag2)
			{
				this.success = false;
				return null;
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			return new string(this.stringBuffer, 0, num);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00027C94 File Offset: 0x00026094
		private string GetNumberString()
		{
			this.SkipWhiteSpaces();
			int lastIndexOfNumber = this.GetLastIndexOfNumber(this.index);
			int length = lastIndexOfNumber - this.index + 1;
			string result = new string(this.json, this.index, length);
			this.index = lastIndexOfNumber + 1;
			return result;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00027CDC File Offset: 0x000260DC
		public float ParseFloatNumber()
		{
			string numberString = this.GetNumberString();
			float result;
			if (!float.TryParse(numberString, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return 0f;
			}
			return result;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00027D10 File Offset: 0x00026110
		public double ParseDoubleNumber()
		{
			string numberString = this.GetNumberString();
			double result;
			if (!double.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return 0.0;
			}
			return result;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00027D48 File Offset: 0x00026148
		private int GetLastIndexOfNumber(int index)
		{
			int i;
			for (i = index; i < this.json.Length; i++)
			{
				char c = this.json[i];
				if ((c < '0' || c > '9') && c != '+' && c != '-' && c != '.' && c != 'e' && c != 'E')
				{
					break;
				}
			}
			return i - 1;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00027DB8 File Offset: 0x000261B8
		private void SkipWhiteSpaces()
		{
			while (this.index < this.json.Length)
			{
				char c = this.json[this.index];
				if (c == '\n')
				{
					this.lineNumber++;
				}
				if (!char.IsWhiteSpace(this.json[this.index]))
				{
					break;
				}
				this.index++;
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00027E2C File Offset: 0x0002622C
		public Lexer.Token LookAhead()
		{
			this.SkipWhiteSpaces();
			int num = this.index;
			return Lexer.NextToken(this.json, ref num);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00027E53 File Offset: 0x00026253
		public Lexer.Token NextToken()
		{
			this.SkipWhiteSpaces();
			return Lexer.NextToken(this.json, ref this.index);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00027E6C File Offset: 0x0002626C
		private static Lexer.Token NextToken(char[] json, ref int index)
		{
			if (index == json.Length)
			{
				return Lexer.Token.None;
			}
			char c = json[index++];
			switch (c)
			{
			case ',':
				return Lexer.Token.Comma;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return Lexer.Token.Number;
			default:
				switch (c)
				{
				case '[':
					return Lexer.Token.SquaredOpen;
				default:
					switch (c)
					{
					case '{':
						return Lexer.Token.CurlyOpen;
					default:
					{
						if (c == '"')
						{
							return Lexer.Token.String;
						}
						index--;
						int num = json.Length - index;
						if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
						{
							index += 5;
							return Lexer.Token.False;
						}
						if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
						{
							index += 4;
							return Lexer.Token.True;
						}
						if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
						{
							index += 4;
							return Lexer.Token.Null;
						}
						return Lexer.Token.None;
					}
					case '}':
						return Lexer.Token.CurlyClose;
					}
					break;
				case ']':
					return Lexer.Token.SquaredClose;
				}
				break;
			case ':':
				return Lexer.Token.Colon;
			}
		}

		// Token: 0x0400411E RID: 16670
		private char[] json;

		// Token: 0x0400411F RID: 16671
		private int index;

		// Token: 0x04004120 RID: 16672
		private bool success = true;

		// Token: 0x04004121 RID: 16673
		private char[] stringBuffer = new char[4096];

		// Token: 0x02000216 RID: 534
		public enum Token
		{
			// Token: 0x04004123 RID: 16675
			None,
			// Token: 0x04004124 RID: 16676
			Null,
			// Token: 0x04004125 RID: 16677
			True,
			// Token: 0x04004126 RID: 16678
			False,
			// Token: 0x04004127 RID: 16679
			Colon,
			// Token: 0x04004128 RID: 16680
			Comma,
			// Token: 0x04004129 RID: 16681
			String,
			// Token: 0x0400412A RID: 16682
			Number,
			// Token: 0x0400412B RID: 16683
			CurlyOpen,
			// Token: 0x0400412C RID: 16684
			CurlyClose,
			// Token: 0x0400412D RID: 16685
			SquaredOpen,
			// Token: 0x0400412E RID: 16686
			SquaredClose
		}
	}
}
