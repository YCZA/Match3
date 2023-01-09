using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Localization.Processors
{
	// Token: 0x02000AE4 RID: 2788
	public class CharacterReplacementProcessor : ILocalizationProcessor
	{
		// Token: 0x060041F7 RID: 16887 RVA: 0x00153E97 File Offset: 0x00152297
		public CharacterReplacementProcessor(Dictionary<string, string> replacementMap)
		{
			if (replacementMap == null)
			{
				throw new InvalidOperationException("ReplacementMap is null");
			}
			this._characterReplacmentMap = replacementMap;
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00153EB7 File Offset: 0x001522B7
		public CharacterReplacementProcessor()
		{
			this._characterReplacmentMap = CharacterReplacementProcessor._sCharacterReplacmentMap;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00153ECA File Offset: 0x001522CA
		public string ProcessString(string input)
		{
			return this.ReplaceCharacters(input);
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00153ED3 File Offset: 0x001522D3
		private string ReplaceCharacters(string content)
		{
			return this._characterReplacmentMap.Aggregate(content, (string current, KeyValuePair<string, string> pair) => current.Replace(pair.Key, pair.Value));
		}

		// Token: 0x04006B06 RID: 27398
		private static readonly Dictionary<string, string> _sCharacterReplacmentMap = new Dictionary<string, string>
		{
			{
				"…",
				"..."
			},
			{
				"–",
				"-"
			},
			{
				"ё",
				"ë"
			},
			{
				"\u00a0",
				" "
			}
		};

		// Token: 0x04006B07 RID: 27399
		private readonly Dictionary<string, string> _characterReplacmentMap;
	}
}
