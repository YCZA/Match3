using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000675 RID: 1653
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/IceAnimator")]
	public class IceAnimator : AAnimator<IceExplosion>
	{
		// Token: 0x0600294C RID: 10572 RVA: 0x000B9C28 File Offset: 0x000B8028
		protected override void DoAppend(IceExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.position);
			float delay = this.animController.fieldDelays[explosion.position];
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, delay, fieldView));
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000B9C88 File Offset: 0x000B8088
		private IEnumerator DelayedDoAppend(IceExplosion explosion, float delay, FieldView view)
		{
			GemView gemView = base.GetGemView(explosion.position, true);
			yield return new WaitForSeconds(delay);
			gemView.Show(explosion.gem);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), view);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.iceExplosionFx[(int)explosion.gem.modifier]), view);
			this.audioService.PlaySFX(AudioId.IceExplosion, false, false, false);
			yield break;
		}

		// Token: 0x040052F2 RID: 21234
		public GameObject explosionDustFx;

		// Token: 0x040052F3 RID: 21235
		public GameObject[] iceExplosionFx;
	}
}
