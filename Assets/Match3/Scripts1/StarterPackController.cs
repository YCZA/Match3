using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x02000B7B RID: 2939
namespace Match3.Scripts1
{
	public class StarterPackController : MonoBehaviour
	{
		// Token: 0x060044CC RID: 17612 RVA: 0x0015D6D4 File Offset: 0x0015BAD4
		public void Init(GameStateService gameState, GeneralConfig generalConfig, ProgressionDataService.Service progression, OffersService offers, SaleService saleService)
		{
			this.gameState = gameState;
			this.generalConfig = generalConfig;
			this.progression = progression;
			this.offers = offers;
			this.saleService = saleService;
			offers.onCurrentOfferChanged.AddListener(new Action(this.UpdateDisplay));
			offers.UpdateCurrentOffer(false);
			this.UpdateDisplay();
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x0015D72C File Offset: 0x0015BB2C
		private void UpdateDisplay()
		{
			IAPData currentOffer = this.offers.CurrentOffer;
			base.gameObject.SetActive(currentOffer != null && currentOffer.IsAvailable && this.gameState.GetSeenFlagCount(currentOffer.iap_name) < this.generalConfig.balance.starter_pack_hud_dismiss_count && this.progression.UnlockedLevel > this.generalConfig.balance.starter_pack_hud_icon_level && this.saleService.CurrentSale == null);
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x0015D7B8 File Offset: 0x0015BBB8
		private void OnDestroy()
		{
			if (this.offers != null)
			{
				this.offers.onCurrentOfferChanged.RemoveListener(new Action(this.UpdateDisplay));
			}
		}

		// Token: 0x04006C90 RID: 27792
		private GameStateService gameState;

		// Token: 0x04006C91 RID: 27793
		private GeneralConfig generalConfig;

		// Token: 0x04006C92 RID: 27794
		private ProgressionDataService.Service progression;

		// Token: 0x04006C93 RID: 27795
		private OffersService offers;

		// Token: 0x04006C94 RID: 27796
		private SaleService saleService;
	}
}
