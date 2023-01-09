using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000706 RID: 1798
namespace Match3.Scripts1
{
	public class M3_CompletedFailedRoot : APtSceneRoot<Match3Score, bool>, IDisposableDialog
	{
		// Token: 0x06002C86 RID: 11398 RVA: 0x000CD1B8 File Offset: 0x000CB5B8
		protected override void Go()
		{
			if (base.registeredFirst)
			{
				this.parameters = this.testScore;
			}
			if (this.livesService.IsCurrentlyUnlimitedLives)
			{
				this.minusOneLife.gameObject.SetActive(false);
			}
			this.buttonClose.onClick.AddListener(delegate()
			{
				this.HandleButtonClose(false);
			});
			this.buttonContinue.onClick.AddListener(delegate()
			{
				this.HandleButtonClose(!this.parameters.success);
			});
			this.labelLevelName.text = string.Format(this.locaService.GetText("ui.level_N", new LocaParam[0]), this.parameters.Config.LevelCollectionConfig.level);
			int selectedTier = (int)this.parameters.Config.SelectedTier;
			this.labelLevelTier.text = this.m3ConfigService.GetTierName(selectedTier);
			this.buttonClose.gameObject.SetActive(!this.parameters.success);
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
			this.objectivesDoneDataSource.Show(this.parameters.ObjectivesNeeded);
			this.audioService.PlaySFX(AudioId.LevelFailed, false, false, false);
			this.canvasGroup.interactable = true;
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000CD307 File Offset: 0x000CB707
		protected override void Awake()
		{
			base.Awake();
			this.dialog.Show();
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x000CD31A File Offset: 0x000CB71A
		private void HandleBackButton()
		{
			this.HandleButtonClose(false);
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000CD323 File Offset: 0x000CB723
		private void CollectAndClose()
		{
			this.onCompleted.Dispatch(false);
			this.dialog.Hide();
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000CD33C File Offset: 0x000CB73C
		private void HandleButtonClose(bool wantsRetry)
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButton));
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

		// Token: 0x040055D2 RID: 21970
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x040055D3 RID: 21971
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040055D4 RID: 21972
		// [WaitForService(true, true)]
		// private TrackingService tracking;

		// Token: 0x040055D5 RID: 21973
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040055D6 RID: 21974
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x040055D7 RID: 21975
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040055D8 RID: 21976
		[SerializeField]
		private M3_ObjectivesDoneDataSource objectivesDoneDataSource;

		// Token: 0x040055D9 RID: 21977
		[SerializeField]
		private TextMeshProUGUI labelLevelName;

		// Token: 0x040055DA RID: 21978
		[SerializeField]
		private TextMeshProUGUI labelLevelTier;

		// Token: 0x040055DB RID: 21979
		[SerializeField]
		private Button buttonContinue;

		// Token: 0x040055DC RID: 21980
		[SerializeField]
		private Button buttonClose;

		// Token: 0x040055DD RID: 21981
		[SerializeField]
		private Match3Score testScore;

		// Token: 0x040055DE RID: 21982
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040055DF RID: 21983
		[SerializeField]
		private GameObject minusOneLife;

		// Token: 0x040055E0 RID: 21984
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040055E1 RID: 21985
		private TextMeshProUGUI[] labels;
	}
}
