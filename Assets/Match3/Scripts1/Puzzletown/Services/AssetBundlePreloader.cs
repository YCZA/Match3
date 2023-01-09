using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000744 RID: 1860
	public class AssetBundlePreloader
	{
		// Token: 0x06002E01 RID: 11777 RVA: 0x000D61A3 File Offset: 0x000D45A3
		public AssetBundlePreloader(AssetBundleService abs)
		{
			this.abs = abs;
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000D61BD File Offset: 0x000D45BD
		public void Preload(IEnumerable<string> specificBundlesToPreload)
		{
			if (specificBundlesToPreload == null)
			{
				WooroutineRunner.StartCoroutine(this.PreloadTriggersRoutine(), null);
			}
			else
			{
				WooroutineRunner.StartCoroutine(this.PreloadSpecificBundlesRoutine(specificBundlesToPreload), null);
			}
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000D61E8 File Offset: 0x000D45E8
		private IEnumerator PreloadTriggersRoutine()
		{
			foreach (IPreloadTrigger trigger in this.triggers)
			{
				if (trigger.ShouldPreload())
				{
					yield return this.PreloadRoutine(trigger);
				}
			}
			yield break;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000D6204 File Offset: 0x000D4604
		private IEnumerator PreloadRoutine(IPreloadTrigger trigger)
		{
			yield return this.PreloadSpecificBundlesRoutine(trigger.BundleNames);
			yield break;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000D6228 File Offset: 0x000D4628
		private IEnumerator PreloadSpecificBundlesRoutine(IEnumerable<string> bundlesToPreload)
		{
			foreach (string bundle in bundlesToPreload)
			{
				Wooroutine<bool> isAvailableRoutine = this.abs.IsBundleAvailable(bundle);
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
					Wooroutine<bool> operation = this.abs.PreLoadBundle(bundle, null);
					yield return operation;
				}
			}
			yield break;
		}

		// Token: 0x04005781 RID: 22401
		private AssetBundleService abs;

		// Token: 0x04005782 RID: 22402
		public List<IPreloadTrigger> triggers = new List<IPreloadTrigger>();
	}
}
