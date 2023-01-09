using System.Collections;
using Match3.Scripts1.Shared.Flows;
using Wooga.UnityFramework;
using Match3.Scripts2.Shop;

// Token: 0x020008A0 RID: 2208
namespace Match3.Scripts1
{
	public class CheckLivesJourney : AFlow
	{
		// Token: 0x06003603 RID: 13827 RVA: 0x00104F88 File Offset: 0x00103388
		public CheckLivesJourney(TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context)
		{
			this._diamondsPurchaseContext = context;
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x00104F98 File Offset: 0x00103398
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (!this.livesService.IsCurrentlyUnlimitedLives && this.livesService.CurrentLives == 0)
			{
				yield return new GetLivesJourney("start_level", this._diamondsPurchaseContext).Start();
			}
			bool hasLifeNow = this.livesService.CurrentLives > 0 || this.livesService.IsCurrentlyUnlimitedLives;
			yield return hasLifeNow;
			yield break;
		}

		// Token: 0x04005E01 RID: 24065
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005E02 RID: 24066
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _diamondsPurchaseContext;
	}
}
