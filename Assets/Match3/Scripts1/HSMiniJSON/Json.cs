using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Match3.Scripts1.HSMiniJSON
{
	// Token: 0x020001DC RID: 476
	public static class Json
	{
		// Token: 0x06000DFC RID: 3580 RVA: 0x00020D40 File Offset: 0x0001F140
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00020D50 File Offset: 0x0001F150
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x020001DD RID: 477
		private sealed class Parser : IDisposable
		{
			// Token: 0x06000DFE RID: 3582 RVA: 0x00020D58 File Offset: 0x0001F158
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06000DFF RID: 3583 RVA: 0x00020D6C File Offset: 0x0001F16C
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x06000E00 RID: 3584 RVA: 0x00020D90 File Offset: 0x0001F190
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x06000E01 RID: 3585 RVA: 0x00020DD0 File Offset: 0x0001F1D0
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06000E02 RID: 3586 RVA: 0x00020DE4 File Offset: 0x0001F1E4
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case Json.Parser.TOKEN.NONE:
						goto IL_37;
					default:
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							string text = this.ParseString();
							if (text == null)
							{
								goto Block_2;
							}
							if (this.NextToken != Json.Parser.TOKEN.COLON)
							{
								goto Block_3;
							}
							this.json.Read();
							dictionary[text] = this.ParseValue();
						}
						break;
					case Json.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			// Token: 0x06000E03 RID: 3587 RVA: 0x00020E70 File Offset: 0x0001F270
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case Json.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						if (nextToken == Json.Parser.TOKEN.NONE)
						{
							return null;
						}
						object item = this.ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case Json.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			// Token: 0x06000E04 RID: 3588 RVA: 0x00020EE8 File Offset: 0x0001F2E8
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06000E05 RID: 3589 RVA: 0x00020F04 File Offset: 0x0001F304
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				default:
					switch (token)
					{
					case Json.Parser.TOKEN.CURLY_OPEN:
						return this.ParseObject();
					case Json.Parser.TOKEN.SQUARED_OPEN:
						return this.ParseArray();
					}
					return null;
				}
			}

			// Token: 0x06000E06 RID: 3590 RVA: 0x00020F74 File Offset: 0x0001F374
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					if (nextChar != '"')
					{
						if (nextChar != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							switch (nextChar)
							{
							case 'r':
								stringBuilder.Append('\r');
								break;
							default:
								if (nextChar != '"' && nextChar != '/' && nextChar != '\\')
								{
									if (nextChar != 'b')
									{
										if (nextChar != 'f')
										{
											if (nextChar == 'n')
											{
												stringBuilder.Append('\n');
											}
										}
										else
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else
								{
									stringBuilder.Append(nextChar);
								}
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								char[] array = new char[4];
								for (int i = 0; i < 4; i++)
								{
									array[i] = this.NextChar;
								}
								stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
								break;
							}
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06000E07 RID: 3591 RVA: 0x000210F4 File Offset: 0x0001F4F4
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x06000E08 RID: 3592 RVA: 0x00021135 File Offset: 0x0001F535
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0002116E File Offset: 0x0001F56E
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00021180 File Offset: 0x0001F580
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00021194 File Offset: 0x0001F594
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x06000E0C RID: 3596 RVA: 0x000211E8 File Offset: 0x0001F5E8
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					switch (peekChar)
					{
					case ',':
						this.json.Read();
						return Json.Parser.TOKEN.COMMA;
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
						return Json.Parser.TOKEN.NUMBER;
					default:
						switch (peekChar)
						{
						case '[':
							return Json.Parser.TOKEN.SQUARED_OPEN;
						default:
							switch (peekChar)
							{
							case '{':
								return Json.Parser.TOKEN.CURLY_OPEN;
							default:
								if (peekChar != '"')
								{
									string nextWord = this.NextWord;
									if (nextWord != null)
									{
										if (nextWord == "false")
										{
											return Json.Parser.TOKEN.FALSE;
										}
										if (nextWord == "true")
										{
											return Json.Parser.TOKEN.TRUE;
										}
										if (nextWord == "null")
										{
											return Json.Parser.TOKEN.NULL;
										}
									}
									return Json.Parser.TOKEN.NONE;
								}
								return Json.Parser.TOKEN.STRING;
							case '}':
								this.json.Read();
								return Json.Parser.TOKEN.CURLY_CLOSE;
							}
							break;
						case ']':
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						break;
					case ':':
						return Json.Parser.TOKEN.COLON;
					}
				}
			}

			// Token: 0x04003FDC RID: 16348
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04003FDD RID: 16349
			private StringReader json;

			// Token: 0x020001DE RID: 478
			private enum TOKEN
			{
				// Token: 0x04003FDF RID: 16351
				NONE,
				// Token: 0x04003FE0 RID: 16352
				CURLY_OPEN,
				// Token: 0x04003FE1 RID: 16353
				CURLY_CLOSE,
				// Token: 0x04003FE2 RID: 16354
				SQUARED_OPEN,
				// Token: 0x04003FE3 RID: 16355
				SQUARED_CLOSE,
				// Token: 0x04003FE4 RID: 16356
				COLON,
				// Token: 0x04003FE5 RID: 16357
				COMMA,
				// Token: 0x04003FE6 RID: 16358
				STRING,
				// Token: 0x04003FE7 RID: 16359
				NUMBER,
				// Token: 0x04003FE8 RID: 16360
				TRUE,
				// Token: 0x04003FE9 RID: 16361
				FALSE,
				// Token: 0x04003FEA RID: 16362
				NULL
			}
		}

		// Token: 0x020001DF RID: 479
		private sealed class Serializer
		{
			// Token: 0x06000E0D RID: 3597 RVA: 0x00021311 File Offset: 0x0001F711
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06000E0E RID: 3598 RVA: 0x00021324 File Offset: 0x0001F724
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06000E0F RID: 3599 RVA: 0x0002134C File Offset: 0x0001F74C
			private void SerializeValue(object value)
			{
				string str;
				IList anArray;
				IDictionary obj;
				if (value == null)
				{
					this.builder.Append("null");
				}
				else if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
				}
				else if (value is bool)
				{
					this.builder.Append((!(bool)value) ? "false" : "true");
				}
				else if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray);
				}
				else if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
				}
				else if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			// Token: 0x06000E10 RID: 3600 RVA: 0x00021420 File Offset: 0x0001F820
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				IEnumerator enumerator = obj.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeString(obj2.ToString());
						this.builder.Append(':');
						this.SerializeValue(obj[obj2]);
						flag = false;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this.builder.Append('}');
			}

			// Token: 0x06000E11 RID: 3601 RVA: 0x000214D4 File Offset: 0x0001F8D4
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				IEnumerator enumerator = anArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object value = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeValue(value);
						flag = false;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this.builder.Append(']');
			}

			// Token: 0x06000E12 RID: 3602 RVA: 0x00021564 File Offset: 0x0001F964
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								int num = Convert.ToInt32(c);
								if (num >= 32 && num <= 126)
								{
									this.builder.Append(c);
								}
								else
								{
									this.builder.Append("\\u");
									this.builder.Append(num.ToString("x4"));
								}
							}
							else
							{
								this.builder.Append("\\\\");
							}
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					}
				}
				this.builder.Append('"');
			}

			// Token: 0x06000E13 RID: 3603 RVA: 0x000216D8 File Offset: 0x0001FAD8
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
				}
				else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
				}
				else if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}

			// Token: 0x04003FEB RID: 16363
			private StringBuilder builder;
		}
	}
}
