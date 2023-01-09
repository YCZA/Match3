using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020006DB RID: 1755
namespace Match3.Scripts1
{
	public class M3_OptionsRoot : APtSceneRoot<Match3Score, bool>, IHandler<ToggleSetting, bool>, IPersistentDialog
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000C8822 File Offset: 0x000C6C22
		protected override bool IsSetup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000C8828 File Offset: 0x000C6C28
		protected override void Go()
		{
			this.buttonQuit.onClick.AddListener(new UnityAction(this.HandleButtonQuit));
			this.buttonResume.onClick.AddListener(new UnityAction(this.HandleButtonResume));
			this.SyncAudioTogglesWithSettings();
			this.confirmedExit = false;
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000C887A File Offset: 0x000C6C7A
		protected override void Awake()
		{
			base.Awake();
			base.Disable();
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000C8888 File Offset: 0x000C6C88
		protected override void OnEnable()
		{
			base.OnEnable();
			BackButtonManager.Instance.AddAction(new Action(this.HandleButtonResume));
			this.dialog.Show();
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000C88B4 File Offset: 0x000C6CB4
		protected override void OnDisable()
		{
			base.OnDisable();
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleButtonResume));
			if (!this.confirmedExit && this.level)
			{
				BackButtonManager.Instance.AddAction(new Action(this.level.HandleBackButtonPressed));
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000C8913 File Offset: 0x000C6D13
		private void HandleButtonQuit()
		{
			WooroutineRunner.StartCoroutine(this.QuitFlow(), null);
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000C8924 File Offset: 0x000C6D24
		private IEnumerator QuitFlow()
		{
			this.confirmedExit = true;
			TournamentScore tScore = this.level.Loader.ScoringController.GetTournamentScore();
			LevelPlayMode playMode = this.level.Loader.ScoringController.GetLevelPlayMode();
			if (playMode == LevelPlayMode.DiveForTreasure || playMode == LevelPlayMode.PirateBreakout)
			{
				this.dialog.Hide();
				Wooroutine<bool> lossAversionFlow = new M3_LossAversionFlow().Start(this.level.Loader.ScoringController);
				yield return lossAversionFlow;
				this.confirmedExit = lossAversionFlow.ReturnValue;
			}
			else if (tScore.TournamentType != TournamentType.Undefined && tScore.CollectedPoints > 0)
			{
				Wooroutine<bool> tournamentLossAversionFlow = new M3_TournamentLossAversionFlow().Start(tScore);
				yield return tournamentLossAversionFlow;
				this.confirmedExit = tournamentLossAversionFlow.ReturnValue;
			}
			if (this.confirmedExit)
			{
				this.level.Loader.ScoringController.CancelLevel();
				this.dialog.Hide();
			}
			yield break;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000C893F File Offset: 0x000C6D3F
		private void HandleButtonResume()
		{
			this.dialog.Hide();
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000C894C File Offset: 0x000C6D4C
		public void Show()
		{
			if (!this.dialog.isActiveAndEnabled)
			{
				base.Enable();
				this.dialog.Show();
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000C8970 File Offset: 0x000C6D70
		private void SyncAudioTogglesWithSettings()
		{
			bool toggle = this.settings.GetToggle(ToggleSetting.Music);
			bool toggle2 = this.settings.GetToggle(ToggleSetting.Sound);
			UiToggleSetting[] componentsInChildren = base.GetComponentsInChildren<UiToggleSetting>(true);
			foreach (UiToggleSetting uiToggleSetting in componentsInChildren)
			{
				ToggleSetting category = uiToggleSetting.GetCategory();
				if (category != ToggleSetting.Music)
				{
					if (category == ToggleSetting.Sound)
					{
						uiToggleSetting.Show(toggle2);
					}
				}
				else
				{
					uiToggleSetting.Show(toggle);
				}
			}
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000C89F5 File Offset: 0x000C6DF5
		public void Handle(ToggleSetting setting, bool value)
		{
			this.settings.SetToggle(setting, value);
			if (setting == ToggleSetting.Music || setting == ToggleSetting.Sound)
			{
				this.audioService.ChangeSetting(setting, value);
			}
		}

		// Token: 0x040054C3 RID: 21699
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x040054C4 RID: 21700
		[WaitForRoot(false, false)]
		public M3_LevelRoot level;

		// Token: 0x040054C5 RID: 21701
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040054C6 RID: 21702
		[WaitForService(true, true)]
		private GameSettingsService settings;

		// Token: 0x040054C7 RID: 21703
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040054C8 RID: 21704
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x040054C9 RID: 21705
		[SerializeField]
		private Button buttonQuit;

		// Token: 0x040054CA RID: 21706
		[SerializeField]
		private Button buttonResume;

		// Token: 0x040054CB RID: 21707
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040054CC RID: 21708
		protected bool confirmedExit;
	}
}
