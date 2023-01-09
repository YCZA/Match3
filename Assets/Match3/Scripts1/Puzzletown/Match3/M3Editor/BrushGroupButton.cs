using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000557 RID: 1367
	[RequireComponent(typeof(Button), typeof(Image))]
	public class BrushGroupButton : ATableViewReusableCell
	{
		// Token: 0x060023EA RID: 9194 RVA: 0x0009F1F1 File Offset: 0x0009D5F1
		private void Awake()
		{
			this.button = base.GetComponent<Button>();
			this.image = base.transform.Find("BrushImage").transform.GetComponent<Image>();
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x0009F21F File Offset: 0x0009D61F
		public override int reusableId
		{
			get
			{
				return 1001;
			}
		}

		// Token: 0x04004F62 RID: 20322
		public const int BRUSH_BUTTON_ID = 1001;

		// Token: 0x04004F63 RID: 20323
		public Button button;

		// Token: 0x04004F64 RID: 20324
		public BrushGroup brushGroup;

		// Token: 0x04004F65 RID: 20325
		public Image image;
	}
}
