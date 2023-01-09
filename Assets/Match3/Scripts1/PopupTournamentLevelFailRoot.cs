using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A46 RID: 2630
namespace Match3.Scripts1
{
	public class PopupTournamentLevelFailRoot : APtSceneRoot<TournamentScore, bool>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003F04 RID: 16132 RVA: 0x00141DC2 File Offset: 0x001401C2
		public void Show()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x00141DF9 File Offset: 0x001401F9
		public override void Setup(TournamentScore parameters)
		{
			base.Setup(parameters);
			this.SetupIcon(parameters.TournamentType);
			this.SetupLabel(parameters.CollectedPoints, parameters.Multiplier);
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00141E24 File Offset: 0x00140224
		private void SetupIcon(TournamentType eventType)
		{
			Sprite sprite = this.spriteManager.GetSprite(eventType);
			this.tournamentTaskIcon.sprite = sprite;
			this.tournamentTaskIcon.gameObject.SetActive(sprite != null);
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00141E64 File Offset: 0x00140264
		private void SetupLabel(int collectedPoints, int multiplier)
		{
			this.collectedPointsLabel.text = string.Format("{0}", collectedPoints * multiplier);
			string substring = string.Format("_x{0}", multiplier);
			bool active = multiplier > 1;
			this.difficultyBonus.SetActive(active);
			this.tournamentMultiplierIcon.gameObject.SetActive(active);
			this.tournamentMultiplierIcon.sprite = this.multiplierSprites.GetSimilar(substring);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00141ED8 File Offset: 0x001402D8
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

		// Token: 0x06003F09 RID: 16137 RVA: 0x00141F08 File Offset: 0x00140308
		private void Close(bool confirmExit)
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			this.onCompleted.Dispatch(confirmExit);
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x00141F56 File Offset: 0x00140356
		private void CloseViaBackButton()
		{
			this.Close(false);
		}

		// Token: 0x04006875 RID: 26741
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006876 RID: 26742
		public TournamentTaskSpriteManager spriteManager;

		// Token: 0x04006877 RID: 26743
		public TextMeshProUGUI collectedPointsLabel;

		// Token: 0x04006878 RID: 26744
		public GameObject difficultyBonus;

		// Token: 0x04006879 RID: 26745
		public Image tournamentTaskIcon;

		// Token: 0x0400687A RID: 26746
		public Image tournamentMultiplierIcon;

		// Token: 0x0400687B RID: 26747
		public SpriteManager multiplierSprites;

		// Token: 0x0400687C RID: 26748
		public AnimatedUi dialog;

		// Token: 0x0400687D RID: 26749
		public Signal<bool> onClose = new Signal<bool>();
	}
}
