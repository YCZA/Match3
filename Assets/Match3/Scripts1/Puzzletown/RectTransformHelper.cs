using UnityEngine;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x02000A97 RID: 2711
	public static class RectTransformHelper
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x0014E710 File Offset: 0x0014CB10
		public static void GetScreenCorners(RectTransform rect, out Vector2 bottomLeft, out Vector2 topRight, Camera cam = null)
		{
			if (!cam)
			{
				Canvas componentInParent = rect.GetComponentInParent<Canvas>();
				if (!(componentInParent != null))
				{
					bottomLeft = Vector2.zero;
					topRight = Vector2.zero;
					return;
				}
				cam = componentInParent.worldCamera;
			}
			Vector3[] array = new Vector3[4];
			rect.GetWorldCorners(array);
			bottomLeft = RectTransformUtility.WorldToScreenPoint(cam, array[0]);
			topRight = RectTransformUtility.WorldToScreenPoint(cam, array[2]);
		}
	}
}
