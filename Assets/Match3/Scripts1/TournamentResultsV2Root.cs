using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Datasources;
using Match3.Scripts1.Puzzletown.Flows;
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
using UnityEngine.UI;

// Token: 0x02000A55 RID: 2645
namespace Match3.Scripts1
{
	public class TournamentResultsV2Root : APtSceneRoot<LeagueModel>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003F55 RID: 16213 RVA: 0x00143BF3 File Offset: 0x00141FF3
		protected void Start()
		{
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00143BF5 File Offset: 0x00141FF5
		protected override void Go()
		{
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x00143BF8 File Offset: 0x00141FF8
		public override void Setup(LeagueModel leagueModel)
		{
			base.Setup(leagueModel);
			this.continueButton.SetActive(false);
			this.rewardCanvasGroup.alpha = 0f;
			this.rewardCanvasGroup.gameObject.SetActive(false);
			PopupTextCell.SetText(this, TextType.Title, this.parameters.config.config.name);
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00143C55 File Offset: 0x00142055
		protected override void OnDisable()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			base.OnDisable();
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00143C74 File Offset: 0x00142074
		public void Handle(PopupOperation evt)
		{
			switch (evt)
			{
				case PopupOperation.Close:
					this.Close(TournamentPopupFlowOperation.Nothing);
					break;
				case PopupOperation.OK:
					this.Close(TournamentPopupFlowOperation.TryEnterTournament);
					break;
				default:
					if (evt == PopupOperation.Skip)
					{
						if (!this.isOpeningPackage)
						{
							WooroutineRunner.StartCoroutine(this.OpenRewardPackageRoutine(), null);
						}
					}
					break;
				case PopupOperation.Details:
					this.Close(TournamentPopupFlowOperation.OpenLevelMap);
					break;
			}
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x00143CE8 File Offset: 0x001420E8
		private IEnumerator OpenRewardPackageRoutine()
		{
			this.isOpeningPackage = true;
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			Materials rewards = this.parameters.GetRewards();
			new CollectTournamentResultsFlow().Start(rewards);
			if (this.reward.gameObject.activeInHierarchy)
			{
				yield return base.StartCoroutine(this.PlayAnimation("CloseTournamentResultReward"));
			}
			this.isOpeningPackage = false;
			this.Close(TournamentPopupFlowOperation.Nothing);
			yield break;
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00143D04 File Offset: 0x00142104
		private IEnumerator PlayAnimation(string name)
		{
			this.windowAnimation.Play(name);
			this.windowAnimation.wrapMode = WrapMode.Once;
			while (this.windowAnimation.isPlaying)
			{
				yield return null;
			}
			this.windowAnimation.wrapMode = WrapMode.Default;
			yield break;
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x00143D28 File Offset: 0x00142128
		private void Close(TournamentPopupFlowOperation whatToDoNext)
		{
			if (this.isOpeningPackage)
			{
				return;
			}
			if (!base.registeredFirst)
			{
				this.townBottomPanel.UpdateTournamentStatus();
			}
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			this.onClose.Dispatch(whatToDoNext);
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00143D98 File Offset: 0x00142198
		private void CloseViaBackButton()
		{
			this.Close(TournamentPopupFlowOperation.Nothing);
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x00143DA4 File Offset: 0x001421A4
		public void Show()
		{
			this.RefreshData();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			this.ShowRewardPackageIfNeeded();
			this.ShowContinueIfNeeded();
			WooroutineRunner.StartCoroutine(this.ScrollToPlayerPositionRoutine(), null);
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x00143E08 File Offset: 0x00142208
		private void ShowContinueIfNeeded()
		{
			if (this.parameters.GetPlayersRewardCategory() == TournamentRewardCategory.None)
			{
				this.continueButton.SetActive(true);
			}
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x00143E34 File Offset: 0x00142234
		private void ShowRewardPackageIfNeeded()
		{
			TournamentRewardCategory playersRewardCategory = this.parameters.GetPlayersRewardCategory();
			if (playersRewardCategory != TournamentRewardCategory.None)
			{
				Sprite sprite;
				Sprite sprite2;
				this.tournamentRewardSprites.GetSprite(playersRewardCategory, out sprite, out sprite2);
				if (sprite != null)
				{
					this.reward.sprite = sprite;
					WooroutineRunner.StartCoroutine(this.ShowPackageRoutine(), null);
				}
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x00143E88 File Offset: 0x00142288
		private IEnumerator ShowPackageRoutine()
		{
			while (this.windowAnimation.isPlaying)
			{
				yield return null;
			}
			this.rewardCanvasGroup.alpha = 1f;
			this.rewardCanvasGroup.gameObject.SetActive(true);
			base.StartCoroutine(this.PlayAnimation("OpenTournamentResultReward"));
			yield break;
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x00143EA4 File Offset: 0x001422A4
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

		// Token: 0x06003F63 RID: 16227 RVA: 0x00143EC0 File Offset: 0x001422C0
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
			tournamentStandingData.taskSprite = this.tournamentTaskSprites.GetSprite(type);
			tournamentStandingData.tournamentType = type;
			tournamentStandingData.trophySprite = this.tournamentTrophySprites.GetSprite(new TournamentTrophy(type, rank));
			int now = this.tournamentService.Now;
			tournamentStandingData.isOnline = (now - standing.updated_at < 900);
			TournamentRewardCategory rewardCategoryFor = rewards.GetRewardCategoryFor(rank);
			this.tournamentRewardSprites.GetSprite(rewardCategoryFor, out tournamentStandingData.rewardSprite, out tournamentStandingData.rewardShadowSprite);
			tournamentStandingData.avatar = this.tournamentProfileSprites.GetSprite(name);
			return tournamentStandingData;
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x00143FB8 File Offset: 0x001423B8
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

		// Token: 0x06003F65 RID: 16229 RVA: 0x00143FDC File Offset: 0x001423DC
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

		// Token: 0x06003F66 RID: 16230 RVA: 0x00144050 File Offset: 0x00142450
		private void RefreshData()
		{
			this.dataSource.Cleanup();
			TournamentStandingsRoot.ViewData viewData = new TournamentStandingsRoot.ViewData
			{
				StandingsData = TournamentResultsV2Root.EMPTY_LEAGUE
			};
			viewData.StandingsData = this.FetchStandingsData(this.parameters);
			LeagueModel parameters = this.parameters;
			int playerPosition = parameters.GetPlayerPosition();
			this.userRankPositionLabel.text = this.localisationService.GetText("ui.tournaments.results.cheer_up", new LocaParam[]
			{
				new LocaParam("{userRankingPosition}", playerPosition)
			});
			this.ShowOnChildren(viewData, true, true);
			this.dataSource.Show(viewData.StandingsData);
			WooroutineRunner.StartCoroutine(this.RefreshFacebookFriends(viewData.StandingsData), null);
		}

		// Token: 0x040068DC RID: 26844
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040068DD RID: 26845
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040068DE RID: 26846
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x040068DF RID: 26847
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040068E0 RID: 26848
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040068E1 RID: 26849
		[WaitForService(true, true)]
		private ILocalizationService localisationService;

		// Token: 0x040068E2 RID: 26850
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x040068E3 RID: 26851
		public Signal<TournamentPopupFlowOperation> onClose = new Signal<TournamentPopupFlowOperation>();

		// Token: 0x040068E4 RID: 26852
		private static TournamentStandingData[] EMPTY_LEAGUE = new TournamentStandingData[0];

		// Token: 0x040068E5 RID: 26853
		public const int RIVAL_INACTIVE_AFTER_SECONDS = 900;

		// Token: 0x040068E6 RID: 26854
		[SerializeField]
		private TournamentStandingsDataSource dataSource;

		// Token: 0x040068E7 RID: 26855
		[SerializeField]
		private TableViewSnapper snapper;

		// Token: 0x040068E8 RID: 26856
		public AnimatedUi dialog;

		// Token: 0x040068E9 RID: 26857
		public TextMeshProUGUI[] trophyLabels;

		// Token: 0x040068EA RID: 26858
		public Image reward;

		// Token: 0x040068EB RID: 26859
		public CanvasGroup rewardCanvasGroup;

		// Token: 0x040068EC RID: 26860
		public GameObject continueButton;

		// Token: 0x040068ED RID: 26861
		public TextMeshProUGUI userRankPositionLabel;

		// Token: 0x040068EE RID: 26862
		public TournamentTaskSpriteManager tournamentTaskSprites;

		// Token: 0x040068EF RID: 26863
		public TournamentTrophySpriteManager tournamentTrophySprites;

		// Token: 0x040068F0 RID: 26864
		public TournamentRewardSpriteManager tournamentRewardSprites;

		// Token: 0x040068F1 RID: 26865
		public TournamentProfileSpriteManager tournamentProfileSprites;

		// Token: 0x040068F2 RID: 26866
		public Animation windowAnimation;

		// Token: 0x040068F3 RID: 26867
		private bool isOpeningPackage;

		// Token: 0x040068F4 RID: 26868
		public TournamentResultTester resultToTest;
	}
}
