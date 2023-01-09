using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000688 RID: 1672
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/TileAnimator")]
	public class TileAnimator : AAnimator<TileExplosion>
	{
		// Token: 0x0600299D RID: 10653 RVA: 0x000BD008 File Offset: 0x000BB408
		protected override void DoAppend(TileExplosion explosion)
		{
			FieldView fieldView = this.boardView.GetFieldView(explosion.Position);
			float delay = this.animController.fieldDelays[explosion.Position];
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(explosion, delay, fieldView));
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000BD068 File Offset: 0x000BB468
		private IEnumerator DelayedDoAppend(TileExplosion explosion, float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.TileHit, false, false, false);
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.tileExplosionFx[explosion.NewAmount]), view);
			view.SetTiles(explosion.NewAmount);
			yield break;
		}

		// Token: 0x0400532B RID: 21291
		public GameObject[] tileExplosionFx;
	}
}
