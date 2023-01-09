using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Datasources;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Leagues;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;

// Token: 0x02000A5C RID: 2652
namespace Match3.Scripts1
{
	public class TournamentStandingsRoot : APtSceneRoot<LeagueModel>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003F85 RID: 16261 RVA: 0x00145752 File Offset: 0x00143B52
		protected override void OnDestroy()
		{
			this.dataSource.Cleanup();
			base.OnDestroy();
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00145768 File Offset: 0x00143B68
		private void RefreshData()
		{
			int hashCode = this.parameters.GetHashCode();
			if (hashCode != this._lastShownModelHashCode)
			{
				this._lastShownModelHashCode = hashCode;
				this.dataSource.Cleanup();
				TournamentStandingsRoot.ViewData viewData = new TournamentStandingsRoot.ViewData
				{
					StandingsData = TournamentStandingsRoot.EMPTY_LEAGUE
				};
				viewData.StandingsData = this.FetchStandingsData(this.parameters);
				this.ShowOnChildren(viewData, true, true);
				this.dataSource.Show(viewData.StandingsData);
				WooroutineRunner.StartCoroutine(this.RefreshFacebookFriends(viewData.StandingsData), null);
			}
			this.SetupTimer();
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x001457F8 File Offset: 0x00143BF8
		private IEnumerator RefreshFacebookFriends(IEnumerable<TournamentStandingData> collectionOfEntries)
		{
			foreach (TournamentStandingData entry in collectionOfEntries)
			{
				if (!string.IsNullOrEmpty(entry.fbData.ID))
				{
					entry.avatar = this.facebookService.GetProfilePicture(entry.fbData);
					if (entry.avatar == null)
					{
						Wooroutine<FacebookService.BoxedSprite> picture = this.facebookService.LoadProfilePicture(entry.fbData);
						yield return picture;
						if (picture.ReturnValue.spr != null)
						{
							entry.avatar = picture.ReturnValue.spr;
							entry.OnAvatarAvailable.Dispatch(entry.fbData.ID, picture.ReturnValue.spr);
						}
					}
					else
					{
						entry.OnAvatarAvailable.Dispatch(entry.fbData.ID, entry.avatar);
					}
				}
			}
			yield break;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0014581C File Offset: 0x00143C1C
		private void SetupTimer()
		{
			TimeSpan timeLeft = TimeSpan.FromSeconds((double)(this.parameters.config.end - this.tournamentService.Now));
			this.timerUI.Setup(this.parameters.config.id, timeLeft);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x00145868 File Offset: 0x00143C68
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt == PopupOperation.OK)
				{
					this.Close(true);
					return;
				}
				if (evt != PopupOperation.Back)
				{
					return;
				}
			}
			this.Close(false);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x001458A1 File Offset: 0x00143CA1
		protected void CloseViaBackButton()
		{
			this.Close(false);
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x001458AC File Offset: 0x00143CAC
		public void Close(bool openLevelMapAfterClose)
		{
			this.townBottomPanelRoot.Refresh();
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			this.onClose.Dispatch(openLevelMapAfterClose);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x00145908 File Offset: 0x00143D08
		protected List<TournamentStandingData> FetchStandingsData(LeagueModel leagueModel)
		{
			List<TournamentStandingData> list = new List<TournamentStandingData>();
			int playerPosition = leagueModel.GetPlayerPosition();
			for (int i = 1; i <= leagueModel.sortedStandings.Length; i++)
			{
				bool isSelf = playerPosition == i;
				list.Add(this.CreateFriendDataFromStanding(leagueModel.config.config.tournamentType, leagueModel.sortedStandings[i - 1], leagueModel.config.config.rewards, i, isSelf));
			}
			return list;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x0014597C File Offset: 0x00143D7C
		protected TournamentStandingData CreateFriendDataFromStanding(TournamentType type, LeagueEntry standing, TournamentRewardConfig rewards, int rank, bool isSelf)
		{
			string name = standing.name;
			string empty = string.Empty;
			PlayerInLeageHelper.TryExtractPlayerName(standing.name, standing.user_data, out name, out empty);
			TournamentStandingData tournamentStandingData = new TournamentStandingData(empty, name);
			if (isSelf)
			{
				tournamentStandingData.name = this.gameStateService.PlayerName;
			}
			tournamentStandingData.points = standing.points;
			tournamentStandingData.rank = rank;
			tournamentStandingData.isSelf = isSelf;
			tournamentStandingData.taskSprite = this.tournamentTaskSpriteManager.GetSprite(type);
			tournamentStandingData.tournamentType = type;
			tournamentStandingData.trophySprite = this.tournamentTrophySpriteManager.GetSprite(new TournamentTrophy(type, rank));
			int now = this.tournamentService.Now;
			tournamentStandingData.isOnline = (now - standing.updated_at < 900);
			TournamentRewardCategory rewardCategoryFor = rewards.GetRewardCategoryFor(rank);
			this.tournamentRewardSpriteManager.GetSprite(rewardCategoryFor, out tournamentStandingData.rewardSprite, out tournamentStandingData.rewardShadowSprite);
			tournamentStandingData.avatar = this.tournamentProfileSpriteManager.GetSprite(name);
			return tournamentStandingData;
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x00145A71 File Offset: 0x00143E71
		protected override void Go()
		{
			this.SetupTitleLabel();
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00145A7C File Offset: 0x00143E7C
		private void SetupTitleLabel()
		{
			string locaKeyForTournamentType = TournamentConfig.GetLocaKeyForTournamentType(this.parameters.config.config.tournamentType);
			this.tournamentTitle.text = this.loca.GetText(locaKeyForTournamentType, new LocaParam[0]);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00145AC4 File Offset: 0x00143EC4
		public void Show()
		{
			this.RefreshData();
			base.Enable();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			WooroutineRunner.StartCoroutine(this.ScrollToPlayerPositionRoutine(), null);
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00145B1F File Offset: 0x00143F1F
		public void ShowRewardsPreview()
		{
			this.audioService.PlaySFX(AudioId.NormalClick, false, false, false);
			SceneManager.Instance.LoadSceneWithParams<TournamentRewardPreviewRoot, LeagueModel>(this.parameters, null);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x00145B48 File Offset: 0x00143F48
		private IEnumerator ScrollToPlayerPositionRoutine()
		{
			if (this.parameters.couldFetchFromServer)
			{
				while (!this.snapper.gameObject.activeInHierarchy)
				{
					yield return null;
				}
				float playerPos = (float)this.parameters.GetPlayerPosition();
				float playerCount = (float)this.parameters.sortedStandings.Length;
				this.snapper.ScrollTo(1f - playerPos / playerCount, 0.01f, delegate(IEnumerator e)
				{
					base.StartCoroutine(e);
				});
			}
			yield break;
		}

		// Token: 0x04006928 RID: 26920
		private static TournamentStandingData[] EMPTY_LEAGUE = new TournamentStandingData[0];

		// Token: 0x04006929 RID: 26921
		[SerializeField]
		private TournamentStandingsDataSource dataSource;

		// Token: 0x0400692A RID: 26922
		[SerializeField]
		private TableViewSnapper snapper;

		// Token: 0x0400692B RID: 26923
		[SerializeField]
		private TournamentTimer timerUI;

		// Token: 0x0400692C RID: 26924
		[SerializeField]
		private TournamentTaskSpriteManager tournamentTaskSpriteManager;

		// Token: 0x0400692D RID: 26925
		[SerializeField]
		private TournamentProfileSpriteManager tournamentProfileSpriteManager;

		// Token: 0x0400692E RID: 26926
		[SerializeField]
		private TournamentTrophySpriteManager tournamentTrophySpriteManager;

		// Token: 0x0400692F RID: 26927
		[SerializeField]
		private TournamentRewardSpriteManager tournamentRewardSpriteManager;

		// Token: 0x04006930 RID: 26928
		[SerializeField]
		private TMP_Text tournamentTitle;

		// Token: 0x04006931 RID: 26929
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanelRoot;

		// Token: 0x04006932 RID: 26930
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006933 RID: 26931
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x04006934 RID: 26932
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04006935 RID: 26933
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006936 RID: 26934
		[WaitForService(true, true)]
		private ILocalizationService loca;

		// Token: 0x04006937 RID: 26935
		public const int RIVAL_INACTIVE_AFTER_SECONDS = 900;

		// Token: 0x04006938 RID: 26936
		public float playerPositionInTableView;

		// Token: 0x04006939 RID: 26937
		private int _lastShownModelHashCode;

		// Token: 0x0400693A RID: 26938
		public AnimatedUi dialog;

		// Token: 0x0400693B RID: 26939
		public Signal<bool> onClose = new Signal<bool>();

		// Token: 0x02000A5D RID: 2653
		public class ViewData
		{
			// Token: 0x0400693C RID: 26940
			public IEnumerable<TournamentStandingData> StandingsData;
		}
	}
}
