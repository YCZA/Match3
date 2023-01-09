using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000878 RID: 2168
namespace Match3.Scripts1
{
	public class PopupSeasonalCurrencyCell : APopupCell<PopupSeasonalCurrencyCell.Data>, IEditorDescription
	{
		// Token: 0x0600355D RID: 13661 RVA: 0x00100349 File Offset: 0x000FE749
		public override bool CanPresent(PopupSeasonalCurrencyCell.Data data)
		{
			return true;
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x0010034C File Offset: 0x000FE74C
		public string GetEditorDescription()
		{
			return "Image";
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x00100353 File Offset: 0x000FE753
		public override void Show(PopupSeasonalCurrencyCell.Data data)
		{
			this.image.sprite = data.sprite;
			this.label.text = data.label;
		}

		// Token: 0x04005D3F RID: 23871
		public Image image;

		// Token: 0x04005D40 RID: 23872
		public TMP_Text label;

		// Token: 0x02000879 RID: 2169
		public class Data
		{
			// Token: 0x04005D41 RID: 23873
			public Sprite sprite;

			// Token: 0x04005D42 RID: 23874
			public string label;
		}
	}
}
