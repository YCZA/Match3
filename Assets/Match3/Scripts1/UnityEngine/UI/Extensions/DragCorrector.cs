using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C1B RID: 3099
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("UI/Extensions/DragCorrector")]
	public class DragCorrector : MonoBehaviour
	{
		// Token: 0x0600492D RID: 18733 RVA: 0x00176B9C File Offset: 0x00174F9C
		private void Start()
		{
			this.dragTH = this.baseTH * (int)Screen.dpi / this.basePPI;
			EventSystem component = base.GetComponent<EventSystem>();
			if (component)
			{
				component.pixelDragThreshold = this.dragTH;
			}
		}

		// Token: 0x04006FB1 RID: 28593
		public int baseTH = 6;

		// Token: 0x04006FB2 RID: 28594
		public int basePPI = 210;

		// Token: 0x04006FB3 RID: 28595
		public int dragTH;
	}
}
