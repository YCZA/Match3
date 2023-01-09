using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Shared.M3Engine;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000681 RID: 1665
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/ShuffleAnimator")]
	public class ShuffleAnimator : AAnimator<ShuffleResult>
	{
		// Token: 0x06002987 RID: 10631 RVA: 0x000BC264 File Offset: 0x000BA664
		protected override void DoAppend(ShuffleResult matchResult)
		{
			this.sequence.Insert(0f, this.boardView.transform.DOLocalRotate(Vector3.zero, base.ModifiedDuration * 1.5f, RotateMode.Fast));
			this.boardView.ShowShuffleBanner(base.ModifiedDuration);
			float speed = this.animController.speed;
			this.animController.speed /= this.slowDownFactor;
			for (int i = 0; i < matchResult.results.Count; i++)
			{
				IMatchResult matchResult2 = matchResult.results[i];
				if (matchResult2 is Move)
				{
					this.moveAnimator.AppendToSequence(this.sequence, (Move)matchResult2);
				}
				else if (matchResult2 is ReplaceGem)
				{
					this.replaceAnimator.AppendToSequence(this.sequence, (ReplaceGem)matchResult2);
				}
			}
			this.animController.speed = speed;
			this.audioService.PlaySFX(AudioId.Reshuffle, false, false, false);
		}

		// Token: 0x04005319 RID: 21273
		public float slowDownFactor = 8f;

		// Token: 0x0400531A RID: 21274
		public MoveAnimator moveAnimator;

		// Token: 0x0400531B RID: 21275
		public ReplaceGemAnimator replaceAnimator;
	}
}
