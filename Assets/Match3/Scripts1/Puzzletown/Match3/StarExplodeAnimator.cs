using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000684 RID: 1668
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/StarExplodeAnimator")]
	public class StarExplodeAnimator : AAnimator<StarExplosion>
	{
		// Token: 0x06002990 RID: 10640 RVA: 0x000BC800 File Offset: 0x000BAC00
		protected override void DoAppend(StarExplosion explosion)
		{
			float num = this.animController.fieldDelays[explosion.Center];
			float num2 = this.cascadeDelay / this.animController.speed;
			foreach (IntVector2 intVector in explosion.HighlightPositions)
			{
				int num3 = IntVector2.SimpleDistance(explosion.Center, intVector);
				this.animController.fieldDelays[intVector] = num + num2 * (float)num3;
			}
			foreach (Gem gem in explosion.Group)
			{
				GemView gemView = base.GetGemView(gem.position, true);
				this.PlayMatchAnimation(gemView, gem);
			}
			this.boardView.StartCoroutine(this.AttachFx(explosion, num));
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000BC920 File Offset: 0x000BAD20
		private IEnumerator AttachFx(StarExplosion explosion, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.LineGemExplode, false, false, false);
			this.lineAnimator.AttachFx(explosion.Center, explosion.blockingPosStartHor, explosion.blockingPosEndHor, GemType.LineHorizontal, delay);
			this.lineAnimator.AttachFx(explosion.Center, explosion.blockingPosStartVert, explosion.blockingPosEndVert, GemType.LineVertical, delay);
			yield break;
		}

		// Token: 0x0400531D RID: 21277
		public float cascadeDelay = 0.085f;

		// Token: 0x0400531E RID: 21278
		public LineGemExplodeAnimator lineAnimator;
	}
}
