using System;
using Match3.Scripts1.Puzzletown.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007D1 RID: 2001
	public class LevelOfDayPlayButtonUI : MonoBehaviour
	{
		// Token: 0x0600313A RID: 12602 RVA: 0x000E7650 File Offset: 0x000E5A50
		public void Init(LevelOfDayService levelOfDayService)
		{
			this.levelOfDayService = levelOfDayService;
			this.AddListener();
			base.gameObject.SetActive(false);
			this.SetupNotificationIcon();
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x000E7671 File Offset: 0x000E5A71
		public void ShowNotificationIcon(bool state)
		{
			this.notificationIcon.SetActive(state);
			if (this.levelOfDayService != null)
			{
				this.levelOfDayService.NotificationSeen = true;
			}
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000E7696 File Offset: 0x000E5A96
		private void SetupNotificationIcon()
		{
			if (this.levelOfDayService != null)
			{
				this.notificationIcon.SetActive(!this.levelOfDayService.NotificationSeen);
			}
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000E76BC File Offset: 0x000E5ABC
		private void OnDestroy()
		{
			this.RemoveListener();
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000E76C4 File Offset: 0x000E5AC4
		private void AddListener()
		{
			// 审核版隐藏 每日关卡
			// #if REVIEW_VERSION
			// 	return;
			// #endif
			if (this.levelOfDayService.IsEnabled)
			{
				this.levelOfDayService.OnTimerChanged.AddListener(new Action<int>(this.HandleTimerChanged));
			}
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000E76F2 File Offset: 0x000E5AF2
		private void RemoveListener()
		{
			if (this.levelOfDayService != null)
			{
				this.levelOfDayService.OnTimerChanged.RemoveListener(new Action<int>(this.HandleTimerChanged));
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000E771B File Offset: 0x000E5B1B
		private void HandleTimerChanged(int remainingSeconds)
		{
			if (remainingSeconds > 0 && remainingSeconds < 86400)	// 小于24小时
			{
				this.timerLabel.Refresh(remainingSeconds, true);
				base.gameObject.SetActive(true);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x040059EA RID: 23018
		public const int SECONDS_IN_ONE_DAY = 86400;

		// Token: 0x040059EB RID: 23019
		public TimerLabel timerLabel;

		// Token: 0x040059EC RID: 23020
		public GameObject notificationIcon;

		// Token: 0x040059ED RID: 23021
		private LevelOfDayService levelOfDayService;
	}
}
