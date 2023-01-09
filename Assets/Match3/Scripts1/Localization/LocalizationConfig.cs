using Match3.Scripts1.Localization.Processors;
using Match3.Scripts1.Localization.Validators;

namespace Match3.Scripts1.Localization
{
	// Token: 0x02000AE1 RID: 2785
	public class LocalizationConfig
	{
		// Token: 0x060041EF RID: 16879 RVA: 0x00153C79 File Offset: 0x00152079
		private static string GetCrowdinUrl(string crowdInFile)
		{
			return "https://api.crowdin.com/api/project//export-file?key=&file=" + crowdInFile + "&language={0}&export_approved_only=0";
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00153C8C File Offset: 0x0015208C
		public static string[] GetCrowdinCsvUrls()
		{
			string[] array = new string[LocalizationConfig.crowdInCsvFiles.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = LocalizationConfig.GetCrowdinUrl(LocalizationConfig.crowdInCsvFiles[i]);
			}
			return array;
		}

		// Token: 0x04006B01 RID: 27393
		private const string CROWDIN_KEY = "";

		// Token: 0x04006B02 RID: 27394
		private const string CROWDIN_PROJECT = "";

		// Token: 0x04006B03 RID: 27395
		private static readonly string[] crowdInCsvFiles = new string[0];

		// Token: 0x04006B04 RID: 27396
		public const string OUTPUT_PATH = "Resources/Localization/";

		// Token: 0x04006B05 RID: 27397
		public static Validator LocaleValidator = new Validator(new ILocalizationProcessor[]
		{
			new CharacterReplacementProcessor()
		}, new ILocalizationValidator[]
		{
			new KeyUsageValidator(WoogaSystemLanguage.English)
		});
	}
}
