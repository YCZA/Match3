namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200070C RID: 1804
	public class M3_ObjectivesDoneDataSource : ArrayDataSource<M3_ObjectiveDoneView, MaterialAmount>
	{
		// Token: 0x06002CC1 RID: 11457 RVA: 0x000CF85C File Offset: 0x000CDC5C
		public override int GetReusableIdForIndex(int index)
		{
			M3_ObjectiveDoneView.State result;
			if (this.GetDataForIndex(index).amount <= 0)
			{
				result = M3_ObjectiveDoneView.State.Succeeded;
			}
			else
			{
				result = M3_ObjectiveDoneView.State.Failed;
			}
			return (int)result;
		}

		// Token: 0x04005638 RID: 22072
		public MaterialAmount[] objectivesNeeded;
	}
}
