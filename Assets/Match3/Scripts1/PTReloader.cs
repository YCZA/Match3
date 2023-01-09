using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.Start;
using UnityEngine;

// Token: 0x02000737 RID: 1847
namespace Match3.Scripts1
{
	public class PTReloader : MonoBehaviour
	{
		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000D4D48 File Offset: 0x000D3148
		private static PTReloader Instance
		{
			get
			{
				if (PTReloader.instance != null)
				{
					return PTReloader.instance;
				}
				PTReloader.instance = new GameObject("PTReloader").AddComponent<PTReloader>();
				global::UnityEngine.Object.DontDestroyOnLoad(PTReloader.instance);
				PTReloader.instance.hideFlags = HideFlags.DontSaveInEditor;
				PTReloader.EnsureCleanupAfterGameStoppedInEditor();
				return PTReloader.instance;
			}
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000D4D9E File Offset: 0x000D319E
		private static void EnsureCleanupAfterGameStoppedInEditor()
		{
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000D4DA0 File Offset: 0x000D31A0
		public static int TimeSpanForRestartInSeconds
		{
			get
			{
				GameEnvironment.Environment currentEnvironment = GameEnvironment.CurrentEnvironment;
				if (currentEnvironment == GameEnvironment.Environment.STAGING)
				{
					return 3600;
				}
				if (currentEnvironment != GameEnvironment.Environment.PRODUCTION)
				{
					return 600;
				}
				return 14400;
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000D4DD8 File Offset: 0x000D31D8
		public static bool ShouldReloadGame()
		{
			int num = (int)(DateTime.UtcNow - PTReloader.goToBackgroundTime).TotalSeconds;
			return num > PTReloader.TimeSpanForRestartInSeconds && PTReloader.IsNotLoadingScenesOrPlayingMatch3() && Application.internetReachability != NetworkReachability.NotReachable && !Application.isEditor;
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000D4E28 File Offset: 0x000D3228
		public static bool IsNotLoadingScenesOrPlayingMatch3()
		{
			return !SceneManager.IsPlayingMatch3 && !SceneManager.IsLoadingAnything;
		}

		// eli key point 重进游戏
		public static void ReloadGame(string reloadReason, bool skipStartScene = false)
		{
			if (!PTReloader.Instance.isReloading)
			{
				PTReloader.Instance.DoReloadGame(reloadReason, skipStartScene);
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000D4E5C File Offset: 0x000D325C
		private void DoReloadGame(string reloadReason, bool skipStartScene)
		{
			base.StartCoroutine(this.ReloadWhenSafe(reloadReason, skipStartScene));
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000D4E70 File Offset: 0x000D3270
		private IEnumerator ReloadWhenSafe(string reloadReason, bool skipStartScene)
		{
			this.isReloading = true;
			if (ServiceLocator.Instance.Has(typeof(AssetBundleService), false))
			{
				yield return ServiceLocator.Instance.Inject(PTReloader.Instance);
			}
			BlockerManager.Clear();
			WooroutineRunner.PrepareForReset();
			CoroutineRunner.Instance.StopAllCoroutines();
			EventSystemRoot eventSystemRoot = global::UnityEngine.Object.FindObjectOfType<EventSystemRoot>();
			if (eventSystemRoot != null)
			{
				eventSystemRoot.Disable();
			}
			yield return SceneManager.Instance.UnloadAllAsync();
			ServiceLocator.Instance.UnregisterAll();
			string reloadBreadcrumb = string.Format("PTReloader: Reload reason: {0}", reloadReason);
			EAHelper.AddBreadcrumb(reloadBreadcrumb);
			WoogaDebug.Log(new object[]
			{
				reloadBreadcrumb
			});
			yield return SceneManager.Instance.LoadSceneWithParams<StartRoot, bool>(skipStartScene, null);
			this.isReloading = false;
			yield break;
		}

		// Token: 0x04005754 RID: 22356
		private const int TIME_SPAN_FOR_RESTART_SECONDS_CI = 600;

		// Token: 0x04005755 RID: 22357
		private const int TIME_SPAN_FOR_RESTART_SECONDS_STAGING = 3600;

		// Token: 0x04005756 RID: 22358
		private const int TIME_SPAN_FOR_RESTART_SECONDS_PRODUCTION = 14400;

		// Token: 0x04005757 RID: 22359
		private static PTReloader instance;

		// Token: 0x04005758 RID: 22360
		public static DateTime goToBackgroundTime;

		// Token: 0x04005759 RID: 22361
		private bool isReloading;

		// Token: 0x0400575A RID: 22362
		[WaitForService(true, true)]
		private AssetBundleService abService;
	}
}
