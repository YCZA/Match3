using UnityEngine;

// Token: 0x020004D7 RID: 1239
namespace Match3.Scripts1
{
	public class CannonChargeColorizer : MonoBehaviour
	{
		// Token: 0x0600228D RID: 8845 RVA: 0x00098F67 File Offset: 0x00097367
		private void OnEnable()
		{
			this.UpdateGlowStrength(0f);
			this.isGlowing = false;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00098F7C File Offset: 0x0009737C
		private void Update()
		{
			if (this.isGlowing && this.elapsedSinceGlowStart <= this.glowFadeInDuration)
			{
				this.UpdateGlowStrength(this.glowCurve.Evaluate(this.elapsedSinceGlowStart / this.glowFadeInDuration));
				this.elapsedSinceGlowStart += Time.deltaTime;
			}
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00098FD5 File Offset: 0x000973D5
		public void StartGlowing()
		{
			if (!this.isGlowing)
			{
				this.isGlowing = true;
				this.elapsedSinceGlowStart = 0f;
			}
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00098FF4 File Offset: 0x000973F4
		private bool EnsureMaterialSet()
		{
			if (this.chargeMaterial == null)
			{
				if (this.meshRenderer == null)
				{
					global::UnityEngine.Debug.LogWarning("Cannon not colored! MeshRenderer is missing!");
					return false;
				}
				this.chargeMaterial = this.meshRenderer.material;
			}
			return true;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x00099044 File Offset: 0x00097444
		private void UpdateGlowStrength(float progress)
		{
			this.mainClr.a = progress;
			this.mainClr2.a = progress;
			this.glowClr.a = progress;
			if (this.EnsureMaterialSet())
			{
				this.chargeMaterial.SetColor("_MainColorA", this.mainClr);
				this.chargeMaterial.SetColor("_MainColorB", this.mainClr2);
				this.chargeMaterial.SetColor("_GlowColor", this.glowClr);
			}
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000990C4 File Offset: 0x000974C4
		public void SetColor(Color mainColor)
		{
			this.mainClr = mainColor;
			if (this.CollectEffect != null)
			{
				var go1 = this.CollectEffect.main;
				go1.startColor = mainColor;
			}
			if (this.EnsureMaterialSet())
			{
				this.chargeMaterial.SetColor("_MainColorA", this.mainClr);
			}
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x00099124 File Offset: 0x00097524
		public void SetColor(Color mainColor, Color glowColor)
		{
			this.mainClr = mainColor;
			this.glowClr = glowColor;
			if (this.CollectEffect != null)
			{
				var go1 = this.CollectEffect.main;
				go1.startColor = mainColor;
			}
			if (this.EnsureMaterialSet())
			{
				this.chargeMaterial.SetColor("_MainColorA", this.mainClr);
				this.chargeMaterial.SetColor("_GlowColor", this.glowClr);
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000991A0 File Offset: 0x000975A0
		public void InitializeWithColor(Color mainColor)
		{
			if (this.mainClr != mainColor)
			{
				this.SetColor(mainColor);
			}
		}

		// Token: 0x04004E1B RID: 19995
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x04004E1C RID: 19996
		[SerializeField]
		private ParticleSystem CollectEffect;

		// Token: 0x04004E1D RID: 19997
		private Material chargeMaterial;

		// Token: 0x04004E1E RID: 19998
		private Color mainClr = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04004E1F RID: 19999
		private Color mainClr2 = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04004E20 RID: 20000
		private Color glowClr = new Color(0f, 0.419f, 1f, 0f);

		// Token: 0x04004E21 RID: 20001
		public AnimationCurve glowCurve;

		// Token: 0x04004E22 RID: 20002
		[Range(0.2f, 2f)]
		public float glowFadeInDuration = 0.5f;

		// Token: 0x04004E23 RID: 20003
		private bool isGlowing;

		// Token: 0x04004E24 RID: 20004
		private float elapsedSinceGlowStart;
	}
}
