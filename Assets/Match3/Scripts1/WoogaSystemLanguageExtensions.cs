using System;
using System.Collections.Generic;
using System.Globalization;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;

// Token: 0x02000AEB RID: 2795
namespace Match3.Scripts1
{
	public static class WoogaSystemLanguageExtensions
	{
		// Token: 0x06004211 RID: 16913 RVA: 0x0015450C File Offset: 0x0015290C
		public static string ToIsoCode(this WoogaSystemLanguage lang)
		{
			switch (lang)
			{
				case WoogaSystemLanguage.LocaKeys:
					return "xx";
				case WoogaSystemLanguage.Afrikaans:
					return "af";
				case WoogaSystemLanguage.Arabic:
					return "ar";
				case WoogaSystemLanguage.Basque:
					return "eu";
				case WoogaSystemLanguage.Belarusian:
					return "be";
				case WoogaSystemLanguage.Bulgarian:
					return "bg";
				case WoogaSystemLanguage.Catalan:
					return "ca";
				case WoogaSystemLanguage.Chinese:
					return "zhtw";
				case WoogaSystemLanguage.Czech:
					return "cs";
				case WoogaSystemLanguage.Danish:
					return "da";
				case WoogaSystemLanguage.Dutch:
					return "nl";
				case WoogaSystemLanguage.English:
					return "en";
				case WoogaSystemLanguage.Estonian:
					return "et";
				case WoogaSystemLanguage.Faroese:
					return "fo";
				case WoogaSystemLanguage.Finnish:
					return "fi";
				case WoogaSystemLanguage.French:
					return "fr";
				case WoogaSystemLanguage.German:
					return "de";
				case WoogaSystemLanguage.Greek:
					return "el";
				case WoogaSystemLanguage.Hebrew:
					return "he";
				case WoogaSystemLanguage.Hugarian:
					return "hu";
				case WoogaSystemLanguage.Icelandic:
					return "is";
				case WoogaSystemLanguage.Indonesian:
					return "id";
				case WoogaSystemLanguage.Italian:
					return "it";
				case WoogaSystemLanguage.Japanese:
					return "jp";
				case WoogaSystemLanguage.Korean:
					return "kr";
				case WoogaSystemLanguage.Latvian:
					return "lv";
				case WoogaSystemLanguage.Lithuanian:
					return "lt";
				case WoogaSystemLanguage.Norwegian:
					return "no";
				case WoogaSystemLanguage.Polish:
					return "pl";
				case WoogaSystemLanguage.Portuguese:
					return "pt";
				case WoogaSystemLanguage.Romanian:
					return "ro";
				case WoogaSystemLanguage.Russian:
					return "ru";
				case WoogaSystemLanguage.SerboCroatian:
					return "sr";
				case WoogaSystemLanguage.Slovak:
					return "sk";
				case WoogaSystemLanguage.Slovenian:
					return "sl";
				case WoogaSystemLanguage.Spanish:
					return "es";
				case WoogaSystemLanguage.Swedish:
					return "sv";
				case WoogaSystemLanguage.Thai:
					return "th";
				case WoogaSystemLanguage.Turkish:
					return "tr";
				case WoogaSystemLanguage.Ukrainian:
					return "uk";
				case WoogaSystemLanguage.Vietnamese:
					return "vi";
				case WoogaSystemLanguage.SimplifiedChinese:
					return "zhcn";
			}
			return "en";
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x001546D4 File Offset: 0x00152AD4
		public static string ToAssociatedRegionCode(this WoogaSystemLanguage lang)
		{
			switch (lang)
			{
				case WoogaSystemLanguage.Afrikaans:
					return "af-ZA";
				case WoogaSystemLanguage.Arabic:
					return "ar-SA";
				case WoogaSystemLanguage.Basque:
					return "eu-ES";
				case WoogaSystemLanguage.Belarusian:
					return "be-BY";
				case WoogaSystemLanguage.Bulgarian:
					return "bg-BG";
				case WoogaSystemLanguage.Catalan:
					return "ca-ES";
				case WoogaSystemLanguage.Chinese:
					return "zh-CN";
				case WoogaSystemLanguage.Czech:
					return "cs-CZ";
				case WoogaSystemLanguage.Danish:
					return "da-DK";
				case WoogaSystemLanguage.Dutch:
					return "nl-NL";
				case WoogaSystemLanguage.English:
					return "en-US";
				case WoogaSystemLanguage.Estonian:
					return "et-EE";
				case WoogaSystemLanguage.Faroese:
					return "fo-FO";
				case WoogaSystemLanguage.Finnish:
					return "fi-FI";
				case WoogaSystemLanguage.French:
					return "fr-FR";
				case WoogaSystemLanguage.German:
					return "de-DE";
				case WoogaSystemLanguage.Greek:
					return "el-GR";
				case WoogaSystemLanguage.Hebrew:
					return "he-IL";
				case WoogaSystemLanguage.Hugarian:
					return "hu-HU";
				case WoogaSystemLanguage.Icelandic:
					return "is-IS";
				case WoogaSystemLanguage.Indonesian:
					return "id-ID";
				case WoogaSystemLanguage.Italian:
					return "it-IT";
				case WoogaSystemLanguage.Japanese:
					return "ja-JP";
				case WoogaSystemLanguage.Korean:
					return "ko-KR";
				case WoogaSystemLanguage.Latvian:
					return "lv-LV";
				case WoogaSystemLanguage.Lithuanian:
					return "lt-LT";
				case WoogaSystemLanguage.Norwegian:
					return "nn-NO";
				case WoogaSystemLanguage.Polish:
					return "pl-PL";
				case WoogaSystemLanguage.Portuguese:
					return "pt-PT";
				case WoogaSystemLanguage.Romanian:
					return "ro-RO";
				case WoogaSystemLanguage.Russian:
					return "ru-RU";
				case WoogaSystemLanguage.Slovak:
					return "sk-SK";
				case WoogaSystemLanguage.Slovenian:
					return "sl-SI";
				case WoogaSystemLanguage.Spanish:
					return "es-ES";
				case WoogaSystemLanguage.Swedish:
					return "sv-SE";
				case WoogaSystemLanguage.Thai:
					return "th-TH";
				case WoogaSystemLanguage.Turkish:
					return "tr-TR";
				case WoogaSystemLanguage.Ukrainian:
					return "uk-UA";
				case WoogaSystemLanguage.Vietnamese:
					return "vi-VN";
				case WoogaSystemLanguage.SimplifiedChinese:
					return "zh-CHS";
			}
			return "en-US";
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x0015488C File Offset: 0x00152C8C
		public static WoogaSystemLanguage ApplicationLanguage()
		{
			// eli todo 暂时只使用英语
			// string value = Application.systemLanguage.ToString();
			// if (Enum.IsDefined(typeof(WoogaSystemLanguage), value))
			// {
			// 	return (WoogaSystemLanguage)Enum.Parse(typeof(WoogaSystemLanguage), value);
			// }
			return WoogaSystemLanguage.English;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x001548DA File Offset: 0x00152CDA
		public static string ToTwoLetterIsoCode(this WoogaSystemLanguage lang)
		{
			return lang.ToIsoCode().Substring(0, 2);
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x001548EC File Offset: 0x00152CEC
		public static string GetCurrencySymbol(this WoogaSystemLanguage lang)
		{
			string result;
			try
			{
				result = CultureInfo.GetCultureInfo(lang.ToAssociatedRegionCode()).NumberFormat.CurrencySymbol;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x00154930 File Offset: 0x00152D30
		public static CultureInfo GetCultureInfoFromCurrencyCode(string currencyCode)
		{
			if (currencyCode == null)
			{
				return new CultureInfo("en-US");
			}
			string text = currencyCode.ToUpper();
			string text2;
			switch (text)
			{
				case "USD":
					text2 = "en-US";
					goto IL_510;
				case "MXN":
					text2 = "es-MX";
					goto IL_510;
				case "CAD":
					text2 = "en-CA";
					goto IL_510;
				case "GBP":
					text2 = "en-GB";
					goto IL_510;
				case "EUR":
					text2 = "de-DE";
					goto IL_510;
				case "SEK":
					text2 = "sv-SE";
					goto IL_510;
				case "DKK":
					text2 = "da-DK";
					goto IL_510;
				case "NOK":
					text2 = "nn-NO";
					goto IL_510;
				case "CHF":
					text2 = "de-CH";
					goto IL_510;
				case "AUD":
					text2 = "en-AU";
					goto IL_510;
				case "NZD":
					text2 = "en-NZ";
					goto IL_510;
				case "JPY":
					text2 = "ja-JP";
					goto IL_510;
				case "CNY":
					text2 = "zh-CN";
					goto IL_510;
				case "HKD":
					text2 = "zh-HK";
					goto IL_510;
				case "SGD":
					text2 = "zh-SG";
					goto IL_510;
				case "TWD":
					text2 = "zh-TW";
					goto IL_510;
				case "INR":
					text2 = "hi-IN";
					goto IL_510;
				case "IDR":
					text2 = "id-ID";
					goto IL_510;
				case "ILS":
					text2 = "he-IL";
					goto IL_510;
				case "RUB":
					text2 = "ru-RU";
					goto IL_510;
				case "SAR":
					text2 = "ar-SA";
					goto IL_510;
				case "ZAR":
					text2 = "af-ZA";
					goto IL_510;
				case "TRY":
					text2 = "tr-TR";
					goto IL_510;
				case "AED":
					text2 = "ar-AE";
					goto IL_510;
				case "KRW":
					text2 = "ko-KR";
					goto IL_510;
				case "ARS":
					text2 = "es-AR";
					goto IL_510;
				case "BRL":
					text2 = "pt-BR";
					goto IL_510;
				case "CLP":
					text2 = "es-CL";
					goto IL_510;
				case "CZK":
					text2 = "cs-CZ";
					goto IL_510;
				case "HNL":
					text2 = "es-HN";
					goto IL_510;
				case "HUF":
					text2 = "hu-HU";
					goto IL_510;
				case "ISK":
					text2 = "is-IS";
					goto IL_510;
				case "PHP":
					text2 = "en-PH";
					goto IL_510;
				case "PLN":
					text2 = "pl-PL";
					goto IL_510;
				case "THB":
					text2 = "th-TH";
					goto IL_510;
				case "VND":
					text2 = "vi-VN";
					goto IL_510;
				case "RON":
					text2 = "ro-RO";
					goto IL_510;
				case "EGP":
					text2 = "ar-EG";
					goto IL_510;
				case "COP":
					text2 = "es-CO";
					goto IL_510;
				case "BGN":
					text2 = "bg-BG";
					goto IL_510;
				case "DZD":
					text2 = "ar-DZ";
					goto IL_510;
				case "LBP":
					text2 = "ar-LY";
					goto IL_510;
				case "HRK":
					text2 = "hr-HR";
					goto IL_510;
			}
			text2 = "en-US";
			CultureInfo result;
			IL_510:
			try
			{
				result = new CultureInfo(text2);
			}
			catch (ArgumentException)
			{
				Log.Warning(string.Format("Culture name {0} is not supported.", text2), null, null);
				CultureInfo cultureInfo = new CultureInfo("en-US");
				result = cultureInfo;
			}
			return result;
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x00154E94 File Offset: 0x00153294
		public static IEnumerable<int> GetUnicodeRange(this WoogaSystemLanguage lang)
		{
			UnicodeRange unicodeRange = new UnicodeRange(32, 127);
			UnicodeRange unicodeRange2 = new UnicodeRange(160, 255);
			UnicodeRange unicodeRange3 = new UnicodeRange(8352, 8399);
			UnicodeRange unicodeRange4 = new UnicodeRange(19968, 40959);
			UnicodeRange unicodeRange5 = new UnicodeRange(1536, 1791);
			if (lang != WoogaSystemLanguage.Japanese)
			{
				return unicodeRange.range;
			}
			return unicodeRange4.range;
		}
	}
}
