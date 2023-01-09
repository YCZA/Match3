using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200065D RID: 1629
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Climber/ClimberCollectedAnimator")]
	public class ClimberCollectedAnimator : AAnimator<ClimberCollected>
	{
		// Token: 0x0600290C RID: 10508 RVA: 0x000B7C7C File Offset: 0x000B607C
		protected override void DoAppend(ClimberCollected collected)
		{
			GemView gemView = base.GetGemView(collected.Position, true);
			this.PlayMatchAnimation(gemView, collected.gem);
			this.audioService.PlaySFX(AudioId.ClimberExplosion, false, false, false);
		}
	}
}
