using Match3.Scripts1.Audio;
using Match3.Scripts1.Shared.DataStructures;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000671 RID: 1649
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/HammerStarExplosionAnimator")]
	public class HammerStarExplosionAnimator : AAnimator<HammerStarExplosion>
	{
		// Token: 0x06002942 RID: 10562 RVA: 0x000B9834 File Offset: 0x000B7C34
		protected override void DoAppend(HammerStarExplosion explosion)
		{
			this.audioService.PlaySFX(AudioId.FishBoostUsed, false, false, false);
			base.BlockSequence(this.sequence, this.boardView.gameObject, 0f);
			IntVector2 center = explosion.Center;
			Map<float> fieldDelays;
			IntVector2 vec;
			(fieldDelays = this.animController.fieldDelays)[vec = center] = fieldDelays[vec] + base.ModifiedDuration * this.matchDelayInPercentOfDuration;
			this.audioService.PlaySFX(AudioId.HammerBoostHit, false, false, false);
			GameObject gameObject = this.boardView.objectPool.Get(this.animatedHammer);
			gameObject.transform.SetParent(this.boardView.transform);
			gameObject.transform.localPosition = (Vector3)center;
			gameObject.Release(base.ModifiedDuration);
			this.starAnimator.AppendToSequence(this.sequence, explosion.explosion);
		}

		// Token: 0x040052EC RID: 21228
		public GameObject animatedHammer;

		// Token: 0x040052ED RID: 21229
		public float matchDelayInPercentOfDuration = 0.85f;

		// Token: 0x040052EE RID: 21230
		public StarExplodeAnimator starAnimator;
	}
}
