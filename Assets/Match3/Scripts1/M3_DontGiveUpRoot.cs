using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000714 RID: 1812
namespace Match3.Scripts1
{
	public class M3_DontGiveUpRoot : APtSceneRoot<ScoringController, bool>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06002CE1 RID: 11489 RVA: 0x000D0400 File Offset: 0x000CE800
		public void Show()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			if (this.parameters.GetLevelPlayMode() == LevelPlayMode.DiveForTreasure || this.parameters.GetLevelPlayMode() == LevelPlayMode.PirateBreakout)
			{
				this.diveForTreasureLevelLabel.text = this.parameters.GetLevel().ToString();
				this.diveForTreasureView.gameObject.SetActive(this.parameters.GetLevelPlayMode() == LevelPlayMode.DiveForTreasure);
				if (!string.IsNullOrEmpty(this.parameters.GetCollectable()))
				{
					this.collectableView.SetActive(true);
				}
				else
				{
					this.collectableView.SetActive(false);
				}
			}
			else
			{
				this.diveForTreasureView.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000D04F0 File Offset: 0x000CE8F0
		public override void Setup(ScoringController parameters)
		{
			base.Setup(parameters);
			TournamentScore tournamentScore = parameters.GetTournamentScore();
			this.tournamentView.SetActive(tournamentScore.CollectedPoints > 0);
			this.SetupIcon(tournamentScore.TournamentType);
			this.SetupLabel(tournamentScore.CollectedPoints, tournamentScore.Multiplier);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000D0544 File Offset: 0x000CE944
		private void SetupIcon(TournamentType eventType)
		{
			Sprite sprite = this.spriteManager.GetSprite(eventType);
			this.tournamentTaskIcon.sprite = sprite;
			this.tournamentTaskIcon.gameObject.SetActive(sprite != null);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000D0581 File Offset: 0x000CE981
		private void SetupLabel(int collectedPoints, int multiplier)
		{
			this.collectedPointsLabel.text = string.Format("{0}", collectedPoints * multiplier);
			if (multiplier > 1)
			{
				this.bonusMultiplier.Show();
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000D05B2 File Offset: 0x000CE9B2
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt == PopupOperation.OK)
				{
					this.Close(false);
				}
			}
			else
			{
				this.Close(true);
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000D05E0 File Offset: 0x000CE9E0
		private void Close(bool confirmExit)
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			this.onCompleted.Dispatch(confirmExit);
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000D062E File Offset: 0x000CEA2E
		private void CloseViaBackButton()
		{
			this.Close(false);
		}

		// Token: 0x04005668 RID: 22120
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005669 RID: 22121
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400566A RID: 22122
		public TournamentTaskSpriteManager spriteManager;

		// Token: 0x0400566B RID: 22123
		public TextMeshProUGUI collectedPointsLabel;

		// Token: 0x0400566C RID: 22124
		public Image tournamentTaskIcon;

		// Token: 0x0400566D RID: 22125
		public AnimatedUi dialog;

		// Token: 0x0400566E RID: 22126
		public MaterialAmountView bonusMultiplier;

		// Token: 0x0400566F RID: 22127
		public MaterialAmountView diveForTreasureView;

		// Token: 0x04005670 RID: 22128
		public GameObject collectableView;

		// Token: 0x04005671 RID: 22129
		public GameObject tournamentView;

		// Token: 0x04005672 RID: 22130
		public TextMeshProUGUI diveForTreasureLevelLabel;

		// Token: 0x04005673 RID: 22131
		public Signal<bool> onClose = new Signal<bool>();
	}
}
