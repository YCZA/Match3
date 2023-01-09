using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000685 RID: 1669
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/StoneAnimator")]
	public class StoneAnimator : AAnimator<StoneExplosion>
	{
		// Token: 0x06002993 RID: 10643 RVA: 0x000BCA74 File Offset: 0x000BAE74
		protected override void DoAppend(StoneExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.Position);
			float num = this.animController.fieldDelays[explosion.CreatedFrom];
			if (explosion.Position != explosion.CreatedFrom)
			{
				float num2 = IntVector2.Distance(explosion.Position, explosion.CreatedFrom);
				num += this.cascadeDelay / this.animController.speed * num2;
			}
			base.BlockSequence(this.sequence, fieldView.gameObject, num);
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, num, fieldView));
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000BCB18 File Offset: 0x000BAF18
		private IEnumerator DelayedDoAppend(StoneExplosion explosion, float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), view);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.stoneExplosionFx[explosion.NewAmount]), view);
			view.SetBlocker(explosion.NewAmount);
			this.audioService.PlaySFX(AudioId.StoneDestroy, false, false, false);
			yield break;
		}

		// Token: 0x0400531F RID: 21279
		public GameObject explosionDustFx;

		// Token: 0x04005320 RID: 21280
		public GameObject[] stoneExplosionFx;

		// Token: 0x04005321 RID: 21281
		public float cascadeDelay = 0.0425f;
	}
}
