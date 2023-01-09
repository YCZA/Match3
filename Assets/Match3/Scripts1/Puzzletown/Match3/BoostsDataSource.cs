namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200071E RID: 1822
	public class BoostsDataSource : ArrayDataSource<BoostView, BoostViewData>
	{
		// Token: 0x06002D13 RID: 11539 RVA: 0x000D11CA File Offset: 0x000CF5CA
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.GetDataForIndex(index).state;
		}
	}
}
