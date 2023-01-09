using Match3.Scripts1.Audio;
using Match3.Scripts1.Shared.DataStructures;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000670 RID: 1648
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/HammerRainbowExplosionAnimator")]
	public class HammerRainbowExplosionAnimator : AAnimator<HammerRainbowExplosion>
	{
		// Token: 0x06002940 RID: 10560 RVA: 0x000B9738 File Offset: 0x000B7B38
		protected override void DoAppend(HammerRainbowExplosion explosion)
		{
			this.audioService.PlaySFX(AudioId.FishBoostUsed, false, false, false);
			base.BlockSequence(this.sequence, this.boardView.gameObject, 0f);
			IntVector2 position = explosion.Position;
			Map<float> fieldDelays;
			IntVector2 vec;
			(fieldDelays = this.animController.fieldDelays)[vec = position] = fieldDelays[vec] + base.ModifiedDuration * this.matchDelayInPercentOfDuration;
			this.audioService.PlaySFX(AudioId.HammerBoostHit, false, false, false);
			GameObject gameObject = this.boardView.objectPool.Get(this.animatedHammer);
			gameObject.transform.SetParent(this.boardView.transform);
			gameObject.transform.localPosition = (Vector3)position;
			gameObject.Release(base.ModifiedDuration);
			this.rainbowAnimator.AppendToSequence(this.sequence, explosion.explosion);
		}

		// Token: 0x040052E9 RID: 21225
		public GameObject animatedHammer;

		// Token: 0x040052EA RID: 21226
		public float matchDelayInPercentOfDuration = 0.85f;

		// Token: 0x040052EB RID: 21227
		public RainbowExplosionAnimator rainbowAnimator;
	}
}
