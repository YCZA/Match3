using System.Collections;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Shop;

// Token: 0x0200089F RID: 2207
namespace Match3.Scripts1
{
	public class GetLivesJourney : AFlow
	{
		// Token: 0x06003601 RID: 13825 RVA: 0x00104E77 File Offset: 0x00103277
		public GetLivesJourney(string purchaseContext, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context)
		{
			this._purchaseContext = purchaseContext;
			this._diamondsPurchaseContext = context;
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x00104E90 File Offset: 0x00103290
		protected override IEnumerator FlowRoutine()
		{
			Wooroutine<LivesShopRoot> gameScene = SceneManager.Instance.LoadScene<LivesShopRoot>(null);
			yield return gameScene;
			yield return gameScene.ReturnValue.TryToBuy(this._purchaseContext, this._diamondsPurchaseContext);
			yield break;
		}

		// Token: 0x04005DFF RID: 24063
		private string _purchaseContext;

		// Token: 0x04005E00 RID: 24064
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _diamondsPurchaseContext;
	}
}
