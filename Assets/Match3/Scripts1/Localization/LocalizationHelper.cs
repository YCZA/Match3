using System.IO;
using UnityEngine;

namespace Match3.Scripts1.Localization
{
	// Token: 0x02000AE3 RID: 2787
	public static class LocalizationHelper
	{
		// Token: 0x060041F5 RID: 16885 RVA: 0x00153E44 File Offset: 0x00152244
		private static string PathForLocale(WoogaSystemLanguage locale)
		{
			return Path.Combine("Localization/", locale.ToIsoCode());
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x00153E58 File Offset: 0x00152258
		public static Locale LoadLanguage(WoogaSystemLanguage languageCode)
		{
			string text = LocalizationHelper.PathForLocale(languageCode);
			LocaleWrapper localeWrapper = Resources.Load<LocaleWrapper>(text);
			if (localeWrapper == null)
			{
				global::UnityEngine.Debug.Log("Cannot find locale resource: " + text);
				return null;
			}
			return localeWrapper.locale;
		}
	}
}
