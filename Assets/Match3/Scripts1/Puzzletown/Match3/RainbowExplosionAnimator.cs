using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Shared.DataStructures;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200067D RID: 1661
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/RainbowExplosionAnimator")]
	public class RainbowExplosionAnimator : AAnimator<IRainbowExplosion>
	{
		// Token: 0x06002979 RID: 10617 RVA: 0x000BB64C File Offset: 0x000B9A4C
		protected override void DoAppend(IRainbowExplosion explosion)
		{
			if (this.initialLineRendererStayTime == 0f)
			{
				this.initialLineRendererStayTime = this.rainbowRay.GetComponent<SinusLineRenderer>().StayTime;
			}
			this.lineRendererStayTime = this.initialLineRendererStayTime * base.ModifiedDuration;
			this.rainbowChargeAudio = null;
			GemView gemView = base.GetGemView(explosion.Center, true);
			Transform transform = gemView.transform;
			this.centerDelay = this.animController.fieldDelays[explosion.Center];
			this.totalRayDelay = this.RAY_DELAY * base.ModifiedDuration * (float)explosion.HighlightPositions.Count + this.lineRendererStayTime;
			this.sequence.Insert(this.centerDelay, transform.DOLocalRotate(Vector3.zero, this.totalRayDelay, RotateMode.Fast));
			this.boardView.StartCoroutine(this.AddCenterEffects(gemView, transform));
			this.delay = this.centerDelay;
			GemColor color = explosion.Group.Color;
			List<IntVector2> list = ListPool<IntVector2>.Create(10);
			Gem gem = default(Gem);
			if (explosion is RainbowSuperGemExplosion)
			{
				gem = ((RainbowSuperGemExplosion)explosion).superGem;
				list = ((RainbowSuperGemExplosion)explosion).ShowSupergemPositions;
			}
			foreach (IntVector2 intVector in explosion.Fields)
			{
				if (!explosion.HighlightPositions.Contains(intVector))
				{
					Map<float> fieldDelays;
					IntVector2 vec;
					(fieldDelays = this.animController.fieldDelays)[vec = intVector] = fieldDelays[vec] + (this.centerDelay + this.totalRayDelay);
				}
			}
			this.boardView.StartCoroutine(this.PlayRainbowExplosionSound(this.centerDelay + this.totalRayDelay));
			foreach (IntVector2 intVector2 in explosion.HighlightPositions)
			{
				Map<float> fieldDelays;
				IntVector2 vec2;
				(fieldDelays = this.animController.fieldDelays)[vec2 = intVector2] = fieldDelays[vec2] + this.totalRayDelay;
				if (intVector2 != explosion.Center)
				{
					IntVector2 vec3;
					(fieldDelays = this.animController.fieldDelays)[vec3 = intVector2] = fieldDelays[vec3] + this.centerDelay;
				}
				Gem superGem = (!list.Contains(intVector2)) ? default(Gem) : gem;
				this.boardView.StartCoroutine(this.AddRayAndChargeEffects(transform, intVector2, color, superGem));
				if (explosion.Group.Contains(intVector2))
				{
					Gem gemAtPosition = explosion.Group.GetGemAtPosition(intVector2);
					gemView = this.boardView.GetGemView(intVector2, true);
					this.PlayMatchAnimation(gemView, gemAtPosition);
				}
				this.delay += this.RAY_DELAY;
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000BB96C File Offset: 0x000B9D6C
		private IEnumerator PlayRainbowExplosionSound(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.RainbowExplode, false, false, false);
			yield break;
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000BB990 File Offset: 0x000B9D90
		private IEnumerator AddCenterEffects(GemView gemView, Transform center)
		{
			yield return new WaitForSeconds(this.centerDelay);
			GameObject centerEffect = gemView.objectPool.Get(this.rainbowCenterEffect);
			centerEffect.transform.SetParent(center.parent);
			centerEffect.transform.position = center.position;
			centerEffect.Release(this.totalRayDelay);
			yield break;
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000BB9BC File Offset: 0x000B9DBC
		private IEnumerator AddRayAndChargeEffects(Transform start, IntVector2 pos, GemColor explosionColor, Gem superGem)
		{
			FieldView targetFieldView = this.boardView.GetFieldView(pos);
			Transform target = targetFieldView.transform;
			float rayAndChargeDuration = this.totalRayDelay + this.centerDelay - this.delay;
			yield return new WaitForSeconds(this.delay);
			GameObject ray = this.boardView.objectPool.Get(this.rainbowRay);
			ray.transform.SetParent(start.parent);
			ray.transform.position = start.position;
			SinusLineRenderer lineRenderer = ray.GetComponent<SinusLineRenderer>();
			lineRenderer.targetPosition = target.position;
			Color color = this.GetGemColor(explosionColor);
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			lineRenderer.StayTime = this.lineRendererStayTime;
			lineRenderer.SetupLineRenderer();
			ray.Release(this.lineRendererStayTime);
			if (this.rainbowChargeAudio == null)
			{
				this.rainbowChargeAudio = this.audioService.PlaySFX(AudioId.ChargeRainbow, false, true, false);
			}
			yield return new WaitForSeconds(this.lineRendererStayTime / 2f);
			if (!superGem.Equals(default(Gem)) && start.position != target.position && !targetFieldView.goChain.activeSelf)
			{
				this.ShowSupergem(superGem, (IntVector2)target.position);
			}
			GameObject charge = this.boardView.objectPool.Get(this.chargingEffect);
			charge.transform.SetParent(target.parent);
			charge.transform.position = target.position;
			charge.Release(rayAndChargeDuration);
			yield return new WaitForSeconds(rayAndChargeDuration);
			if (this.rainbowChargeAudio != null)
			{
				this.rainbowChargeAudio.Stop();
				this.rainbowChargeAudio = null;
			}
			yield break;
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000BB9F4 File Offset: 0x000B9DF4
		private Color GetGemColor(GemColor color)
		{
			return this.boardView.GemColorToColor.GetColorFromGemColor(color);
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000BBA08 File Offset: 0x000B9E08
		private void ShowSupergem(Gem superGem, IntVector2 position)
		{
			GemView gemView = this.boardView.GetGemView(position, true);
			if (gemView != null && !gemView.HasModifier)
			{
				gemView.Show(superGem);
			}
		}

		// Token: 0x0400530B RID: 21259
		public GameObject rainbowRay;

		// Token: 0x0400530C RID: 21260
		public GameObject chargingEffect;

		// Token: 0x0400530D RID: 21261
		public GameObject rainbowCenterEffect;

		// Token: 0x0400530E RID: 21262
		private float RAY_DELAY = 0.08f;

		// Token: 0x0400530F RID: 21263
		private float delay;

		// Token: 0x04005310 RID: 21264
		private float totalRayDelay;

		// Token: 0x04005311 RID: 21265
		private float centerDelay;

		// Token: 0x04005312 RID: 21266
		private float lineRendererStayTime;

		// Token: 0x04005313 RID: 21267
		private float initialLineRendererStayTime;

		// Token: 0x04005314 RID: 21268
		private AudioSource rainbowChargeAudio;
	}
}
