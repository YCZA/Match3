using System.Collections;
using Match3.Scripts1.Audio;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200068A RID: 1674
	[CreateAssetMenu(menuName = "Puzzletown/Animators/DirtAndTreasure/TreasureCollectedAnimator")]
	public class TreasureCollectedAnimator : AAnimator<TreasureCollected>
	{
		// Token: 0x060029A3 RID: 10659 RVA: 0x000BD2B8 File Offset: 0x000BB6B8
		protected override void DoAppend(TreasureCollected collected)
		{
			float num = this.animController.fieldDelays[collected.CreatedFrom];
			GemView gemView = this.boardView.GetGemView(collected.position, true);
			if (collected.CreatedFrom != collected.position && num > this.animController.fieldDelays[collected.position])
			{
				this.animController.fieldDelays[collected.position] = num;
			}
			WooroutineRunner.StartCoroutine(this.PlaySoundRoutine(num), null);
			this.matchAnimator.PlayMatchAnimation(gemView, collected.gem);
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000BD360 File Offset: 0x000BB760
		private IEnumerator PlaySoundRoutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.TreasureUnveiled, false, false, false);
			yield break;
		}
	}
}
