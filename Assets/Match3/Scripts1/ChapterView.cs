using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;

// Token: 0x020006DF RID: 1759
namespace Match3.Scripts1
{
	public class ChapterView : ATableViewReusableCell<ChapterState>, IDataView<ChapterInfo>, IHandler<PopupOperation>
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000C8C59 File Offset: 0x000C7059
		// (set) Token: 0x06002BBC RID: 11196 RVA: 0x000C8C61 File Offset: 0x000C7061
		public ChapterInfo data { get; private set; }

		// Token: 0x06002BBD RID: 11197 RVA: 0x000C8C6A File Offset: 0x000C706A
		public void Show(ChapterInfo chapter)
		{
			this.data = chapter;
			if (this.label)
			{
				this.label.text = chapter.id.ToString();
			}
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000C8C9F File Offset: 0x000C709F
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.OK)
			{
				this.HandleOnParent(this.data);
			}
		}

		// Token: 0x040054D7 RID: 21719
		public TMP_Text label;
	}
}
