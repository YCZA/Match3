using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200065A RID: 1626
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Chameleon/ChameleonMoveAnimator")]
	public class ChameleonMoveAnimator : AAnimator<ChameleonMove>
	{
		// Token: 0x06002902 RID: 10498 RVA: 0x000B77AC File Offset: 0x000B5BAC
		protected override void DoAppend(ChameleonMove matchResult)
		{
			this.audioService.PlaySFX(AudioId.ChameleonMoving, false, false, false);
			IntVector2 from = matchResult.Move.from;
			FieldView fieldView = this.boardView.GetFieldView(from);
			float num = this.animController.fieldDelays[from];
			ChameleonView chameleonView = this.boardView.GetChameleonView(fieldView.GridPosition);
			chameleonView.SetChameleonIsStuck(false);
			chameleonView.PlayMoveAnimation();
			base.BlockSequence(this.sequence, fieldView.gameObject, num + this.swapAnimator.duration);
			this.boardView.StartCoroutine(this.DelayedDoAppend(matchResult, num));
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000B7854 File Offset: 0x000B5C54
		private IEnumerator DelayedDoAppend(ChameleonMove matchResult, float delay)
		{
			yield return new WaitForSeconds(delay + this.moveAnimationDelay);
			this.SwapChameleon(matchResult);
			yield break;
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000B7880 File Offset: 0x000B5C80
		private void SwapChameleon(ChameleonMove matchResult)
		{
			Move move = matchResult.Move;
			GemView gemView = base.GetGemView(move.from, false);
			if (gemView != null)
			{
				this.InsertAnimation(move, gemView, (Vector3)move.to);
			}
			GemView gemView2 = base.GetGemView(move.to, false);
			if (gemView2 != null)
			{
				this.InsertAnimation(move, gemView2, (Vector3)move.from);
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000B78F4 File Offset: 0x000B5CF4
		private void InsertAnimation(Move move, GemView gemView, Vector3 targetPosition)
		{
			Tweener t = gemView.transform.DOLocalMove(targetPosition, base.ModifiedDuration, false);
			if (gemView.GetComponentInChildren<ChameleonView>() == null)
			{
				this.sequence.Insert(0f, t.SetEase(this.swapAnimator.curve));
				this.swapAnimator.PlaySwapAnimation(gemView, move);
			}
			else
			{
				this.sequence.Insert(0f, t.SetEase(Ease.InOutSine));
			}
		}

		// Token: 0x040052C4 RID: 21188
		public float moveAnimationDelay = 0.1f;

		// Token: 0x040052C5 RID: 21189
		public SwapAnimator swapAnimator;
	}
}
