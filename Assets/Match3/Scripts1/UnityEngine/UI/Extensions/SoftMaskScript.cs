using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BDE RID: 3038
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/Extensions/SoftMaskScript")]
	public class SoftMaskScript : MonoBehaviour
	{
		// Token: 0x0600473A RID: 18234 RVA: 0x0016AE8C File Offset: 0x0016928C
		private void Start()
		{
			if (this.MaskArea == null)
			{
				this.MaskArea = base.GetComponent<RectTransform>();
			}
			Text component = base.GetComponent<Text>();
			if (component != null)
			{
				this.mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				component.material = this.mat;
				this.cachedCanvas = component.canvas;
				this.cachedCanvasTransform = this.cachedCanvas.transform;
				if (base.transform.parent.GetComponent<Mask>() == null)
				{
					base.transform.parent.gameObject.AddComponent<Mask>();
				}
				base.transform.parent.GetComponent<Mask>().enabled = false;
				return;
			}
			Graphic component2 = base.GetComponent<Graphic>();
			if (component2 != null)
			{
				this.mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				component2.material = this.mat;
				this.cachedCanvas = component2.canvas;
				this.cachedCanvasTransform = this.cachedCanvas.transform;
			}
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0016AFA0 File Offset: 0x001693A0
		private void Update()
		{
			if (this.cachedCanvas != null)
			{
				this.SetMask();
			}
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0016AFBC File Offset: 0x001693BC
		private void SetMask()
		{
			Rect canvasRect = this.GetCanvasRect();
			Vector2 size = canvasRect.size;
			this.maskScale.Set(1f / size.x, 1f / size.y);
			this.maskOffset = -canvasRect.min;
			this.maskOffset.Scale(this.maskScale);
			this.mat.SetTextureOffset("_AlphaMask", this.maskOffset);
			this.mat.SetTextureScale("_AlphaMask", this.maskScale);
			this.mat.SetTexture("_AlphaMask", this.AlphaMask);
			this.mat.SetFloat("_HardBlend", (float)((!this.HardBlend) ? 0 : 1));
			this.mat.SetInt("_FlipAlphaMask", (!this.FlipAlphaMask) ? 0 : 1);
			this.mat.SetInt("_NoOuterClip", (!this.DontClipMaskScalingRect) ? 0 : 1);
			this.mat.SetFloat("_CutOff", this.CutOff);
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0016B0E0 File Offset: 0x001694E0
		public Rect GetCanvasRect()
		{
			if (this.cachedCanvas == null)
			{
				return default(Rect);
			}
			this.MaskArea.GetWorldCorners(this.m_WorldCorners);
			for (int i = 0; i < 4; i++)
			{
				this.m_CanvasCorners[i] = this.cachedCanvasTransform.InverseTransformPoint(this.m_WorldCorners[i]);
			}
			return new Rect(this.m_CanvasCorners[0].x, this.m_CanvasCorners[0].y, this.m_CanvasCorners[2].x - this.m_CanvasCorners[0].x, this.m_CanvasCorners[2].y - this.m_CanvasCorners[0].y);
		}

		// Token: 0x04006E4C RID: 28236
		private Material mat;

		// Token: 0x04006E4D RID: 28237
		private Canvas cachedCanvas;

		// Token: 0x04006E4E RID: 28238
		private Transform cachedCanvasTransform;

		// Token: 0x04006E4F RID: 28239
		private readonly Vector3[] m_WorldCorners = new Vector3[4];

		// Token: 0x04006E50 RID: 28240
		private readonly Vector3[] m_CanvasCorners = new Vector3[4];

		// Token: 0x04006E51 RID: 28241
		[Tooltip("The area that is to be used as the container.")]
		public RectTransform MaskArea;

		// Token: 0x04006E52 RID: 28242
		[Tooltip("Texture to be used to do the soft alpha")]
		public Texture AlphaMask;

		// Token: 0x04006E53 RID: 28243
		[Tooltip("At what point to apply the alpha min range 0-1")]
		[Range(0f, 1f)]
		public float CutOff;

		// Token: 0x04006E54 RID: 28244
		[Tooltip("Implement a hard blend based on the Cutoff")]
		public bool HardBlend;

		// Token: 0x04006E55 RID: 28245
		[Tooltip("Flip the masks alpha value")]
		public bool FlipAlphaMask;

		// Token: 0x04006E56 RID: 28246
		[Tooltip("If a different Mask Scaling Rect is given, and this value is true, the area around the mask will not be clipped")]
		public bool DontClipMaskScalingRect;

		// Token: 0x04006E57 RID: 28247
		private Vector2 maskOffset = Vector2.zero;

		// Token: 0x04006E58 RID: 28248
		private Vector2 maskScale = Vector2.one;
	}
}
