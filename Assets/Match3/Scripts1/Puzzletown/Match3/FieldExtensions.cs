namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000581 RID: 1409
	public static class FieldExtensions
	{
		// Token: 0x060024E6 RID: 9446 RVA: 0x000A4F90 File Offset: 0x000A3390
		public static bool CanBeTargetedByBoost(this Field field, Boosts boostType)
		{
			switch (boostType)
			{
			case Boosts.boost_hammer:
			case Boosts.boost_star:
				return field.gem.IsMatchable || field.HasCrates || (field.IsBlocked && !field.IsColorWheel) || field.numChains > 0 || field.gem.IsCovered || field.gem.BlocksLineGem() || field.isGrowingWindow;
			case Boosts.boost_rainbow:
				return field.HasGem && !field.GemBlocked && field.gem.IsMatchable;
			default:
				return false;
			}
		}
	}
}
