

// Token: 0x0200058F RID: 1423
namespace Match3.Scripts1
{
	public class Crate : AFieldModifier
	{
		// Token: 0x0600252F RID: 9519 RVA: 0x000A5A1F File Offset: 0x000A3E1F
		public Crate(int count) : base(count)
		{
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000A5A28 File Offset: 0x000A3E28
		public static int GetIndex(GemColor color, int hp)
		{
			return (int)color * (int)GemColor.Yellow + hp;
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000A5A2F File Offset: 0x000A3E2F
		public static int GetHp(int indexCrate)
		{
			return indexCrate % 4;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000A5A34 File Offset: 0x000A3E34
		public static GemColor GetColor(int indexCrate)
		{
			return (GemColor)(indexCrate / 4);
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000A5A3C File Offset: 0x000A3E3C
		public static bool MatchesColor(int indexCrate, GemColor color)
		{
			GemColor color2 = Crate.GetColor(indexCrate);
			return color2 == GemColor.Undefined || color2 == color;
		}

		// Token: 0x04005095 RID: 20629
		public const int MAX_HP_COUNT = 3;

		// Token: 0x04005096 RID: 20630
		public const int COUNT_DIFFERENT_HP = 4;
	}
}
