using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

// Token: 0x02000AD3 RID: 2771
namespace Match3.Scripts1
{
	public static class StringExtensions
	{
		// Token: 0x060041AF RID: 16815 RVA: 0x00153370 File Offset: 0x00151770
		public static Stream ToStream(this string str)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(str);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x001533A0 File Offset: 0x001517A0
		public static HashSet<char> ToHashset(this string str)
		{
			HashSet<char> hashSet = new HashSet<char>();
			foreach (char item in str)
			{
				hashSet.Add(item);
			}
			return hashSet;
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x001533DD File Offset: 0x001517DD
		public static string ToLocalUpper(this string str, CultureInfo cultureInfo)
		{
			if (cultureInfo == null)
			{
				return str.ToUpper();
			}
			return str.ToUpper(cultureInfo);
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x001533F3 File Offset: 0x001517F3
		public static bool EqualsIgnoreCase(this string str, string other)
		{
			return (str == null && other == null) || str.EqualsIgnoreCase(other, true);
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x0015340C File Offset: 0x0015180C
		public static bool EqualsIgnoreCase(this string str, string other, bool useCurrentCulture)
		{
			return (str == null && other == null) || str.Equals(other, (!useCurrentCulture) ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x00153431 File Offset: 0x00151831
		public static bool StartsWithIgnoreCase(this string str, string other)
		{
			return (str == null && other == null) || str.StartsWithIgnoreCase(other, true);
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0015344A File Offset: 0x0015184A
		public static bool StartsWithIgnoreCase(this string str, string other, bool useCurrentCulture)
		{
			return (str == null && other == null) || str.StartsWith(other, (!useCurrentCulture) ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0015346F File Offset: 0x0015186F
		public static bool EndsWithIgnoreCase(this string str, string other)
		{
			return (str == null && other == null) || str.EndsWithIgnoreCase(other, true);
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x00153488 File Offset: 0x00151888
		public static bool EndsWithIgnoreCase(this string str, string other, bool useCurrentCulture)
		{
			return (str == null && other == null) || str.EndsWith(other, (!useCurrentCulture) ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x001534AD File Offset: 0x001518AD
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return toCheck == null || source.IndexOf(toCheck, comp) >= 0;
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x001534C6 File Offset: 0x001518C6
		public static bool ContainsIgnoreCase(this string source, string toCheck)
		{
			return source.Contains(toCheck, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x001534D0 File Offset: 0x001518D0
		public static string CombinePath(this string str, params string[] otherStrings)
		{
			string text = str;
			foreach (string path in otherStrings)
			{
				text = Path.Combine(text, path);
			}
			return text;
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00153502 File Offset: 0x00151902
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x0015350A File Offset: 0x0015190A
		public static string SplitCamelCase(this string str)
		{
			return Regex.Replace(Regex.Replace(str, "(\\P{Ll})(\\P{Ll}\\p{Ll})", "$1 $2"), "(\\p{Ll})(\\P{Ll})", "$1 $2");
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x0015352B File Offset: 0x0015192B
		public static string Substring(this string str, Func<string, int> startFunction, Func<string, int> lengthFunction = null)
		{
			if (lengthFunction == null)
			{
				return str.Substring(startFunction(str));
			}
			return str.Substring(startFunction(str), lengthFunction(str));
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00153555 File Offset: 0x00151955
		public static string FirstCharToUpper(this string str)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x00153568 File Offset: 0x00151968
		public static string ReplaceFirstOccurrence(this string str, string find, string replace)
		{
			int startIndex = str.IndexOf(find);
			return str.Remove(startIndex, find.Length).Insert(startIndex, replace);
		}
	}
}
