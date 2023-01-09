using System;
using System.Collections;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000728 RID: 1832
namespace Match3.Scripts1
{
	public abstract class M3_ALevelStartRoot : APtSceneRoot<LevelConfig, bool>, IHandler<PopupOperation>, IHandler<BoostViewData, BoostOperation>, IPersistentDialog
	{
		// Token: 0x06002D4F RID: 11599 RVA: 0x000D2995 File Offset: 0x000D0D95
		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.OnInitialized.WasDispatched)
			{
				this.Show(this.parameters);
			}
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000D29B9 File Offset: 0x000D0DB9
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.RemoveListeners();
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000D29C7 File Offset: 0x000D0DC7
		protected virtual void RemoveListeners()
		{
			if (this.boosterService != null)
			{
				this.boosterService.onBoostsChanged.RemoveListener(new Action<BoostViewData, int>(this.HandleBoostsChanged));
			}
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000D29F0 File Offset: 0x000D0DF0
		protected override void Go()
		{
			this.SetupBoosts();
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000D29F8 File Offset: 0x000D0DF8
		protected void UpdateCurrentSalesBanner(ScreenOrientation orientation)
		{
			ScreenOrientation similarOrientation = AUiAdjuster.GetSimilarOrientation(orientation);
			if (similarOrientation == ScreenOrientation.LandscapeLeft)
			{
				this.UpdateSalesBanner(this.landscapeSaleNotificationView);
				this.portraitSaleNotificationView.Hide();
			}
			else
			{
				this.UpdateSalesBanner(this.portraitSaleNotificationView);
				this.landscapeSaleNotificationView.Hide();
			}
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000D2A48 File Offset: 0x000D0E48
		protected void UpdateSalesBanner(SaleNotificationView saleView)
		{
			if (this.sbsService != null && this.sbsService.SbsConfig.feature_switches.show_sales_banner_level_start)
			{
				// Sale currentSale = this.saleService.CurrentSale;
				Sale currentSale = null;
				if (currentSale != null)
				{
					string discountStr = string.Empty;
					if (currentSale.value > 0f)
					{
						discountStr = string.Format(this.locaService.GetText("ui.monthly.sale.popup.extra_value.badge", new LocaParam[0]), currentSale.value);
					}
					else
					{
						discountStr = string.Format(this.locaService.GetText("ui.monthly.sale.popup.discount.badge", new LocaParam[0]), currentSale.discount);
					}
					saleView.Show(currentSale, this.configService.general.notifications.low_time_event, discountStr, delegate
					{
						this.UpdateSalesBanner(saleView);
					});
				}
				else
				{
					saleView.Hide();
				}
			}
			else
			{
				saleView.Hide();
			}
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000D2B58 File Offset: 0x000D0F58
		protected void SetupBoosts()
		{
			this.boostsData = this.boosterService.GetPreGameBoostInfos();
			this.boosterService.onBoostsChanged.AddListener(new Action<BoostViewData, int>(this.HandleBoostsChanged));
			this.preBombAndLineGem = Enum.GetName(typeof(Boosts), Boosts.boost_pre_bomb_linegem);
			this.preDoubleFish = Enum.GetName(typeof(Boosts), Boosts.boost_pre_double_fish);
			this.preRainbow = Enum.GetName(typeof(Boosts), Boosts.boost_pre_rainbow);
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000D2BE8 File Offset: 0x000D0FE8
		protected virtual void SetupTournamentInfo()
		{
			// TournamentType apparentOngoingTournamentType = this.tournamentService.GetApparentOngoingTournamentType();
			TournamentType apparentOngoingTournamentType = TournamentType.Undefined;
			if (apparentOngoingTournamentType == TournamentType.Undefined)
			{
				this.tournamentInfoUI.gameObject.SetActive(false);
			}
			else
			{
				this.tournamentInfoUI.gameObject.SetActive(true);
				// this.tournamentInfoUI.Setup(apparentOngoingTournamentType, this.locaService, this.helpshiftService);
			}
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000D2C46 File Offset: 0x000D1046
		protected virtual void SetupObjective(ALevelCollectionConfig config)
		{
			this.imageObjective.sprite = this.goalSprites.GetSimilar(config.objective);
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000D2C64 File Offset: 0x000D1064
		protected void HandleBoostsChanged(BoostViewData info, int delta)
		{
			if (delta > 0 && base.gameObject.activeInHierarchy)
			{
				this.Handle(info, BoostOperation.Use);
			}
			else
			{
				this.UpdateBoostViews();
			}
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000D2C90 File Offset: 0x000D1090
		protected void UpdateBoostViews()
		{
			this.boostsData = this.boosterService.GetPreGameBoostInfos();
			this.ShowBoostData(this.boostsData);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000D2CB0 File Offset: 0x000D10B0
		public void Handle(PopupOperation op)
		{
			if (this.buyBoostRoutine != null || this.dialog.IsClosing)
			{
				return;
			}
			bool useBombAndLinGem = Array.Find<BoostViewData>(this.boostsData, (BoostViewData d) => d.name.Equals(this.preBombAndLineGem)).state == BoostState.Selected;
			bool useDoubleFish = Array.Find<BoostViewData>(this.boostsData, (BoostViewData d) => d.name.Equals(this.preDoubleFish)).state == BoostState.Selected;
			bool useRainbow = Array.Find<BoostViewData>(this.boostsData, (BoostViewData d) => d.name.Equals(this.preRainbow)).state == BoostState.Selected;
			this.parameters.preBoostConfig = new PreBoostConfig(useBombAndLinGem, useDoubleFish, useRainbow);
			// 只处理ok和close两种操作
			if (op != PopupOperation.OK)
			{
				if (op == PopupOperation.Close)
				{
					this.onCompleted.Dispatch(false);
				}
			}
			else
			{
				this.onCompleted.Dispatch(true);
			}
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButton));
			this.Hide(true);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000D2DA0 File Offset: 0x000D11A0
		public void Handle(BoostViewData boostData, BoostOperation op)
		{
			BoostViewData boostViewData = Array.Find<BoostViewData>(this.boostsData, (BoostViewData d) => d.name == boostData.name);
			if (boostViewData == null)
			{
				return;
			}
			if (op != BoostOperation.Use)
			{
				if (op != BoostOperation.Cancel)
				{
					if (op == BoostOperation.Add)
					{
						this.HandleBoostAdded(boostData);
						this.audioService.PlaySFX(AudioId.ClickAddBoost, false, false, false);
					}
				}
				else
				{
					boostViewData.state = BoostState.Active;
					this.audioService.PlaySFX(AudioId.ClickCancelBoost, false, false, false);
				}
			}
			else
			{
				if (boostViewData.state == BoostState.Selected)
				{
					boostViewData.state = BoostState.Active;
				}
				else
				{
					boostViewData.state = BoostState.Selected;
				}
				this.audioService.PlaySFX(AudioId.ClickUseBoost, false, false, false);
			}
			this.ShowBoostData(this.boostsData);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000D2E7C File Offset: 0x000D127C
		protected void HandleBoostAdded(BoostViewData data)
		{
			if (this.buyBoostRoutine != null || this.dialog.IsClosing)
			{
				return;
			}
			BuyBoostFlow.Input input = new BuyBoostFlow.Input(data, "pregame");
			this.buyBoostRoutine = WooroutineRunner.StartCoroutine(this.BuyBoostRoutine(input), null);
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000D2EC4 File Offset: 0x000D12C4
		protected IEnumerator BuyBoostRoutine(BuyBoostFlow.Input input)
		{
			this.Hide(true);
			yield return new BuyBoostFlow(null).Start(input);
			this.Hide(false);
			BoostViewData info = this.boosterService.GetPreGameBoostInfos().First((BoostViewData i) => i.name == input.info.name);
			if (info.amount > 0)
			{
				BoostViewData boostViewData = this.boostsData.First((BoostViewData b) => b.name == input.info.name);
				boostViewData.state = info.state;
				boostViewData.amount = info.amount;
				this.Handle(boostViewData, BoostOperation.Use);
			}
			this.buyBoostRoutine = null;
			yield break;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000D2EE6 File Offset: 0x000D12E6
		public void Hide(bool state)
		{
			if (!state)
			{
				this.Show(this.parameters);
			}
			else
			{
				this.dialog.Hide();
				this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			}
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000D2F20 File Offset: 0x000D1320
		protected virtual void Show(LevelConfig level)
		{
			if (this.parameters == null)
			{
				return;
			}
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
			this.SetTitle(string.Format("{0} {1}", this.locaService.GetText("ui.level", new LocaParam[0]), this.parameters.LevelCollectionConfig.level));
			base.gameObject.SetActive(true);
			this.dialog.Show();
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.ShowBoostData(this.boostsData);
			this.SetupObjective(level.LevelCollectionConfig);
			this.SetupTournamentInfo();
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000D2FD3 File Offset: 0x000D13D3
		protected void HandleBackButton()
		{
			this.Handle(PopupOperation.Close);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000D2FDC File Offset: 0x000D13DC
		protected virtual void SetTitle(string title)
		{
			this.labelTitle.text = title;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000D2FEA File Offset: 0x000D13EA
		protected virtual void ShowBoostData(BoostViewData[] boostsData)
		{
			this.boostsDataSource.Show(boostsData);
		}

		// Token: 0x040056F1 RID: 22257
		[WaitForService(true, true)]
		protected ILocalizationService locaService;

		// Token: 0x040056F2 RID: 22258
		[WaitForService(true, true)]
		protected ConfigService configService;

		// Token: 0x040056F3 RID: 22259
		[WaitForService(true, true)]
		protected AudioService audioService;

		// Token: 0x040056F4 RID: 22260
		[WaitForService(true, true)]
		protected SBSService sbsService;

		// Token: 0x040056F5 RID: 22261
		// [WaitForService(true, true)]
		// protected SaleService saleService;

		// Token: 0x040056F6 RID: 22262
		[WaitForService(true, true)]
		protected BoostsService boosterService;

		// Token: 0x040056F7 RID: 22263
		[WaitForService(true, true)]
		protected M3ConfigService m3ConfigService;

		// Token: 0x040056F8 RID: 22264
		// [WaitForService(true, true)]
		// protected TournamentService tournamentService;

		// Token: 0x040056F9 RID: 22265
		// [WaitForService(true, true)]
		// protected HelpshiftService helpshiftService;

		// Token: 0x040056FA RID: 22266
		[SerializeField]
		protected TextMeshProUGUI labelTitle;

		// Token: 0x040056FB RID: 22267
		[SerializeField]
		protected Image imageObjective;

		// Token: 0x040056FC RID: 22268
		[SerializeField]
		protected BoostsDataSource boostsDataSource;

		// Token: 0x040056FD RID: 22269
		[SerializeField]
		protected SpriteManager goalSprites;

		// Token: 0x040056FE RID: 22270
		[SerializeField]
		protected TournamentInfoUI tournamentInfoUI;

		// Token: 0x040056FF RID: 22271
		[SerializeField]
		protected CanvasGroup window;

		// Token: 0x04005700 RID: 22272
		[SerializeField]
		protected AnimatedUi dialog;

		// Token: 0x04005701 RID: 22273
		[SerializeField]
		protected SaleNotificationView portraitSaleNotificationView;

		// Token: 0x04005702 RID: 22274
		[SerializeField]
		protected SaleNotificationView landscapeSaleNotificationView;

		// Token: 0x04005703 RID: 22275
		protected BoostViewData[] boostsData;

		// Token: 0x04005704 RID: 22276
		protected Coroutine buyBoostRoutine;

		// Token: 0x04005705 RID: 22277
		protected string preBombAndLineGem;

		// Token: 0x04005706 RID: 22278
		protected string preDoubleFish;

		// Token: 0x04005707 RID: 22279
		protected string preRainbow;
	}
}
