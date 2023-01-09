namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A0F RID: 2575
	public class QuestViewOutOfContent : QuestView
	{
		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0013A0D5 File Offset: 0x001384D5
		public override QuestView.CellType questStatus
		{
			get
			{
				return QuestView.CellType.endOfContent;
			}
		}
	}
}
