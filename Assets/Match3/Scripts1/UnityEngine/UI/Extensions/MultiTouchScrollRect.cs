using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BA6 RID: 2982
	[AddComponentMenu("UI/Extensions/MultiTouchScrollRect")]
	public class MultiTouchScrollRect : ScrollRect
	{
		// Token: 0x060045DA RID: 17882 RVA: 0x001621E9 File Offset: 0x001605E9
		public override void OnBeginDrag(PointerEventData eventData)
		{
			this.pid = eventData.pointerId;
			base.OnBeginDrag(eventData);
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x001621FE File Offset: 0x001605FE
		public override void OnDrag(PointerEventData eventData)
		{
			if (this.pid == eventData.pointerId)
			{
				base.OnDrag(eventData);
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x00162218 File Offset: 0x00160618
		public override void OnEndDrag(PointerEventData eventData)
		{
			this.pid = -100;
			base.OnEndDrag(eventData);
		}

		// Token: 0x04006D5E RID: 27998
		private int pid = -100;
	}
}
