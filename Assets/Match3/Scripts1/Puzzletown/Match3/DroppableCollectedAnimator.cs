using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000669 RID: 1641
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/DroppableCollectedAnimator")]
	public class DroppableCollectedAnimator : AAnimator<DroppableCollected>
	{
		// Token: 0x0600292E RID: 10542 RVA: 0x000B8DAC File Offset: 0x000B71AC
		protected override void DoAppend(DroppableCollected collected)
		{
			GemView gemView = base.GetGemView(collected.Position, true);
			this.PlayMatchAnimation(gemView, collected.gem);
			this.audioService.PlaySFX(AudioId.DropableExplode, false, false, false);
		}
	}
}
