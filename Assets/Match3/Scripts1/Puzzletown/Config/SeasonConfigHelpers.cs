namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x0200049F RID: 1183
	public static class SeasonConfigHelpers
	{
		// Token: 0x06002175 RID: 8565 RVA: 0x0008C872 File Offset: 0x0008AC72
		public static string BundleNameForSeason(string season)
		{
			return "buildings_seasonal_" + season;
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x0008C87F File Offset: 0x0008AC7F
		public static string TMProIconName(string season)
		{
			return string.Format("<sprite=\"ui_icon_season_currency_{0}\" index=0>", season);
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x0008C88C File Offset: 0x0008AC8C
		public static string FxTextureForSeason(string season)
		{
			return string.Format("Assets/Puzzletown/Town/Art/Buildings/seasonal_{0}/ui_particle_screeneffect_{0}.png", season);
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x0008C899 File Offset: 0x0008AC99
		public static string SpriteManagerForSeason(string season, bool isSeasonalsV3)
		{
			if (isSeasonalsV3)
			{
				return string.Format("Assets/Puzzletown/Town/Art/Buildings/seasonal_{0}/SeasonSpriteManager_{0}_v3.prefab", season);
			}
			return string.Format("Assets/Puzzletown/Town/Art/Buildings/seasonal_{0}/SeasonSpriteManager_{0}.prefab", season);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x0008C8B8 File Offset: 0x0008ACB8
		public static string IconNameForSeason(string season)
		{
			return string.Format("Assets/Puzzletown/Town/Art/Buildings/seasonal_{0}/ui_tab_illustration_{0}.png", season);
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x0008C8C5 File Offset: 0x0008ACC5
		public static string PromoIllustrationForSeason(string season)
		{
			return string.Format("Assets/Puzzletown/Town/Art/Buildings/seasonal_{0}/promo_illustration_{0}.png", season);
		}
	}
}
