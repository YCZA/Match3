namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A07 RID: 2567
	public class QuestTasksDataSource : ArrayDataSource<QuestTaskProgressView, QuestViewCurrent.TaskProgressViewData>
	{
		// Token: 0x06003DCE RID: 15822 RVA: 0x001392B8 File Offset: 0x001376B8
		public override int GetReusableIdForIndex(int index)
		{
			QuestViewCurrent.TaskProgressViewData dataForIndex = this.GetDataForIndex(index);
			if (dataForIndex.IsCollected)
			{
				return 3;
			}
			if (dataForIndex.isComplete)
			{
				return 2;
			}
			return 1;
		}
	}
}
