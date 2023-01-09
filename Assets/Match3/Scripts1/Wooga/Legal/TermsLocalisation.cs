using System.Collections.Generic;
using Wooga.Newtonsoft.Json;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Legal
{
	// Token: 0x02000B59 RID: 2905
	public class TermsLocalisation
	{
		// Token: 0x060043F0 RID: 17392 RVA: 0x0015A8B0 File Offset: 0x00158CB0
		public TermsLocalisation(Dictionary<string, string> localisations)
		{
			this.localisations = localisations;
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060043F1 RID: 17393 RVA: 0x0015A8CA File Offset: 0x00158CCA
		public static long CurrentVersion
		{
			get
			{
				return 1L;
			}
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0015A8D0 File Offset: 0x00158CD0
		public static string GetLocalization(string locaKey)
		{
			string text;
			TermsLocalisation.GetInstance().localisations.TryGetValue(locaKey, out text);
			if (text != null)
			{
				text = text.Replace("<terms_link>", "<link=\"" + TermsLocalisation.termsLink + "\"><u>").Replace("</terms_link>", "</u></link>").Replace("<policy_link>", "<link=\"" + TermsLocalisation.policyLink + "\"><u>").Replace("</policy_link>", "</u></link>");
			}
			return text;
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0015A953 File Offset: 0x00158D53
		public static void Unload()
		{
			TermsLocalisation.termsLocalisation = null;
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0015A95C File Offset: 0x00158D5C
		private static TermsLocalisation GetInstance()
		{
			if (TermsLocalisation.termsLocalisation == null)
			{
				if (TermsLocalisation.language == null)
				{
					TermsLocalisation.langMap.TryGetValue(Application.systemLanguage, out TermsLocalisation.language);
				}
				TextAsset textAsset = Resources.Load<TextAsset>(string.Format("legal_terms/{0}/{1}", TermsLocalisation.CurrentVersion, TermsLocalisation.language));
				if (textAsset == null)
				{
					textAsset = Resources.Load<TextAsset>(string.Format("legal_terms/{0}/{1}", TermsLocalisation.CurrentVersion, "en"));
				}
				TermsLocalisation.termsLocalisation = JSON.Deserialize<TermsLocalisation>(textAsset.text);
			}
			return TermsLocalisation.termsLocalisation;
		}

		// Token: 0x04006C47 RID: 27719
		private const string FALLBACK_LANGUAGE = "en";

		// Token: 0x04006C48 RID: 27720
		private const string RESOURCE_PATH = "legal_terms/{0}/{1}";

		// Token: 0x04006C49 RID: 27721
		public static string language;

		// public static string termsLink = "https://www.wooga.com";
		public static string termsLink = "host2333";

		// public static string policyLink = "https://www.wooga.com";
		public static string policyLink = "host23333";

		// Token: 0x04006C4C RID: 27724
		private static TermsLocalisation termsLocalisation = null;

		// Token: 0x04006C4D RID: 27725
		private static Dictionary<SystemLanguage, string> langMap = new Dictionary<SystemLanguage, string>
		{
			{
				SystemLanguage.English,
				"en"
			},
			{
				SystemLanguage.German,
				"de"
			},
			{
				SystemLanguage.French,
				"fr"
			},
			{
				SystemLanguage.Italian,
				"it"
			},
			{
				SystemLanguage.Spanish,
				"es"
			},
			{
				SystemLanguage.Portuguese,
				"pt"
			},
			{
				SystemLanguage.Dutch,
				"nl"
			},
			{
				SystemLanguage.Danish,
				"da"
			},
			{
				SystemLanguage.Norwegian,
				"nb"
			},
			{
				SystemLanguage.Swedish,
				"sv"
			},
			{
				SystemLanguage.Russian,
				"ru"
			}
		};

		// Token: 0x04006C4E RID: 27726
		[JsonProperty]
		private Dictionary<string, string> localisations = new Dictionary<string, string>();
	}
}
