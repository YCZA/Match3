using System.Linq;
using Match3.Scripts2.Env;

namespace Match3.Scripts1.Localization
{
	// Token: 0x02000AED RID: 2797
	public class PTLocalizationConfig
	{
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x00154F4A File Offset: 0x0015334A
		public static WoogaSystemLanguage[] AVAILABLE_LANGUAGES
		{
			get
			{
				return PTLocalizationConfig.GetAvailableLanguages();
			}
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x00154F54 File Offset: 0x00153354
		private static WoogaSystemLanguage[] GetAvailableLanguages()
		{
			GameEnvironment.Environment currentEnvironment = GameEnvironment.CurrentEnvironment;
			WoogaSystemLanguage[] array = PTLocalizationConfig.PRODUCTION_LANGUAGES;
			if (currentEnvironment == GameEnvironment.Environment.CI)
			{
				array = array.Concat(PTLocalizationConfig.CI_LANGUAGES).ToArray<WoogaSystemLanguage>();
			}
			return array;
		}

		// Token: 0x04006B44 RID: 27460
		public const string RUNTIME_PATH = "Localization/";

		// Token: 0x04006B45 RID: 27461
		public const WoogaSystemLanguage DEFAULT_LANGUAGE = WoogaSystemLanguage.English;

		// eli todo 暂时只使用英语, 屏蔽其它语言
		// Token: 0x04006B46 RID: 27462
		public static readonly WoogaSystemLanguage[] PRODUCTION_LANGUAGES = new WoogaSystemLanguage[]
		{
			WoogaSystemLanguage.English,
			// WoogaSystemLanguage.French,
			// WoogaSystemLanguage.Italian,
			// WoogaSystemLanguage.German,
			// WoogaSystemLanguage.Spanish,
			// WoogaSystemLanguage.Portuguese,
			// WoogaSystemLanguage.Dutch,
			// WoogaSystemLanguage.Danish,
			// WoogaSystemLanguage.Norwegian,
			// WoogaSystemLanguage.Swedish,
			// WoogaSystemLanguage.Russian,
			// WoogaSystemLanguage.SimplifiedChinese,
		};

		// Token: 0x04006B47 RID: 27463
		public static readonly WoogaSystemLanguage[] CI_LANGUAGES = new WoogaSystemLanguage[1];
	}
}
