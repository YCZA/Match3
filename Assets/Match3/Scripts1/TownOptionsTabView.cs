using Match3.Scripts1.UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009EE RID: 2542
namespace Match3.Scripts1
{
	public class TownOptionsTabView : AUiTabView<TownOptionsTabData>, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06003D63 RID: 15715 RVA: 0x00136494 File Offset: 0x00134894
		public override void Show(TownOptionsTabData tab)
		{
			base.Show(tab);
			this.image.sprite = this.imageSource.GetSimilar(tab.data.ToString());
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x001364C4 File Offset: 0x001348C4
		public new void OnPointerDown(PointerEventData evt)
		{
			this.HandleOnParent(this._data.data);
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x001364D7 File Offset: 0x001348D7
		public new void OnPointerUp(PointerEventData evt)
		{
		}

		// Token: 0x0400663C RID: 26172
		public Image image;

		// Token: 0x0400663D RID: 26173
		public SpriteManager imageSource;
	}
}
