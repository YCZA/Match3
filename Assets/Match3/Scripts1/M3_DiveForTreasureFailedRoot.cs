using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000708 RID: 1800
namespace Match3.Scripts1
{
	public class M3_DiveForTreasureFailedRoot : APtSceneRoot<Match3Score, bool>, IDisposableDialog
	{
		// Token: 0x06002CA8 RID: 11432 RVA: 0x000CF358 File Offset: 0x000CD758
		protected override void Go()
		{
			if (this.livesService.IsCurrentlyUnlimitedLives && this.minusOneLife != null)
			{
				this.minusOneLife.gameObject.SetActive(false);
			}
			this.countdownTimer.SetTargetTime(this.gameStateService.DiveForTreasure.EndTime, false, null);
			this.buttonContinue.onClick.AddListener(delegate()
			{
				this.Close();
			});
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.objectivesDoneDataSource.Show(this.parameters.ObjectivesNeeded);
			this.audioService.PlaySFX(AudioId.LevelFailed, false, false, false);
			this.canvasGroup.interactable = true;
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000CF41C File Offset: 0x000CD81C
		protected override void Awake()
		{
			base.Awake();
			this.dialog.Show();
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000CF42F File Offset: 0x000CD82F
		private void Close()
		{
			this.onCompleted.Dispatch(false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x04005616 RID: 22038
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x04005617 RID: 22039
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005618 RID: 22040
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005619 RID: 22041
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x0400561A RID: 22042
		[SerializeField]
		private M3_ObjectivesDoneDataSource objectivesDoneDataSource;

		// Token: 0x0400561B RID: 22043
		[SerializeField]
		private Button buttonContinue;

		// Token: 0x0400561C RID: 22044
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x0400561D RID: 22045
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x0400561E RID: 22046
		[SerializeField]
		private GameObject minusOneLife;

		// Token: 0x0400561F RID: 22047
		[SerializeField]
		private AnimatedUi dialog;
	}
}
