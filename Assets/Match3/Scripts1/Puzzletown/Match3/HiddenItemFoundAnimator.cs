using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000672 RID: 1650
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/HiddenItemFoundAnimator")]
	public class HiddenItemFoundAnimator : AAnimator<HiddenItemFound>
	{
		// Token: 0x06002944 RID: 10564 RVA: 0x000B9930 File Offset: 0x000B7D30
		protected override void DoAppend(HiddenItemFound matchResult)
		{
			HiddenItemView hiddenItemView = this.boardView.GetHiddenItemView(matchResult.id);
			float delay = this.animController.fieldDelays[matchResult.hitPosition];
			base.BlockSequence(this.sequence, this.boardView.gameObject, delay);
			hiddenItemView.Reveal(delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(hiddenItemView, delay));
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000B999C File Offset: 0x000B7D9C
		private IEnumerator DelayedDoAppend(HiddenItemView item, float delay)
		{
			float currentAnimationDuration = this.animationDuration / this.animController.speed;
			this.boardView.onHiddenItemFound.Dispatch(item, currentAnimationDuration + delay);
			yield return new WaitForSeconds(delay);
			item.StartAnimation(this.hiddenItemRevealed, currentAnimationDuration);
			this.audioService.PlaySFX(AudioId.HiddenItemFound, false, false, false);
			yield return new WaitForSeconds(currentAnimationDuration);
			if (item)
			{
				item.Hide();
			}
			yield break;
		}

		// Token: 0x040052EF RID: 21231
		public AnimationClip hiddenItemRevealed;

		// Token: 0x040052F0 RID: 21232
		public float animationDuration = 0.5f;
	}
}
