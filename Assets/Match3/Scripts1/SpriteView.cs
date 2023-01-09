using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200084B RID: 2123
namespace Match3.Scripts1
{
	public class SpriteView : ATableViewReusableCell, IDataView<Sprite>
	{
		// Token: 0x0600348E RID: 13454 RVA: 0x000FBA12 File Offset: 0x000F9E12
		public void Show(Sprite sprite)
		{
			if (this.image && sprite)
			{
				this.image.sprite = sprite;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x000FBA3B File Offset: 0x000F9E3B
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04005C8F RID: 23695
		public Image image;
	}
}
