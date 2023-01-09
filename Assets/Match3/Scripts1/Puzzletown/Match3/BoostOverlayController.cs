using System;
using System.Collections;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200071C RID: 1820
	public class BoostOverlayController
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000D0CC1 File Offset: 0x000CF0C1
		// (set) Token: 0x06002D02 RID: 11522 RVA: 0x000D0CC9 File Offset: 0x000CF0C9
		public Boosts CurrentSelectedBoost { get; protected set; }

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x000D0CD2 File Offset: 0x000CF0D2
		public bool IsBoostOverlayPending
		{
			get
			{
				return this.waitForAnimationRoutine != null;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x000D0CE0 File Offset: 0x000CF0E0
		// (set) Token: 0x06002D05 RID: 11525 RVA: 0x000D0CE8 File Offset: 0x000CF0E8
		public float TickRoutineElapsedTime { get; protected set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002D06 RID: 11526 RVA: 0x000D0CF1 File Offset: 0x000CF0F1
		// (set) Token: 0x06002D07 RID: 11527 RVA: 0x000D0CF9 File Offset: 0x000CF0F9
		public bool IsEnabled { get; protected set; }

		// Token: 0x06002D08 RID: 11528 RVA: 0x000D0D04 File Offset: 0x000CF104
		public void Init(BoostsUiRoot boostsUiRoot)
		{
			if (boostsUiRoot != null && boostsUiRoot.options != null)
			{
				this.IsEnabled = true;
				this.root = boostsUiRoot;
				this.matchEngine = this.root.options.level.Loader.MatchEngine;
				this.SetBoostOverlayVisible(false, Boosts.invalid);
				boostsUiRoot.onBoostSelected.AddListener(new Action<BoostViewData>(this.OverlayHandleBoostSelected));
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000D0D7B File Offset: 0x000CF17B
		public void UpdateView(bool isAnyBoostSelected)
		{
			if (!isAnyBoostSelected)
			{
				this.SetBoostOverlayVisible(false, Boosts.invalid);
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000D0D8B File Offset: 0x000CF18B
		protected void OverlayHandleBoostSelected(BoostViewData boostViewData)
		{
			this.SetBoostOverlayVisible(boostViewData.state == BoostState.Selected, boostViewData.Type);
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000D0DA4 File Offset: 0x000CF1A4
		protected void SetBoostOverlayVisible(bool visible, Boosts boostType)
		{
			if (!this.IsEnabled || this.root == null)
			{
				return;
			}
			bool isInstant = visible && this.waitForAnimationRoutine == null && this.CurrentSelectedBoost != Boosts.invalid;
			this.CurrentSelectedBoost = ((!visible) ? Boosts.invalid : boostType);
			this.TryStopWaitForAnimationRoutine();
			BoostOverlayState newState = new BoostOverlayState(visible, isInstant, boostType);
			this.waitForAnimationRoutine = this.root.StartCoroutine(this.WaitForAnimationAndFadeOverlayInRoutine(newState));
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000D0E2C File Offset: 0x000CF22C
		protected IEnumerator WaitForAnimationAndFadeOverlayInRoutine(BoostOverlayState newState)
		{
			while (this.matchEngine == null || this.matchEngine.IsResolvingMoves)
			{
				yield return null;
				if (this.matchEngine == null)
				{
					this.matchEngine = this.root.options.level.Loader.MatchEngine;
				}
			}
			this.waitForAnimationRoutine = null;
			if (newState.isOn && newState.boostType != Boosts.invalid)
			{
				Application.targetFrameRate = FPSService.ReducedTargetFrameRate;
			}
			this.onBoostOverlayStateChanged.Dispatch(newState);
			this.StartTickingIfNeeded(newState);
			yield break;
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000D0E50 File Offset: 0x000CF250
		private void StartTickingIfNeeded(BoostOverlayState newState)
		{
			if (!newState.shouldChangeInstantly)
			{
				if (this.tickRoutine == null)
				{
					this.tickRoutine = this.root.StartCoroutine(this.TickRoutine());
				}
				else
				{
					this.TickRoutineElapsedTime = 0f;
				}
			}
			else
			{
				this.onTick.Dispatch(0f);
				this.TickRoutineElapsedTime = 0f;
				if (this.tickRoutine != null)
				{
					this.root.StopCoroutine(this.tickRoutine);
					this.tickRoutine = null;
				}
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000D0EE0 File Offset: 0x000CF2E0
		private IEnumerator TickRoutine()
		{
			this.TickRoutineElapsedTime = 0f;
			while (this.TickRoutineElapsedTime < BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS + 0.1f)
			{
				yield return null;
				this.TickRoutineElapsedTime += Time.deltaTime;
				this.onTick.Dispatch(this.TickRoutineElapsedTime);
			}
			this.TickRoutineElapsedTime = 0f;
			if (this.CurrentSelectedBoost == Boosts.invalid)
			{
				Application.targetFrameRate = FPSService.TargetFrameRate;
			}
			this.tickRoutine = null;
			yield break;
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000D0EFB File Offset: 0x000CF2FB
		public void Cleanup()
		{
			Application.targetFrameRate = FPSService.TargetFrameRate;
			this.TryStopWaitForAnimationRoutine();
			this.onBoostOverlayStateChanged.RemoveAllListeners();
			this.onTick.RemoveAllListeners();
			this.root = null;
			this.matchEngine = null;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000D0F31 File Offset: 0x000CF331
		protected void TryStopWaitForAnimationRoutine()
		{
			if (this.waitForAnimationRoutine != null)
			{
				this.root.StopCoroutine(this.waitForAnimationRoutine);
				this.waitForAnimationRoutine = null;
			}
		}

		// Token: 0x0400568B RID: 22155
		public static float BOOST_OVERLAY_FADE_DURATION_SECS = 0.2f;

		// Token: 0x0400568C RID: 22156
		public readonly Signal<BoostOverlayState> onBoostOverlayStateChanged = new Signal<BoostOverlayState>();

		// Token: 0x0400568D RID: 22157
		public readonly Signal<float> onTick = new Signal<float>();

		// Token: 0x0400568F RID: 22159
		private Coroutine waitForAnimationRoutine;

		// Token: 0x04005690 RID: 22160
		private Coroutine tickRoutine;

		// Token: 0x04005692 RID: 22162
		private BoostsUiRoot root;

		// Token: 0x04005693 RID: 22163
		private PTMatchEngine matchEngine;
	}
}
