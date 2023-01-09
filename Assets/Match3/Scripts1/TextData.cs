using Match3.Scripts1.Puzzletown.UI;

namespace Match3.Scripts1
{
	// Token: 0x0200087D RID: 2173
	public struct TextData
	{
		// Token: 0x06003565 RID: 13669 RVA: 0x001003F1 File Offset: 0x000FE7F1
		private TextData(string text, TextType type)
		{
			this.text = text;
			this.type = type;
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x00100401 File Offset: 0x000FE801
		public static TextData Title(string text)
		{
			return new TextData(text, TextType.Title);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0010040A File Offset: 0x000FE80A
		public static TextData Content(string text)
		{
			return new TextData(text, TextType.Content);
		}

		// Token: 0x04005D48 RID: 23880
		public string text;

		// Token: 0x04005D49 RID: 23881
		public TextType type;
	}
}
