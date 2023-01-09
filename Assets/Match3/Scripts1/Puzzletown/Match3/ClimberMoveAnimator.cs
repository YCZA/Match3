using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200065F RID: 1631
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Climber/ClimberMoveAnimator")]
	public class ClimberMoveAnimator : AAnimator<ClimberMove>
	{
		// Token: 0x06002912 RID: 10514 RVA: 0x000B807B File Offset: 0x000B647B
		protected override void DoAppend(ClimberMove climberMove)
		{
			if (climberMove.move.isSwap)
			{
				return;
			}
			this.moveAnimator.PlayMoveAnimation(climberMove.move, base.ModifiedDuration, AudioId.ClimberJump);
		}

		// Token: 0x040052CE RID: 21198
		public MoveAnimator moveAnimator;
	}
}
