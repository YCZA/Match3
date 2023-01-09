using System.Collections;
using Match3.Scripts1.Audio;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000651 RID: 1617
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/BombExplodeAnimator")]
	public class BombExplodeAnimator : AAnimator<BombExplosion>
	{
		// Token: 0x060028E5 RID: 10469 RVA: 0x000B64F8 File Offset: 0x000B48F8
		protected override void DoAppend(BombExplosion explosion)
		{
			bool flag = explosion.gem.type != GemType.Undefined;
			GemView gemView = (!flag || explosion.isSuperBomb) ? base.GetGemView(explosion.Center, true) : this.boardView.CreateGemView(explosion.gem);
			float num = this.animController.fieldDelays[explosion.gem.position];
			float num2 = this.cascadeDelay / this.animController.speed;
			foreach (IntVector2 intVector in explosion.HighlightPositions)
			{
				int num3 = IntVector2.SimpleDistance(explosion.Center, intVector);
				this.animController.fieldDelays[intVector] = num + num2 * (float)num3;
			}
			foreach (Gem gem in explosion.Group)
			{
				if (gem.position != explosion.Center)
				{
					GemView gemView2 = base.GetGemView(gem.position, true);
					this.PlayMatchAnimation(gemView2, gem);
				}
			}
			if (flag)
			{
				gemView.GetComponent<FlashBomb>().Show(base.ModifiedDuration, num, true);
			}
			else
			{
				this.PlayMatchAnimation(gemView, explosion.gem);
			}
			this.boardView.StartCoroutine(this.ShowBombExplosion(gemView, explosion, num));
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000B66B0 File Offset: 0x000B4AB0
		private IEnumerator ShowBombExplosion(GemView bombGemView, BombExplosion explosion, float delay)
		{
			bool isFirstExplosion = explosion.gem.type != GemType.Undefined;
			bool isSuperBomb = explosion.isSuperBomb;
			yield return new WaitForSeconds(delay);
			this.AttachBombFx(bombGemView, isSuperBomb);
			AudioId audioId = (!isSuperBomb) ? AudioId.BombExplode : AudioId.ExplodeSuperBomb;
			this.audioService.PlaySFX(audioId, false, false, false);
			if (isFirstExplosion)
			{
				bombGemView.Show(explosion.gem);
			}
			else
			{
				bombGemView.GetComponent<FlashBomb>().Stop();
			}
			yield break;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000B66E0 File Offset: 0x000B4AE0
		private void AttachBombFx(GemView view, bool isSuperBomb)
		{
			GameObject gameObject = (!isSuperBomb) ? view.objectPool.Get(this.prefabExplosion3x3) : view.objectPool.Get(this.prefabExplosion5x5);
			view.AttachFxToGemView(gameObject);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			float delay = base.ModifiedDuration;
			if (component != null && component.main.duration > base.ModifiedDuration)
			{
				delay = component.main.duration;
			}
			gameObject.Release(delay);
		}

		// Token: 0x040052B3 RID: 21171
		public GameObject prefabExplosion3x3;

		// Token: 0x040052B4 RID: 21172
		public GameObject prefabExplosion5x5;

		// Token: 0x040052B5 RID: 21173
		public float cascadeDelay = 0.1275f;
	}
}
