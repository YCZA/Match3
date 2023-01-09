using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000658 RID: 1624
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/ChainAnimator")]
	public class ChainAnimator : AAnimator<ChainExplosion>
	{
		// Token: 0x060028FB RID: 10491 RVA: 0x000B72B8 File Offset: 0x000B56B8
		protected override void DoAppend(ChainExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.Position);
			float delay = this.animController.fieldDelays[explosion.Position];
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, delay, fieldView));
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000B7318 File Offset: 0x000B5718
		private IEnumerator DelayedDoAppend(ChainExplosion explosion, float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), view);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.chainExplosionFx[explosion.NewAmount]), view);
			view.SetChains(explosion.NewAmount);
			this.audioService.PlaySFX(AudioId.ChainBreak, false, false, false);
			yield break;
		}

		// Token: 0x040052BE RID: 21182
		public GameObject explosionDustFx;

		// Token: 0x040052BF RID: 21183
		public GameObject[] chainExplosionFx;
	}
}
