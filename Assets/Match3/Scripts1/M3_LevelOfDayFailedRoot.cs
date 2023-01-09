using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000709 RID: 1801
namespace Match3.Scripts1
{
	public class M3_LevelOfDayFailedRoot : APtSceneRoot<Match3Score, bool>, IDisposableDialog
	{
		// Token: 0x06002CAD RID: 11437 RVA: 0x000CF46E File Offset: 0x000CD86E
		protected override void OnDestroy()
		{
			this.RemoveListener();
			base.OnDestroy();
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000CF47C File Offset: 0x000CD87C
		protected override void Go()
		{
			base.registeredFirst |= (this.parameters != null && this.parameters.isUITestingOnly);
			LevelOfDayModel levelOfDayModel = (!base.registeredFirst) ? null : this.testModel;
			if (base.registeredFirst)
			{
				this.parameters = this.testScore;
				levelOfDayModel.endUTCTime = this.timeService.Now.ToUnixTimeStamp() + 3600;
			}
			else
			{
				LevelOfDayModel levelOfDayModel2 = null;
				this.gameStateService.LevelOfDayData.TryGetSavedLevelOfDayModel(out levelOfDayModel2);
				if (levelOfDayModel2 != null)
				{
					levelOfDayModel2.currentDay = ((!this.levelOfDayService.CanPlayerStillTry()) ? 1 : levelOfDayModel2.currentDay);
				}
			}
			this.SetupUIElements(levelOfDayModel);
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
			if (!base.registeredFirst)
			{
				this.objectivesDoneDataSource.Show(this.parameters.ObjectivesNeeded);
			}
			this.audioService.PlaySFX(AudioId.LevelFailed, false, false, false);
			this.canvasGroup.interactable = true;
			this.dialog.Show();
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000CF5A8 File Offset: 0x000CD9A8
		protected void SetupUIElements(LevelOfDayModel lodModelOverride)
		{
			int remainingSeconds = this.levelOfDayService.GetRemainingSeconds();
			bool flag = this.levelOfDayService.CanPlayerStillTry();
			int triesSoFar = this.levelOfDayService.GetCurrentTryCount();
			if (lodModelOverride != null)
			{
				int utcNow = this.timeService.Now.ToUnixTimeStamp();
				flag = lodModelOverride.CanPlayerStillTry(utcNow, out remainingSeconds);
				triesSoFar = lodModelOverride.GetTryCount();
			}
			this.SetupBadLuckMessage(flag);
			this.SetupButtons(flag);
			this.SetupTimer(remainingSeconds, flag);
			this.SetupTryStatus(triesSoFar);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000CF61E File Offset: 0x000CDA1E
		protected void SetupTryStatus(int triesSoFar)
		{
			this.tryStatus.Init(triesSoFar, true, this.dialog.open.length);
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000CF63D File Offset: 0x000CDA3D
		protected void SetupBadLuckMessage(bool canPlayerStillTry)
		{
			this.badLuckMessage.SetActive(!canPlayerStillTry);
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000CF64E File Offset: 0x000CDA4E
		protected void SetupTimer(int remainingSeconds, bool canPlayerStillTry)
		{
			this.timerLabel.Refresh(remainingSeconds, remainingSeconds >= 0 && canPlayerStillTry);
			if (remainingSeconds >= 0 && canPlayerStillTry)
			{
				this.AddListener();
			}
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000CF67A File Offset: 0x000CDA7A
		protected void AddListener()
		{
			this.levelOfDayService.OnTimerChanged.AddListener(new Action<int>(this.HandleTimerChanged));
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000CF698 File Offset: 0x000CDA98
		protected void RemoveListener()
		{
			if (this.levelOfDayService != null)
			{
				this.levelOfDayService.OnTimerChanged.RemoveListener(new Action<int>(this.HandleTimerChanged));
			}
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000CF6C1 File Offset: 0x000CDAC1
		protected void HandleTimerChanged(int remainingSeconds)
		{
			this.timerLabel.Refresh(remainingSeconds, remainingSeconds >= 0);
			if (remainingSeconds < 0)
			{
				this.ShowButtons(false);
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000CF6E4 File Offset: 0x000CDAE4
		protected void SetupButtons(bool canRetry)
		{
			this.buttonOK.onClick.AddListener(delegate()
			{
				this.HandleButtonClose(false);
			});
			this.buttonRetry.onClick.AddListener(delegate()
			{
				this.HandleButtonClose(!this.parameters.success);
			});
			this.buttonClose.onClick.AddListener(delegate()
			{
				this.HandleButtonClose(false);
			});
			this.ShowButtons(canRetry);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000CF74C File Offset: 0x000CDB4C
		protected void ShowButtons(bool canRetry)
		{
			this.buttonRetry.gameObject.SetActive(canRetry);
			this.buttonOK.gameObject.SetActive(!canRetry);
			this.buttonClose.gameObject.SetActive(canRetry);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000CF784 File Offset: 0x000CDB84
		private void HandleBackButton()
		{
			this.HandleButtonClose(false);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000CF78D File Offset: 0x000CDB8D
		private void CollectAndClose()
		{
			this.onCompleted.Dispatch(false);
			this.dialog.Hide();
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000CF7A8 File Offset: 0x000CDBA8
		private void HandleButtonClose(bool wantsRetry)
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButton));
			if (!this.isClosing)
			{
				this.isClosing = true;
				if (wantsRetry)
				{
					this.onCompleted.Dispatch(true);
					this.dialog.Hide();
				}
				else
				{
					this.CollectAndClose();
				}
			}
		}

		// Token: 0x04005620 RID: 22048
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x04005621 RID: 22049
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005622 RID: 22050
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04005623 RID: 22051
		[WaitForService(true, true)]
		private LevelOfDayService levelOfDayService;

		// Token: 0x04005624 RID: 22052
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04005625 RID: 22053
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005626 RID: 22054
		[SerializeField]
		private M3_ObjectivesDoneDataSource objectivesDoneDataSource;

		// Token: 0x04005627 RID: 22055
		[SerializeField]
		private Button buttonRetry;

		// Token: 0x04005628 RID: 22056
		[SerializeField]
		private Button buttonOK;

		// Token: 0x04005629 RID: 22057
		[SerializeField]
		private Button buttonClose;

		// Token: 0x0400562A RID: 22058
		[SerializeField]
		private Match3Score testScore;

		// Token: 0x0400562B RID: 22059
		[SerializeField]
		private LevelOfDayModel testModel;

		// Token: 0x0400562C RID: 22060
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x0400562D RID: 22061
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x0400562E RID: 22062
		[SerializeField]
		private TimerLabel timerLabel;

		// Token: 0x0400562F RID: 22063
		[SerializeField]
		private GameObject badLuckMessage;

		// Token: 0x04005630 RID: 22064
		[SerializeField]
		private LevelOfDayTryStatusUI tryStatus;

		// Token: 0x04005631 RID: 22065
		private bool isClosing;
	}
}
