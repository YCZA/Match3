using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000667 RID: 1639
	[CreateAssetMenu(menuName = "Puzzletown/Animators/DirtAndTreasure/DirtAnimator")]
	public class DirtAnimator : AAnimator<DirtExplosion>
	{
		// Token: 0x06002929 RID: 10537 RVA: 0x000B8B28 File Offset: 0x000B6F28
		protected override void DoAppend(DirtExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.position);
			float delay = this.animController.fieldDelays[explosion.CreatedFrom];
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, delay, fieldView));
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000B8B88 File Offset: 0x000B6F88
		private IEnumerator DelayedDoAppend(DirtExplosion explosion, float delay, FieldView view)
		{
			GemView gemView = this.boardView.GetGemView(explosion.position, true);
			if (explosion.gem.modifier == GemModifier.Undefined && explosion.gem.color == GemColor.Dirt)
			{
				this.boardView.ReleaseView(gemView, delay);
			}
			yield return new WaitForSeconds(delay);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.explosionDustFx), view);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.dirtExplosionFx[explosion.newHp]), view);
			if (explosion.gem.modifier != GemModifier.Undefined)
			{
				gemView.Show(explosion.gem);
				this.boardView.UpdateDirtBorder();
			}
			this.audioService.PlaySFX(AudioId.DirtExplosion, false, false, false);
			yield break;
		}

		// Token: 0x040052D9 RID: 21209
		public GameObject explosionDustFx;

		// Token: 0x040052DA RID: 21210
		public GameObject[] dirtExplosionFx;
	}
}
