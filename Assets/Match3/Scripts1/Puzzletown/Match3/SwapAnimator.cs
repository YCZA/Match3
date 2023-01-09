using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000687 RID: 1671
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/SwapAnimator")]
	public class SwapAnimator : AAnimator<Move>
	{
		// Token: 0x06002998 RID: 10648 RVA: 0x000BCE48 File Offset: 0x000BB248
		public void OnEnable()
		{
			if (this.animationIds.IsNullOrEmptyCollection())
			{
				this.animationIds = new Dictionary<IntVector2, AnimationClip>
				{
					{
						IntVector2.Up,
						this.clipUp
					},
					{
						IntVector2.Right,
						this.clipRight
					},
					{
						IntVector2.Down,
						this.clipDown
					},
					{
						IntVector2.Left,
						this.clipLeft
					}
				};
			}
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000BCEB8 File Offset: 0x000BB2B8
		public void PlaySwapAnimation(GemView view, Move move)
		{
			if (move.needsAnimation)
			{
				IntVector2 key = IntVector2.Direction(move.from, move.to);
				AnimationClip clip = this.animationIds[key];
				view.Play(clip, base.ModifiedDuration, true);
			}
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000BCF00 File Offset: 0x000BB300
		protected override void DoAppend(Move move)
		{
			if (!move.isSwap)
			{
				return;
			}
			this.InsertAnimation(move, base.GetGemView(move.from, false), (Vector3)move.to);
			this.InsertAnimation(move, base.GetGemView(move.to, false), (Vector3)move.from);
			if (Time.time > this.lastSwapAt + this.duration * 1.3f)
			{
				this.audioService.PlaySFX(AudioId.Swap, false, false, false);
			}
			else
			{
				this.audioService.PlaySFX(AudioId.InvalidSwap, false, false, false);
			}
			this.lastSwapAt = Time.time;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000BCFB0 File Offset: 0x000BB3B0
		private void InsertAnimation(Move move, GemView gemView, Vector3 targetPosition)
		{
			if (!gemView)
			{
				return;
			}
			this.sequence.Insert(0f, gemView.transform.DOLocalMove(targetPosition, base.ModifiedDuration, false).SetEase(this.curve));
			this.PlaySwapAnimation(gemView, move);
		}

		// Token: 0x04005324 RID: 21284
		public AnimationCurve curve;

		// Token: 0x04005325 RID: 21285
		[SerializeField]
		private AnimationClip clipUp;

		// Token: 0x04005326 RID: 21286
		[SerializeField]
		private AnimationClip clipDown;

		// Token: 0x04005327 RID: 21287
		[SerializeField]
		private AnimationClip clipLeft;

		// Token: 0x04005328 RID: 21288
		[SerializeField]
		private AnimationClip clipRight;

		// Token: 0x04005329 RID: 21289
		private Dictionary<IntVector2, AnimationClip> animationIds;

		// Token: 0x0400532A RID: 21290
		private float lastSwapAt = -99f;
	}
}
