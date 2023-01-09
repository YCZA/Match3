using UnityEngine;

// Token: 0x02000ACB RID: 2763
namespace Match3.Scripts1
{
	public static class RectTransformExtensions
	{
		// Token: 0x0600419F RID: 16799 RVA: 0x00152EF7 File Offset: 0x001512F7
		public static Canvas GetCanvas(this RectTransform rt)
		{
			return rt.GetComponentInParent<Canvas>();
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x00152F00 File Offset: 0x00151300
		public static Rect GetRelativeRect(this RectTransform rectTrans)
		{
			Vector2 normalizedPosition = rectTrans.GetNormalizedPosition();
			Rect rect = rectTrans.rect;
			Rect rect2 = rectTrans.GetCanvas().GetComponent<RectTransform>().rect;
			Vector2 vector = new Vector2(1f / rect2.width, 1f / rect2.height);
			float width = rect.width * vector.x;
			float height = rect.height * vector.y;
			Rect result = new Rect(normalizedPosition.x, normalizedPosition.y, width, height);
			return result;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00152F8C File Offset: 0x0015138C
		public static Vector2 GetNormalizedPosition(this RectTransform rt)
		{
			Vector3[] array = new Vector3[4];
			rt.GetWorldCorners(array);
			Camera worldCamera = rt.GetCanvas().worldCamera;
			Vector2 result = RectTransformUtility.WorldToScreenPoint(worldCamera, array[0]);
			Vector2 scale = new Vector2(1f / (float)Screen.width, 1f / (float)Screen.height);
			result.Scale(scale);
			return result;
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x00152FF0 File Offset: 0x001513F0
		public static void SetPivotWithoutPositionChange(this RectTransform rectTransform, Vector2 pivot)
		{
			if (rectTransform == null)
			{
				return;
			}
			Vector2 size = rectTransform.rect.size;
			Vector2 vector = rectTransform.pivot - pivot;
			Vector3 b = new Vector3(vector.x * size.x, vector.y * size.y);
			rectTransform.pivot = pivot;
			rectTransform.localPosition -= b;
		}
	}
}
