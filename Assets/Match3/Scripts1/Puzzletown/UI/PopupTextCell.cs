using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200087E RID: 2174
	public class PopupTextCell : APopupCell<TextData>, IEditorDescription, ICategorised<TextType>
	{
		// Token: 0x06003569 RID: 13673 RVA: 0x0010041B File Offset: 0x000FE81B
		public override void Show(TextData data)
		{
			Debug.LogWarning(gameObject.name);
			this.label.text = data.text;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x0010042F File Offset: 0x000FE82F
		public override bool CanPresent(TextData data)
		{
			return data.type == this.type;
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x00100440 File Offset: 0x000FE840
		public string GetEditorDescription()
		{
			return this.type.ToString();
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x00100453 File Offset: 0x000FE853
		public TextType GetCategory()
		{
			return this.type;
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x0010045C File Offset: 0x000FE85C
		public static void SetText(Component self, TextType type, string text)
		{
			self.ExecuteOnChild(type, delegate(PopupTextCell t)
			{
				t.label.text = text;
			});
		}

		// Token: 0x04005D4A RID: 23882
		public Text label;

		// Token: 0x04005D4B RID: 23883
		public TextType type;
	}
}
