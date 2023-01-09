using System.Collections;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000254 RID: 596
	public class WaitForSpineAnimationComplete : IEnumerator
	{
		// Token: 0x06001243 RID: 4675 RVA: 0x00035C50 File Offset: 0x00034050
		public WaitForSpineAnimationComplete(TrackEntry trackEntry)
		{
			this.SafeSubscribe(trackEntry);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00035C5F File Offset: 0x0003405F
		private void HandleComplete(AnimationState state, int trackIndex, int loopCount)
		{
			this.m_WasFired = true;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00035C68 File Offset: 0x00034068
		private void SafeSubscribe(TrackEntry trackEntry)
		{
			if (trackEntry == null)
			{
				global::UnityEngine.Debug.LogWarning("TrackEntry was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
			}
			else
			{
				trackEntry.Complete += this.HandleComplete;
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00035C98 File Offset: 0x00034098
		public WaitForSpineAnimationComplete NowWaitFor(TrackEntry trackEntry)
		{
			this.SafeSubscribe(trackEntry);
			return this;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00035CA2 File Offset: 0x000340A2
		bool IEnumerator.MoveNext()
		{
			if (this.m_WasFired)
			{
				((IEnumerator)this).Reset();
				return false;
			}
			return true;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00035CB8 File Offset: 0x000340B8
		void IEnumerator.Reset()
		{
			this.m_WasFired = false;
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x00035CC1 File Offset: 0x000340C1
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04004287 RID: 17031
		private bool m_WasFired;
	}
}
