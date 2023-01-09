using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000679 RID: 1657
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/LineGemMatchAnimator")]
	public class LineGemMatchAnimator : AMatchWithGemAnimator<LineGemMatch>
	{
		// Token: 0x06002961 RID: 10593 RVA: 0x000BAA1A File Offset: 0x000B8E1A
		protected override void PlaySound()
		{
			this.audioService.PlaySFX(AudioId.LineGemMatch, false, false, false);
		}
	}
}
