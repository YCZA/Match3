using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000582 RID: 1410
	public class BoostFactory
	{
		// Token: 0x060024E7 RID: 9447 RVA: 0x000A5043 File Offset: 0x000A3443
		public BoostFactory(Fields fields, bool isWaterLevel)
		{
			this.fields = fields;
			this.isWaterLevel = isWaterLevel;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000A505C File Offset: 0x000A345C
		public IBoost GetBoost(Boosts type, IntVector2 position)
		{
			switch (type)
			{
			case Boosts.boost_hammer:
				return new BoostHammer(this.fields, position, this.isWaterLevel);
			case Boosts.boost_star:
				return new BoostStar(this.fields, position);
			case Boosts.boost_rainbow:
				return new BoostRainbow(this.fields, position);
			case Boosts.boost_lh_creating_line_horizontal:
				return new BoostLineGemHorizontalCreation(this.fields, position);
			case Boosts.boost_lh_creating_line_vertical:
				return new BoostLineGemVerticalCreation(this.fields, position);
			case Boosts.boost_lh_exploding_rainbow:
				return new BoostLastHurrayRainbow(this.fields, position);
			case Boosts.boost_lh_exploding_supergem:
				return new BoostLastHurraySuperGem(this.fields, position);
			default:
				WoogaDebug.LogWarning(new object[]
				{
					"boost not implemented",
					type
				});
				return null;
			}
		}

		// Token: 0x0400506A RID: 20586
		private readonly Fields fields;

		// Token: 0x0400506B RID: 20587
		private readonly bool isWaterLevel;
	}
}
