using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x020002FE RID: 766
	public static class AssetBundleLoader
	{
		// eli key point: LoadAssetAsync()
		public static IEnumerator<T> LoadAssetAsync<T>(AssetBundle assetBundle, string assetName)
		{
			// eli todo 暂时用resource.load2
			// new code
			int lastPointPos = assetName.LastIndexOf('.');
			assetName = assetName.Substring(0, lastPointPos);
			assetName = assetName.Replace("Assets/", "");
			// Debug.Log("LoadAssetAsync " + assetName);
			var asset = Resources.Load(assetName);
			if (asset == null)
			{
				Debug.LogWarning($"{assetName} is null");
			}

			T result; 
			try
			{
				if (asset is Texture2D)
				{
					var texture = asset as Texture2D;
					asset = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f, 0.5f));
				}
				result = (T) (object) asset;
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				throw;
			}
			yield return result;
			// old code
			// AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(assetName);
			// while (!request.isDone)
			// {
				// yield return default(T);
			// }
			// if (!(request.asset == null))
			// {
				// yield return (T)((object)request.asset);
				// yield break;
			// }
			// if (assetBundle.Contains(assetName))
			// {
				// throw new AssetInvalidContentException(assetBundle.name, assetName, typeof(T).Name);
			// }
			// throw new AssetNotFoundException(assetBundle.name, assetName);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0006D1F4 File Offset: 0x0006B5F4
		public static IEnumerator<global::UnityEngine.Object> LoadAssetAsync(Type assetType, AssetBundle assetBundle, string assetName)
		{
			Debug.LogError("LoadAssetAsync2 " + assetName);
			AssetBundleRequest request = assetBundle.LoadAssetAsync(assetName, assetType);
			while (!request.isDone)
			{
				yield return null;
			}
			if (!(request.asset == null))
			{
				yield return request.asset;
				yield break;
			}
			if (assetBundle.Contains(assetName))
			{
				throw new AssetInvalidContentException(assetBundle.name, assetName, assetType.Name);
			}
			throw new AssetNotFoundException(assetBundle.name, assetName);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0006D220 File Offset: 0x0006B620
		public static IEnumerator<T> LoadAssetAsyncAndClose<T>(AssetBundle assetBundle, string assetName)
		{
			try
			{
				AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(assetName);
				while (!request.isDone)
				{
					yield return default(T);
				}
				if (request.asset == null)
				{
					if (assetBundle.Contains(assetName))
					{
						throw new AssetInvalidContentException(assetBundle.name, assetName, typeof(T).Name);
					}
					throw new AssetNotFoundException(assetBundle.name, assetName);
				}
				else
				{
					yield return (T)((object)request.asset);
				}
			}
			finally
			{
				if (assetBundle != null)
				{
					assetBundle.Unload(false);
				}
			}
			yield break;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0006D244 File Offset: 0x0006B644
		public static IEnumerator<global::UnityEngine.Object> LoadAssetAsyncAndClose(Type assetType, AssetBundle assetBundle, string assetName)
		{
			try
			{
				AssetBundleRequest request = assetBundle.LoadAssetAsync(assetName, assetType);
				while (!request.isDone)
				{
					yield return null;
				}
				if (request.asset == null)
				{
					if (assetBundle.Contains(assetName))
					{
						throw new AssetInvalidContentException(assetBundle.name, assetName, assetType.Name);
					}
					throw new AssetNotFoundException(assetBundle.name, assetName);
				}
				else
				{
					yield return request.asset;
				}
			}
			finally
			{
				if (assetBundle != null)
				{
					assetBundle.Unload(false);
				}
			}
			yield break;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0006D270 File Offset: 0x0006B670
		public static T LoadAssetSync<T>(AssetBundle assetBundle, string assetName) where T : global::UnityEngine.Object
		{
			Debug.LogError("LoadAssetAsync3 " + assetName);
			if (!assetBundle.Contains(assetName))
			{
				throw new AssetNotFoundException(assetBundle.name, assetName);
			}
			T t = assetBundle.LoadAsset<T>(assetName);
			if (t == null)
			{
				throw new AssetInvalidContentException(assetBundle.name, assetName, typeof(T).Name);
			}
			return t;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0006D2CC File Offset: 0x0006B6CC
		public static global::UnityEngine.Object LoadAssetSync(Type assetType, AssetBundle assetBundle, string assetName)
		{
			Debug.LogError("LoadAssetAsync4 " + assetName);
			if (!assetBundle.Contains(assetName))
			{
				throw new AssetNotFoundException(assetBundle.name, assetName);
			}
			global::UnityEngine.Object @object = assetBundle.LoadAsset(assetName, assetType);
			if (@object == null)
			{
				throw new AssetInvalidContentException(assetBundle.name, assetName, assetType.Name);
			}
			return @object;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0006D31C File Offset: 0x0006B71C
		public static T LoadAssetSyncAndClose<T>(AssetBundle assetBundle, string assetName) where T : global::UnityEngine.Object
		{
			T result;
			try
			{
				result = AssetBundleLoader.LoadAssetSync<T>(assetBundle, assetName);
			}
			finally
			{
				if (assetBundle != null)
				{
					assetBundle.Unload(false);
				}
			}
			return result;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0006D35C File Offset: 0x0006B75C
		public static global::UnityEngine.Object LoadAssetSyncAndClose(Type assetType, AssetBundle assetBundle, string assetName)
		{
			global::UnityEngine.Object result;
			try
			{
				result = AssetBundleLoader.LoadAssetSync(assetType, assetBundle, assetName);
			}
			finally
			{
				if (assetBundle != null)
				{
					assetBundle.Unload(false);
				}
			}
			return result;
		}

		// eli key point: LoadSceneAsync()
		public static IEnumerator<BundledScene> LoadSceneAsync(string level, bool isAditive)
		{
			Debug.LogError("LoadAssetAsync5 " + level);
			return AssetBundleLoader.LoadAndWaitForScene(level, isAditive).ContinueWith(() => new BundledScene(level));
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0006D3D3 File Offset: 0x0006B7D3
		public static BundledScene LoadSceneSync(string level, bool isAdditive)
		{
			if (isAdditive)
			{
				SceneManager.LoadScene(level, LoadSceneMode.Additive);
			}
			else
			{
				SceneManager.LoadScene(level, LoadSceneMode.Single);
			}
			return new BundledScene(level);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0006D3F4 File Offset: 0x0006B7F4
		private static IEnumerator LoadAndWaitForScene(string level, bool isAdditive)
		{
			if (isAdditive)
			{
				yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
			}
			else
			{
				yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
			}
			yield break;
		}
	}
}
