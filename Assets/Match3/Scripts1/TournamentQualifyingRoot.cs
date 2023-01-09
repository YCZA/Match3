using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A53 RID: 2643
namespace Match3.Scripts1
{
	public class TournamentQualifyingRoot : APtSceneRoot<LeagueModel>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003F46 RID: 16198 RVA: 0x00143530 File Offset: 0x00141930
		protected override IEnumerator GoRoutine()
		{
			this.SetupTimer();
			yield return this.SetupIllustration(this.parameters.config.config.tournamentType);
			this.SetupTrophies(true, this.parameters.config.config.tournamentType, -1);
			this.qualifying.Setup(this.parameters, this.tournamentTaskSprites, this.locaService);
			this.qualifying.gameObject.SetActive(true);
			this.timerUI.gameObject.SetActive(true);
			this.gamestateService.SetSeenFlagWithTimestamp("tournamentSeen", DateTime.UtcNow);
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			yield break;
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x0014354C File Offset: 0x0014194C
		private void SetupTimer()
		{
			TimeSpan timeLeft = TimeSpan.FromSeconds((double)(this.parameters.config.end - this.tournamentService.Now));
			this.timerUI.Setup(this.parameters.config.id, timeLeft);
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x00143598 File Offset: 0x00141998
		private IEnumerator SetupIllustration(TournamentType tournamentType)
		{
			string illustrationPath = TournamentConfig.GetTournamentIllustrationPath(tournamentType);
			Wooroutine<Sprite> asset = this.abs.LoadAsset<Sprite>("tournament_illustrations", illustrationPath);
			yield return asset;
			this.tournamentIllustration.sprite = asset.ReturnValue;
			this.tournamentIllustration.color = Color.white;
			yield break;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x001435BC File Offset: 0x001419BC
		private void SetupTrophies(bool isQualifying, TournamentType tType, int playerPosition = -1)
		{
			for (int i = 0; i < this.trophies.Length; i++)
			{
				this.trophies[i].gameObject.SetActive(false);
			}
			int num = tType - TournamentType.Bomb;
			if (num >= 0 && num < this.trophies.Length && num < this.trophyLabels.Length)
			{
				this.trophies[num].gameObject.SetActive(true);
				this.trophyLabels[num].text = ((!isQualifying && playerPosition >= 1) ? string.Format("{0}", playerPosition) : "?");
			}
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x00143663 File Offset: 0x00141A63
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt != PopupOperation.OK)
				{
					if (evt == PopupOperation.Details)
					{
						this.Close(TournamentPopupFlowOperation.OpenLevelMap);
					}
				}
				else
				{
					this.Close(TournamentPopupFlowOperation.TryEnterTournament);
				}
			}
			else
			{
				this.Close(TournamentPopupFlowOperation.Nothing);
			}
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x001436A4 File Offset: 0x00141AA4
		private void Close(TournamentPopupFlowOperation whatToDoNext)
		{
			this.townBottomPanel.UpdateTournamentStatus();
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			this.onClose.Dispatch(whatToDoNext);
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x001436FD File Offset: 0x00141AFD
		private void CloseViaBackButton()
		{
			this.Close(TournamentPopupFlowOperation.Nothing);
		}

		// Token: 0x040068C0 RID: 26816
		public const string TOURNAMENT_SEEN_FLAG_NAME = "tournamentSeen";

		// Token: 0x040068C1 RID: 26817
		[WaitForService(true, true)]
		private GameStateService gamestateService;

		// Token: 0x040068C2 RID: 26818
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040068C3 RID: 26819
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040068C4 RID: 26820
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040068C5 RID: 26821
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x040068C6 RID: 26822
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x040068C7 RID: 26823
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x040068C8 RID: 26824
		public Signal<TournamentPopupFlowOperation> onClose = new Signal<TournamentPopupFlowOperation>();

		// Token: 0x040068C9 RID: 26825
		public AnimatedUi dialog;

		// Token: 0x040068CA RID: 26826
		public TournamentQualifyingUI qualifying;

		// Token: 0x040068CB RID: 26827
		public TournamentTimer timerUI;

		// Token: 0x040068CC RID: 26828
		public Image tournamentIllustration;

		// Token: 0x040068CD RID: 26829
		public GameObject[] trophies;

		// Token: 0x040068CE RID: 26830
		public TextMeshProUGUI[] trophyLabels;

		// Token: 0x040068CF RID: 26831
		public TournamentTaskSpriteManager tournamentTaskSprites;

		// Token: 0x040068D0 RID: 26832
		public TournamentTrophySpriteManager tournamentTrophySprites;
	}
}
