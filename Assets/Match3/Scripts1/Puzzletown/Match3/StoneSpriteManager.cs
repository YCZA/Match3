namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006CD RID: 1741
	public class StoneSpriteManager : ACountSpriteManager
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002B6F RID: 11119 RVA: 0x000C7540 File Offset: 0x000C5940
		public override int MaxCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002B70 RID: 11120 RVA: 0x000C7543 File Offset: 0x000C5943
		public override string Prefix
		{
			get
			{
				return "stone_{0}hp";
			}
		}
	}
}
