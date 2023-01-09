using System;
using System.Collections;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;

// Token: 0x0200089B RID: 2203
namespace Match3.Scripts1
{
	public class BuyMoreMovesFlow : AFlow
	{
		// Token: 0x060035F4 RID: 13812 RVA: 0x00103BA0 File Offset: 0x00101FA0
		public BuyMoreMovesFlow(ScoringController controller, LevelConfig levelConfig, BoostsUiRoot boostsUI, TrackingService.PurchaseFlowContext resourcePurchaseContext, BuyMoreMovesFlow.TrackingContext purchaseContext)
		{
			WoogaDebug.Log(new object[]
			{
				"Starting BuyMoreMoves Journey"
			});
			this._scoringController = controller;
			this._levelConfig = levelConfig;
			this._boostsUI = boostsUI;
			this._resourcePurchaseContext = resourcePurchaseContext;
			this._purchaseContext = purchaseContext;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00103BE0 File Offset: 0x00101FE0
		private int calculateCost(BoosterData data)
		{
			if (this.configService.SbsConfig.feature_switches.buy_more_moves_offer && this.progressionService.Data.UnlockedLevel < this.banners.levelUntilAddMovesAreFree && this._levelConfig.SelectedTier < AreaConfig.Tier.b)
			{
				return 0;
			}
			return data.cost;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x00103C40 File Offset: 0x00102040
		private IEnumerator ChooseActionRoutine(bool movePackEnabled, IAPData iapData, IAPContent[] iapContents, int cost, TrackingService.MoreMovesPurchaseTrackingContext moreMovesContext)
		{
			bool confirmedExit = false;
			this.audioService.PlaySFX(AudioId.OutOfMoves, false, false, false);
			while (!confirmedExit)
			{
				bool cannotAfford = !this.gameStateService.Resources.HasEnoughMaterial("diamonds", cost);
				bool purchaseMovePack = cannotAfford && movePackEnabled;
				if (purchaseMovePack)
				{
					this.banners.ShowBuyMoreMoves(this._levelConfig, iapData);
					// this.trackingService.TrackMoreMovesPurchaseAction("shop_open", iapData, iapContents, moreMovesContext, string.Empty);
				}
				else
				{
					this.banners.ShowOutOfMoves(this._levelConfig);
				}
				while (this.banners.operation == PopupOperation.None)
				{
					yield return null;
				}
				confirmedExit = true;
				if (this.banners.operation == PopupOperation.Close)
				{
					TournamentScore tournamentScore = this._scoringController.GetTournamentScore();
					if (this._scoringController.GetLevelPlayMode() == LevelPlayMode.DiveForTreasure || this._scoringController.GetLevelPlayMode() == LevelPlayMode.PirateBreakout)
					{
						this.banners.dialog.Hide();
						Wooroutine<bool> lossAversionFlow = new M3_LossAversionFlow().Start(this._scoringController);
						yield return lossAversionFlow;
						confirmedExit = lossAversionFlow.ReturnValue;
						if (!confirmedExit)
						{
							this.banners.dialog.Show();
						}
					}
					else if (tournamentScore.TournamentType != TournamentType.Undefined && tournamentScore.CollectedPoints > 0)
					{
						Wooroutine<bool> tournamentLossAversionFlow = new M3_TournamentLossAversionFlow().Start(tournamentScore);
						yield return tournamentLossAversionFlow;
						confirmedExit = tournamentLossAversionFlow.ReturnValue;
					}
				}
			}
			yield break;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x00103C80 File Offset: 0x00102080
		private IEnumerator PurchaseActionRoutine(bool movePackEnabled, bool cannotAfford, IAPData iapData, IAPContent[] iapContents, int cost, BoosterData boosterData, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters purchaseDiamondsContext, TrackingService.MoreMovesPurchaseTrackingContext moreMovesContext)
		{
			bool purchaseMovePack = cannotAfford && movePackEnabled;
			bool moreMovesPurchased = false;
			if (purchaseMovePack)
			{
				PurchaseMovesPackFlow movePackFlow = new PurchaseMovesPackFlow(iapData);
				yield return movePackFlow.Start();
				if (movePackFlow.Result.success)
				{
					moreMovesPurchased = true;
					IAPContent iapcontent = iapContents[0];
					this.boosterService.AddBoost(iapcontent.item_resource, iapcontent.item_amount);
					// this.trackingService.TrackMoreMovesPurchaseAction("purchase_done", iapData, iapContents, moreMovesContext, string.Empty);
				}
				else
				{
					// this.trackingService.TrackMoreMovesPurchaseAction("purchase_failed", iapData, iapContents, moreMovesContext, movePackFlow.Result.failureReason.ToString());
				}
			}
			else
			{
				if (cannotAfford)
				{
					yield return new PurchaseDiamondsJourney(purchaseDiamondsContext, true).Start();
				}
				if (this.gameStateService.Resources.Pay(new MaterialAmount("diamonds", cost, MaterialAmountUsage.Undefined, 0)))
				{
					moreMovesPurchased = true;
					this._resourcePurchaseContext.det4 = boosterData.amount.ToString();
					// this.trackingService.TrackPurchase(this._resourcePurchaseContext, 0, cost);
				}
			}
			// 购买步数
			if (moreMovesPurchased)
			{
				this._scoringController.AddMoves(boosterData.amount);
				this._scoringController.MoreMovesBought();
				this.banners.Hide();
			}
			else
			{
				yield return this.FlowRoutine();
			}
			yield break;
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x00103CD8 File Offset: 0x001020D8
		protected override IEnumerator FlowRoutine()
		{
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonPressed));
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			BoosterData boosterData = this.configService.general.boosters.Find((BoosterData e) => e.type == "moves");
			TrackingService.MoreMovesPurchaseTrackingContext moreMovesContext = new TrackingService.MoreMovesPurchaseTrackingContext
			{
				source1 = this._purchaseContext.source1,
				source2 = this._levelConfig.Name,
				moveCount = boosterData.amount
			};
			TownDiamondsPanelRoot.TownDiamondsPanelRootParameters purchaseDiamondsContext = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
			{
				source1 = this._purchaseContext.source1,
				source2 = this._purchaseContext.source2
			};
			string iapName = "offer_post_moves";
			IAPData iapData = this.iapService.IAPs.FirstOrDefault((IAPData iap) => iap.iap_name == iapName);
			IAPContent[] iapContents = this.iapService.GetContents(iapData);
			if (iapContents.Length != 1)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Misconfigured more moves pack IAP, should only have 1 content item"
				});
			}
			int cost = this.calculateCost(boosterData);
			// bool hasProductInfo = iapData.storeProduct != null;
			bool hasProductInfo = true;
			bool movePackFeatureEnabled = this.sbsService.SbsConfig.feature_switches.enable_movepack_iap;
			bool movePackEnabled = movePackFeatureEnabled && hasProductInfo;
			yield return this.ChooseActionRoutine(movePackEnabled, iapData, iapContents, cost, moreMovesContext);
			bool cannotAfford = !this.gameStateService.Resources.HasEnoughMaterial("diamonds", cost);
			if (this.banners.operation == PopupOperation.OK)
			{
				yield return this.PurchaseActionRoutine(movePackEnabled, cannotAfford, iapData, iapContents, cost, boosterData, purchaseDiamondsContext, moreMovesContext);
			}
			else
			{
				bool flag = cannotAfford && movePackEnabled;
				if (flag)
				{
					// this.trackingService.TrackMoreMovesPurchaseAction("give_up", iapData, iapContents, moreMovesContext, string.Empty);
				}
			}
			if (this._boostsUI != null)
			{
				this._boostsUI.UnselectActive();
			}
			this.banners.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButtonPressed));
			yield break;
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x00103CF3 File Offset: 0x001020F3
		private void HandleBackButtonPressed()
		{
			this.banners.Handle(PopupOperation.Close);
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonPressed));
		}

		// Token: 0x04005DE3 RID: 24035
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005DE4 RID: 24036
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04005DE5 RID: 24037
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005DE6 RID: 24038
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005DE7 RID: 24039
		// [WaitForService(true, true)]
		// private TrackingService trackingService;

		// Token: 0x04005DE8 RID: 24040
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04005DE9 RID: 24041
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x04005DEA RID: 24042
		[WaitForService(true, true)]
		private BoostsService boosterService;

		// Token: 0x04005DEB RID: 24043
		[WaitForService(true, true)]
		private IAPService iapService;

		// Token: 0x04005DEC RID: 24044
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005DED RID: 24045
		[WaitForRoot(false, false)]
		private M3_BannersRoot banners;

		// Token: 0x04005DEE RID: 24046
		private readonly ScoringController _scoringController;

		// Token: 0x04005DEF RID: 24047
		private readonly LevelConfig _levelConfig;

		// Token: 0x04005DF0 RID: 24048
		private readonly BoostsUiRoot _boostsUI;

		// Token: 0x04005DF1 RID: 24049
		private readonly TrackingService.PurchaseFlowContext _resourcePurchaseContext;

		// Token: 0x04005DF2 RID: 24050
		private readonly BuyMoreMovesFlow.TrackingContext _purchaseContext;

		// Token: 0x0200089C RID: 2204
		public class TrackingContext
		{
			// Token: 0x04005DF3 RID: 24051
			public string source1;

			// Token: 0x04005DF4 RID: 24052
			public string source2;
		}
	}
}
