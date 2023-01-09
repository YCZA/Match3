using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Match3.Scripts1.Localization
{
	// Token: 0x02000AE2 RID: 2786
	public static class LocalizationCrowdinHelper
	{
		// Token: 0x060041F2 RID: 16882 RVA: 0x00153CFF File Offset: 0x001520FF
		private static void DisableSSLCertificateCheck()
		{
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x00153D04 File Offset: 0x00152104
		private static string[] CsvUrlsForLanguage(WoogaSystemLanguage languageCode)
		{
			string[] crowdinCsvUrls = LocalizationConfig.GetCrowdinCsvUrls();
			string arg = PTLocalizationService.ToCrowdinCode(languageCode);
			for (int i = 0; i < crowdinCsvUrls.Length; i++)
			{
				crowdinCsvUrls[i] = string.Format(crowdinCsvUrls[i], arg);
			}
			return crowdinCsvUrls;
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x00153D40 File Offset: 0x00152140
		public static Locale DownloadCsvFromCrowdin(WoogaSystemLanguage languageCode)
		{
			LocalizationCrowdinHelper.DisableSSLCertificateCheck();
			List<string> list = null;
			string[] array = LocalizationCrowdinHelper.CsvUrlsForLanguage(languageCode);
			try
			{
				list = new List<string>();
				foreach (string text in array)
				{
					HttpWebRequest httpWebRequest = WebRequest.Create(text) as HttpWebRequest;
					HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
					if (httpWebResponse.StatusCode != HttpStatusCode.OK)
					{
						global::UnityEngine.Debug.LogWarning("Failed request to Crowdin: " + httpWebResponse.StatusDescription + text);
						return null;
					}
					StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
					for (string text2 = streamReader.ReadLine(); text2 != null; text2 = streamReader.ReadLine())
					{
						if (!text2.IsNullOrEmpty())
						{
							list.Add(text2);
						}
					}
				}
			}
			catch (WebException ex)
			{
				global::UnityEngine.Debug.LogWarning("Failed request to Crowdin: " + ex.Message);
				return null;
			}
			return Locale.CreateFromCsv(languageCode, list);
		}
	}
}
