using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Match3.Scripts1.Localization
{
	// Token: 0x02000ADE RID: 2782
	[Serializable]
	public class Locale
	{
		// Token: 0x060041E6 RID: 16870 RVA: 0x0015398C File Offset: 0x00151D8C
		public static Dictionary<string, string> CreateMap(Locale locale)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (locale != null)
			{
				foreach (Locale.Entry entry in locale.Entries)
				{
					dictionary[entry.key] = entry.val;
				}
			}
			return dictionary;
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x001539E4 File Offset: 0x00151DE4
		public static Locale CreateFromCsv(WoogaSystemLanguage languageCode, List<string> translationLines)
		{
			List<Locale.Entry> list = new List<Locale.Entry>();
			foreach (string text in translationLines)
			{
				System.Text.RegularExpressions.Match match = Locale.CSV_REGEX.Match(text);
				if (!string.IsNullOrEmpty(text) && match.Success)
				{
					list.Add(Locale.ParseLine(text));
				}
			}
			return new Locale
			{
				LanguageCode = languageCode,
				Entries = list.ToArray()
			};
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x00153A88 File Offset: 0x00151E88
		public void Overlay(Locale other)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < this.Entries.Length; i++)
			{
				dictionary[this.Entries[i].key] = i;
			}
			foreach (Locale.Entry entry in other.Entries)
			{
				if (dictionary.ContainsKey(entry.key))
				{
					this.Entries[dictionary[entry.key]].val = entry.val;
				}
				else
				{
					Array.Resize<Locale.Entry>(ref this.Entries, this.Entries.Length + 1);
					this.Entries[this.Entries.Length - 1] = entry;
				}
			}
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x00153B64 File Offset: 0x00151F64
		private static Locale.Entry ParseLine(string line)
		{
			line = Locale.FixCsvEscaping(line);
			int num = line.IndexOf(Locale.SEPARATOR_CHAR);
			string key = line.Substring(0, num);
			string text = line.Substring(num + 1);
			num = text.IndexOf(Locale.SEPARATOR_CHAR);
			string text2 = text.Substring(num + 1);
			num = text2.IndexOf(Locale.SEPARATOR_CHAR);
			string val = text2.Substring(num + 1);
			return new Locale.Entry(key, val);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00153BD0 File Offset: 0x00151FD0
		private static string FixCsvEscaping(string input)
		{
			input = input.Substring(1, input.Length - 2);
			input = input.Replace("\",\"", ",");
			input = input.Replace("\\\"", "\"");
			input = input.Replace("\\\\n", "\n");
			input = input.Replace("\\n", "\n");
			return input;
		}

		// Token: 0x04006AF9 RID: 27385
		public Locale.Entry[] Entries;

		// Token: 0x04006AFA RID: 27386
		public WoogaSystemLanguage LanguageCode;

		// Token: 0x04006AFB RID: 27387
		private static readonly char SEPARATOR_CHAR = ',';

		// Token: 0x04006AFC RID: 27388
		private static string CSV_PATTERN = "((.+)([\",\"](.+)){3})";

		// Token: 0x04006AFD RID: 27389
		private static Regex CSV_REGEX = new Regex(Locale.CSV_PATTERN);

		// Token: 0x02000ADF RID: 2783
		[Serializable]
		public struct Entry
		{
			// Token: 0x060041EC RID: 16876 RVA: 0x00153C59 File Offset: 0x00152059
			public Entry(string key, string val)
			{
				this.key = key;
				this.val = val;
			}

			// Token: 0x04006AFE RID: 27390
			public string key;

			// Token: 0x04006AFF RID: 27391
			public string val;
		}
	}
}
