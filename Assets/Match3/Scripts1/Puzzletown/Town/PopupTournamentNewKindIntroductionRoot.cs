using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Localization.Runtime;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x02000A47 RID: 2631
	public class PopupTournamentNewKindIntroductionRoot : APtSceneRoot<LeagueModel>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003F0C RID: 16140 RVA: 0x00141F68 File Offset: 0x00140368
		protected override IEnumerator GoRoutine()
		{
			this.leagueModel = this.parameters;
			this.tournamentType = this.tournamentService.GetActiveTournamentEventConfig().config.tournamentType;
			this.trophyImage.sprite = this.tournamentTrophySpriteManager.GetSprite(new TournamentTrophy(this.tournamentType, 1));
			this.tutorialText.SetKey(PopupTournamentNewKindIntroductionRoot.GetTournamentTutorialTextKey(this.tournamentType));
			string illustrationPath = TournamentConfig.GetTournamentIllustrationPath(this.tournamentType);
			Wooroutine<Sprite> asset = this.abs.LoadAsset<Sprite>("tournament_illustrations", illustrationPath);
			yield return asset;
			yield return null;
			this.tournamentIllustration.sprite = asset.ReturnValue;
			this.tournamentIllustration.color = Color.white;
			this.Show();
			yield break;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00141F83 File Offset: 0x00140383
		private void Show()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x00141FBC File Offset: 0x001403BC
		private void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			string seenFlagNameOfTournament = PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(this.tournamentType);
			if (!seenFlagNameOfTournament.IsNullOrEmpty())
			{
				this.gameStateService.SetSeenFlag(seenFlagNameOfTournament);
			}
			this.gameStateService.Save(false);
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0014202D File Offset: 0x0014042D
		public void Handle(PopupOperation evt)
		{
			if (evt == PopupOperation.Close || evt == PopupOperation.OK)
			{
				new TournamentPopupFlow().Start(this.leagueModel);
				this.Close();
			}
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x00142060 File Offset: 0x00140460
		public static string GetSeenFlagNameOfTournament(TournamentType tournamentType)
		{
			switch (tournamentType)
			{
			case TournamentType.Strawberry:
				return "NewTournamentMigrationPopUp";
			case TournamentType.Banana:
				return "NewBananaTournamentMigrationPopUp";
			case TournamentType.Plum:
				return "NewPlumTournamentMigrationPopUp";
			case TournamentType.Apple:
				return "NewAppleTournamentMigrationPopUp";
			case TournamentType.Starfruit:
				return "NewStarfruitTournamentMigrationPopUp";
			case TournamentType.Grape:
				return "NewGrapeTournamentMigrationPopUp";
			default:
				return string.Empty;
			}
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x001420BC File Offset: 0x001404BC
		private static string GetTournamentTutorialTextKey(TournamentType tournamentType)
		{
			switch (tournamentType)
			{
			case TournamentType.Strawberry:
				return "ui.tournaments.tutorial_type4_strawberry.1";
			case TournamentType.Banana:
				return "ui.tournaments.tutorial_type5_banana.1";
			case TournamentType.Plum:
				return "ui.tournaments.tutorial_type6_plum.1";
			case TournamentType.Apple:
				return "ui.tournaments.tutorial_type7_apple.1";
			case TournamentType.Starfruit:
				return "ui.tournaments.tutorial_type8_starfruit.1";
			case TournamentType.Grape:
				return "ui.tournaments.tutorial_type9_grape.1";
			default:
				return string.Empty;
			}
		}

		// Token: 0x0400687E RID: 26750
		public const string NEW_TOURNAMENT_POP_UP_SEEN_FLAG_NAME = "NewTournamentMigrationPopUp";

		// Token: 0x0400687F RID: 26751
		private const string NEW_BANANA_TOURNAMENT_POP_UP_SEEN_FLAG_NAME = "NewBananaTournamentMigrationPopUp";

		// Token: 0x04006880 RID: 26752
		private const string NEW_PLUM_TOURNAMENT_POP_UP_SEEN_FLAG_NAME = "NewPlumTournamentMigrationPopUp";

		// Token: 0x04006881 RID: 26753
		private const string NEW_APPLE_TOURNAMENT_POP_UP_SEEN_FLAG_NAME = "NewAppleTournamentMigrationPopUp";

		// Token: 0x04006882 RID: 26754
		private const string NEW_STARFRUIT_TOURNAMENT_POP_UP_SEEN_FLAG_NAME = "NewStarfruitTournamentMigrationPopUp";

		// Token: 0x04006883 RID: 26755
		private const string NEW_GRAPE_TOURNAMENT_POP_UP_SEEN_FLAG_NAME = "NewGrapeTournamentMigrationPopUp";

		// Token: 0x04006884 RID: 26756
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006885 RID: 26757
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006886 RID: 26758
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x04006887 RID: 26759
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04006888 RID: 26760
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006889 RID: 26761
		[SerializeField]
		private Image tournamentIllustration;

		// Token: 0x0400688A RID: 26762
		[SerializeField]
		private Image trophyImage;

		// Token: 0x0400688B RID: 26763
		[SerializeField]
		private PTTextLocalizer tutorialText;

		// Token: 0x0400688C RID: 26764
		[SerializeField]
		private TournamentTrophySpriteManager tournamentTrophySpriteManager;

		// Token: 0x0400688D RID: 26765
		private LeagueModel leagueModel;

		// Token: 0x0400688E RID: 26766
		private TournamentType tournamentType;

		// Token: 0x02000A48 RID: 2632
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003F12 RID: 16146 RVA: 0x00142117 File Offset: 0x00140517
			public Trigger(PopupManager popupManager, GameStateService gameStateService, TournamentService tournamentService)
			{
				this.popupManager = popupManager;
				this.gameStateService = gameStateService;
				this.tournamentService = tournamentService;
			}

			// Token: 0x06003F13 RID: 16147 RVA: 0x00142134 File Offset: 0x00140534
			public override bool ShouldTrigger()
			{
				TournamentEventConfig activeTournamentEventConfig = this.tournamentService.GetActiveTournamentEventConfig();
				if (activeTournamentEventConfig == null)
				{
					return false;
				}
				int unlock_at_level = this.tournamentService.configService.general.tournaments.unlock_at_level;
				bool flag = !this.tournamentService.progressionService.IsLocked(unlock_at_level);
				TournamentType tournamentType = activeTournamentEventConfig.config.tournamentType;
				string seenFlagNameOfTournament = PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(tournamentType);
				return flag && TournamentConfig.IsFruitTournament(tournamentType) && !this.gameStateService.GetSeenFlag(seenFlagNameOfTournament);
			}

			// Token: 0x06003F14 RID: 16148 RVA: 0x001421C0 File Offset: 0x001405C0
			public override IEnumerator Run()
			{
				this.popupManager.SkipTriggers = true;
				LeagueModel leagueModel = this.tournamentService.GetActiveLeagueState();
				Wooroutine<PopupTournamentNewKindIntroductionRoot> popup = SceneManager.Instance.LoadSceneWithParams<PopupTournamentNewKindIntroductionRoot, LeagueModel>(leagueModel, null);
				yield return popup;
				yield return popup.ReturnValue.onDestroyed;
				yield break;
			}

			// Token: 0x0400688F RID: 26767
			private readonly PopupManager popupManager;

			// Token: 0x04006890 RID: 26768
			private readonly GameStateService gameStateService;

			// Token: 0x04006891 RID: 26769
			private readonly TournamentService tournamentService;
		}
	}
}
