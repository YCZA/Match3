using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.EventSystems
{
	// Token: 0x02000B06 RID: 2822
	public class RotateEventData : BaseEventData
	{
		// Token: 0x060042A1 RID: 17057 RVA: 0x00155C1E File Offset: 0x0015401E
		public RotateEventData(EventSystem ES, float d = 0f) : base(ES)
		{
			this.rotateDelta = d;
		}

		// Token: 0x04006B85 RID: 27525
		public PointerEventData[] data = new PointerEventData[2];

		// Token: 0x04006B86 RID: 27526
		public float rotateDelta;
	}
}
