using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004D4 RID: 1236
	public class Recolorizer : MonoBehaviour
	{
		// Token: 0x0600227B RID: 8827 RVA: 0x000987FC File Offset: 0x00096BFC
		public void SetColor(GemColor gemColor)
		{
			if (this.materialInstance == null)
			{
				this.materialInstance = new Material(this.material);
				foreach (MeshRenderer meshRenderer in this.meshRenderers)
				{
					meshRenderer.sharedMaterial = this.materialInstance;
				}
			}
			ColorGradientTuple colors = this.effectColorManager.GetColors(gemColor);
			if (colors != null)
			{
				this.materialInstance.SetColor(Recolorizer.MAIN_COLOR_A, colors.colorA);
				this.materialInstance.SetColor(Recolorizer.MAIN_COLOR_B, colors.colorB);
				this.materialInstance.SetColor(Recolorizer.GLOW_COLOR, colors.colorC);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Butterfly: Could not recolor Butterfly, because the color of '" + gemColor.ToString() + "' was not defined!"
				});
			}
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000988DA File Offset: 0x00096CDA
		public ColorGradientTuple GetColors(GemColor gemColor)
		{
			return this.effectColorManager.GetColors(gemColor);
		}

		// Token: 0x04004DF5 RID: 19957
		public MeshRenderer[] meshRenderers;

		// Token: 0x04004DF6 RID: 19958
		public EffectColorManager effectColorManager;

		// Token: 0x04004DF7 RID: 19959
		public Material material;

		// Token: 0x04004DF8 RID: 19960
		private static readonly int GLOW_COLOR = Shader.PropertyToID("_GlowColor");

		// Token: 0x04004DF9 RID: 19961
		private static readonly int MAIN_COLOR_A = Shader.PropertyToID("_MainColorA");

		// Token: 0x04004DFA RID: 19962
		private static readonly int MAIN_COLOR_B = Shader.PropertyToID("_MainColorB");

		// Token: 0x04004DFB RID: 19963
		private Material materialInstance;
	}
}
