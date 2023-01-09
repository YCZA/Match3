using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200067B RID: 1659
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/MoveAnimator")]
	public class MoveAnimator : AAnimator<Move>
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x000BB3EC File Offset: 0x000B97EC
		public void PlayMoveAnimation(Move move, float moveDuration, AudioId id)
		{
			GemView gemView = base.GetGemView(move.from, true);
			gemView.PlayExtraMoveAnimation();
			Vector3 endValue = (Vector3)move.to;
			this.sequence.Insert(0f, gemView.transform.DOLocalMove(endValue, moveDuration, false).SetEase(Ease.Linear));
			if (move.needsAnimation)
			{
				float landDuration = this.animController.landingDuration / this.animController.speed;
				this.dropAnimator.PlayDropAnimation(gemView, move.IsFinal, moveDuration, landDuration);
				if (move.IsFinal)
				{
					this.audioService.PlaySFX(id, false, false, false);
				}
			}
			if (move.destroyGem)
			{
				this.boardView.ReleaseView(gemView, moveDuration);
			}
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000BB4AE File Offset: 0x000B98AE
		protected override void DoAppend(Move move)
		{
			if (move.isSwap)
			{
				return;
			}
			this.PlayMoveAnimation(move, base.ModifiedDuration, AudioId.GemLanded);
		}
	}
}
