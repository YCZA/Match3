using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Localization.Validators
{
	// Token: 0x02000AE8 RID: 2792
	public class CharacterValidator : ILocalizationValidator
	{
		// Token: 0x06004201 RID: 16897 RVA: 0x00154032 File Offset: 0x00152432
		public CharacterValidator(string supportedCharacters)
		{
			this._supportedCharacterSet = supportedCharacters.ToHashset();
			this._supportedCharacterSet.UnionWith(CharacterValidator._sBuiltInSupportedCharacters);
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00154056 File Offset: 0x00152456
		public CharacterValidator(HashSet<char> supportedCharacterSet)
		{
			this._supportedCharacterSet = supportedCharacterSet;
			this._supportedCharacterSet.UnionWith(CharacterValidator._sBuiltInSupportedCharacters);
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x00154078 File Offset: 0x00152478
		public bool Validate(Locale locale)
		{
			bool result = true;
			HashSet<char> hashSet = new HashSet<char>(this._supportedCharacterSet);
			foreach (Locale.Entry entry in locale.Entries)
			{
				HashSet<char> hashSet2 = entry.val.ToHashset();
				hashSet.IntersectWith(hashSet2);
				IEnumerable<char> missingCharacters = hashSet2.Intersect(this._supportedCharacterSet);
				if (this.HandleMissingCharacters(entry, missingCharacters))
				{
					result = false;
				}
				IEnumerable<char> intersectedChars = hashSet2.Intersect(CharacterValidator.unsupportedCharacters);
				if (this.ContainsUnsupportedChars(entry, intersectedChars))
				{
					result = false;
				}
			}
			this.HandleUnusedCharacters(hashSet);
			return result;
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x0015411C File Offset: 0x0015251C
		private bool HandleUnusedCharacters(IEnumerable<char> unusedCharacters)
		{
			if (unusedCharacters.Any<char>())
			{
				string str = string.Join(string.Empty, (from p in unusedCharacters
				select p.ToString()).ToArray<string>());
				global::UnityEngine.Debug.Log("LOCALIZATION: UNUSED BUT SUPPORTED CHARACTERS = " + str);
				return false;
			}
			return true;
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x0015417C File Offset: 0x0015257C
		private bool HandleMissingCharacters(Locale.Entry entry, IEnumerable<char> missingCharacters)
		{
			if (!missingCharacters.IsNullOrEmptyEnumerable())
			{
				foreach (char c in missingCharacters)
				{
					string format = "LOCALIZATION: Unsupported characters used in: {0}  \"{1}\"";
					global::UnityEngine.Debug.LogWarning(string.Format(format, entry.key, c));
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x001541F8 File Offset: 0x001525F8
		private bool ContainsUnsupportedChars(Locale.Entry entry, IEnumerable<char> intersectedChars)
		{
			bool result = false;
			if (!intersectedChars.IsNullOrEmptyEnumerable())
			{
				result = true;
				foreach (char c in intersectedChars)
				{
					string format = "LOCALIZATION: Unsupported characters used in: {0}  \"{1}\"";
					WoogaDebug.LogWarning(new object[]
					{
						string.Format(format, entry.key, entry.val, c)
					});
				}
			}
			return result;
		}

		// Token: 0x04006B0B RID: 27403
		private static readonly HashSet<char> _sBuiltInSupportedCharacters = new HashSet<char>
		{
			'\n',
			' '
		};

		// Token: 0x04006B0C RID: 27404
		private readonly HashSet<char> _supportedCharacterSet;

		// Token: 0x04006B0D RID: 27405
		private static readonly HashSet<char> unsupportedCharacters = new HashSet<char>
		{
			'�'
		};
	}
}
