using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD7 RID: 3031
	[AddComponentMenu("UI/Effects/Extensions/UIImageCrop")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIImageCrop : MonoBehaviour
	{
		// Token: 0x06004716 RID: 18198 RVA: 0x0016A3C0 File Offset: 0x001687C0
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0016A3C8 File Offset: 0x001687C8
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			this.XCropProperty = Shader.PropertyToID("_XCrop");
			this.YCropProperty = Shader.PropertyToID("_YCrop");
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UI Image Crop"));
				}
				this.mat = this.mGraphic.material;
			}
			else
			{
				global::UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0016A481 File Offset: 0x00168881
		public void OnValidate()
		{
			this.SetMaterial();
			this.SetXCrop(this.XCrop);
			this.SetYCrop(this.YCrop);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0016A4A1 File Offset: 0x001688A1
		public void SetXCrop(float xcrop)
		{
			this.XCrop = Mathf.Clamp01(xcrop);
			this.mat.SetFloat(this.XCropProperty, this.XCrop);
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0016A4C6 File Offset: 0x001688C6
		public void SetYCrop(float ycrop)
		{
			this.YCrop = Mathf.Clamp01(ycrop);
			this.mat.SetFloat(this.YCropProperty, this.YCrop);
		}

		// Token: 0x04006E3B RID: 28219
		private MaskableGraphic mGraphic;

		// Token: 0x04006E3C RID: 28220
		private Material mat;

		// Token: 0x04006E3D RID: 28221
		private int XCropProperty;

		// Token: 0x04006E3E RID: 28222
		private int YCropProperty;

		// Token: 0x04006E3F RID: 28223
		public float XCrop;

		// Token: 0x04006E40 RID: 28224
		public float YCrop;
	}
}
