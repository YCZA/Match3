namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200082A RID: 2090
	public class VideoAdHelper
	{
		// Token: 0x060033EF RID: 13295 RVA: 0x000F826C File Offset: 0x000F666C
		public static string GetAdPlacementAsString(AdPlacement placement)
		{
			switch (placement)
			{
			case AdPlacement.AdWheel:
				return "wheel";
			case AdPlacement.Challenges:
				return "paw_bonus";
			case AdPlacement.DailyGift:
				return "daily_gift";
			case AdPlacement.LivesShop:
				return "out_of_lives";
			case AdPlacement.Challenges_V2:
				return "challenges_get_now";
			default:
				return "unspecified";
			}
		}
	}
}
