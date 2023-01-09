using UnityEngine;

namespace Match3.Scripts1.Shared.UI
{
	// Token: 0x02000B34 RID: 2868
	[RequireComponent(typeof(Canvas))]
	public class CanvasSorter : MonoBehaviour
	{
		// Token: 0x06004352 RID: 17234 RVA: 0x00158309 File Offset: 0x00156709
		private void Start()
		{
			this.parentCanvas = base.transform.parent.GetComponentInParent<Canvas>();
			this.Apply();
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x00158327 File Offset: 0x00156727
		public void Apply()
		{
			WoogaDebug.LogWarning(new object[]
			{
				"apply",
				this.parentCanvas.name
			});
			base.GetComponent<Canvas>().sortingOrder = this.parentCanvas.sortingOrder + this.sortOrderInParent;
		}

		// Token: 0x04006BDA RID: 27610
		public int sortOrderInParent = 1;

		// Token: 0x04006BDB RID: 27611
		private Canvas parentCanvas;
	}
}
