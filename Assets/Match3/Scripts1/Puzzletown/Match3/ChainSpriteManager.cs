namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000697 RID: 1687
	public class ChainSpriteManager : ACountSpriteManager
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002A20 RID: 10784 RVA: 0x000C10F0 File Offset: 0x000BF4F0
		public override int MaxCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x000C10F3 File Offset: 0x000BF4F3
		public override string Prefix
		{
			get
			{
				return "chain_{0}hp";
			}
		}
	}
}
