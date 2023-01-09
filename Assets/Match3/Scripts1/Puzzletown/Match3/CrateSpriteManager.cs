namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006A3 RID: 1699
	public class CrateSpriteManager : ACountSpriteManager
	{
		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002A5C RID: 10844 RVA: 0x000C1760 File Offset: 0x000BFB60
		public override int MaxCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x000C1763 File Offset: 0x000BFB63
		public override string Prefix
		{
			get
			{
				return "crate_{0}hp";
			}
		}
	}
}
