using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3.Scoring;

namespace Match3.Scripts1.PTTracking
{
	// Token: 0x0200081A RID: 2074
	public static class Match3Score_Tracking
	{
		// Token: 0x06003358 RID: 13144 RVA: 0x000F3EE4 File Offset: 0x000F22E4
		public static void PopulateTracking(this Match3Score score, Dictionary<string, object> data)
		{
			data["bs"] = score.scorePreHurrah;
			data["fs"] = score.scorePostHurrah;
			data["lga"] = score.lineGemsActivated;
			data["fa"] = score.fishActivated;
			data["rga"] = score.rainbowsActivated;
			data["ba"] = score.bombsActivated;
			data["pre_rainbow"] = score.preGameRainbowsUsed;
			data["pre_bombline"] = score.preGameBombLinegemUsed;
			data["pre_fish"] = score.preGameDoubleFishUsed;
			data["in_hammer"] = score.ingameHammerUsed;
			data["in_star"] = score.ingameStarUsed;
			data["in_rainbow"] = score.ingameRainbowUsed;
			data["post_moves"] = score.postMoves;
			data["mc"] = score.MovesTaken;
			data["reshuffles"] = score.reshuffles;
			data["cc"] = ((!score.AllCollected.Contains("coins")) ? 0 : score.AllCollected["coins"]);
			data["ca"] = score.levelCascades;
		}
	}
}
