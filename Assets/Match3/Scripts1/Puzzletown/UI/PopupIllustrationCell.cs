using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000870 RID: 2160
	public class PopupIllustrationCell : APopupCell<IllustrationType>, IEditorDescription, ICategorised<IllustrationType>
	{
		// Token: 0x06003541 RID: 13633 RVA: 0x00100199 File Offset: 0x000FE599
		public override void Show(IllustrationType data)
		{
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0010019B File Offset: 0x000FE59B
		public override bool CanPresent(IllustrationType data)
		{
			return data == this.type;
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x001001A6 File Offset: 0x000FE5A6
		public string GetEditorDescription()
		{
			return this.type.ToString();
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x001001B9 File Offset: 0x000FE5B9
		public IllustrationType GetCategory()
		{
			return this.type;
		}

		// Token: 0x04005D37 RID: 23863
		public IllustrationType type;
	}
}
