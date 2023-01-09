using System;
using System.Collections.Generic;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.AssetBundleManager.Internal;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x020002F5 RID: 757
	public class AssetBundleCache : IDisposable
	{
		// Token: 0x060017CA RID: 6090 RVA: 0x0006CF27 File Offset: 0x0006B327
		public static AssetBundleCache GetInstance()
		{
			if (AssetBundleCache._Instance == null)
			{
				AssetBundleCache._Instance = new AssetBundleCache();
			}
			return AssetBundleCache._Instance;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0006CF44 File Offset: 0x0006B344
		public static void Clear()
		{
			if (AssetBundleCache._Instance != null)
			{
				AssetBundleCache._Instance.Dispose();
				AssetBundleCache._Instance = null;
			}
			AssetBundle[] array = Resources.FindObjectsOfTypeAll<AssetBundle>();
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					AssetBundle assetBundle = array[i];
					assetBundle.Unload(false);
				}
				catch
				{
				}
			}
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0006CFAC File Offset: 0x0006B3AC
		public static bool HasBundles()
		{
			return AssetBundleCache._Instance != null && AssetBundleCache._Instance._bundles.Count > 0;
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0006CFCD File Offset: 0x0006B3CD
		public void AddBundle(string bundleName, AssetBundle assetBundle)
		{
			this._bundles.Add(bundleName, assetBundle);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0006CFDC File Offset: 0x0006B3DC
		public bool IsLoaded(string bundleName)
		{
			return this._bundles.ContainsKey(bundleName);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0006CFEA File Offset: 0x0006B3EA
		public AssetBundle GetBundle(string bundleName)
		{
			return this._bundles.Get(bundleName, null);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0006CFFC File Offset: 0x0006B3FC
		public void Unload(string bundleName, bool unloadAllLoadedObjects)
		{
			AssetBundle assetBundle;
			if (this._bundles.TryGetValue(bundleName, out assetBundle))
			{
				assetBundle.Unload(unloadAllLoadedObjects);
				this._bundles.Remove(bundleName);
			}
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x0006D030 File Offset: 0x0006B430
		public void Dispose()
		{
			if (this._bundles != null)
			{
				foreach (KeyValuePair<string, AssetBundle> keyValuePair in this._bundles)
				{
					try
					{
						if (keyValuePair.Value != null)
						{
							keyValuePair.Value.Unload(false);
						}
					}
					catch (Exception ex)
					{
						Log.WarningFormatted("Could not unload bundle: {0}, {1}", new object[]
						{
							keyValuePair.Key,
							ex
						});
					}
				}
				this._bundles.Clear();
			}
		}

		// Token: 0x04004794 RID: 18324
		private static AssetBundleCache _Instance;

		// Token: 0x04004795 RID: 18325
		private readonly Dictionary<string, AssetBundle> _bundles = new Dictionary<string, AssetBundle>();
	}
}
