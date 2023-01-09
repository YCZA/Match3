using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200067E RID: 1662
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/RainbowMatchAnimator")]
	public class RainbowMatchAnimator : AMatchWithGemAnimator<RainbowMatch>
	{
		// Token: 0x06002980 RID: 10624 RVA: 0x000BBF6E File Offset: 0x000BA36E
		protected override void PlaySound()
		{
			this.audioService.PlaySFX(AudioId.RainbowMatch, false, false, false);
		}
	}
}
