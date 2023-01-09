using Match3.Scripts1.Audio;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000662 RID: 1634
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/ColorWheelExplosionAnimator")]
	public class ColorWheelExplosionAnimator : AAnimator<ColorWheelExplosion>
	{
		// Token: 0x06002919 RID: 10521 RVA: 0x000B8300 File Offset: 0x000B6700
		protected override void DoAppend(ColorWheelExplosion explosion)
		{
			ColorWheelView colorWheelView = this.boardView.GetColorWheelView(explosion.Center);
			base.BlockSequence(this.sequence, colorWheelView.gameObject, 0f);
			GameObject gameObject = colorWheelView.objectPool.Get(this.vfx);
			gameObject.transform.SetParent(colorWheelView.transform.parent);
			gameObject.transform.position = colorWheelView.transform.position;
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
				GemView gemView = base.GetGemView(gem.position, true);
				this.PlayMatchAnimation(gemView, gem);
			}
			this.audioService.PlaySFX(AudioId.ExplodeRainbowRainbow, false, false, false);
			colorWheelView.PlayRemovalAnimation();
			gameObject.Release(duration);
			this.boardView.DestroyColorWheelView(explosion.Center, duration);
		}

		// Token: 0x040052D1 RID: 21201
		public GameObject vfx;

		// Token: 0x040052D2 RID: 21202
		public float cascadeDelay = 0.0765f;
	}
}
