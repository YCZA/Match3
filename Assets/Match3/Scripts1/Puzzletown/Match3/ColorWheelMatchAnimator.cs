using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000663 RID: 1635
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/ColorWheelMatchAnimator")]
	public class ColorWheelMatchAnimator : AAnimator<ColorWheelMatch>
	{
		// Token: 0x0600291B RID: 10523 RVA: 0x000B84D4 File Offset: 0x000B68D4
		protected override void DoAppend(ColorWheelMatch wheelMatch)
		{
			ColorWheelView colorWheelView = this.boardView.GetColorWheelView(wheelMatch.gridPosition);
			float delay = this.animController.fieldDelays[wheelMatch.createdFrom];
			this.boardView.StartCoroutine(this.DelayedDoAppend(delay, colorWheelView, wheelMatch.gemColor));
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000B8528 File Offset: 0x000B6928
		private IEnumerator DelayedDoAppend(float delay, ColorWheelView view, GemColor colorToRemove)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.ColorWheelCharge, false, false, false);
			view.RemoveColor(colorToRemove);
			yield break;
		}
	}
}
