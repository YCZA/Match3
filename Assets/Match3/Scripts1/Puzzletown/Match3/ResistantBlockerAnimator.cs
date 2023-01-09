using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000680 RID: 1664
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/ResistantBlockerAnimator")]
	public class ResistantBlockerAnimator : AAnimator<ResistantBlockerExplosion>
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x000BBFD0 File Offset: 0x000BA3D0
		protected override void DoAppend(ResistantBlockerExplosion explosion)
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

		// Token: 0x06002985 RID: 10629 RVA: 0x000BC074 File Offset: 0x000BA474
		private IEnumerator DelayedDoAppend(ResistantBlockerExplosion explosion, float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), view);
			int hp = ResistantBlocker.GetHp(explosion.NewAmount);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.resistantBlockerExplosionFx[hp]), view);
			view.SetBlocker(explosion.NewAmount);
			if (explosion.NewAmount == 0)
			{
				this.audioService.PlaySFX(AudioId.ResistantBlockerFreed, false, false, false);
				this.boardView.onModifierCollected.Dispatch(view.transform, "resistant_blocker");
			}
			AudioId hitAudioId = (hp != 2) ? AudioId.ResistantBlockerNormalHit : AudioId.ResistantBlockerHit3Hp;
			this.audioService.PlaySFX(hitAudioId, false, false, false);
			yield break;
		}

		// Token: 0x04005315 RID: 21269
		public GameObject explosionDustFx;

		// Token: 0x04005316 RID: 21270
		public GameObject[] resistantBlockerExplosionFx;

		// Token: 0x04005317 RID: 21271
		public float cascadeDelay = 0.0425f;

		// Token: 0x04005318 RID: 21272
		private const int MAX_HP = 2;
	}
}
