using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000653 RID: 1619
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/BombMatchAnimator")]
	public class BombMatchAnimator : AMatchWithGemAnimator<BombMatch>
	{
		// Token: 0x060028EB RID: 10475 RVA: 0x000B6902 File Offset: 0x000B4D02
		protected override void PlaySound()
		{
			this.audioService.PlaySFX(AudioId.BombGemCreated, false, false, false);
		}
	}
}
