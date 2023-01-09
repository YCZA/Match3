using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Localization.Validators
{
	// Token: 0x02000AE9 RID: 2793
	public class KeyUsageValidator : ILocalizationValidator
	{
		// Token: 0x06004209 RID: 16905 RVA: 0x001542D6 File Offset: 0x001526D6
		public KeyUsageValidator(Locale defaultLocale)
		{
			this.defaultKeys = (from e in defaultLocale.Entries
			select e.key).ToHashSet<string>();
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00154314 File Offset: 0x00152714
		public KeyUsageValidator(WoogaSystemLanguage defaultLocale)
		{
			Locale locale = LocalizationHelper.LoadLanguage(defaultLocale);
			if (locale != null)
			{
				this.defaultKeys = (from e in locale.Entries
				select e.key).ToHashSet<string>();
			}
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x00154368 File Offset: 0x00152768
		public bool Validate(Locale locale)
		{
			if (this.defaultKeys == null)
			{
				global::UnityEngine.Debug.LogWarning("Data for default locale not available.");
				return false;
			}
			bool result = true;
			HashSet<string> hashSet = (from e in locale.Entries
			select e.key).ToHashSet<string>();
			IEnumerable<string> enumerable = this.defaultKeys.Except(hashSet);
			IEnumerable<string> enumerable2 = hashSet.Except(this.defaultKeys);
			if (enumerable.Any<string>())
			{
				this.HandleInDefaultButNotLocale(locale, enumerable);
				result = false;
			}
			if (enumerable2.Any<string>())
			{
				KeyUsageValidator.HandleInInputButNotDefault(locale, enumerable2);
				result = false;
			}
			return result;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x00154400 File Offset: 0x00152800
		private static void HandleInInputButNotDefault(Locale input, IEnumerable<string> inInputButNotDefault)
		{
			global::UnityEngine.Debug.LogWarning("There are entries in the locale but not in the default language: " + input.LanguageCode);
			foreach (string str in inInputButNotDefault)
			{
				global::UnityEngine.Debug.LogWarning("\t" + str);
			}
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x00154478 File Offset: 0x00152878
		private void HandleInDefaultButNotLocale(Locale input, IEnumerable<string> inDefaultButNotInput)
		{
			global::UnityEngine.Debug.LogWarning("There are entries in the default language but not in the locale: " + input.LanguageCode);
			foreach (string str in inDefaultButNotInput)
			{
				global::UnityEngine.Debug.LogWarning("\t" + str);
			}
		}

		// Token: 0x04006B0F RID: 27407
		private readonly HashSet<string> defaultKeys;
	}
}
