using Match3.Scripts1.Audio;
using Match3.Scripts1.Shared.DataStructures;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200066F RID: 1647
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/HammerMatchAnimator")]
	public class HammerMatchAnimator : AAnimator<HammerMatch>
	{
		// Token: 0x0600293E RID: 10558 RVA: 0x000B9640 File Offset: 0x000B7A40
		protected override void DoAppend(HammerMatch match)
		{
			this.audioService.PlaySFX(AudioId.FishBoostUsed, false, false, false);
			base.BlockSequence(this.sequence, this.boardView.gameObject, 0f);
			IntVector2 position = match.position;
			Map<float> fieldDelays;
			IntVector2 vec;
			(fieldDelays = this.animController.fieldDelays)[vec = position] = fieldDelays[vec] + base.ModifiedDuration * this.matchDelayInPercentOfDuration;
			this.audioService.PlaySFX(AudioId.HammerBoostHit, false, false, false);
			GameObject gameObject = this.boardView.objectPool.Get(this.animatedHammer);
			gameObject.transform.SetParent(this.boardView.transform);
			gameObject.transform.localPosition = (Vector3)position;
			gameObject.Release(base.ModifiedDuration);
			this.matchAnimator.AppendToSequence(this.sequence, match.match);
		}

		// Token: 0x040052E7 RID: 21223
		public GameObject animatedHammer;

		// Token: 0x040052E8 RID: 21224
		public float matchDelayInPercentOfDuration = 0.85f;
	}
}
