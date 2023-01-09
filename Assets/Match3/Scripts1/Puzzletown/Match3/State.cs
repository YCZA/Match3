namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006ED RID: 1773
	public struct M3_LevelSelectionTier
	{
		// Token: 0x04005542 RID: 21826
		public AreaConfig.Tier tier;

		// Token: 0x04005543 RID: 21827
		public M3_LevelSelectionTier.State state;

		// Token: 0x020006EE RID: 1774
		public enum State
		{
			// Token: 0x04005545 RID: 21829
			Pending,
			// Token: 0x04005546 RID: 21830
			Completed,
			// Token: 0x04005547 RID: 21831
			Active
		}
	}
}
