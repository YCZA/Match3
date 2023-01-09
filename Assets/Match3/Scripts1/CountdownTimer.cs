using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A20 RID: 2592
namespace Match3.Scripts1
{
	[RequireComponent(typeof(TimeSpanView))]
	public class CountdownTimer : MonoBehaviour
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x00008B09 File Offset: 0x00006F09
		private TimeSpanView TimeSpanView
		{
			get
			{
				if (this.timeSpanView == null)
				{
					this.timeSpanView = base.GetComponent<TimeSpanView>();
				}
				return this.timeSpanView;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x00008B2E File Offset: 0x00006F2E
		private DateTime TimerTime
		{
			get
			{
				return (!this.isUTC) ? DateTime.Now : DateTime.UtcNow;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x00008B4A File Offset: 0x00006F4A
		private TimeSpan TimeRemaining
		{
			get
			{
				return this.targetTime - this.TimerTime;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x00008B5D File Offset: 0x00006F5D
		private bool InLowTime
		{
			get
			{
				return this.TimerTime > this.lowTimeStart;
			}
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00008B70 File Offset: 0x00006F70
		public void SetTargetTime(DateTime targetTime, bool isUTC = true, Action onTargetTimeReached = null)
		{
			this.SetTargetTime(targetTime, targetTime, isUTC, onTargetTimeReached);
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00008B7C File Offset: 0x00006F7C
		public void SetTargetTime(DateTime targetTime, DateTime lowTimeStart, bool isUTC = true, Action onTargetTimeReached = null)
		{
			this.targetTime = targetTime;
			this.lowTimeStart = lowTimeStart;
			this.isUTC = isUTC;
			this.onTargetTimeReached = onTargetTimeReached;
			this.hasDispatchedTarget = false;
			if (this.refreshRoutine != null)
			{
				base.StopCoroutine(this.refreshRoutine);
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.refreshRoutine = base.StartCoroutine(this.RefreshRoutine());
			}
			this.initialized = true;
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00008BF0 File Offset: 0x00006FF0
		private IEnumerator RefreshRoutine()
		{
			WaitForSeconds waitTime = new WaitForSeconds(1f);
			while (base.gameObject.activeInHierarchy)
			{
				yield return this.TimeSpanView.SetTimeSpan(this.TimeRemaining, this.InLowTime);
				if (this.TimeRemaining.TotalSeconds <= 0.0 && this.onTargetTimeReached != null && !this.hasDispatchedTarget)
				{
					this.onTargetTimeReached();
					this.hasDispatchedTarget = true;
				}
				yield return waitTime;
			}
			yield break;
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00008C0B File Offset: 0x0000700B
		private void OnEnable()
		{
			if (this.initialized)
			{
				if (this.refreshRoutine != null)
				{
					base.StopCoroutine(this.refreshRoutine);
				}
				this.refreshRoutine = base.StartCoroutine(this.RefreshRoutine());
			}
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00008C41 File Offset: 0x00007041
		private void OnDisable()
		{
			if (this.refreshRoutine != null)
			{
				base.StopCoroutine(this.refreshRoutine);
			}
			this.hasDispatchedTarget = false;
		}

		// Token: 0x04006748 RID: 26440
		private Coroutine refreshRoutine;

		// Token: 0x04006749 RID: 26441
		private DateTime targetTime;

		// Token: 0x0400674A RID: 26442
		private DateTime lowTimeStart;

		// Token: 0x0400674B RID: 26443
		private bool initialized;

		// Token: 0x0400674C RID: 26444
		private bool isUTC;

		// Token: 0x0400674D RID: 26445
		private bool hasDispatchedTarget;

		// Token: 0x0400674E RID: 26446
		private Action onTargetTimeReached;

		// Token: 0x0400674F RID: 26447
		private TimeSpanView timeSpanView;
	}
}
