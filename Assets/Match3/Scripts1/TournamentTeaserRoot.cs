using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;

// Token: 0x02000A60 RID: 2656
namespace Match3.Scripts1
{
	public class TournamentTeaserRoot : APtSceneRoot, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003FA1 RID: 16289 RVA: 0x00146230 File Offset: 0x00144630
		private void OnValidate()
		{
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x00146232 File Offset: 0x00144632
		protected override void Go()
		{
			// 审核版不显示tropicat
			// #if !REVIEW_VERSION
				this.Localize();
				this.dialog.Show();
				BackButtonManager.Instance.AddAction(new Action(this.Close));
				this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			// #endif 
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00146270 File Offset: 0x00144670
		private void Localize()
		{
			string key = "ui.tournaments.teaser.unlock_info";
			LocaParam locaParam = new LocaParam("{tournamentUnlockLevel}", this.configService.general.tournaments.unlock_at_level);
			this.unlockInfoLabel.text = this.locaService.GetText(key, new LocaParam[]
			{
				locaParam
			});
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x001462D3 File Offset: 0x001446D3
		private void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0014630A File Offset: 0x0014470A
		public void Handle(PopupOperation evt)
		{
			if (evt == PopupOperation.Close)
			{
				this.Close();
			}
		}

		// Token: 0x0400694C RID: 26956
		public static string SEEN_FLAG = "TOUR_TEASER";

		// Token: 0x0400694D RID: 26957
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x0400694E RID: 26958
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400694F RID: 26959
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006950 RID: 26960
		public AnimatedUi dialog;

		// Token: 0x04006951 RID: 26961
		public TextMeshProUGUI unlockInfoLabel;

		// Token: 0x02000A61 RID: 2657
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003FA7 RID: 16295 RVA: 0x00146334 File Offset: 0x00144734
			public Trigger(TournamentService tournamentService, ConfigService configService, GameStateService gameStateService, ProgressionDataService.Service progressionService)
			{
				this.tournamentService = tournamentService;
				this.configService = configService;
				this.gameStateService = gameStateService;
				this.progressionService = progressionService;
			}

			// Token: 0x06003FA8 RID: 16296 RVA: 0x0014635C File Offset: 0x0014475C
			public override bool ShouldTrigger()
			{
				if (this.tournamentService.Status.IsTeased && !this.gameStateService.GetSeenFlag(TournamentTeaserRoot.SEEN_FLAG))
				{
					int teaser_popup_at_level = this.configService.general.tournaments.teaser_popup_at_level;
					return !this.progressionService.IsLocked(teaser_popup_at_level);
				}
				return false;
			}

			// Token: 0x06003FA9 RID: 16297 RVA: 0x001463BC File Offset: 0x001447BC
			public override IEnumerator Run()
			{
				yield return new ShowTournamentTeaserPopupFlow().Start();
				yield break;
			}

			// Token: 0x04006952 RID: 26962
			private readonly TournamentService tournamentService;

			// Token: 0x04006953 RID: 26963
			private readonly ConfigService configService;

			// Token: 0x04006954 RID: 26964
			private readonly GameStateService gameStateService;

			// Token: 0x04006955 RID: 26965
			private readonly ProgressionDataService.Service progressionService;
		}
	}
}
