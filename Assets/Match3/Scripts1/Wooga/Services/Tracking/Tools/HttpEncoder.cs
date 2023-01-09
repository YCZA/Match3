using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools
{
	// Token: 0x0200045B RID: 1115
	public class HttpEncoder
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x00086EE4 File Offset: 0x000852E4
		private static IDictionary<string, char> Entities
		{
			get
			{
				object obj = HttpEncoder.entitiesLock;
				IDictionary<string, char> result;
				lock (obj)
				{
					if (HttpEncoder.entities == null)
					{
						HttpEncoder.InitEntities();
					}
					result = HttpEncoder.entities;
				}
				return result;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x00086F30 File Offset: 0x00085330
		public static HttpEncoder Default
		{
			get
			{
				return HttpEncoder.defaultEncoder;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x00086F37 File Offset: 0x00085337
		public static HttpEncoder Current
		{
			get
			{
				return HttpEncoder.defaultEncoder;
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00086F3E File Offset: 0x0008533E
		protected internal virtual void HeaderNameValueEncode(string headerName, string headerValue, out string encodedHeaderName, out string encodedHeaderValue)
		{
			if (string.IsNullOrEmpty(headerName))
			{
				encodedHeaderName = headerName;
			}
			else
			{
				encodedHeaderName = HttpEncoder.EncodeHeaderString(headerName);
			}
			if (string.IsNullOrEmpty(headerValue))
			{
				encodedHeaderValue = headerValue;
			}
			else
			{
				encodedHeaderValue = HttpEncoder.EncodeHeaderString(headerValue);
			}
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00086F78 File Offset: 0x00085378
		private static void StringBuilderAppend(string s, ref StringBuilder sb)
		{
			if (sb == null)
			{
				sb = new StringBuilder(s);
			}
			else
			{
				sb.Append(s);
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00086F98 File Offset: 0x00085398
		private static string EncodeHeaderString(string input)
		{
			StringBuilder stringBuilder = null;
			foreach (char c in input)
			{
				if ((c < ' ' && c != '\t') || c == '\u007f')
				{
					HttpEncoder.StringBuilderAppend(string.Format("%{0:x2}", (int)c), ref stringBuilder);
				}
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			return input;
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00087003 File Offset: 0x00085403
		protected internal virtual void HtmlAttributeEncode(string value, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			output.Write(HttpEncoder.HtmlAttributeEncode(value));
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x0008702E File Offset: 0x0008542E
		protected internal virtual void HtmlDecode(string value, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(HttpEncoder.HtmlDecode(value));
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0008704D File Offset: 0x0008544D
		protected internal virtual void HtmlEncode(string value, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(HttpEncoder.HtmlEncode(value));
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x0008706C File Offset: 0x0008546C
		protected internal virtual byte[] UrlEncode(byte[] bytes, int offset, int count)
		{
			return HttpEncoder.UrlEncodeToBytes(bytes, offset, count);
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00087078 File Offset: 0x00085478
		protected internal virtual string UrlPathEncode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			MemoryStream memoryStream = new MemoryStream();
			int length = value.Length;
			for (int i = 0; i < length; i++)
			{
				HttpEncoder.UrlPathEncodeChar(value[i], memoryStream);
			}
			return Encoding.ASCII.GetString(memoryStream.ToArray());
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000870D0 File Offset: 0x000854D0
		internal static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			int num = bytes.Length;
			if (num == 0)
			{
				return new byte[0];
			}
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > num - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			MemoryStream memoryStream = new MemoryStream(count);
			int num2 = offset + count;
			for (int i = offset; i < num2; i++)
			{
				HttpEncoder.UrlEncodeChar((char)bytes[i], memoryStream, false);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00087164 File Offset: 0x00085564
		internal static string HtmlEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return string.Empty;
			}
			bool flag = false;
			foreach (char c in s)
			{
				if (c == '&' || c == '"' || c == '<' || c == '>' || c > '\u009f' || c == '\'')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = s.Length;
			for (int j = 0; j < length; j++)
			{
				char c2 = s[j];
				if (c2 != '&')
				{
					if (c2 != '\'')
					{
						switch (c2)
						{
						case '<':
							stringBuilder.Append("&lt;");
							break;
						default:
							switch (c2)
							{
							case '＜':
								stringBuilder.Append("&#65308;");
								break;
							default:
								if (c2 != '"')
								{
									if (c2 > '\u009f' && c2 < 'Ā')
									{
										stringBuilder.Append("&#");
										StringBuilder stringBuilder2 = stringBuilder;
										int num = (int)c2;
										stringBuilder2.Append(num.ToString());
										stringBuilder.Append(";");
									}
									else
									{
										stringBuilder.Append(c2);
									}
								}
								else
								{
									stringBuilder.Append("&quot;");
								}
								break;
							case '＞':
								stringBuilder.Append("&#65310;");
								break;
							}
							break;
						case '>':
							stringBuilder.Append("&gt;");
							break;
						}
					}
					else
					{
						stringBuilder.Append("&#39;");
					}
				}
				else
				{
					stringBuilder.Append("&amp;");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00087344 File Offset: 0x00085744
		internal static string HtmlAttributeEncode(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			bool flag = false;
			foreach (char c in s)
			{
				if (c == '&' || c == '"' || c == '<' || c == '\'')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = s.Length;
			for (int j = 0; j < length; j++)
			{
				char c2 = s[j];
				if (c2 != '&')
				{
					if (c2 != '\'')
					{
						if (c2 != '"')
						{
							if (c2 != '<')
							{
								stringBuilder.Append(c2);
							}
							else
							{
								stringBuilder.Append("&lt;");
							}
						}
						else
						{
							stringBuilder.Append("&quot;");
						}
					}
					else
					{
						stringBuilder.Append("&#39;");
					}
				}
				else
				{
					stringBuilder.Append("&amp;");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00087464 File Offset: 0x00085864
		internal static string HtmlDecode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return string.Empty;
			}
			if (s.IndexOf('&') == -1)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			int length = s.Length;
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (num == 0)
				{
					if (c == '&')
					{
						stringBuilder2.Append(c);
						stringBuilder.Append(c);
						num = 1;
					}
					else
					{
						stringBuilder3.Append(c);
					}
				}
				else if (c == '&')
				{
					num = 1;
					if (flag2)
					{
						stringBuilder2.Append(num2.ToString());
						flag2 = false;
					}
					stringBuilder3.Append(stringBuilder2.ToString());
					stringBuilder2.Length = 0;
					stringBuilder2.Append('&');
				}
				else if (num == 1)
				{
					if (c == ';')
					{
						num = 0;
						stringBuilder3.Append(stringBuilder2.ToString());
						stringBuilder3.Append(c);
						stringBuilder2.Length = 0;
					}
					else
					{
						num2 = 0;
						flag = false;
						if (c != '#')
						{
							num = 2;
						}
						else
						{
							num = 3;
						}
						stringBuilder2.Append(c);
						stringBuilder.Append(c);
					}
				}
				else if (num == 2)
				{
					stringBuilder2.Append(c);
					if (c == ';')
					{
						string text = stringBuilder2.ToString();
						if (text.Length > 1 && HttpEncoder.Entities.ContainsKey(text.Substring(1, text.Length - 2)))
						{
							text = HttpEncoder.Entities[text.Substring(1, text.Length - 2)].ToString();
						}
						stringBuilder3.Append(text);
						num = 0;
						stringBuilder2.Length = 0;
						stringBuilder.Length = 0;
					}
				}
				else if (num == 3)
				{
					if (c == ';')
					{
						if (num2 == 0)
						{
							stringBuilder3.Append(stringBuilder.ToString() + ";");
						}
						else if (num2 > 65535)
						{
							stringBuilder3.Append("&#");
							stringBuilder3.Append(num2.ToString());
							stringBuilder3.Append(";");
						}
						else
						{
							stringBuilder3.Append((char)num2);
						}
						num = 0;
						stringBuilder2.Length = 0;
						stringBuilder.Length = 0;
						flag2 = false;
					}
					else if (flag && Uri.IsHexDigit(c))
					{
						num2 = num2 * 16 + Uri.FromHex(c);
						flag2 = true;
						stringBuilder.Append(c);
					}
					else if (char.IsDigit(c))
					{
						num2 = num2 * 10 + (int)(c - '0');
						flag2 = true;
						stringBuilder.Append(c);
					}
					else if (num2 == 0 && (c == 'x' || c == 'X'))
					{
						flag = true;
						stringBuilder.Append(c);
					}
					else
					{
						num = 2;
						if (flag2)
						{
							stringBuilder2.Append(num2.ToString());
							flag2 = false;
						}
						stringBuilder2.Append(c);
					}
				}
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder3.Append(stringBuilder2.ToString());
			}
			else if (flag2)
			{
				stringBuilder3.Append(num2.ToString());
			}
			return stringBuilder3.ToString();
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000877E8 File Offset: 0x00085BE8
		internal static bool NotEncoded(char c)
		{
			return c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_';
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00087824 File Offset: 0x00085C24
		internal static void UrlEncodeChar(char c, Stream result, bool isUnicode)
		{
			if (c > 'ÿ')
			{
				result.WriteByte(37);
				result.WriteByte(117);
				int num = (int)(c >> 12);
				result.WriteByte((byte)HttpEncoder.hexChars[num]);
				num = (int)(c >> 8 & '\u000f');
				result.WriteByte((byte)HttpEncoder.hexChars[num]);
				num = (int)(c >> 4 & '\u000f');
				result.WriteByte((byte)HttpEncoder.hexChars[num]);
				num = (int)(c & '\u000f');
				result.WriteByte((byte)HttpEncoder.hexChars[num]);
				return;
			}
			if (c > ' ' && HttpEncoder.NotEncoded(c))
			{
				result.WriteByte((byte)c);
				return;
			}
			if (c == ' ')
			{
				result.WriteByte(43);
				return;
			}
			if (c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z')
			{
				if (isUnicode && c > '\u007f')
				{
					result.WriteByte(37);
					result.WriteByte(117);
					result.WriteByte(48);
					result.WriteByte(48);
				}
				else
				{
					result.WriteByte(37);
				}
				int num2 = (int)(c >> 4);
				result.WriteByte((byte)HttpEncoder.hexChars[num2]);
				num2 = (int)(c & '\u000f');
				result.WriteByte((byte)HttpEncoder.hexChars[num2]);
			}
			else
			{
				result.WriteByte((byte)c);
			}
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x0008796C File Offset: 0x00085D6C
		internal static void UrlPathEncodeChar(char c, Stream result)
		{
			if (c < '!' || c > '~')
			{
				byte[] bytes = Encoding.UTF8.GetBytes(c.ToString());
				for (int i = 0; i < bytes.Length; i++)
				{
					result.WriteByte(37);
					int num = bytes[i] >> 4;
					result.WriteByte((byte)HttpEncoder.hexChars[num]);
					num = (int)(bytes[i] & 15);
					result.WriteByte((byte)HttpEncoder.hexChars[num]);
				}
			}
			else if (c == ' ')
			{
				result.WriteByte(37);
				result.WriteByte(50);
				result.WriteByte(48);
			}
			else
			{
				result.WriteByte((byte)c);
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x00087A18 File Offset: 0x00085E18
		private static void InitEntities()
		{
			HttpEncoder.entities = new SortedDictionary<string, char>(StringComparer.Ordinal);
			HttpEncoder.entities.Add("nbsp", '\u00a0');
			HttpEncoder.entities.Add("iexcl", '¡');
			HttpEncoder.entities.Add("cent", '¢');
			HttpEncoder.entities.Add("pound", '£');
			HttpEncoder.entities.Add("curren", '¤');
			HttpEncoder.entities.Add("yen", '¥');
			HttpEncoder.entities.Add("brvbar", '¦');
			HttpEncoder.entities.Add("sect", '§');
			HttpEncoder.entities.Add("uml", '¨');
			HttpEncoder.entities.Add("copy", '©');
			HttpEncoder.entities.Add("ordf", 'ª');
			HttpEncoder.entities.Add("laquo", '«');
			HttpEncoder.entities.Add("not", '¬');
			HttpEncoder.entities.Add("shy", '­');
			HttpEncoder.entities.Add("reg", '®');
			HttpEncoder.entities.Add("macr", '¯');
			HttpEncoder.entities.Add("deg", '°');
			HttpEncoder.entities.Add("plusmn", '±');
			HttpEncoder.entities.Add("sup2", '²');
			HttpEncoder.entities.Add("sup3", '³');
			HttpEncoder.entities.Add("acute", '´');
			HttpEncoder.entities.Add("micro", 'µ');
			HttpEncoder.entities.Add("para", '¶');
			HttpEncoder.entities.Add("middot", '·');
			HttpEncoder.entities.Add("cedil", '¸');
			HttpEncoder.entities.Add("sup1", '¹');
			HttpEncoder.entities.Add("ordm", 'º');
			HttpEncoder.entities.Add("raquo", '»');
			HttpEncoder.entities.Add("frac14", '¼');
			HttpEncoder.entities.Add("frac12", '½');
			HttpEncoder.entities.Add("frac34", '¾');
			HttpEncoder.entities.Add("iquest", '¿');
			HttpEncoder.entities.Add("Agrave", 'À');
			HttpEncoder.entities.Add("Aacute", 'Á');
			HttpEncoder.entities.Add("Acirc", 'Â');
			HttpEncoder.entities.Add("Atilde", 'Ã');
			HttpEncoder.entities.Add("Auml", 'Ä');
			HttpEncoder.entities.Add("Aring", 'Å');
			HttpEncoder.entities.Add("AElig", 'Æ');
			HttpEncoder.entities.Add("Ccedil", 'Ç');
			HttpEncoder.entities.Add("Egrave", 'È');
			HttpEncoder.entities.Add("Eacute", 'É');
			HttpEncoder.entities.Add("Ecirc", 'Ê');
			HttpEncoder.entities.Add("Euml", 'Ë');
			HttpEncoder.entities.Add("Igrave", 'Ì');
			HttpEncoder.entities.Add("Iacute", 'Í');
			HttpEncoder.entities.Add("Icirc", 'Î');
			HttpEncoder.entities.Add("Iuml", 'Ï');
			HttpEncoder.entities.Add("ETH", 'Ð');
			HttpEncoder.entities.Add("Ntilde", 'Ñ');
			HttpEncoder.entities.Add("Ograve", 'Ò');
			HttpEncoder.entities.Add("Oacute", 'Ó');
			HttpEncoder.entities.Add("Ocirc", 'Ô');
			HttpEncoder.entities.Add("Otilde", 'Õ');
			HttpEncoder.entities.Add("Ouml", 'Ö');
			HttpEncoder.entities.Add("times", '×');
			HttpEncoder.entities.Add("Oslash", 'Ø');
			HttpEncoder.entities.Add("Ugrave", 'Ù');
			HttpEncoder.entities.Add("Uacute", 'Ú');
			HttpEncoder.entities.Add("Ucirc", 'Û');
			HttpEncoder.entities.Add("Uuml", 'Ü');
			HttpEncoder.entities.Add("Yacute", 'Ý');
			HttpEncoder.entities.Add("THORN", 'Þ');
			HttpEncoder.entities.Add("szlig", 'ß');
			HttpEncoder.entities.Add("agrave", 'à');
			HttpEncoder.entities.Add("aacute", 'á');
			HttpEncoder.entities.Add("acirc", 'â');
			HttpEncoder.entities.Add("atilde", 'ã');
			HttpEncoder.entities.Add("auml", 'ä');
			HttpEncoder.entities.Add("aring", 'å');
			HttpEncoder.entities.Add("aelig", 'æ');
			HttpEncoder.entities.Add("ccedil", 'ç');
			HttpEncoder.entities.Add("egrave", 'è');
			HttpEncoder.entities.Add("eacute", 'é');
			HttpEncoder.entities.Add("ecirc", 'ê');
			HttpEncoder.entities.Add("euml", 'ë');
			HttpEncoder.entities.Add("igrave", 'ì');
			HttpEncoder.entities.Add("iacute", 'í');
			HttpEncoder.entities.Add("icirc", 'î');
			HttpEncoder.entities.Add("iuml", 'ï');
			HttpEncoder.entities.Add("eth", 'ð');
			HttpEncoder.entities.Add("ntilde", 'ñ');
			HttpEncoder.entities.Add("ograve", 'ò');
			HttpEncoder.entities.Add("oacute", 'ó');
			HttpEncoder.entities.Add("ocirc", 'ô');
			HttpEncoder.entities.Add("otilde", 'õ');
			HttpEncoder.entities.Add("ouml", 'ö');
			HttpEncoder.entities.Add("divide", '÷');
			HttpEncoder.entities.Add("oslash", 'ø');
			HttpEncoder.entities.Add("ugrave", 'ù');
			HttpEncoder.entities.Add("uacute", 'ú');
			HttpEncoder.entities.Add("ucirc", 'û');
			HttpEncoder.entities.Add("uuml", 'ü');
			HttpEncoder.entities.Add("yacute", 'ý');
			HttpEncoder.entities.Add("thorn", 'þ');
			HttpEncoder.entities.Add("yuml", 'ÿ');
			HttpEncoder.entities.Add("fnof", 'ƒ');
			HttpEncoder.entities.Add("Alpha", 'Α');
			HttpEncoder.entities.Add("Beta", 'Β');
			HttpEncoder.entities.Add("Gamma", 'Γ');
			HttpEncoder.entities.Add("Delta", 'Δ');
			HttpEncoder.entities.Add("Epsilon", 'Ε');
			HttpEncoder.entities.Add("Zeta", 'Ζ');
			HttpEncoder.entities.Add("Eta", 'Η');
			HttpEncoder.entities.Add("Theta", 'Θ');
			HttpEncoder.entities.Add("Iota", 'Ι');
			HttpEncoder.entities.Add("Kappa", 'Κ');
			HttpEncoder.entities.Add("Lambda", 'Λ');
			HttpEncoder.entities.Add("Mu", 'Μ');
			HttpEncoder.entities.Add("Nu", 'Ν');
			HttpEncoder.entities.Add("Xi", 'Ξ');
			HttpEncoder.entities.Add("Omicron", 'Ο');
			HttpEncoder.entities.Add("Pi", 'Π');
			HttpEncoder.entities.Add("Rho", 'Ρ');
			HttpEncoder.entities.Add("Sigma", 'Σ');
			HttpEncoder.entities.Add("Tau", 'Τ');
			HttpEncoder.entities.Add("Upsilon", 'Υ');
			HttpEncoder.entities.Add("Phi", 'Φ');
			HttpEncoder.entities.Add("Chi", 'Χ');
			HttpEncoder.entities.Add("Psi", 'Ψ');
			HttpEncoder.entities.Add("Omega", 'Ω');
			HttpEncoder.entities.Add("alpha", 'α');
			HttpEncoder.entities.Add("beta", 'β');
			HttpEncoder.entities.Add("gamma", 'γ');
			HttpEncoder.entities.Add("delta", 'δ');
			HttpEncoder.entities.Add("epsilon", 'ε');
			HttpEncoder.entities.Add("zeta", 'ζ');
			HttpEncoder.entities.Add("eta", 'η');
			HttpEncoder.entities.Add("theta", 'θ');
			HttpEncoder.entities.Add("iota", 'ι');
			HttpEncoder.entities.Add("kappa", 'κ');
			HttpEncoder.entities.Add("lambda", 'λ');
			HttpEncoder.entities.Add("mu", 'μ');
			HttpEncoder.entities.Add("nu", 'ν');
			HttpEncoder.entities.Add("xi", 'ξ');
			HttpEncoder.entities.Add("omicron", 'ο');
			HttpEncoder.entities.Add("pi", 'π');
			HttpEncoder.entities.Add("rho", 'ρ');
			HttpEncoder.entities.Add("sigmaf", 'ς');
			HttpEncoder.entities.Add("sigma", 'σ');
			HttpEncoder.entities.Add("tau", 'τ');
			HttpEncoder.entities.Add("upsilon", 'υ');
			HttpEncoder.entities.Add("phi", 'φ');
			HttpEncoder.entities.Add("chi", 'χ');
			HttpEncoder.entities.Add("psi", 'ψ');
			HttpEncoder.entities.Add("omega", 'ω');
			HttpEncoder.entities.Add("thetasym", 'ϑ');
			HttpEncoder.entities.Add("upsih", 'ϒ');
			HttpEncoder.entities.Add("piv", 'ϖ');
			HttpEncoder.entities.Add("bull", '•');
			HttpEncoder.entities.Add("hellip", '…');
			HttpEncoder.entities.Add("prime", '′');
			HttpEncoder.entities.Add("Prime", '″');
			HttpEncoder.entities.Add("oline", '‾');
			HttpEncoder.entities.Add("frasl", '⁄');
			HttpEncoder.entities.Add("weierp", '℘');
			HttpEncoder.entities.Add("image", 'ℑ');
			HttpEncoder.entities.Add("real", 'ℜ');
			HttpEncoder.entities.Add("trade", '™');
			HttpEncoder.entities.Add("alefsym", 'ℵ');
			HttpEncoder.entities.Add("larr", '←');
			HttpEncoder.entities.Add("uarr", '↑');
			HttpEncoder.entities.Add("rarr", '→');
			HttpEncoder.entities.Add("darr", '↓');
			HttpEncoder.entities.Add("harr", '↔');
			HttpEncoder.entities.Add("crarr", '↵');
			HttpEncoder.entities.Add("lArr", '⇐');
			HttpEncoder.entities.Add("uArr", '⇑');
			HttpEncoder.entities.Add("rArr", '⇒');
			HttpEncoder.entities.Add("dArr", '⇓');
			HttpEncoder.entities.Add("hArr", '⇔');
			HttpEncoder.entities.Add("forall", '∀');
			HttpEncoder.entities.Add("part", '∂');
			HttpEncoder.entities.Add("exist", '∃');
			HttpEncoder.entities.Add("empty", '∅');
			HttpEncoder.entities.Add("nabla", '∇');
			HttpEncoder.entities.Add("isin", '∈');
			HttpEncoder.entities.Add("notin", '∉');
			HttpEncoder.entities.Add("ni", '∋');
			HttpEncoder.entities.Add("prod", '∏');
			HttpEncoder.entities.Add("sum", '∑');
			HttpEncoder.entities.Add("minus", '−');
			HttpEncoder.entities.Add("lowast", '∗');
			HttpEncoder.entities.Add("radic", '√');
			HttpEncoder.entities.Add("prop", '∝');
			HttpEncoder.entities.Add("infin", '∞');
			HttpEncoder.entities.Add("ang", '∠');
			HttpEncoder.entities.Add("and", '∧');
			HttpEncoder.entities.Add("or", '∨');
			HttpEncoder.entities.Add("cap", '∩');
			HttpEncoder.entities.Add("cup", '∪');
			HttpEncoder.entities.Add("int", '∫');
			HttpEncoder.entities.Add("there4", '∴');
			HttpEncoder.entities.Add("sim", '∼');
			HttpEncoder.entities.Add("cong", '≅');
			HttpEncoder.entities.Add("asymp", '≈');
			HttpEncoder.entities.Add("ne", '≠');
			HttpEncoder.entities.Add("equiv", '≡');
			HttpEncoder.entities.Add("le", '≤');
			HttpEncoder.entities.Add("ge", '≥');
			HttpEncoder.entities.Add("sub", '⊂');
			HttpEncoder.entities.Add("sup", '⊃');
			HttpEncoder.entities.Add("nsub", '⊄');
			HttpEncoder.entities.Add("sube", '⊆');
			HttpEncoder.entities.Add("supe", '⊇');
			HttpEncoder.entities.Add("oplus", '⊕');
			HttpEncoder.entities.Add("otimes", '⊗');
			HttpEncoder.entities.Add("perp", '⊥');
			HttpEncoder.entities.Add("sdot", '⋅');
			HttpEncoder.entities.Add("lceil", '⌈');
			HttpEncoder.entities.Add("rceil", '⌉');
			HttpEncoder.entities.Add("lfloor", '⌊');
			HttpEncoder.entities.Add("rfloor", '⌋');
			HttpEncoder.entities.Add("lang", '〈');
			HttpEncoder.entities.Add("rang", '〉');
			HttpEncoder.entities.Add("loz", '◊');
			HttpEncoder.entities.Add("spades", '♠');
			HttpEncoder.entities.Add("clubs", '♣');
			HttpEncoder.entities.Add("hearts", '♥');
			HttpEncoder.entities.Add("diams", '♦');
			HttpEncoder.entities.Add("quot", '"');
			HttpEncoder.entities.Add("amp", '&');
			HttpEncoder.entities.Add("lt", '<');
			HttpEncoder.entities.Add("gt", '>');
			HttpEncoder.entities.Add("OElig", 'Œ');
			HttpEncoder.entities.Add("oelig", 'œ');
			HttpEncoder.entities.Add("Scaron", 'Š');
			HttpEncoder.entities.Add("scaron", 'š');
			HttpEncoder.entities.Add("Yuml", 'Ÿ');
			HttpEncoder.entities.Add("circ", 'ˆ');
			HttpEncoder.entities.Add("tilde", '˜');
			HttpEncoder.entities.Add("ensp", '\u2002');
			HttpEncoder.entities.Add("emsp", '\u2003');
			HttpEncoder.entities.Add("thinsp", '\u2009');
			HttpEncoder.entities.Add("zwnj", '‌');
			HttpEncoder.entities.Add("zwj", '‍');
			HttpEncoder.entities.Add("lrm", '‎');
			HttpEncoder.entities.Add("rlm", '‏');
			HttpEncoder.entities.Add("ndash", '–');
			HttpEncoder.entities.Add("mdash", '—');
			HttpEncoder.entities.Add("lsquo", '‘');
			HttpEncoder.entities.Add("rsquo", '’');
			HttpEncoder.entities.Add("sbquo", '‚');
			HttpEncoder.entities.Add("ldquo", '“');
			HttpEncoder.entities.Add("rdquo", '”');
			HttpEncoder.entities.Add("bdquo", '„');
			HttpEncoder.entities.Add("dagger", '†');
			HttpEncoder.entities.Add("Dagger", '‡');
			HttpEncoder.entities.Add("permil", '‰');
			HttpEncoder.entities.Add("lsaquo", '‹');
			HttpEncoder.entities.Add("rsaquo", '›');
			HttpEncoder.entities.Add("euro", '€');
		}

		// Token: 0x04004B85 RID: 19333
		private static char[] hexChars = "0123456789abcdef".ToCharArray();

		// Token: 0x04004B86 RID: 19334
		private static object entitiesLock = new object();

		// Token: 0x04004B87 RID: 19335
		private static SortedDictionary<string, char> entities;

		// Token: 0x04004B88 RID: 19336
		private static HttpEncoder defaultEncoder = new HttpEncoder();
	}
}
