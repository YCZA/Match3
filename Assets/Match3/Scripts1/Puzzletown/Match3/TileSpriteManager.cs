namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006CE RID: 1742
	public class TileSpriteManager : ACountSpriteManager
	{
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002B72 RID: 11122 RVA: 0x000C7552 File Offset: 0x000C5952
		public override int MaxCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x000C7555 File Offset: 0x000C5955
		public override string Prefix
		{
			get
			{
				return "tile_{0}hp";
			}
		}
	}
}
