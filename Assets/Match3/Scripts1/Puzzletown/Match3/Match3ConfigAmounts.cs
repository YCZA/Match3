using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200061C RID: 1564
	public static class Match3ConfigAmounts
	{
		// Token: 0x04005227 RID: 21031
		public const int STACKED_GEM_SMALL = 3;

		// Token: 0x04005228 RID: 21032
		public const int STACKED_GEM_MEDIUM = 5;

		// Token: 0x04005229 RID: 21033
		public const int STACKED_GEM_BIG = 10;

		// Token: 0x0400522A RID: 21034
		public const int CANNON_PRE_CHARGE_SMALL = 0;

		// Token: 0x0400522B RID: 21035
		public const int CANNON_PRE_CHARGE_MEDIUM = 15;

		// Token: 0x0400522C RID: 21036
		public const int CANNON_PRE_CHARGE_BIG = 30;

		// Token: 0x0400522D RID: 21037
		public const int CANNON_MAX_CHARGE = 45;

		// Token: 0x0400522E RID: 21038
		public static Dictionary<GemType, int> typeValueMap = new Dictionary<GemType, int>
		{
			{
				GemType.StackedGemSmall,
				3
			},
			{
				GemType.StackedGemMedium,
				5
			},
			{
				GemType.StackedGemBig,
				10
			}
		};

		// Token: 0x0400522F RID: 21039
		public static Dictionary<GemModifier, int> modifierValueMap = new Dictionary<GemModifier, int>
		{
			{
				GemModifier.PreChargedLittle,
				0
			},
			{
				GemModifier.PreChargedMedium,
				15
			},
			{
				GemModifier.PreChargedMuch,
				30
			}
		};
	}
}
