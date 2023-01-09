namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A10 RID: 2576
	public class QuestViewPending : QuestView
	{
		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x0013A0E0 File Offset: 0x001384E0
		public override QuestView.CellType questStatus
		{
			get
			{
				return QuestView.CellType.pending;
			}
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x0013A0E4 File Offset: 0x001384E4
		public override void Show(QuestUIData data)
		{
			base.Show(data);
			if (base.root == null)
			{
				return;
			}
			string text = base.root.loc.GetText(base.GetTitleKey(data.data), new LocaParam[0]);
			string text2 = base.root.loc.GetText(base.GetDescriptionKey(data.data), new LocaParam[0]);
			PopupTextCell.SetText(this, TextType.Title, text);
			PopupTextCell.SetText(this, TextType.Content, text2);
		}
	}
}
