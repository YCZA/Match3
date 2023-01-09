using UnityEngine.EventSystems;

// Token: 0x020008FE RID: 2302
namespace Match3.Scripts1
{
	public interface ITapAndHoldHandler : IEventSystemHandler
	{
		// Token: 0x0600380C RID: 14348
		void OnTapAndHold(PointerEventData evt);
	}
}
