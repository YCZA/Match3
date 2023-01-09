using System.Collections;
using Match3.Scripts1.Audio;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000657 RID: 1623
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/CannonExplodeAnimator")]
	public class CannonExplodeAnimator : AAnimator<CannonExplosion>
	{
		// Token: 0x060028F7 RID: 10487 RVA: 0x000B6F30 File Offset: 0x000B5330
		protected override void DoAppend(CannonExplosion explosion)
		{
			float num = this.preExplosionDelay;
			FieldView fieldView = this.boardView.GetFieldView(explosion.Center);
			base.BlockSequence(this.sequence, fieldView.gameObject, num);
			float num2 = this.cascadeDelay / this.animController.speed;
			foreach (IntVector2 intVector in explosion.HighlightPositions)
			{
				int num3 = IntVector2.SimpleDistance(explosion.Center, intVector);
				this.animController.fieldDelays[intVector] = num + num2 * (float)num3;
			}
			this.animController.fieldDelays[explosion.Center] = this.fullPreExplosionDelay;
			foreach (Gem gem in explosion.Group)
			{
				GemView gemView = base.GetGemView(gem.position, true);
				this.matchAnimator.PlayMatchAnimation(gemView, gem);
			}
			this.TryPlayPreExplosionEffect(base.GetGemView(explosion.Center, true));
			this.boardView.StartCoroutine(this.AttachFx(fieldView, explosion.Group.Color, num));
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000B70A8 File Offset: 0x000B54A8
		private void TryPlayPreExplosionEffect(GemView gemView)
		{
			CannonView modifierView = gemView.GetModifierView<CannonView>();
			if (modifierView != null)
			{
				modifierView.PlayPreExplosionEffect();
			}
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000B70D0 File Offset: 0x000B54D0
		private IEnumerator AttachFx(FieldView cannon, GemColor color, float delay)
		{
			yield return new WaitForSeconds(delay - this.sfxBuildUpDelay);
			this.audioService.PlaySFX(AudioId.CannonExplode, false, false, false);
			yield return new WaitForSeconds(this.sfxBuildUpDelay);
			GameObject explodeFxInstance = this.boardView.objectPool.Get(this.cannonFx);
			CannonRecolorizer fxColors = explodeFxInstance.GetComponent<CannonRecolorizer>();
			PlayCameraShake cameraShake = explodeFxInstance.GetComponent<PlayCameraShake>();
			if (cameraShake.CamShake == null)
			{
				cameraShake.CamShake = this.boardView.cam.GetComponent<CameraShake>();
			}
			fxColors.SetColor(this.boardView.GemColorToColor.GetColorFromGemColor(color));
			base.SetFxToFieldview(explodeFxInstance, cannon);
			explodeFxInstance.Release(base.ModifiedDuration);
			yield break;
		}

		// Token: 0x040052B9 RID: 21177
		public float preExplosionDelay = 0.8f;

		// Token: 0x040052BA RID: 21178
		public float sfxBuildUpDelay = 0.4f;

		// Token: 0x040052BB RID: 21179
		private float fullPreExplosionDelay = 1f;

		// Token: 0x040052BC RID: 21180
		public float cascadeDelay = 0.085f;

		// Token: 0x040052BD RID: 21181
		public GameObject cannonFx;
	}
}
