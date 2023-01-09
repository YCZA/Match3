namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005B0 RID: 1456
	public static class GemExtensions
	{
		// Token: 0x06002613 RID: 9747 RVA: 0x000AA178 File Offset: 0x000A8578
		public static bool IsAnyBombGem(this Gem gem)
		{
			return gem.type == GemType.Bomb || gem.IsActivatedBombGem();
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000AA190 File Offset: 0x000A8590
		public static bool IsActivatedBombGem(this Gem gem)
		{
			return gem.type == GemType.ActivatedBomb || gem.type == GemType.ActivatedSuperBomb;
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000AA1AC File Offset: 0x000A85AC
		public static int SortByParameterDescending(Gem a, Gem b)
		{
			if (a.parameter > b.parameter)
			{
				return -1;
			}
			if (a.parameter < b.parameter)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x000AA1D9 File Offset: 0x000A85D9
		public static bool IsLineGem(this Gem gem)
		{
			return gem.type == GemType.LineVertical || gem.type == GemType.LineHorizontal;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000AA1F5 File Offset: 0x000A85F5
		public static bool BlocksLineGem(this Gem gem)
		{
			return gem.IsCannonball;
		}
	}
}
