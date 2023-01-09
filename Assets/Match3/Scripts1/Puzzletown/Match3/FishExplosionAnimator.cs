using Match3.Scripts1.Shared.DataStructures;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200066A RID: 1642
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/FishExplosionAnimator")]
	public class FishExplosionAnimator : AAnimator<FishExplosion>
	{
		// Token: 0x06002930 RID: 10544 RVA: 0x000B8DF0 File Offset: 0x000B71F0
		protected override void DoAppend(FishExplosion explosion)
		{
			foreach (Gem gem in explosion.Group)
			{
				GemView gemView = base.GetGemView(gem.position, true);
				this.PlayMatchAnimation(gemView, gem);
				Map<float> fieldDelays;
				IntVector2 position;
				(fieldDelays = this.animController.fieldDelays)[position = gem.position] = fieldDelays[position] + this.matchAnimator.duration;
			}
		}
	}
}
