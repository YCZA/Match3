using System.Linq;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Shared.UI
{
	// Token: 0x02000B36 RID: 2870
	public class PlaceOnTop : MonoBehaviour
	{
		// Token: 0x06004355 RID: 17237 RVA: 0x00158381 File Offset: 0x00156781
		private void OnEnable()
		{
			if (!this.isStatic)
			{
				this.UpdateOrder(this.layer);
			}
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0015839C File Offset: 0x0015679C
		public void UpdateOrder(UILayer layer)
		{
			this.layer = layer;
			int num = ((int)layer * (int)(UILayer)1000);
			int maxSortOrder = this.GetMaxSortOrder();
			Canvas component = base.GetComponent<Canvas>();
			component.sortingOrder = ((maxSortOrder <= num) ? num : (maxSortOrder + 1));
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x001583DC File Offset: 0x001567DC
		private int GetMaxSortOrder()
		{
			return (from c in global::UnityEngine.Object.FindObjectsOfType<Canvas>().Where(delegate(Canvas c)
			{
				PlaceOnTop component = c.GetComponent<PlaceOnTop>();
				return !component || component.layer == this.layer;
			})
			select c.sortingOrder).Max();
		}

		// Token: 0x04006BDF RID: 27615
		public readonly Signal onUpdated = new Signal();

		// Token: 0x04006BE0 RID: 27616
		public UILayer layer;

		// Token: 0x04006BE1 RID: 27617
		public bool isStatic = true;
	}
}
