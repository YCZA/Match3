using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F7 RID: 2551
	public class PirateBreakoutBadgeUi : MonoBehaviour
	{
		// Token: 0x06003D7F RID: 15743 RVA: 0x00136780 File Offset: 0x00134B80
		public IEnumerator SetupRoutine(TownBottomPanelRoot tBPRoot)
		{
			this.townBottomPanelRoot = tBPRoot;
			if (!this.initialized)
			{
				yield return ServiceLocator.Instance.Inject(this);
				this.initialized = true;
			}
			base.gameObject.SetActive(this.pirateBreakoutService.PirateBreakoutFeatureActiveAndEnabled());
			this.timer.SetTargetTime(this.gameStateService.PirateBreakout.EndTime, false, null);
			this.loadingSpinner.SetActive(!this.timeService.IsTimeValid || !this.pirateBreakoutService.AreAssetBundlesAvailable());
			this.AddSlowUpdate(new SlowUpdate(this.SlowUpdate), 3);
			yield break;
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x001367A4 File Offset: 0x00134BA4
		private void UpdateNotificationIndicator()
		{
			bool active = true;
			if (this.gameStateService.IsSeenFlagTimestampSet("weeklyEventSeen"))
			{
				DateTime timeStamp = this.gameStateService.GetTimeStamp("weeklyEventSeen");
				active = (DateTime.UtcNow > timeStamp.AddSeconds((double)this.configService.general.notifications.attention_indicator_cooldown));
			}
			this.notificationBlob.SetActive(active);
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00136810 File Offset: 0x00134C10
		private void SlowUpdate()
		{
			this.UpdateNotificationIndicator();
			this.loadingSpinner.SetActive(!this.timeService.IsTimeValid || !this.pirateBreakoutService.AreAssetBundlesAvailable());
			base.gameObject.SetActive(this.pirateBreakoutService.PirateBreakoutFeatureActiveAndEnabled());
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x00136868 File Offset: 0x00134C68
		public void OnButtonTap()
		{
			if (this.townBottomPanelRoot == null || !this.townBottomPanelRoot.IsInteractable)
			{
				return;
			}
			if (this.loadingSpinner.activeSelf)
			{
				SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
				return;
			}
			base.StartCoroutine(this.ShowPirateBreakoutFlowRoutine());
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x001368C8 File Offset: 0x00134CC8
		private IEnumerator ShowPirateBreakoutFlowRoutine()
		{
			this.trackingService.TrackWeeklyEventBadgeOpen(LevelPlayMode.PirateBreakout);
			Coroutine loadFlow = new LoadPirateBreakoutFlow(null).Start();
			yield return loadFlow;
			yield break;
		}

		// Token: 0x0400664B RID: 26187
		public CountdownTimer timer;

		// Token: 0x0400664C RID: 26188
		public GameObject loadingSpinner;

		// Token: 0x0400664D RID: 26189
		public GameObject notificationBlob;

		// Token: 0x0400664E RID: 26190
		private bool initialized;

		// Token: 0x0400664F RID: 26191
		[WaitForService(true, true)]
		private PirateBreakoutService pirateBreakoutService;

		// Token: 0x04006650 RID: 26192
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04006651 RID: 26193
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006652 RID: 26194
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04006653 RID: 26195
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006654 RID: 26196
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006655 RID: 26197
		private TownBottomPanelRoot townBottomPanelRoot;
	}
}
