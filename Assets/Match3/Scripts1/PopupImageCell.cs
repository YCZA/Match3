using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000871 RID: 2161
namespace Match3.Scripts1
{
	public class PopupImageCell : APopupCell<Sprite>, IEditorDescription
	{
		// Token: 0x06003546 RID: 13638 RVA: 0x001001C9 File Offset: 0x000FE5C9
		public override bool CanPresent(Sprite image)
		{
			return true;
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x001001CC File Offset: 0x000FE5CC
		public string GetEditorDescription()
		{
			return "Image";
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x001001D3 File Offset: 0x000FE5D3
		public override void Show(Sprite data)
		{
			this.image.sprite = data;
		}

		// Token: 0x04005D38 RID: 23864
		public Image image;
	}
}
