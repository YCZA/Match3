using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200064A RID: 1610
	public abstract class ABlinkingAnimatedView : AReleasable
	{
		// Token: 0x060028CA RID: 10442 RVA: 0x000B5F41 File Offset: 0x000B4341
		protected virtual int GetRevealBlinkingTimeRangeInMs()
		{
			return 50000;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000B5F48 File Offset: 0x000B4348
		protected virtual void Awake()
		{
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000B5F56 File Offset: 0x000B4356
		protected virtual void OnEnable()
		{
			this.boardView = global::UnityEngine.Object.FindObjectOfType<BoardView>();
			this.nextBlink = (float)this.boardView.viewRandomHelper.Next(this.GetRevealBlinkingTimeRangeInMs()) / 1000f;
			this.timer = 0f;
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000B5F91 File Offset: 0x000B4391
		protected virtual void Update()
		{
			this.timer += Time.deltaTime;
			if (this.timer >= this.nextBlink)
			{
				this.Blink();
			}
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000B5FBC File Offset: 0x000B43BC
		protected virtual void Blink()
		{
			this.animator.SetTrigger("Blink");
			this.GetNextBlinkingTimer();
			this.timer = 0f;
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000B5FDF File Offset: 0x000B43DF
		private void GetNextBlinkingTimer()
		{
			this.nextBlink = (float)this.boardView.viewRandomHelper.Next(5000, 50000) / 1000f;
		}

		// Token: 0x04005299 RID: 21145
		private const string BLINK_EVENT_NAME = "Blink";

		// Token: 0x0400529A RID: 21146
		private const int MIN_BLINKING_PAUSE_MS = 5000;

		// Token: 0x0400529B RID: 21147
		private const int MAX_BLINKING_PAUSE_MS = 50000;

		// Token: 0x0400529C RID: 21148
		private const int REVEAL_BLINKING_TIME_RANGE_MS = 50000;

		// Token: 0x0400529D RID: 21149
		protected BoardView boardView;

		// Token: 0x0400529E RID: 21150
		protected Animator animator;

		// Token: 0x0400529F RID: 21151
		private float timer;

		// Token: 0x040052A0 RID: 21152
		private float nextBlink;
	}
}
