using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009C7 RID: 2503
	public class DiveForTreasureBadgeUi : MonoBehaviour
	{
		// Token: 0x06003CA2 RID: 15522 RVA: 0x0012FF78 File Offset: 0x0012E378
		public IEnumerator SetupRoutine(TownBottomPanelRoot tBPRoot)
		{
			this.townBottomPanelRoot = tBPRoot;
			if (!this.initialized)
			{
				yield return ServiceLocator.Instance.Inject(this);
				this.initialized = true;
			}
			base.gameObject.SetActive(this.diveForTreasureService.DiveForTreasureFeatureActiveAndEnabled());
			this.timer.SetTargetTime(this.gameStateService.DiveForTreasure.EndTime, false, null);
			this.loadingSpinner.SetActive(!this.timeService.IsTimeValid || !this.diveForTreasureService.AreAssetBundlesAvailable());
			this.AddSlowUpdate(new SlowUpdate(this.SlowUpdate), 3);
			yield break;
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x0012FF9C File Offset: 0x0012E39C
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

		// Token: 0x06003CA4 RID: 15524 RVA: 0x00130008 File Offset: 0x0012E408
		private void SlowUpdate()
		{
			this.UpdateNotificationIndicator();
			this.loadingSpinner.SetActive(!this.timeService.IsTimeValid || !this.diveForTreasureService.AreAssetBundlesAvailable());
			base.gameObject.SetActive(this.diveForTreasureService.DiveForTreasureFeatureActiveAndEnabled());
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x00130060 File Offset: 0x0012E460
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
			base.StartCoroutine(this.ShowDiveForTreasureFlowRoutine());
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x001300C0 File Offset: 0x0012E4C0
		private IEnumerator ShowDiveForTreasureFlowRoutine()
		{
			this.trackingService.TrackWeeklyEventBadgeOpen(LevelPlayMode.DiveForTreasure);
			Coroutine loadDiveForTreasure = new LoadDiveForTreasureFlow().Start();
			yield return loadDiveForTreasure;
			yield break;
		}

		// Token: 0x0400655A RID: 25946
		public CountdownTimer timer;

		// Token: 0x0400655B RID: 25947
		[FormerlySerializedAs("loadingSpinnger")]
		public GameObject loadingSpinner;

		// Token: 0x0400655C RID: 25948
		public GameObject notificationBlob;

		// Token: 0x0400655D RID: 25949
		private bool initialized;

		// Token: 0x0400655E RID: 25950
		[WaitForService(true, true)]
		private DiveForTreasureService diveForTreasureService;

		// Token: 0x0400655F RID: 25951
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04006560 RID: 25952
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006561 RID: 25953
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04006562 RID: 25954
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006563 RID: 25955
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006564 RID: 25956
		private TownBottomPanelRoot townBottomPanelRoot;
	}
}
