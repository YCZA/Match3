using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

// Token: 0x020009F8 RID: 2552
namespace Match3.Scripts1
{
	public class PopupDailyDealOfferRoot : APtSceneRoot, IDisposableDialog, IHandler<PopupOperation>
	{
		// Token: 0x06003D85 RID: 15749 RVA: 0x00136AF8 File Offset: 0x00134EF8
		protected override void Go()
		{
			this.countdownTimer.SetTargetTime(this.gameStateService.DailyDeals.CurrentDealExpirationDate, false, null);
			foreach (MaterialAmountView materialAmountView in this.bonusRewardViews)
			{
				materialAmountView.gameObject.SetActive(false);
			}
			DailyDealsConfig.Deal currentDeal = this.gameStateService.DailyDeals.CurrentDeal;
			Sprite buildingSprite = this.dailyDealsService.BuildingSprite;
			if (!string.IsNullOrEmpty(currentDeal.bonus_2_type))
			{
				this.bonusRewardViews[1].Show(new MaterialAmount(currentDeal.bonus_2_type, currentDeal.bonus_2_amount, MaterialAmountUsage.Undefined, 0));
			}
			bool flag = string.IsNullOrEmpty(currentDeal.bonus_3_type);
			if (!flag)
			{
				this.bonusRewardViews[2].Show(new MaterialAmount(currentDeal.bonus_3_type, currentDeal.bonus_3_amount, MaterialAmountUsage.Undefined, 0));
			}
			this.singleBonusRewardArea.SetActive(flag);
			this.multipleBonusRewardArea.SetActive(!flag);
			this.largeRewardArea.gameObject.SetActive(false);
			if (!string.IsNullOrEmpty(currentDeal.bonus_1_type))
			{
				if (currentDeal.bonus_1_type.StartsWith("iso_"))
				{
					this.singleBonusRewardArea.SetActive(false);
					this.multipleBonusRewardArea.SetActive(false);
					BuildingConfig config = this.configService.buildingConfigList.GetConfig(currentDeal.bonus_1_type);
					if (config != null && buildingSprite != null)
					{
						BuildingShopData data = new BuildingShopData
						{
							buildingImage = buildingSprite,
							buildingName = config.name,
							data = config
						};
						this.largeRewardArea.gameObject.SetActive(true);
						this.largeRewardArea.Show(data);
					}
					else
					{
						this.largeRewardArea.gameObject.SetActive(false);
					}
				}
				else
				{
					MaterialAmount mat = new MaterialAmount(currentDeal.bonus_1_type, currentDeal.bonus_1_amount, MaterialAmountUsage.Undefined, 0);
					this.bonusRewardViews[0].Show(mat);
					this.mediumBonusRewardView.Show(mat);
					this.singleBonusRewardArea.SetActive(flag);
				}
			}
			this.multipleBonusRewardArea.SetActive(!flag);
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.dialog.Show();
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00136D80 File Offset: 0x00135180
		public void Close()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00136DB8 File Offset: 0x001351B8
		public void Handle(PopupOperation op)
		{
			if (op != PopupOperation.Close)
			{
				if (op == PopupOperation.OK)
				{
					if (!this.dialog.IsOpening)
					{
						base.StartCoroutine(this.ShowDiamondPanelFlow());
					}
				}
			}
			else
			{
				this.Close();
			}
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00136E08 File Offset: 0x00135208
		private IEnumerator ShowDiamondPanelFlow()
		{
			TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters();
			context.source1 = "daily_deal";
			context.source2 = "auto";
			this.diamondsPanelRoot.context = context;
			yield return new PurchaseDiamondsJourney(context, false).Start();
			this.Close();
			yield break;
		}

		// Token: 0x04006656 RID: 26198
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04006657 RID: 26199
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006658 RID: 26200
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006659 RID: 26201
		[WaitForService(true, true)]
		private DailyDealsService dailyDealsService;

		// Token: 0x0400665A RID: 26202
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400665B RID: 26203
		[WaitForRoot(false, false)]
		private TownDiamondsPanelRoot diamondsPanelRoot;

		// Token: 0x0400665C RID: 26204
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x0400665D RID: 26205
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x0400665E RID: 26206
		[SerializeField]
		private GameObject singleBonusRewardArea;

		// Token: 0x0400665F RID: 26207
		[SerializeField]
		private GameObject multipleBonusRewardArea;

		// Token: 0x04006660 RID: 26208
		[SerializeField]
		private BuildingShopView largeRewardArea;

		// Token: 0x04006661 RID: 26209
		[SerializeField]
		private MaterialAmountView mediumBonusRewardView;

		// Token: 0x04006662 RID: 26210
		[SerializeField]
		private List<MaterialAmountView> bonusRewardViews;

		// Token: 0x020009F9 RID: 2553
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003D89 RID: 15753 RVA: 0x00136E23 File Offset: 0x00135223
			public Trigger(ProgressionDataService.Service progressionService, SBSService sbsService, GameStateService gameStateService, TownDiamondsPanelRoot townDiamondsPanelRoot, int numberOfPopupRuns)
			{
				this.progressionService = progressionService;
				this.sbsService = sbsService;
				this.numberOfPopupRuns = numberOfPopupRuns;
				this.gameStateService = gameStateService;
				this.townDiamondsPanelRoot = townDiamondsPanelRoot;
			}

			// Token: 0x06003D8A RID: 15754 RVA: 0x00136E50 File Offset: 0x00135250
			public override bool ShouldTrigger()
			{
				return this.numberOfPopupRuns == this.sbsService.SbsConfig.dailydealsconfig.balancing.number_of_island_loads && Application.internetReachability != NetworkReachability.NotReachable && this.gameStateService.DailyDeals.CurrentDeal != null && this.gameStateService.DailyDeals.CurrentDealActive && !this.gameStateService.DailyDeals.CurrentDealPurchased && this.townDiamondsPanelRoot.NumberOfWindowOpens < 1 && this.progressionService.UnlockedLevel >= this.sbsService.SbsConfig.dailydealsconfig.balancing.unlock_level;
			}

			// Token: 0x06003D8B RID: 15755 RVA: 0x00136F0C File Offset: 0x0013530C
			public override IEnumerator Run()
			{
				Wooroutine<PopupDailyDealOfferRoot> scene = SceneManager.Instance.LoadScene<PopupDailyDealOfferRoot>(null);
				yield return scene;
				yield return scene.ReturnValue.onDestroyed;
				yield break;
			}

			// Token: 0x04006663 RID: 26211
			private ProgressionDataService.Service progressionService;

			// Token: 0x04006664 RID: 26212
			private SBSService sbsService;

			// Token: 0x04006665 RID: 26213
			private GameStateService gameStateService;

			// Token: 0x04006666 RID: 26214
			private TownDiamondsPanelRoot townDiamondsPanelRoot;

			// Token: 0x04006667 RID: 26215
			private int numberOfPopupRuns;
		}
	}
}
