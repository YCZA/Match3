using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.EventSystems
{
	// Token: 0x02000B05 RID: 2821
	public class PinchEventData : BaseEventData
	{
		// Token: 0x060042A0 RID: 17056 RVA: 0x00155C02 File Offset: 0x00154002
		public PinchEventData(EventSystem ES, float d = 0f) : base(ES)
		{
			this.pinchDelta = d;
		}

		// Token: 0x04006B83 RID: 27523
		public PointerEventData[] data = new PointerEventData[2];

		// Token: 0x04006B84 RID: 27524
		public float pinchDelta;
	}
}
