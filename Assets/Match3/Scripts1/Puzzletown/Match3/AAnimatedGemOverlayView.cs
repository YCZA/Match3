using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000649 RID: 1609
	public abstract class AAnimatedGemOverlayView : ABlinkingAnimatedView
	{
		// Token: 0x060028C6 RID: 10438 RVA: 0x000B6010 File Offset: 0x000B4410
		public void SwitchState(OverlayAnimationState state)
		{
			bool value = state == OverlayAnimationState.Move;
			this.animator.SetBool(AAnimatedGemOverlayView.IS_MOVING, value);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000B6033 File Offset: 0x000B4433
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SwitchState(OverlayAnimationState.Idle);
		}

		// Token: 0x04005298 RID: 21144
		private static readonly int IS_MOVING = Animator.StringToHash("isMoving");
	}
}
