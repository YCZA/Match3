using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Shared.ResourceManager
{
	// Token: 0x02000B1A RID: 2842
	public class BundledSpriteManagerLoaderFlow : AFlowR<BundledSpriteManagerLoaderFlow.Input, SpriteManager>
	{
		// Token: 0x060042D1 RID: 17105 RVA: 0x001561F0 File Offset: 0x001545F0
		protected override IEnumerator FlowRoutine(BundledSpriteManagerLoaderFlow.Input bundleInput)
		{
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<bool> isAvailableRoutine = this.assetBundleService.IsBundleAvailable(bundleInput.bundleName);
			yield return isAvailableRoutine;
			bool isAvailable = false;
			try
			{
				isAvailable = isAvailableRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex.Message
				});
			}
			if (!isAvailable)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"not available",
					bundleInput.bundleName
				});
				yield return null;
				yield break;
			}
			Wooroutine<GameObject> spriteManager = this.assetBundleService.LoadAsset<GameObject>(bundleInput.bundleName, bundleInput.path);
			yield return spriteManager;
			if (spriteManager.ReturnValue != null)
			{
				yield return spriteManager.ReturnValue.GetComponent<SpriteManager>();
			}
			else
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04006B96 RID: 27542
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x02000B1B RID: 2843
		public class Input
		{
			// Token: 0x04006B97 RID: 27543
			public string bundleName;

			// Token: 0x04006B98 RID: 27544
			public string path;
		}
	}
}
