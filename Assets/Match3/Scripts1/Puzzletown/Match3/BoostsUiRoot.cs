using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000720 RID: 1824
	public class BoostsUiRoot : APtSceneRoot, IHandler<BoostViewData, BoostOperation>
	{
		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000D14C4 File Offset: 0x000CF8C4
		public BoostOverlayController BoostOverlayController
		{
			get
			{
				if (this.boostOverlayController == null)
				{
					this.boostOverlayController = new BoostOverlayController();
				}
				if (!this.boostOverlayController.IsEnabled && this.sbsService != null && this.sbsService.SbsConfig != null && this.sbsService.SbsConfig.feature_switches.ingame_boost_hilight)
				{
					this.boostOverlayController.Init(this);
				}
				return this.boostOverlayController;
			}
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000D1540 File Offset: 0x000CF940
		protected override void Go()
		{
			this.SetupCamera();
			foreach (Button button in this.optionsButtons)
			{
				button.onClick.AddListener(new UnityAction(this.HandleOptionsClicked));
			}
			this.boosterService.onBoostsChanged.AddListener(new Action<BoostViewData, int>(this.HandleBoostsChanged));
			if (base.registeredFirst)
			{
				this.data = this.testData;
				foreach (BoostsDataSource boostsDataSource in this.dataSources)
				{
					boostsDataSource.Show(this.data);
				}
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000D1638 File Offset: 0x000CFA38
		protected void SetupCamera()
		{
			Canvas componentInChildren = base.GetComponentInChildren<Canvas>();
			componentInChildren.worldCamera = Camera.main;
			componentInChildren.sortingLayerName = "UIMask";
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000D1664 File Offset: 0x000CFA64
		public void Init(ScoringController scoringController, bool usesWaterAllFields)
		{
			base.GetComponent<BoostsHintAnimation>().Init(scoringController.onMovesChanged, scoringController.onGameOver, this.onClicked);
			scoringController.onGameOver.AddListener(new Action<Match3Score>(this.HandleGameOver));
			this.usesWaterAllFields = usesWaterAllFields;
			this.UpdateView();
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000D16B2 File Offset: 0x000CFAB2
		private void HandleGameOver(Match3Score score)
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.Cleanup();
			}
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000D16CA File Offset: 0x000CFACA
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.boosterService != null)
			{
				this.boosterService.onBoostsChanged.RemoveListener(new Action<BoostViewData, int>(this.HandleBoostsChanged));
				this.CleanupBoostOverlayController();
			}
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000D16FF File Offset: 0x000CFAFF
		private void CleanupBoostOverlayController()
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.Cleanup();
			}
			this.boostOverlayController = null;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000D1720 File Offset: 0x000CFB20
		public void UnselectActive()
		{
			BoostViewData boostViewData = this.data.FirstOrDefault((BoostViewData d) => d.state == BoostState.Active || d.state == BoostState.Selected);
			if (boostViewData != null)
			{
				this.Handle(boostViewData, BoostOperation.Cancel);
			}
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000D1764 File Offset: 0x000CFB64
		public void Handle(BoostViewData evt, BoostOperation op)
		{
			BoostViewData boostViewData = Array.Find<BoostViewData>(this.data, (BoostViewData d) => d.name == evt.name);
			if (boostViewData == null || boostViewData.state == BoostState.Inactive)
			{
				return;
			}
			(from d in this.data
			where d.state == BoostState.Selected
			select d).ForEach(delegate(BoostViewData d)
			{
				d.state = BoostState.Active;
			});
			if (op != BoostOperation.Use)
			{
				if (op != BoostOperation.Cancel)
				{
					if (op == BoostOperation.Add)
					{
						this.onBoostAdded.Dispatch(boostViewData);
						this.audioService.PlaySFX(AudioId.ClickAddBoost, false, false, false);
					}
				}
				else
				{
					boostViewData.state = BoostState.Active;
					this.audioService.PlaySFX(AudioId.ClickCancelBoost, false, false, false);
					this.onBoostSelected.Dispatch(boostViewData);
				}
			}
			else
			{
				boostViewData.state = BoostState.Selected;
				this.onBoostSelected.Dispatch(boostViewData);
				this.audioService.PlaySFX(AudioId.ClickUseBoost, false, false, false);
			}
			foreach (BoostsDataSource boostsDataSource in this.dataSources)
			{
				boostsDataSource.Show(this.data);
			}
			this.onClicked.Dispatch();
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000D18EC File Offset: 0x000CFCEC
		public void UpdateView()
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.UpdateView(this.boosterService.IsAnyIngameBoostSelected());
			}
			this.data = this.boosterService.GetInGameBoostInfos(this.usesWaterAllFields);
			foreach (BoostsDataSource boostsDataSource in this.dataSources)
			{
				boostsDataSource.Show(this.data);
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000D1988 File Offset: 0x000CFD88
		public void Hide()
		{
			this.fadeInAnimation["M3BottomFadeIn"].speed = -1f;
			this.fadeInAnimation.Play("M3BottomFadeIn");
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000D19B5 File Offset: 0x000CFDB5
		private void HandleBoostsChanged(BoostViewData info, int delta)
		{
			this.UpdateView();
			if (delta > 0)
			{
				this.Handle(info, BoostOperation.Use);
			}
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000D19CC File Offset: 0x000CFDCC
		public void HandleOptionsClicked()
		{
			this.options.Show();
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000D19DC File Offset: 0x000CFDDC
		public void ToogleOptionsButtons()
		{
			foreach (Button button in this.optionsButtons)
			{
				button.gameObject.SetActive(!button.gameObject.activeSelf);
			}
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000D1A4C File Offset: 0x000CFE4C
		public void SetBoostsAmount(string hammerType, int currentAmount, int newAmount)
		{
		}

		// Token: 0x040056A7 RID: 22183
		public const string BOOSTS_UI_SORTING_LAYER = "UIMask";

		// Token: 0x040056A8 RID: 22184
		public readonly Signal<BoostViewData> onBoostSelected = new Signal<BoostViewData>();

		// Token: 0x040056A9 RID: 22185
		public readonly Signal<BoostViewData> onBoostAdded = new Signal<BoostViewData>();

		// Token: 0x040056AA RID: 22186
		public readonly Signal onClicked = new Signal();

		// Token: 0x040056AB RID: 22187
		public bool usesWaterAllFields;

		// Token: 0x040056AC RID: 22188
		[WaitForService(true, true)]
		private BoostsService boosterService;

		// Token: 0x040056AD RID: 22189
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040056AE RID: 22190
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040056AF RID: 22191
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x040056B0 RID: 22192
		[WaitForRoot(false, false)]
		public M3_OptionsRoot options;

		// Token: 0x040056B1 RID: 22193
		[SerializeField]
		private BoostViewData[] testData;

		// Token: 0x040056B2 RID: 22194
		[SerializeField]
		private List<BoostsDataSource> dataSources;

		// Token: 0x040056B3 RID: 22195
		[SerializeField]
		private List<Button> optionsButtons;

		// Token: 0x040056B4 RID: 22196
		[SerializeField]
		private Animation fadeInAnimation;

		// Token: 0x040056B5 RID: 22197
		private BoostViewData[] data;

		// Token: 0x040056B6 RID: 22198
		private BoostOverlayController boostOverlayController;
	}
}
