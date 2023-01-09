using System.IO;
using Wooga.Core.Utilities;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000304 RID: 772
	public class BundleManagerCDNConfig
	{
		// Token: 0x06001846 RID: 6214 RVA: 0x0006F20C File Offset: 0x0006D60C
		public static OptionalResult<BundleManagerCDNConfig> TryGet()
		{
			TextAsset textAsset = Resources.Load<TextAsset>(BundleManagerCDNConfig.ConfigPath);
			return (!(textAsset != null)) ? null : BundleManagerCDNConfig.Parse(textAsset.text);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0006F248 File Offset: 0x0006D648
		public static BundleManagerCDNConfig Parse(string json)
		{
			BundleManagerCDNConfig result;
			try
			{
				result = JsonUtility.FromJson<BundleManagerCDNConfig>(json);
			}
			catch
			{
				Log.ErrorFormatted("Could not parse: {0}", new object[]
				{
					json
				});
				throw;
			}
			return result;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0006F28C File Offset: 0x0006D68C
		public void Save()
		{
			File.WriteAllText("Assets/AssetBundleManager/Resources/AssetBundleManager/CDNConfig.json.txt", JsonUtility.ToJson(this, true));
		}

		// Token: 0x040047B4 RID: 18356
		public static string ConfigPath = "AssetBundleManager/CDNConfig.json";

		// Token: 0x040047B5 RID: 18357
		private const string ConfigAssetPath = "Assets/AssetBundleManager/Resources/AssetBundleManager/CDNConfig.json.txt";

		// Token: 0x040047B6 RID: 18358
		public static BundleManagerCDNConfig DontStrip = new BundleManagerCDNConfig();

		// Token: 0x040047B7 RID: 18359
		public string SBSAccount;

		// Token: 0x040047B8 RID: 18360
		public string Environment;
	}
}
