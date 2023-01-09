using Match3.Scripts1.UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020008D0 RID: 2256
	public class VillagerTapBox : MonoBehaviour, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060036EC RID: 14060 RVA: 0x0010C4B8 File Offset: 0x0010A8B8
		public void OnPointerUp(PointerEventData evt)
		{
			VillagerView componentInParent = base.GetComponentInParent<VillagerView>();
			this.HandleOnParent(componentInParent);
		}
	}
}
