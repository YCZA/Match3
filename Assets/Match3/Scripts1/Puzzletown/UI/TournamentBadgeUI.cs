using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A4C RID: 2636
	public class TournamentBadgeUI : MonoBehaviour
	{
		// Token: 0x06003F1F RID: 16159 RVA: 0x001427F4 File Offset: 0x00140BF4
		private void OnValidate()
		{
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x001427F8 File Offset: 0x00140BF8
		public IEnumerator SetupRoutine(TournamentBadgeUIConfig config, TownBottomPanelRoot tBPRoot)
		{
			this.townBottomPanelRoot = tBPRoot;
			if (!this.initialized)
			{
				yield return ServiceLocator.Instance.Inject(this);
				this.initialized = true;
			}
			if (config.IsNotBeingRefreshed)
			{
				this.displayedLeagueModel = config.leagueModel;
				this.timer.gameObject.SetActive(true);
				this.timer.Setup(config.leagueModel.config.id, config.timeLeft);
				this.SetupTrophyAndGlow(this.displayedLeagueModel.config.config.tournamentType);
				this.waitSpinner.gameObject.SetActive(false);
				this.unlockInfo.gameObject.SetActive(false);
			}
			else
			{
				this.displayedLeagueModel = null;
				this.timer.gameObject.SetActive(false);
				this.SetupTrophyAndGlow(config.apparentTournamentType);
				this.waitSpinner.gameObject.SetActive(!config.showTeaser);
				this.unlockInfo.gameObject.SetActive(config.showTeaser);
				if (config.showTeaser)
				{
					this.LocalizeUnlockLabel();
				}
			}
			this.SetupNotificationAndRanking(config.playerStatus, config.playerCurrentPosition);
			this.button.interactable = !config.showBlocked;
			yield break;
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x00142824 File Offset: 0x00140C24
		private void LocalizeUnlockLabel()
		{
			if (this.configService != null && this.configService.general != null && this.configService.general.tournaments != null)
			{
				string key = "ui.tournaments.teaser.icon";
				LocaParam locaParam = new LocaParam("{tournamentUnlockLevel}", this.configService.general.tournaments.unlock_at_level);
				this.unlockLevelLabel.text = this.locaService.GetText(key, new LocaParam[]
				{
					locaParam
				});
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x001428B8 File Offset: 0x00140CB8
		private void SetupTrophyAndGlow(TournamentType tType)
		{
			if (this.spriteManager != null)
			{
				if (this.trophy != null)
				{
					this.trophy.sprite = this.spriteManager.GetSprite(new TournamentBadgeIcon(tType, false));
				}
				if (this.glow != null)
				{
					this.glow.sprite = this.spriteManager.GetSprite(new TournamentBadgeIcon(tType, true));
				}
			}
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x00142934 File Offset: 0x00140D34
		private void SetupNotificationAndRanking(PlayerLeagueStatus playerStatus, int playersCurrentPosition)
		{
			bool active = true;
			if (this.gamestateService.IsSeenFlagTimestampSet("tournamentSeen"))
			{
				DateTime timeStamp = this.gamestateService.GetTimeStamp("tournamentSeen");
				active = (DateTime.UtcNow > timeStamp.AddSeconds((double)this.configService.general.notifications.attention_indicator_cooldown));
			}
			if (playerStatus != PlayerLeagueStatus.Entered)
			{
				if (playerStatus != PlayerLeagueStatus.None)
				{
					this.playerRankIndicator.gameObject.SetActive(false);
					this.notificationBlob.gameObject.SetActive(active);
				}
				else
				{
					this.playerRankIndicator.gameObject.SetActive(false);
					this.notificationBlob.gameObject.SetActive(false);
				}
			}
			else
			{
				string text = (playersCurrentPosition <= 0) ? "?" : string.Format("{0}", playersCurrentPosition);
				this.playerRankIndicator.Show(text);
				this.playerRankIndicator.gameObject.SetActive(true);
				this.notificationBlob.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x00142A46 File Offset: 0x00140E46
		private bool DisplayingActiveTournamentModel()
		{
			return this.displayedLeagueModel != null;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x00142A54 File Offset: 0x00140E54
		public void OnButtonTap()
		{
			// 当点击tournament按钮时
			if (this.townBottomPanelRoot == null || !this.townBottomPanelRoot.IsInteractable)
			{
				return;
			}
			if (this.DisplayingActiveTournamentModel())
			{
				base.StartCoroutine(this.ShowTournamentFlowRoutine());
			}
			else
			{
				base.StartCoroutine(this.TryShowTeaserPopupRoutine());
			}
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00142AB0 File Offset: 0x00140EB0
		private IEnumerator TryShowTeaserPopupRoutine()
		{
			if (this.tournamentService.Status.IsTeased)
			{
				yield return new ShowTournamentTeaserPopupFlow().Start();
			}
			yield break;
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00142ACC File Offset: 0x00140ECC
		private IEnumerator ShowTournamentFlowRoutine()
		{
			yield return new TournamentPopupFlow().Start(this.displayedLeagueModel);
			yield break;
		}

		// Token: 0x0400689C RID: 26780
		public TournamentTimer timer;

		// Token: 0x0400689D RID: 26781
		public GameObject notificationBlob;

		// Token: 0x0400689E RID: 26782
		public UiIndicator playerRankIndicator;

		// Token: 0x0400689F RID: 26783
		public TournamentBadgeSpriteManager spriteManager;

		// Token: 0x040068A0 RID: 26784
		public Image trophy;

		// Token: 0x040068A1 RID: 26785
		public Image glow;

		// Token: 0x040068A2 RID: 26786
		public GameObject waitSpinner;

		// Token: 0x040068A3 RID: 26787
		public GameObject unlockInfo;

		// Token: 0x040068A4 RID: 26788
		public TextMeshProUGUI unlockLevelLabel;

		// Token: 0x040068A5 RID: 26789
		private LeagueModel displayedLeagueModel;

		// Token: 0x040068A6 RID: 26790
		private bool initialized;

		// Token: 0x040068A7 RID: 26791
		public Button button;

		// Token: 0x040068A8 RID: 26792
		[WaitForService(true, true)]
		private GameStateService gamestateService;

		// Token: 0x040068A9 RID: 26793
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040068AA RID: 26794
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040068AB RID: 26795
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040068AC RID: 26796
		private TownBottomPanelRoot townBottomPanelRoot;
	}
}
