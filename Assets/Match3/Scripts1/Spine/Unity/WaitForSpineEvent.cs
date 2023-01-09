using System.Collections;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000255 RID: 597
	public class WaitForSpineEvent : IEnumerator
	{
		// Token: 0x0600124A RID: 4682 RVA: 0x00035CC4 File Offset: 0x000340C4
		public WaitForSpineEvent(AnimationState state, EventData eventDataReference, bool unsubscribeAfterFiring = true)
		{
			this.Subscribe(state, eventDataReference, unsubscribeAfterFiring);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00035CD5 File Offset: 0x000340D5
		public WaitForSpineEvent(SkeletonAnimation skeletonAnimation, EventData eventDataReference, bool unsubscribeAfterFiring = true)
		{
			this.Subscribe(skeletonAnimation.state, eventDataReference, unsubscribeAfterFiring);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00035CEB File Offset: 0x000340EB
		public WaitForSpineEvent(AnimationState state, string eventName, bool unsubscribeAfterFiring = true)
		{
			this.SubscribeByName(state, eventName, unsubscribeAfterFiring);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00035CFC File Offset: 0x000340FC
		public WaitForSpineEvent(SkeletonAnimation skeletonAnimation, string eventName, bool unsubscribeAfterFiring = true)
		{
			this.SubscribeByName(skeletonAnimation.state, eventName, unsubscribeAfterFiring);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00035D14 File Offset: 0x00034114
		private void Subscribe(AnimationState state, EventData eventDataReference, bool unsubscribe)
		{
			if (state == null)
			{
				global::UnityEngine.Debug.LogWarning("AnimationState argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			if (eventDataReference == null)
			{
				global::UnityEngine.Debug.LogWarning("eventDataReference argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			this.m_AnimationState = state;
			this.m_TargetEvent = eventDataReference;
			state.Event += this.HandleAnimationStateEvent;
			this.m_unsubscribeAfterFiring = unsubscribe;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00035D78 File Offset: 0x00034178
		private void SubscribeByName(AnimationState state, string eventName, bool unsubscribe)
		{
			if (state == null)
			{
				global::UnityEngine.Debug.LogWarning("AnimationState argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			if (string.IsNullOrEmpty(eventName))
			{
				global::UnityEngine.Debug.LogWarning("eventName argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			this.m_AnimationState = state;
			this.m_EventName = eventName;
			state.Event += this.HandleAnimationStateEventByName;
			this.m_unsubscribeAfterFiring = unsubscribe;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00035DE4 File Offset: 0x000341E4
		private void HandleAnimationStateEventByName(AnimationState state, int trackIndex, Event e)
		{
			if (state != this.m_AnimationState)
			{
				return;
			}
			this.m_WasFired |= (e.Data.Name == this.m_EventName);
			if (this.m_WasFired && this.m_unsubscribeAfterFiring)
			{
				state.Event -= this.HandleAnimationStateEventByName;
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00035E4C File Offset: 0x0003424C
		private void HandleAnimationStateEvent(AnimationState state, int trackIndex, Event e)
		{
			if (state != this.m_AnimationState)
			{
				return;
			}
			this.m_WasFired |= (e.Data == this.m_TargetEvent);
			if (this.m_WasFired && this.m_unsubscribeAfterFiring)
			{
				state.Event -= this.HandleAnimationStateEvent;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x00035EA9 File Offset: 0x000342A9
		// (set) Token: 0x06001253 RID: 4691 RVA: 0x00035EB1 File Offset: 0x000342B1
		public bool WillUnsubscribeAfterFiring
		{
			get
			{
				return this.m_unsubscribeAfterFiring;
			}
			set
			{
				this.m_unsubscribeAfterFiring = value;
			}
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00035EBA File Offset: 0x000342BA
		public WaitForSpineEvent NowWaitFor(AnimationState state, EventData eventDataReference, bool unsubscribeAfterFiring = true)
		{
			((IEnumerator)this).Reset();
			this.Clear(state);
			this.Subscribe(state, eventDataReference, unsubscribeAfterFiring);
			return this;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00035ED3 File Offset: 0x000342D3
		public WaitForSpineEvent NowWaitFor(AnimationState state, string eventName, bool unsubscribeAfterFiring = true)
		{
			((IEnumerator)this).Reset();
			this.Clear(state);
			this.SubscribeByName(state, eventName, unsubscribeAfterFiring);
			return this;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00035EEC File Offset: 0x000342EC
		private void Clear(AnimationState state)
		{
			state.Event -= this.HandleAnimationStateEvent;
			state.Event -= this.HandleAnimationStateEventByName;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00035F12 File Offset: 0x00034312
		bool IEnumerator.MoveNext()
		{
			if (this.m_WasFired)
			{
				((IEnumerator)this).Reset();
				return false;
			}
			return true;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00035F28 File Offset: 0x00034328
		void IEnumerator.Reset()
		{
			this.m_WasFired = false;
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x00035F31 File Offset: 0x00034331
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04004288 RID: 17032
		private EventData m_TargetEvent;

		// Token: 0x04004289 RID: 17033
		private string m_EventName;

		// Token: 0x0400428A RID: 17034
		private AnimationState m_AnimationState;

		// Token: 0x0400428B RID: 17035
		private bool m_WasFired;

		// Token: 0x0400428C RID: 17036
		private bool m_unsubscribeAfterFiring;
	}
}
