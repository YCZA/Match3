using UnityEngine;

// Token: 0x02000964 RID: 2404
namespace Match3.Scripts1
{
	public abstract class AOverheadUiView : AVisibleGameObject
	{
		// Token: 0x06003AA2 RID: 15010 RVA: 0x001182E1 File Offset: 0x001166E1
		public void SetPosition(Vector3 position)
		{
			base.transform.position = position;
			AOverheadUiView.SortByDepth(base.transform);
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x001182FC File Offset: 0x001166FC
		public static void SortByDepth(Transform t)
		{
			if (t == null)
			{
				return;
			}
			Transform parent = t.parent;
			if (parent == null)
			{
				return;
			}
			float num = AOverheadUiView.SortValue(t);
			int i;
			for (i = 0; i < parent.childCount; i++)
			{
				if (AOverheadUiView.SortValue(parent.GetChild(i)) < num)
				{
					break;
				}
			}
			t.SetSiblingIndex(i);
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00118368 File Offset: 0x00116768
		private static float SortValue(Transform t)
		{
			return t.position.x + t.position.z;
		}
	}
}
