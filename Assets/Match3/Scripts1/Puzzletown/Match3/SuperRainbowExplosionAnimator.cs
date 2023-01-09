using Match3.Scripts1.Audio;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000686 RID: 1670
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/SuperRainbowExplosionAnimator")]
	public class SuperRainbowExplosionAnimator : AAnimator<SuperRainbowExplosion>
	{
		// Token: 0x06002996 RID: 10646 RVA: 0x000BCC84 File Offset: 0x000BB084
		protected override void DoAppend(SuperRainbowExplosion explosion)
		{
			GemView gemView = base.GetGemView(explosion.Center, true);
			base.BlockSequence(this.sequence, gemView.gameObject, 0f);
			GameObject gameObject = gemView.objectPool.Get(this.vfx);
			gameObject.transform.SetParent(gemView.transform.parent);
			gameObject.transform.position = gemView.transform.position;
			float duration = this.vfx.GetComponent<ParticleSystem>().main.duration;
			gameObject.SetActive(true);
			float num = this.animController.fieldDelays[explosion.Center];
			float num2 = this.cascadeDelay / this.animController.speed;
			foreach (IntVector2 intVector in explosion.HighlightPositions)
			{
				float num3 = IntVector2.Distance(explosion.Center, intVector);
				this.animController.fieldDelays[intVector] = num + num2 * num3;
			}
			foreach (Gem gem in explosion.Group)
			{
				gemView = base.GetGemView(gem.position, true);
				this.PlayMatchAnimation(gemView, gem);
			}
			this.audioService.PlaySFX(AudioId.ExplodeRainbowRainbow, false, false, false);
			gameObject.Release(duration);
		}

		// Token: 0x04005322 RID: 21282
		public GameObject vfx;

		// Token: 0x04005323 RID: 21283
		public float cascadeDelay = 0.0765f;
	}
}
