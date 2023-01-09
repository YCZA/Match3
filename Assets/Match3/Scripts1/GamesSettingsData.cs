using System;
using UnityEngine;

// Token: 0x0200079B RID: 1947
namespace Match3.Scripts1
{
	[Serializable]
	public class GamesSettingsData
	{
		// Token: 0x06002FC3 RID: 12227 RVA: 0x000E0E82 File Offset: 0x000DF282
		public static GamesSettingsData SettingsFromString(string data)
		{
			return JsonUtility.FromJson<GamesSettingsData>(data);
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000E0E8A File Offset: 0x000DF28A
		public static string SettingsToString(GamesSettingsData settings)
		{
			return JsonUtility.ToJson(settings);
		}

		// Token: 0x040058E1 RID: 22753
		public WoogaSystemLanguage CurrentLanguage;
	}
}
