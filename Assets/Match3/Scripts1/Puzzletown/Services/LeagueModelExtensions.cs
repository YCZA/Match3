using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200080B RID: 2059
	public static class LeagueModelExtensions
	{
		// Token: 0x060032DA RID: 13018 RVA: 0x000EF99C File Offset: 0x000EDD9C
		public static bool IsValid(this LeagueModel model)
		{
			return model != null && model.config != null && model.config.config != null && model.config.config.tournamentType != TournamentType.Undefined;
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x000EF9D8 File Offset: 0x000EDDD8
		public static DecoTrophyItemWon GetDecoTrophyWon(this LeagueModel model)
		{
			if (model.IsValid())
			{
				int playerPosition = model.GetPlayerPosition();
				if (playerPosition < 4)
				{
					switch (model.config.config.tournamentType)
					{
					case TournamentType.Bomb:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.BombBronze : DecoTrophyItemWon.BombSilver) : DecoTrophyItemWon.BombGold;
					case TournamentType.Butterfly:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.ButterflyBronze : DecoTrophyItemWon.ButterflySilver) : DecoTrophyItemWon.ButterflyGold;
					case TournamentType.Line:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.LineBronze : DecoTrophyItemWon.LineSilver) : DecoTrophyItemWon.LineGold;
					case TournamentType.Strawberry:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.StrawberryBronze : DecoTrophyItemWon.StrawberrySilver) : DecoTrophyItemWon.StrawberryGold;
					case TournamentType.Banana:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.BananaBronze : DecoTrophyItemWon.BananaSilver) : DecoTrophyItemWon.BananaGold;
					case TournamentType.Plum:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.PlumBronze : DecoTrophyItemWon.PlumSilver) : DecoTrophyItemWon.PlumGold;
					case TournamentType.Apple:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.AppleBronze : DecoTrophyItemWon.AppleSilver) : DecoTrophyItemWon.AppleGold;
					case TournamentType.Starfruit:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.StarfruitBronze : DecoTrophyItemWon.StarfruitSilver) : DecoTrophyItemWon.StarfruitGold;
					case TournamentType.Grape:
						return (playerPosition != 1) ? ((playerPosition != 2) ? DecoTrophyItemWon.GrapeBronze : DecoTrophyItemWon.GrapeSilver) : DecoTrophyItemWon.GrapeGold;
					}
				}
			}
			return DecoTrophyItemWon.None;
		}
	}
}
