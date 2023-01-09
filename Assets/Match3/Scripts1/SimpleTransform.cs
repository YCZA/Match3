using System;
using UnityEngine;

// Token: 0x02000AA4 RID: 2724
namespace Match3.Scripts1
{
	[Serializable]
	public class SimpleTransform
	{
		// Token: 0x060040BD RID: 16573 RVA: 0x00150374 File Offset: 0x0014E774
		public SimpleTransform(RectTransform transform)
		{
			this.position = transform.anchoredPosition;
			this.rotation = transform.localRotation.eulerAngles;
			this.sizeDelta = transform.sizeDelta;
			this.anchorMin = transform.anchorMin;
			this.anchorMax = transform.anchorMax;
			this.pivot = transform.pivot;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x001503E4 File Offset: 0x0014E7E4
		public void ApplyTo(RectTransform transform)
		{
			transform.anchoredPosition = this.position;
			transform.localRotation = Quaternion.Euler(this.rotation);
			if (this.sizeDelta != Vector3.zero)
			{
				transform.sizeDelta = this.sizeDelta;
			}
			transform.anchorMin = this.anchorMin;
			transform.anchorMax = this.anchorMax;
			transform.pivot = this.pivot;
		}

		// Token: 0x04006A5C RID: 27228
		public Vector3 position;

		// Token: 0x04006A5D RID: 27229
		public Vector3 rotation;

		// Token: 0x04006A5E RID: 27230
		public Vector3 scale;

		// Token: 0x04006A5F RID: 27231
		public Vector3 sizeDelta;

		// Token: 0x04006A60 RID: 27232
		public Vector2 anchorMin;

		// Token: 0x04006A61 RID: 27233
		public Vector2 anchorMax;

		// Token: 0x04006A62 RID: 27234
		public Vector2 pivot;
	}
}
