using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BEC RID: 3052
	[AddComponentMenu("Layout/Extensions/Radial Layout")]
	public class RadialLayout : LayoutGroup
	{
		// Token: 0x060047A1 RID: 18337 RVA: 0x0016E457 File Offset: 0x0016C857
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CalculateRadial();
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x0016E465 File Offset: 0x0016C865
		public override void SetLayoutHorizontal()
		{
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x0016E467 File Offset: 0x0016C867
		public override void SetLayoutVertical()
		{
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x0016E469 File Offset: 0x0016C869
		public override void CalculateLayoutInputVertical()
		{
			this.CalculateRadial();
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x0016E471 File Offset: 0x0016C871
		public override void CalculateLayoutInputHorizontal()
		{
			this.CalculateRadial();
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0016E47C File Offset: 0x0016C87C
		private void CalculateRadial()
		{
			this.m_Tracker.Clear();
			if (base.transform.childCount == 0)
			{
				return;
			}
			float num = (this.MaxAngle - this.MinAngle) / (float)base.transform.childCount;
			float num2 = this.StartAngle;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null)
				{
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
					Vector3 a = new Vector3(Mathf.Cos(num2 * 0.017453292f), Mathf.Sin(num2 * 0.017453292f), 0f);
					rectTransform.localPosition = a * this.fDistance;
					RectTransform rectTransform2 = rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform.pivot = vector;
					vector = vector;
					rectTransform.anchorMax = vector;
					rectTransform2.anchorMin = vector;
					num2 += num;
				}
			}
		}

		// Token: 0x04006E88 RID: 28296
		public float fDistance;

		// Token: 0x04006E89 RID: 28297
		[Range(0f, 360f)]
		public float MinAngle;

		// Token: 0x04006E8A RID: 28298
		[Range(0f, 360f)]
		public float MaxAngle;

		// Token: 0x04006E8B RID: 28299
		[Range(0f, 360f)]
		public float StartAngle;
	}
}
