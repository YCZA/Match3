using Match3.Scripts1.Shared.UI;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200083D RID: 2109
namespace Match3.Scripts1
{
	public class PopupLivesPurchasedRoot : APurchasedPopupRoot
	{
		// Token: 0x06003460 RID: 13408 RVA: 0x000FA5F8 File Offset: 0x000F89F8
		private void Start()
		{
			Button[] componentsInChildren = base.GetComponentsInChildren<Button>(true);
			foreach (Button button in componentsInChildren)
			{
				button.onClick.AddListener(new UnityAction(base.Destroy));
			}
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x000FA640 File Offset: 0x000F8A40
		protected override void Go()
		{
			base.Go();
			PlaceOnTop componentInChildren = base.GetComponentInChildren<PlaceOnTop>();
			componentInChildren.UpdateOrder(componentInChildren.layer);
			base.GetComponentInChildren<TownResourcePanelLoader>().UpdateOrder();
		}
	}
}
