namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006EC RID: 1772
	public class M3_LevelSelectionTiersDataSource : ArrayDataSource<M3_LevelSelectionTierView, M3_LevelSelectionTier>
	{
		// Token: 0x06002C17 RID: 11287 RVA: 0x000CB218 File Offset: 0x000C9618
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.GetDataForIndex(index).state;
		}
	}
}
