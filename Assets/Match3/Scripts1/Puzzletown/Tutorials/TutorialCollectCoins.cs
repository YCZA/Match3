using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts2.Building.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A79 RID: 2681
	[CreateAssetMenu(fileName = "ScriptTutorialCollectCoins", menuName = "Puzzletown/Tutorials/Create/TutorialCollectCoins")]
	public class TutorialCollectCoins : ATutorialScript
	{
		// Token: 0x06004015 RID: 16405 RVA: 0x00149060 File Offset: 0x00147460
		protected override IEnumerator ExecuteRoutine()
		{
			TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
			tutorialRunner.GameStateService.Resources.AddMaterial("coins", this.coinsAmount, true);
			TownUiRoot townUiRoot = global::UnityEngine.Object.FindObjectOfType<TownUiRoot>();
			TownShopRoot townShop = townUiRoot.ShopDialog;
			while (townShop == null || !townShop.onShopReady.WasDispatched)
			{
				yield return null;
			}
			townShop.scrollRect.content.localPosition = Vector3.zero;
			if (this.useDialogIcon)
			{
				TutorialOverlayRoot tutorialOverlay = global::UnityEngine.Object.FindObjectOfType<TutorialOverlayRoot>();
				while (tutorialOverlay == null)
				{
					tutorialOverlay = global::UnityEngine.Object.FindObjectOfType<TutorialOverlayRoot>();
					yield return null;
				}
				WooroutineRunner.StartCoroutine(this.DelayedAddCoins(tutorialOverlay, townUiRoot), null);
			}
			else
			{
				townUiRoot.CollectMaterials(new MaterialAmount("coins", 100, MaterialAmountUsage.Undefined, 0), townUiRoot.transform, true);
			}
			yield return new WaitForEndOfFrame();
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0014907C File Offset: 0x0014747C
		private IEnumerator DelayedAddCoins(TutorialOverlayRoot tutorialOverlay, TownUiRoot townUiRoot)
		{
			yield return new WaitForSeconds(0.5f);
			townUiRoot.CollectMaterials(new MaterialAmount("coins", 100, MaterialAmountUsage.Undefined, 0), tutorialOverlay.speechBubble.icon, true);
			yield break;
		}

		// Token: 0x040069C7 RID: 27079
		public int coinsAmount;

		// Token: 0x040069C8 RID: 27080
		public bool useDialogIcon;
	}
}
