using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000661 RID: 1633
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Climber/ClimberSwapAnimator")]
	public class ClimberSwapAnimator : AAnimator<ClimberMove>
	{
		// Token: 0x06002916 RID: 10518 RVA: 0x000B81B8 File Offset: 0x000B65B8
		protected override void DoAppend(ClimberMove climberMove)
		{
			Move move = climberMove.move;
			if (!move.isSwap)
			{
				return;
			}
			this.InsertAnimation(move, base.GetGemView(move.from, false), (Vector3)move.to);
			this.InsertAnimation(move, base.GetGemView(move.to, false), (Vector3)move.from);
			AudioId audioId = AudioId.ClimberJump;
			if (Time.time < this.lastSwapAt + this.duration * 1.3f)
			{
				audioId = AudioId.InvalidSwap;
			}
			this.audioService.PlaySFX(audioId, false, false, false);
			this.lastSwapAt = Time.time;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000B8260 File Offset: 0x000B6660
		private void InsertAnimation(Move move, GemView gemView, Vector3 targetPosition)
		{
			if (!gemView)
			{
				return;
			}
			Tweener t = gemView.transform.DOLocalMove(targetPosition, base.ModifiedDuration, false);
			if (gemView.GetComponentInChildren<ClimberView>() == null)
			{
				this.sequence.Insert(0f, t.SetEase(this.swapAnimator.curve));
				this.swapAnimator.PlaySwapAnimation(gemView, move);
			}
			else
			{
				this.sequence.Insert(0f, t.SetEase(Ease.InOutSine));
			}
		}

		// Token: 0x040052CF RID: 21199
		private float lastSwapAt = -99f;

		// Token: 0x040052D0 RID: 21200
		public SwapAnimator swapAnimator;
	}
}
