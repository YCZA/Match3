using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006B3 RID: 1715
	public class GrowingWindowView : AReleasable
	{
		// Token: 0x06002AD5 RID: 10965 RVA: 0x000C415C File Offset: 0x000C255C
		public void AnimateGrowingWindow(Field field, GrowingWindowView.GrowDirection growDirection)
		{
			bool flag = (field.gridPosition.x + field.gridPosition.y) % 2 == 0;
			switch (growDirection)
			{
			case GrowingWindowView.GrowDirection.None:
				this.growingWindowAnimator.Play("BambooAppearEffect_init");
				break;
			case GrowingWindowView.GrowDirection.Up:
				this.growingWindowAnimator.Play((!flag) ? "BambooAppearEffect_up_typeA" : "BambooAppearEffect_up_typeB");
				break;
			case GrowingWindowView.GrowDirection.Down:
				this.growingWindowAnimator.Play((!flag) ? "BambooAppearEffect_down_typeB" : "BambooAppearEffect_down_typeA");
				break;
			case GrowingWindowView.GrowDirection.Left:
				this.growingWindowAnimator.Play("BambooAppearEffect_right");
				break;
			case GrowingWindowView.GrowDirection.Right:
				this.growingWindowAnimator.Play("BambooAppearEffect_left");
				break;
			}
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x000C4230 File Offset: 0x000C2630
		public void AnimateGrowingWindowBridge(Field field)
		{
			bool flag = (field.gridPosition.x + field.gridPosition.y) % 2 == 0;
			this.growingWindowAnimator.Play((!flag) ? "BambooAppearEffect_init_typeB" : "BambooAppearEffect_init_typeA");
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000C427A File Offset: 0x000C267A
		public void AnimateClearGrowingWindow()
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this.growingWindowAnimator.Play("BambooAppearEffect_clear");
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x000C429D File Offset: 0x000C269D
		public void ResetAndDisableGameObject()
		{
			this.ResetView();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000C42B1 File Offset: 0x000C26B1
		private void ResetView()
		{
			this.spriteGrowingWindow.enabled = true;
			this.spriteGrowingWindowBridge1.gameObject.SetActive(false);
			this.spriteGrowingWindowBridge2.gameObject.SetActive(false);
		}

		// Token: 0x04005424 RID: 21540
		[SerializeField]
		public SpriteRenderer spriteGrowingWindow;

		// Token: 0x04005425 RID: 21541
		[SerializeField]
		public SpriteRenderer spriteGrowingWindowBridge1;

		// Token: 0x04005426 RID: 21542
		[SerializeField]
		public SpriteRenderer spriteGrowingWindowBridge2;

		// Token: 0x04005427 RID: 21543
		[SerializeField]
		public Animator growingWindowAnimator;

		// Token: 0x020006B4 RID: 1716
		public enum GrowDirection
		{
			// Token: 0x04005429 RID: 21545
			None,
			// Token: 0x0400542A RID: 21546
			Up,
			// Token: 0x0400542B RID: 21547
			Down,
			// Token: 0x0400542C RID: 21548
			Left,
			// Token: 0x0400542D RID: 21549
			Right
		}
	}
}
