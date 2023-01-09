

// Token: 0x02000A44 RID: 2628
namespace Match3.Scripts1
{
	public static class DecoTrophyItemWonExtensions
	{
		// Token: 0x06003F00 RID: 16128 RVA: 0x00141B74 File Offset: 0x0013FF74
		public static string AsString(this DecoTrophyItemWon item)
		{
			switch (item)
			{
				case DecoTrophyItemWon.BombGold:
					return "bomb_gold";
				case DecoTrophyItemWon.BombSilver:
					return "bomb_silver";
				case DecoTrophyItemWon.BombBronze:
					return "bomb_bronze";
				case DecoTrophyItemWon.ButterflyGold:
					return "butterfly_gold";
				case DecoTrophyItemWon.ButterflySilver:
					return "butterfly_silver";
				case DecoTrophyItemWon.ButterflyBronze:
					return "butterfly_bronze";
				case DecoTrophyItemWon.LineGold:
					return "line_gold";
				case DecoTrophyItemWon.LineSilver:
					return "line_silver";
				case DecoTrophyItemWon.LineBronze:
					return "line_bronze";
				case DecoTrophyItemWon.StrawberryGold:
					return "strawberry_gold";
				case DecoTrophyItemWon.StrawberrySilver:
					return "strawberry_silver";
				case DecoTrophyItemWon.StrawberryBronze:
					return "strawberry_bronze";
				case DecoTrophyItemWon.BananaGold:
					return "banana_gold";
				case DecoTrophyItemWon.BananaSilver:
					return "banana_silver";
				case DecoTrophyItemWon.BananaBronze:
					return "banana_bronze";
				case DecoTrophyItemWon.PlumGold:
					return "plum_gold";
				case DecoTrophyItemWon.PlumSilver:
					return "plum_silver";
				case DecoTrophyItemWon.PlumBronze:
					return "plum_bronze";
				case DecoTrophyItemWon.AppleGold:
					return "apple_gold";
				case DecoTrophyItemWon.AppleSilver:
					return "apple_silver";
				case DecoTrophyItemWon.AppleBronze:
					return "apple_bronze";
				case DecoTrophyItemWon.StarfruitGold:
					return "starfruit_gold";
				case DecoTrophyItemWon.StarfruitSilver:
					return "starfruit_silver";
				case DecoTrophyItemWon.StarfruitBronze:
					return "starfruit_bronze";
				case DecoTrophyItemWon.GrapeGold:
					return "grape_gold";
				case DecoTrophyItemWon.GrapeSilver:
					return "grape_silver";
				case DecoTrophyItemWon.GrapeBronze:
					return "grape_bronze";
				default:
					return string.Empty;
			}
		}
	}
}
