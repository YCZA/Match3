using System.Collections.Generic;
using System.Globalization;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

// Token: 0x02000ADD RID: 2781
namespace Match3.Scripts1
{
	public interface ILocalizationService : IService, global::Wooga.UnityFramework.IInitializable
	{
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060041D9 RID: 16857
		List<string> LocaleKeys { get; }

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060041DA RID: 16858
		WoogaSystemLanguage Language { get; }

		// Token: 0x060041DB RID: 16859
		CultureInfo GetCultureInfo();

		// Token: 0x060041DC RID: 16860
		string NumberToString(int number);

		// Token: 0x060041DD RID: 16861
		string GetText(string key, params LocaParam[] arr);

		// Token: 0x060041DE RID: 16862
		string GetTextInLanguage(string key, WoogaSystemLanguage language, params LocaParam[] arr);

		// Token: 0x060041DF RID: 16863
		bool IsLanguageAvailable(WoogaSystemLanguage language);

		// Token: 0x060041E0 RID: 16864
		void ChangeLanguage(WoogaSystemLanguage lang);

		// Token: 0x060041E1 RID: 16865
		void UpdateLanguage();

		// Token: 0x060041E2 RID: 16866
		void SetGlobalReplaceKeys(List<GlobalReplaceKey> replaceKeys);

		// Token: 0x060041E3 RID: 16867
		List<string> GetKeysWithSubstring(string substring);

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060041E4 RID: 16868
		Signal LanguageChanged { get; }
	}
}
