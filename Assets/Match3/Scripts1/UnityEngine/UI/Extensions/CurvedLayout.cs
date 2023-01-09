using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BE3 RID: 3043
	[AddComponentMenu("Layout/Extensions/Curved Layout")]
	public class CurvedLayout : LayoutGroup
	{
		// Token: 0x06004767 RID: 18279 RVA: 0x0016C580 File Offset: 0x0016A980
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CalculateRadial();
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0016C58E File Offset: 0x0016A98E
		public override void SetLayoutHorizontal()
		{
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0016C590 File Offset: 0x0016A990
		public override void SetLayoutVertical()
		{
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0016C592 File Offset: 0x0016A992
		public override void CalculateLayoutInputVertical()
		{
			this.CalculateRadial();
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0016C59A File Offset: 0x0016A99A
		public override void CalculateLayoutInputHorizontal()
		{
			this.CalculateRadial();
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0016C5A4 File Offset: 0x0016A9A4
		private void CalculateRadial()
		{
			this.m_Tracker.Clear();
			if (base.transform.childCount == 0)
			{
				return;
			}
			Vector2 pivot = new Vector2((float)((int)base.childAlignment % (int)TextAnchor.MiddleLeft) * 0.5f, ((float)base.childAlignment / (float)TextAnchor.MiddleLeft) * 0.5f);
			Vector3 a = new Vector3(base.GetStartOffset(0, base.GetTotalPreferredSize(0)), base.GetStartOffset(1, base.GetTotalPreferredSize(1)), 0f);
			float num = 0f;
			float num2 = 1f / (float)base.transform.childCount;
			Vector3 b = this.itemAxis.normalized * this.itemSize;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null)
				{
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
					Vector3 a2 = a + b;
					a = (rectTransform.localPosition = a2 + (num - this.centerpoint) * this.CurveOffset);
					rectTransform.pivot = pivot;
					RectTransform rectTransform2 = rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform.anchorMax = vector;
					rectTransform2.anchorMin = vector;
					num += num2;
				}
			}
		}

		// Token: 0x04006E74 RID: 28276
		public Vector3 CurveOffset;

		// Token: 0x04006E75 RID: 28277
		[Tooltip("axis along which to place the items, Normalized before use")]
		public Vector3 itemAxis;

		// Token: 0x04006E76 RID: 28278
		[Tooltip("size of each item along the Normalized axis")]
		public float itemSize;

		// Token: 0x04006E77 RID: 28279
		public float centerpoint = 0.5f;
	}
}
