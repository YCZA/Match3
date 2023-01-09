using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A4E RID: 2638
	[RequireComponent(typeof(TournamentBadgeUI))]
	public class TournamentBadgeUIController : MonoBehaviour
	{
		// Token: 0x06003F2D RID: 16173 RVA: 0x00142F58 File Offset: 0x00141358
		public void Init(TournamentService tService, GameStateService gService, TownBottomPanelRoot tBPanel)
		{
			this.tournamentService = tService;
			this.gameStateService = gService;
			this.townBottomPanel = tBPanel;
			this.UpdateTournamentStatus();
			this.AddListeners();
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00142F7B File Offset: 0x0014137B
		public void UpdateTournamentStatus()
		{
			// 审核版不显示tournament按钮
			// #if REVIEW_VERSION
			// 	gameObject.SetActive(false);
			// 	return;
			// #endif
			if (this.tournamentService == null)
			{
				this.TryShowBadge(false);
				return;
			}
			if (this.tournamentService.Status.IsTeased)
			{
				this.ShowTournamentTeaserBadge();
			}
			else
			{
				this.ShowTournamentBadgeProper();
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00142FB6 File Offset: 0x001413B6
		private void ShowTournamentTeaserBadge()
		{
			this.PrepareTournamentBadge(null, false, true);
			this.TryShowBadge(true);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00142FCC File Offset: 0x001413CC
		private void ShowTournamentBadgeProper()
		{
			LeagueModel leagueModel = null;
			if (this.tournamentService != null)
			{
				leagueModel = this.tournamentService.GetActiveLeagueState();
			}
			bool shouldShowBadge = false;
			if (leagueModel != null)
			{
				if (this.tournamentService.Status.IsUnlocked)
				{
					shouldShowBadge = this.PrepareTournamentBadge(leagueModel, false, false);
				}
				else if (this.gameStateService.Debug.ShowCheatMenus)
				{
					shouldShowBadge = this.PrepareTournamentBadge(leagueModel, true, false);
				}
			}
			this.TryShowBadge(shouldShowBadge);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00143044 File Offset: 0x00141444
		private void OnDestroy()
		{
			this.RemoveListeners();
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0014304C File Offset: 0x0014144C
		private void TryShowBadge(bool shouldShowBadge)
		{
			base.gameObject.SetActive(shouldShowBadge && this.Badge != null);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00143070 File Offset: 0x00141470
		private bool PrepareTournamentBadge(LeagueModel leagueModel, bool showBlocked = false, bool showTeased = false)
		{
			if (showTeased && this.Badge != null)
			{
				WooroutineRunner.StartCoroutine(this.Badge.SetupRoutine(TournamentBadgeUIConfig.CreateConfigForTeaser(), this.townBottomPanel), null);
				return true;
			}
			if (leagueModel.IsValid())
			{
				TournamentBadgeUIConfig config = default(TournamentBadgeUIConfig);
				if (!leagueModel.fetchInProgress)
				{
					TimeSpan timeLeft = TimeSpan.Zero;
					if (leagueModel.config.end > this.tournamentService.Now)
					{
						timeLeft = TimeSpan.FromSeconds((double)(leagueModel.config.end - this.tournamentService.Now));
					}
					config = new TournamentBadgeUIConfig(leagueModel, timeLeft, showBlocked, TournamentType.Undefined, false);
				}
				else
				{
					TournamentType tournamentType = leagueModel.config.config.tournamentType;
					if (tournamentType != TournamentType.Undefined)
					{
						config = TournamentBadgeUIConfig.CreateConfigForLeagueUnderRefresh(tournamentType);
					}
				}
				if (this.Badge != null && config.apparentTournamentType != TournamentType.Undefined)
				{
					WooroutineRunner.StartCoroutine(this.Badge.SetupRoutine(config, this.townBottomPanel), null);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x00143178 File Offset: 0x00141578
		private void AddListeners()
		{
			if (this.tournamentService != null)
			{
				this.tournamentService.leagueModelCache.onActiveEventStateChanged.AddListener(new Action(this.UpdateTournamentStatus));
			}
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x001431A6 File Offset: 0x001415A6
		public void RemoveListeners()
		{
			if (this.tournamentService != null)
			{
				this.tournamentService.leagueModelCache.onActiveEventStateChanged.RemoveListener(new Action(this.UpdateTournamentStatus));
			}
		}

		// Token: 0x040068B4 RID: 26804
		private TournamentService tournamentService;

		// Token: 0x040068B5 RID: 26805
		private GameStateService gameStateService;

		// Token: 0x040068B6 RID: 26806
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x040068B7 RID: 26807
		public TournamentBadgeUI Badge;
	}
}
