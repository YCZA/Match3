using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200070D RID: 1805
namespace Match3.Scripts1
{
	public class M3_PirateBreakoutFailedRoot : APtSceneRoot<Match3Score, bool>, IDisposableDialog
	{
		// Token: 0x06002CC3 RID: 11459 RVA: 0x000CF894 File Offset: 0x000CDC94
		protected override void Go()
		{
			if (this.livesService.IsCurrentlyUnlimitedLives && this.minusOneLife != null)
			{
				this.minusOneLife.gameObject.SetActive(false);
			}
			this.countdownTimer.SetTargetTime(this.gameStateService.PirateBreakout.EndTime, false, null);
			this.buttonContinue.onClick.AddListener(delegate()
			{
				this.Close(true);
			});
			this.buttonClose.onClick.AddListener(delegate()
			{
				this.Close(false);
			});
			BackButtonManager.Instance.AddAction(delegate
			{
				this.Close(false);
			});
			this.objectivesDoneDataSource.Show(this.parameters.ObjectivesNeeded);
			this.audioService.PlaySFX(AudioId.LevelFailed, false, false, false);
			this.canvasGroup.interactable = true;
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000CF974 File Offset: 0x000CDD74
		protected override void Awake()
		{
			base.Awake();
			this.dialog.Show();
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000CF987 File Offset: 0x000CDD87
		private void Close(bool wantsRetry)
		{
			this.onCompleted.Dispatch(wantsRetry);
			BackButtonManager.Instance.RemoveAction(delegate
			{
				this.Close(false);
			});
			this.dialog.Hide();
		}

		// Token: 0x04005639 RID: 22073
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400563A RID: 22074
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400563B RID: 22075
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x0400563C RID: 22076
		[SerializeField]
		private M3_ObjectivesDoneDataSource objectivesDoneDataSource;

		// Token: 0x0400563D RID: 22077
		[SerializeField]
		private Button buttonClose;

		// Token: 0x0400563E RID: 22078
		[SerializeField]
		private Button buttonContinue;

		// Token: 0x0400563F RID: 22079
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x04005640 RID: 22080
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04005641 RID: 22081
		[SerializeField]
		private GameObject minusOneLife;

		// Token: 0x04005642 RID: 22082
		[SerializeField]
		private AnimatedUi dialog;
	}
}
