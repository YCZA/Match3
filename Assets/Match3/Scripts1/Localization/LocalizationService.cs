using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Localization
{
	// Token: 0x020007DA RID: 2010
	public abstract class LocalizationService : AService, ILocalizationService, IService, global::Wooga.UnityFramework.IInitializable
	{
		// Token: 0x0600318E RID: 12686 RVA: 0x000E90B0 File Offset: 0x000E74B0
		protected LocalizationService(GameSettingsService gameSettingsService)
		{
			this._gameSettingsService = gameSettingsService;
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x000E90EB File Offset: 0x000E74EB
		public List<string> LocaleKeys
		{
			get
			{
				return this._sortedKeys;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x000E90F3 File Offset: 0x000E74F3
		public Signal LanguageChanged
		{
			get
			{
				return this.languageChanged;
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000E90FC File Offset: 0x000E74FC
		public List<string> GetKeysWithSubstring(string substring)
		{
			List<string> list = new List<string>();
			int count = this._sortedKeys.Count;
			for (int i = 0; i < count; i++)
			{
				string text = this._sortedKeys[i];
				if (text.Contains(substring))
				{
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x000E914E File Offset: 0x000E754E
		public WoogaSystemLanguage Language
		{
			get
			{
				return this._currentLocale.LanguageCode;
			}
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000E915B File Offset: 0x000E755B
		public void SetGlobalReplaceKeys(List<GlobalReplaceKey> replaceKeys)
		{
			this._globalReplaceKeys = replaceKeys;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000E9164 File Offset: 0x000E7564
		public CultureInfo GetCultureInfo()
		{
			if (this._dictCultureInfos == null)
			{
				this._dictCultureInfos = new Dictionary<WoogaSystemLanguage, CultureInfo>();
			}
			if (!this._dictCultureInfos.ContainsKey(this.Language))
			{
				CultureInfo cultureInfo = CultureInfo.GetCultureInfo(this.Language.ToAssociatedRegionCode());
				this._dictCultureInfos[this.Language] = cultureInfo;
			}
			return this._dictCultureInfos[this.Language];
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000E91D1 File Offset: 0x000E75D1
		public string NumberToString(int number)
		{
			return number.ToString("N0", this.GetCultureInfo().NumberFormat);
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000E91EC File Offset: 0x000E75EC
		public virtual string GetText(string key, params LocaParam[] arr)
		{
			if (this._currentLocaleMap.ContainsKey(key))
			{
				string localisedValue = this._currentLocaleMap[key];
				return this.GetFormattedText(localisedValue, arr);
			}
			return key;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000E9224 File Offset: 0x000E7624
		public string GetTextInLanguage(string key, WoogaSystemLanguage language, params LocaParam[] arr)
		{
			if (language == this.Language || language == WoogaSystemLanguage.LocaKeys)
			{
				return this.GetText(key, arr);
			}
			if (!this.IsLanguageAvailable(language))
			{
				global::UnityEngine.Debug.LogWarning(string.Concat(new object[]
				{
					"Trying to get text from unsupported language: ",
					language,
					", for key: ",
					key
				}));
				return this.GetText(key, arr);
			}
			Locale locale = LocalizationHelper.LoadLanguage(language);
			Dictionary<string, string> dictionary = Locale.CreateMap(locale);
			if (dictionary.ContainsKey(key))
			{
				string localisedValue = dictionary[key];
				return this.GetFormattedText(localisedValue, arr);
			}
			return this.GetText(key, arr);
		}

		// eli key point 改变语言 change language
		public void ChangeLanguage(WoogaSystemLanguage lang)
		{
			if (!this.IsLanguageAvailable(lang))
			{
				global::UnityEngine.Debug.LogWarning("There is no support for chosen locale: " + lang);
				lang = WoogaSystemLanguage.English;
			}
			this._currentLocale = LocalizationHelper.LoadLanguage(lang);
			this._currentLocaleMap = Locale.CreateMap(this._currentLocale);
			this._sortedKeys = new List<string>(from l in this._currentLocaleMap
			select l.Key);
			this._sortedKeys.Sort();
			this._gameSettingsService.Language = lang;
			this.LanguageChanged.Dispatch();
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000E9368 File Offset: 0x000E7768
		public void UpdateLanguage()
		{
			this._currentLocale = LocalizationCrowdinHelper.DownloadCsvFromCrowdin(this.Language);
			this._currentLocaleMap = Locale.CreateMap(this._currentLocale);
			this._sortedKeys = new List<string>(from l in this._currentLocaleMap
			select l.Key);
			this._sortedKeys.Sort();
			this.LanguageChanged.Dispatch();
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000E93E0 File Offset: 0x000E77E0
		public virtual bool IsLanguageAvailable(WoogaSystemLanguage language)
		{
			return PTLocalizationConfig.AVAILABLE_LANGUAGES.Contains(language);
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000E93F0 File Offset: 0x000E77F0
		protected string GetFormattedText(string localisedValue, params LocaParam[] arr)
		{
			string text = localisedValue;
			if (!arr.IsNullOrEmptyCollection())
			{
				for (int i = 0; i < arr.Length; i++)
				{
					string name = arr[i].name;
					string value = arr[i].value;
					text = text.Replace(name, value);
					if (string.IsNullOrEmpty(value))
					{
						Log.Warning("Loca", string.Format("Empty value for key: {0}", name), null);
					}
				}
			}
			if (this._globalReplaceKeys != null)
			{
				foreach (GlobalReplaceKey globalReplaceKey in this._globalReplaceKeys)
				{
					text = Regex.Replace(text, globalReplaceKey.key, globalReplaceKey.value, RegexOptions.IgnoreCase);
				}
			}
			localisedValue = text;
			return LocalizationService.ReplaceIconParams(localisedValue);
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000E94D8 File Offset: 0x000E78D8
		private static string ReplaceIconParams(string text)
		{
			int num = 0;
			while (text.Contains("[[") && num < 500)
			{
				try
				{
					text = LocalizationService.ReplaceIconParam(text);
				}
				catch (Exception ex)
				{
					WoogaDebug.LogWarning(new object[]
					{
						ex
					});
					break;
				}
				num++;
			}
			if (num == 500)
			{
				WoogaDebug.LogError(new object[]
				{
					"There was an error replacing filling out params for: ",
					text
				});
			}
			return text;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000E9560 File Offset: 0x000E7960
		private static string ReplaceIconParam(string text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04005A15 RID: 23061
		protected readonly GameSettingsService _gameSettingsService;

		// Token: 0x04005A16 RID: 23062
		private Locale _currentLocale = new Locale();

		// Token: 0x04005A17 RID: 23063
		private Dictionary<string, string> _currentLocaleMap = new Dictionary<string, string>();

		// Token: 0x04005A18 RID: 23064
		private Dictionary<WoogaSystemLanguage, CultureInfo> _dictCultureInfos;

		// Token: 0x04005A19 RID: 23065
		private List<string> _sortedKeys = new List<string>();

		// Token: 0x04005A1A RID: 23066
		private List<GlobalReplaceKey> _globalReplaceKeys;

		// Token: 0x04005A1B RID: 23067
		private readonly Signal languageChanged = new Signal();
	}
}
