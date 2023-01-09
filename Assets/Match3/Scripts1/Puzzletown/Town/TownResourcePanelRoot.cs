using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x02000A14 RID: 2580
	public class TownResourcePanelRoot : APtSceneRoot
	{
		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003E0D RID: 15885 RVA: 0x0013A61F File Offset: 0x00138A1F
		private Canvas Canvas
		{
			get
			{
				if (!this._canvas)
				{
					this._canvas = base.GetComponentInChildren<Canvas>();
				}
				return this._canvas;
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0013A644 File Offset: 0x00138A44
		public TownResourcePanelRoot.State Show(Canvas canvas, TownResourceElement elements, bool showDailyDealPopout = false)
		{
			TownResourcePanelRoot.State state = new TownResourcePanelRoot.State(elements, canvas.sortingOrder + 1, canvas, showDailyDealPopout, false);
			this.Show(state, true);
			return state;
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0013A66C File Offset: 0x00138A6C
		public void Show(TownResourcePanelRoot.State state, bool addToStack = true)
		{
			this.ShowOnChildren(state.visibleResources, true, true);
			TownResourceView[] componentsInChildren = base.GetComponentsInChildren<TownResourceView>();
			this.HideLockedViews(componentsInChildren);
			this.Canvas.enabled = true;
			this.Canvas.sortingOrder = state.canvasOrder;
			if (addToStack && (this.states.Count == 0 || this.states.Last<TownResourcePanelRoot.State>() != state))
			{
				this.states.Add(state);
			}
			this.dailyDealPopout.gameObject.SetActive(state.showDailyDealPopout);
			this.villageRankTooltip.gameObject.SetActive(false);
			int num = componentsInChildren.Count((TownResourceView v) => v.CheckMatch(TownResourceElement.Diamonds | TownResourceElement.Lives | TownResourceElement.Harmony | TownResourceElement.Coins | TownResourceElement.Seasonal) && v.IsVisible);
			bool flag = num < 4;
			this.placeholderLeft.SetActive(flag);
			this.placeholderRight.SetActive(flag || state.keepSpaceForCloseButton);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0013A75C File Offset: 0x00138B5C
		private void HideLockedViews(TownResourceView[] views)
		{
			foreach (TownResourceView townResourceView in views)
			{
				if ((townResourceView.state == TownResourceElement.Harmony || townResourceView.state == TownResourceElement.Coins) && this.progression.UnlockedLevel < 3)
				{
					townResourceView.Hide();
				}
				// if (townResourceView.state == TownResourceElement.Seasonal && (!this.seasonService.IsSeasonalsV3 || !this.seasonService.IsActive))
				// {
					// townResourceView.Hide();
				// }
			}
		}

		// 购买后，materials飞的动画
		public float CollectMaterials(MaterialAmount mat, Transform source, bool allowResources = true)
		{
			string type = mat.type;
			if (type != null)
			{
				if (type == "lives" || type == "coins" || type == "harmony" || type == "diamonds")
				{
					if (allowResources && this.doobers != null)
					{
						Image image = this.FindIconByMaterial(mat.type);
						return (!image) ? 0f : this.doobers.SpawnDoobers(mat, source, image.rectTransform, null);
					}
				}
			}
			return 0f;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0013A8A4 File Offset: 0x00138CA4
		public void RemoveState(TownResourcePanelRoot.State state)
		{
			bool flag = false;
			for (int i = this.states.Count - 1; i >= 0; i--)
			{
				if (this.states[i] == state)
				{
					this.states.RemoveAt(i);
					flag = true;
					break;
				}
			}
			if (this.states.Count > 0)
			{
				this.Show(this.states[this.states.Count - 1], false);
			}
			else if (flag)
			{
				this.Canvas.enabled = false;
			}
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0013A940 File Offset: 0x00138D40
		private void UpdateVrTooltip()
		{
			this.showVillageRankTipTitle.text = this.localizationService.GetText("ui.hud.village_rank.title", new LocaParam[]
			{
				new LocaParam("{rank}", this.harmonyObserver.CurrentRank)
			});
			int num = this.harmonyObserver.HarmonyRequried - this.harmonyObserver.HarmonyCollected;
			int num2 = this.harmonyObserver.CurrentRank + 1;
			string value = (num < 0) ? string.Empty : num.ToString();
			string text = this.localizationService.GetText("ui.hud.village_rank.description", new LocaParam[]
			{
				new LocaParam("{remaining}", value),
				new LocaParam("{next}", num2)
			});
			this.showVillageRankTipMessage.text = text.Replace("  ", " ");
			this.harmonyDisplay.SetVillageRankHarmonyObserver(this.harmonyObserver);
			this.harmonyDisplay.SetValue(this.stateService.Resources.GetAmount("harmony"));
			VillageRank nextRankData = this.harmonyObserver.NextRankData;
			MaterialAmount[] source = new MaterialAmount[0];
			if (nextRankData != null)
			{
				source = new MaterialAmount[]
				{
					new MaterialAmount("coins", nextRankData.reward_coins, MaterialAmountUsage.Undefined, 0),
					new MaterialAmount("diamonds", nextRankData.reward_diamonds, MaterialAmountUsage.Undefined, 0),
					new MaterialAmount(nextRankData.reward_booster_type, nextRankData.reward_booster_amount, MaterialAmountUsage.Undefined, 0)
				};
			}
			this.rewardmaterials.Show(from r in source
			where r.amount > 0
			select r);
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x0013AB21 File Offset: 0x00138F21
		private void UpdateVrTooltipAndObserver(VillageRankHarmonyObserver observer)
		{
			this.harmonyObserver = observer;
			this.UpdateVrTooltip();
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0013AB30 File Offset: 0x00138F30
		public void SetHarmonyObserver(VillageRankHarmonyObserver harmonyObserver)
		{
			harmonyObserver.OnVillageRankProgressChanged.AddListener(new Action<VillageRankHarmonyObserver>(this.UpdateVrTooltipAndObserver));
			harmonyObserver.OnVillageRankChanged.AddListener(new Action<VillageRankHarmonyObserver>(this.UpdateVrTooltipAndObserver));
			this.showVillageRankTip.onClick.AddListener(delegate()
			{
				base.StartCoroutine(this.RunVillageRankTooltip());
			});
			this.villageRankTooltip.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.cancelVillageRankTooltip = true;
			});
			this.UpdateVrTooltipAndObserver(harmonyObserver);
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0013ABB0 File Offset: 0x00138FB0
		private IEnumerator RunVillageRankTooltip()
		{
			this.villageRankTooltip.alpha = 0f;
			this.cancelVillageRankTooltip = false;
			this.villageRankTooltip.gameObject.SetActive(true);
			yield return this.villageRankTooltip.DOFade(1f, 0.3f).WaitForCompletion();
			while (!this.cancelVillageRankTooltip)
			{
				yield return null;
			}
			yield return this.villageRankTooltip.DOFade(0f, 0.3f).WaitForCompletion();
			this.villageRankTooltip.gameObject.SetActive(false);
			this.villageRankTooltip.alpha = 0f;
			yield break;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x0013ABCC File Offset: 0x00138FCC
		protected override void Go()
		{
			this.livesDisplayUpdater = base.GetComponentInChildren<LivesDisplayUpdater>(true);
			this.UpdateLabels(this.stateService.Resources);
			this.villageRankTooltip.gameObject.SetActive(false);
			this.purchaseDiamondsButton.onClick.AddListener(new UnityAction(this.onPurchaseDiamonds.Dispatch));
			this.dailyDealPopout.onClick.AddListener(new UnityAction(this.onPurchaseDiamonds.Dispatch));
			this.purchaseLivessButton.onClick.AddListener(new UnityAction(this.onPurchaseLives.Dispatch));
			this.stateService.Resources.onChanged.AddListener(new Action<MaterialChange>(this.HandleResourceChanged));
			this.livesService.OnLifeTimerChanged.AddListener(new Action(this.HandleLifeTimerChanged));
			this.localizationService.LanguageChanged.AddListener(new Action(this.HandleLifeTimerChanged));
			this.localizationService.LanguageChanged.AddListener(new Action(this.UpdateVrTooltip));
			this.AddSlowUpdate(new SlowUpdate(this.UpdateDailyDeal), 3);
			this.HandleLifeTimerChanged();
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x0013ACFC File Offset: 0x001390FC
		protected override IEnumerator GoRoutine()
		{
			// if (this.seasonService.IsSeasonalsV3)
			// {
			// 	Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
			// 	yield return spriteManagerRoutine;
			// 	if (spriteManagerRoutine.ReturnValue != null)
			// 	{
			// 		Sprite similar = spriteManagerRoutine.ReturnValue.GetSimilar("season_currency");
			// 		this.seasonalImage.sprite = similar;
			// 	}
			// }
			yield break;
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x0013AD18 File Offset: 0x00139118
		private void UpdateDailyDeal()
		{
			TownResourcePanelRoot.State state = null;
			if (this.states != null && this.states.Count > 0)
			{
				state = this.states[this.states.Count - 1];
			}
			if (state != null)
			{
				// this.dailyDealPopout.gameObject.SetActive(this.dailyDealsService != null && state.showDailyDealPopout && this.dailyDealsService.ShouldShowDailyDealPopout());
			}
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x0013AD98 File Offset: 0x00139198
		public Image FindIconByMaterial(string name)
		{
			AMaterialAmountDisplay amaterialAmountDisplay = Array.Find<AMaterialAmountDisplay>(base.GetComponentsInChildren<AMaterialAmountDisplay>(true), (AMaterialAmountDisplay d) => d.gameObject.activeInHierarchy && d.material == name);
			return (!(amaterialAmountDisplay != null)) ? null : amaterialAmountDisplay.icon;
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0013ADE3 File Offset: 0x001391E3
		private void HandleLifeTimerChanged()
		{
			this.livesDisplayUpdater.UpdateLifeTimer(this.livesService, this.stateService, this.localizationService);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x0013AE04 File Offset: 0x00139204
		private void UpdateLabels(ResourceDataService resources)
		{
			foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in base.GetComponentsInChildren<MaterialAmountDisplayLabel>(true))
			{
				materialAmountDisplayLabel.SetValue(resources.GetAmount(materialAmountDisplayLabel.material));
			}
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x0013AE44 File Offset: 0x00139244
		private void HandleResourceChanged(MaterialChange change)
		{
			this.UpdateLabel(change);
			if (change.Delta < 0)
			{
				this.audioService.PlaySFX(this.audioService.GetSimilar(change.name + "Spent"), false, false, false);
			}
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x0013AE90 File Offset: 0x00139290
		private void UpdateLabel(MaterialChange change)
		{
			MaterialAmountDisplayLabel materialAmountDisplayLabel = Array.Find<MaterialAmountDisplayLabel>(base.GetComponentsInChildren<MaterialAmountDisplayLabel>(true), (MaterialAmountDisplayLabel d) => d.material == change.name);
			if (materialAmountDisplayLabel != null)
			{
				materialAmountDisplayLabel.SetValue(change.after);
			}
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x0013AEE0 File Offset: 0x001392E0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.stateService != null)
			{
				this.stateService.Resources.onChanged.RemoveListener(new Action<MaterialChange>(this.HandleResourceChanged));
			}
			if (this.livesService != null)
			{
				this.livesService.OnLifeTimerChanged.RemoveListener(new Action(this.HandleLifeTimerChanged));
			}
			if (this.localizationService != null)
			{
				this.localizationService.LanguageChanged.RemoveListener(new Action(this.HandleLifeTimerChanged));
				this.localizationService.LanguageChanged.RemoveListener(new Action(this.UpdateVrTooltip));
			}
		}

		// Token: 0x040066E1 RID: 26337
		private const int MAX_RESOURCE_VIEWS = 4;

		// Token: 0x040066E2 RID: 26338
		public readonly Signal onPurchaseDiamonds = new Signal();

		// Token: 0x040066E3 RID: 26339
		public readonly Signal onPurchaseLives = new Signal();

		// Token: 0x040066E4 RID: 26340
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x040066E5 RID: 26341
		// [WaitForService(true, true)]
		// public SeasonService seasonService;

		// Token: 0x040066E6 RID: 26342
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x040066E7 RID: 26343
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x040066E8 RID: 26344
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040066E9 RID: 26345
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x040066EA RID: 26346
		// [WaitForService(true, true)]
		// private QuestService quests;

		// Token: 0x040066EB RID: 26347
		// [WaitForService(false, true)]
		// private DailyDealsService dailyDealsService;

		// Token: 0x040066EC RID: 26348
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x040066ED RID: 26349
		[SerializeField]
		private Button purchaseDiamondsButton;

		// Token: 0x040066EE RID: 26350
		[SerializeField]
		private Button purchaseLivessButton;

		// Token: 0x040066EF RID: 26351
		[SerializeField]
		private Button showVillageRankTip;

		// Token: 0x040066F0 RID: 26352
		[SerializeField]
		private CanvasGroup villageRankTooltip;

		// Token: 0x040066F1 RID: 26353
		[SerializeField]
		private TMP_Text showVillageRankTipTitle;

		// Token: 0x040066F2 RID: 26354
		[SerializeField]
		private TMP_Text showVillageRankTipMessage;

		// Token: 0x040066F3 RID: 26355
		[SerializeField]
		private MaterialsDataSource rewardmaterials;

		// Token: 0x040066F4 RID: 26356
		[SerializeField]
		private MaterialAmountDisplayGauge harmonyDisplay;

		// Token: 0x040066F5 RID: 26357
		[SerializeField]
		private Button dailyDealPopout;

		// Token: 0x040066F6 RID: 26358
		[SerializeField]
		private Image seasonalImage;

		// Token: 0x040066F7 RID: 26359
		[SerializeField]
		private GameObject placeholderLeft;

		// Token: 0x040066F8 RID: 26360
		[SerializeField]
		private GameObject placeholderRight;

		// Token: 0x040066F9 RID: 26361
		private LivesDisplayUpdater livesDisplayUpdater;

		// Token: 0x040066FA RID: 26362
		private VillageRankHarmonyObserver harmonyObserver;

		// Token: 0x040066FB RID: 26363
		private bool cancelVillageRankTooltip;

		// Token: 0x040066FC RID: 26364
		private List<TownResourcePanelRoot.State> states = new List<TownResourcePanelRoot.State>();

		// Token: 0x040066FD RID: 26365
		private Canvas _canvas;

		// Token: 0x02000A15 RID: 2581
		public class State
		{
			// Token: 0x06003E24 RID: 15908 RVA: 0x0013AFC5 File Offset: 0x001393C5
			public State(TownResourceElement visibleResources, int canvasOrder, Canvas parent, bool showDailyDealPopout = false, bool keepSpaceForCloseButton = false)
			{
				this.visibleResources = visibleResources;
				this.keepSpaceForCloseButton = keepSpaceForCloseButton;
				this.canvasOrder = canvasOrder;
				this.canvas = parent;
				this.showDailyDealPopout = showDailyDealPopout;
			}

			// Token: 0x06003E25 RID: 15909 RVA: 0x0013AFF4 File Offset: 0x001393F4
			public override string ToString()
			{
				return string.Format("VisibleResources: {0}, CanvasOrder: {1}, Parent: {2}", this.visibleResources, this.canvasOrder, (!(this.canvas != null)) ? null : this.canvas.transform.parent.name);
			}

			// Token: 0x04006700 RID: 26368
			public TownResourceElement visibleResources;

			// Token: 0x04006701 RID: 26369
			public bool keepSpaceForCloseButton;

			// Token: 0x04006702 RID: 26370
			public int canvasOrder;

			// Token: 0x04006703 RID: 26371
			public Canvas canvas;

			// Token: 0x04006704 RID: 26372
			public bool showDailyDealPopout;
		}
	}
}
