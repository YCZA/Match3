using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000655 RID: 1621
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/CannonballExplosionAnimator")]
	public class CannonballExplosionAnimator : AAnimator<CannonballExplosion>
	{
		// Token: 0x060028EF RID: 10479 RVA: 0x000B69F8 File Offset: 0x000B4DF8
		protected override void DoAppend(CannonballExplosion explosion)
		{
			IntVector2 position = explosion.position;
			GemView gemView = this.boardView.GetGemView(position, true);
			FieldView fieldView = this.boardView.GetFieldView(position);
			float num = this.animController.fieldDelays[explosion.CreatedFrom];
			if (position != explosion.CreatedFrom)
			{
				float num2 = IntVector2.Distance(position, explosion.CreatedFrom);
				num += this.cascadeDelay / this.animController.speed * num2;
			}
			this.boardView.ReleaseView(gemView, num);
			base.BlockSequence(this.sequence, gemView.gameObject, num);
			this.boardView.StartCoroutine(this.DelayedDoAppend(num, fieldView));
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000B6AB0 File Offset: 0x000B4EB0
		private IEnumerator DelayedDoAppend(float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), view);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.cannonballExplosionFx), view);
			this.audioService.PlaySFX(AudioId.CannonBallExplode, false, false, false);
			yield break;
		}

		// Token: 0x040052B6 RID: 21174
		public GameObject explosionDustFx;

		// Token: 0x040052B7 RID: 21175
		public GameObject cannonballExplosionFx;

		// Token: 0x040052B8 RID: 21176
		public float cascadeDelay = 0.0425f;
	}
}
