using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools
{
	// Token: 0x0200045C RID: 1116
	public sealed class HttpUtility
	{
		// Token: 0x06002057 RID: 8279 RVA: 0x00088DE0 File Offset: 0x000871E0
		public static void HtmlAttributeEncode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			HttpEncoder.Current.HtmlAttributeEncode(s, output);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00088E00 File Offset: 0x00087200
		public static string HtmlAttributeEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				HttpEncoder.Current.HtmlAttributeEncode(s, stringWriter);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00088E54 File Offset: 0x00087254
		public static string UrlDecode(string str)
		{
			return HttpUtility.UrlDecode(str, Encoding.UTF8);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00088E61 File Offset: 0x00087261
		private static char[] GetChars(MemoryStream b, Encoding e)
		{
			return e.GetChars(b.GetBuffer(), 0, (int)b.Length);
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00088E78 File Offset: 0x00087278
		private static void WriteCharBytes(IList buf, char ch, Encoding e)
		{
			if (ch > 'ÿ')
			{
				foreach (byte b in e.GetBytes(new char[]
				{
					ch
				}))
				{
					buf.Add(b);
				}
			}
			else
			{
				buf.Add((byte)ch);
			}
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x00088EDC File Offset: 0x000872DC
		public static string UrlDecode(string s, Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
			{
				return s;
			}
			if (e == null)
			{
				e = Encoding.UTF8;
			}
			long num = (long)s.Length;
			List<byte> list = new List<byte>();
			int num2 = 0;
			while ((long)num2 < num)
			{
				char c = s[num2];
				if (c == '%' && (long)(num2 + 2) < num && s[num2 + 1] != '%')
				{
					int @char;
					if (s[num2 + 1] == 'u' && (long)(num2 + 5) < num)
					{
						@char = HttpUtility.GetChar(s, num2 + 2, 4);
						if (@char != -1)
						{
							HttpUtility.WriteCharBytes(list, (char)@char, e);
							num2 += 5;
						}
						else
						{
							HttpUtility.WriteCharBytes(list, '%', e);
						}
					}
					else if ((@char = HttpUtility.GetChar(s, num2 + 1, 2)) != -1)
					{
						HttpUtility.WriteCharBytes(list, (char)@char, e);
						num2 += 2;
					}
					else
					{
						HttpUtility.WriteCharBytes(list, '%', e);
					}
				}
				else if (c == '+')
				{
					HttpUtility.WriteCharBytes(list, ' ', e);
				}
				else
				{
					HttpUtility.WriteCharBytes(list, c, e);
				}
				num2++;
			}
			byte[] bytes = list.ToArray();
			return e.GetString(bytes);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00089024 File Offset: 0x00087424
		public static string UrlDecode(byte[] bytes, Encoding e)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpUtility.UrlDecode(bytes, 0, bytes.Length, e);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x0008903C File Offset: 0x0008743C
		private static int GetInt(byte b)
		{
			char c = (char)b;
			if (c >= '0' && c <= '9')
			{
				return (int)(c - '0');
			}
			if (c >= 'a' && c <= 'f')
			{
				return (int)(c - 'a' + '\n');
			}
			if (c >= 'A' && c <= 'F')
			{
				return (int)(c - 'A' + '\n');
			}
			return -1;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00089094 File Offset: 0x00087494
		private static int GetChar(byte[] bytes, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				int @int = HttpUtility.GetInt(bytes[i]);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000890D4 File Offset: 0x000874D4
		private static int GetChar(string str, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				char c = str[i];
				if (c > '\u007f')
				{
					return -1;
				}
				int @int = HttpUtility.GetInt((byte)c);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00089128 File Offset: 0x00087528
		public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
		{
			if (bytes == null)
			{
				return null;
			}
			if (count == 0)
			{
				return string.Empty;
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			StringBuilder stringBuilder = new StringBuilder();
			MemoryStream memoryStream = new MemoryStream();
			int num = count + offset;
			int i = offset;
			while (i < num)
			{
				if (bytes[i] != 37 || i + 2 >= count || bytes[i + 1] == 37)
				{
					goto IL_123;
				}
				if (bytes[i + 1] == 117 && i + 5 < num)
				{
					if (memoryStream.Length > 0L)
					{
						stringBuilder.Append(HttpUtility.GetChars(memoryStream, e));
						memoryStream.SetLength(0L);
					}
					int @char = HttpUtility.GetChar(bytes, i + 2, 4);
					if (@char == -1)
					{
						goto IL_123;
					}
					stringBuilder.Append((char)@char);
					i += 5;
				}
				else
				{
					int @char;
					if ((@char = HttpUtility.GetChar(bytes, i + 1, 2)) == -1)
					{
						goto IL_123;
					}
					memoryStream.WriteByte((byte)@char);
					i += 2;
				}
				IL_16B:
				i++;
				continue;
				IL_123:
				if (memoryStream.Length > 0L)
				{
					stringBuilder.Append(HttpUtility.GetChars(memoryStream, e));
					memoryStream.SetLength(0L);
				}
				if (bytes[i] == 43)
				{
					stringBuilder.Append(' ');
					goto IL_16B;
				}
				stringBuilder.Append((char)bytes[i]);
				goto IL_16B;
			}
			if (memoryStream.Length > 0L)
			{
				stringBuilder.Append(HttpUtility.GetChars(memoryStream, e));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000892D1 File Offset: 0x000876D1
		public static byte[] UrlDecodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpUtility.UrlDecodeToBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000892E5 File Offset: 0x000876E5
		public static byte[] UrlDecodeToBytes(string str)
		{
			return HttpUtility.UrlDecodeToBytes(str, Encoding.UTF8);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000892F2 File Offset: 0x000876F2
		public static byte[] UrlDecodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			return HttpUtility.UrlDecodeToBytes(e.GetBytes(str));
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x0008931C File Offset: 0x0008771C
		public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			if (count == 0)
			{
				return new byte[0];
			}
			int num = bytes.Length;
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset > num - count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			MemoryStream memoryStream = new MemoryStream();
			int num2 = offset + count;
			for (int i = offset; i < num2; i++)
			{
				char c = (char)bytes[i];
				if (c == '+')
				{
					c = ' ';
				}
				else if (c == '%' && i < num2 - 2)
				{
					int @char = HttpUtility.GetChar(bytes, i + 1, 2);
					if (@char != -1)
					{
						c = (char)@char;
						i += 2;
					}
				}
				memoryStream.WriteByte((byte)c);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000893E8 File Offset: 0x000877E8
		public static string UrlEncode(string str)
		{
			return HttpUtility.UrlEncode(str, Encoding.UTF8);
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000893F8 File Offset: 0x000877F8
		public static string UrlEncode(string s, Encoding Enc)
		{
			if (s == null)
			{
				return null;
			}
			if (s == string.Empty)
			{
				return string.Empty;
			}
			bool flag = false;
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z')
				{
					if (!HttpEncoder.NotEncoded(c))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return s;
			}
			byte[] bytes = new byte[Enc.GetMaxByteCount(s.Length)];
			int bytes2 = Enc.GetBytes(s, 0, s.Length, bytes, 0);
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytes(bytes, 0, bytes2));
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000894CF File Offset: 0x000878CF
		public static string UrlEncode(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return string.Empty;
			}
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytes(bytes, 0, bytes.Length));
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000894FB File Offset: 0x000878FB
		public static string UrlEncode(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return string.Empty;
			}
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytes(bytes, offset, count));
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x00089525 File Offset: 0x00087925
		public static byte[] UrlEncodeToBytes(string str)
		{
			return HttpUtility.UrlEncodeToBytes(str, Encoding.UTF8);
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x00089534 File Offset: 0x00087934
		public static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			if (str.Length == 0)
			{
				return new byte[0];
			}
			byte[] bytes = e.GetBytes(str);
			return HttpUtility.UrlEncodeToBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x0008956D File Offset: 0x0008796D
		public static byte[] UrlEncodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return new byte[0];
			}
			return HttpUtility.UrlEncodeToBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x00089590 File Offset: 0x00087990
		public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpEncoder.Current.UrlEncode(bytes, offset, count);
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000895A7 File Offset: 0x000879A7
		public static string UrlEncodeUnicode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeUnicodeToBytes(str));
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000895C4 File Offset: 0x000879C4
		public static byte[] UrlEncodeUnicodeToBytes(string str)
		{
			if (str == null)
			{
				return null;
			}
			if (str.Length == 0)
			{
				return new byte[0];
			}
			MemoryStream memoryStream = new MemoryStream(str.Length);
			foreach (char c in str)
			{
				HttpEncoder.UrlEncodeChar(c, memoryStream, true);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00089628 File Offset: 0x00087A28
		public static string HtmlDecode(string s)
		{
			if (s == null)
			{
				return null;
			}
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				HttpEncoder.Current.HtmlDecode(s, stringWriter);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0008967C File Offset: 0x00087A7C
		public static void HtmlDecode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (!string.IsNullOrEmpty(s))
			{
				HttpEncoder.Current.HtmlDecode(s, output);
			}
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000896A8 File Offset: 0x00087AA8
		public static string HtmlEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				HttpEncoder.Current.HtmlEncode(s, stringWriter);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000896FC File Offset: 0x00087AFC
		public static void HtmlEncode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (!string.IsNullOrEmpty(s))
			{
				HttpEncoder.Current.HtmlEncode(s, output);
			}
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00089726 File Offset: 0x00087B26
		public static string HtmlEncode(object value)
		{
			if (value == null)
			{
				return null;
			}
			return HttpUtility.HtmlEncode(value.ToString());
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0008973B File Offset: 0x00087B3B
		public static string JavaScriptStringEncode(string value)
		{
			return HttpUtility.JavaScriptStringEncode(value, false);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00089744 File Offset: 0x00087B44
		public static string JavaScriptStringEncode(string value, bool addDoubleQuotes)
		{
			if (string.IsNullOrEmpty(value))
			{
				return (!addDoubleQuotes) ? string.Empty : "\"\"";
			}
			int length = value.Length;
			bool flag = false;
			for (int i = 0; i < length; i++)
			{
				char c = value[i];
				if ((c >= '\0' && c <= '\u001f') || c == '"' || c == '\'' || c == '<' || c == '>' || c == '\\')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return (!addDoubleQuotes) ? value : ("\"" + value + "\"");
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (addDoubleQuotes)
			{
				stringBuilder.Append('"');
			}
			for (int j = 0; j < length; j++)
			{
				char c = value[j];
				if ((c >= '\0' && c <= '\a') || (c == '\v' || (c >= '\u000e' && c <= '\u001f')) || c == '\'' || c == '<' || c == '>')
				{
					stringBuilder.AppendFormat("\\u{0:x4}", (int)c);
				}
				else
				{
					int num = (int)c;
					switch (num)
					{
					case 8:
						stringBuilder.Append("\\b");
						break;
					case 9:
						stringBuilder.Append("\\t");
						break;
					case 10:
						stringBuilder.Append("\\n");
						break;
					default:
						if (num != 34)
						{
							if (num != 92)
							{
								stringBuilder.Append(c);
							}
							else
							{
								stringBuilder.Append("\\\\");
							}
						}
						else
						{
							stringBuilder.Append("\\\"");
						}
						break;
					case 12:
						stringBuilder.Append("\\f");
						break;
					case 13:
						stringBuilder.Append("\\r");
						break;
					}
				}
			}
			if (addDoubleQuotes)
			{
				stringBuilder.Append('"');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x00089960 File Offset: 0x00087D60
		public static string UrlPathEncode(string s)
		{
			return HttpEncoder.Current.UrlPathEncode(s);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0008996D File Offset: 0x00087D6D
		public static NameValueCollection ParseQueryString(string query)
		{
			return HttpUtility.ParseQueryString(query, Encoding.UTF8);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x0008997C File Offset: 0x00087D7C
		public static NameValueCollection ParseQueryString(string query, Encoding encoding)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
			{
				return new HttpUtility.HttpQSCollection();
			}
			if (query[0] == '?')
			{
				query = query.Substring(1);
			}
			NameValueCollection result = new HttpUtility.HttpQSCollection();
			HttpUtility.ParseQueryString(query, encoding, result);
			return result;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000899FC File Offset: 0x00087DFC
		internal static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
		{
			if (query.Length == 0)
			{
				return;
			}
			string text = HttpUtility.HtmlDecode(query);
			int length = text.Length;
			int i = 0;
			bool flag = true;
			while (i <= length)
			{
				int num = -1;
				int num2 = -1;
				for (int j = i; j < length; j++)
				{
					if (num == -1 && text[j] == '=')
					{
						num = j + 1;
					}
					else if (text[j] == '&')
					{
						num2 = j;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					if (text[i] == '?')
					{
						i++;
					}
				}
				string name;
				if (num == -1)
				{
					name = null;
					num = i;
				}
				else
				{
					name = HttpUtility.UrlDecode(text.Substring(i, num - i - 1), encoding);
				}
				if (num2 < 0)
				{
					i = -1;
					num2 = text.Length;
				}
				else
				{
					i = num2 + 1;
				}
				string val = HttpUtility.UrlDecode(text.Substring(num, num2 - num), encoding);
				result.Add(name, val);
				if (i == -1)
				{
					break;
				}
			}
		}

		// Token: 0x0200045D RID: 1117
		private sealed class HttpQSCollection : NameValueCollection
		{
			// Token: 0x0600207C RID: 8316 RVA: 0x00089B1C File Offset: 0x00087F1C
			public override string ToString()
			{
				int count = this.Count;
				if (count == 0)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder();
				string[] allKeys = this.AllKeys;
				for (int i = 0; i < count; i++)
				{
					stringBuilder.AppendFormat("{0}={1}&", allKeys[i], HttpUtility.UrlEncode(base[allKeys[i]]));
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Length--;
				}
				return stringBuilder.ToString();
			}
		}
	}
}
