using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200068C RID: 1676
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/WaterAnimator")]
	public class WaterAnimator : AAnimator<WaterExplosion>
	{
		// Token: 0x060029A9 RID: 10665 RVA: 0x000BD61C File Offset: 0x000BBA1C
		protected override void DoAppend(WaterExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.Position);
			float delay = this.animController.fieldDelays[explosion.Position];
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(delay, fieldView));
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000BD67C File Offset: 0x000BBA7C
		private IEnumerator DelayedDoAppend(float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.waterExplosionFx), view);
			view.SetWatered(3);
			this.audioService.PlaySFX(AudioId.WaterSpread, false, false, false);
			yield break;
		}

		// Token: 0x0400532D RID: 21293
		public GameObject waterExplosionFx;
	}
}
