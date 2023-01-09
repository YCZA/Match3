using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000497 RID: 1175
	[Serializable]
	public class IslandAreaConfigs
	{
		// Token: 0x06002147 RID: 8519 RVA: 0x0008BD40 File Offset: 0x0008A140
		public int GlobalAreaToLocalArea(int globalArea)
		{
			return this.configs.First((IslandAreaConfig island) => island.global_area == globalArea).local_area;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0008BD78 File Offset: 0x0008A178
		public int LocalAreaToGlobalArea(int localArea, int islandId)
		{
			return this.configs.First((IslandAreaConfig config) => config.island_id == islandId && config.local_area == localArea).global_area;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0008BDB8 File Offset: 0x0008A1B8
		public int FirstGlobalArea(int islandId)
		{
			return this.configs.First((IslandAreaConfig config) => config.island_id == islandId).global_area;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x0008BDF0 File Offset: 0x0008A1F0
		public IEnumerable<int> GlobalAreasOnIsland(int islandId)
		{
			return from config in this.configs
			where config.island_id == islandId
			select config.global_area;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0008BE44 File Offset: 0x0008A244
		public int NumAreasOnIsland(int islandId)
		{
			return this.configs.Count((IslandAreaConfig config) => config.island_id == islandId);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x0008BE78 File Offset: 0x0008A278
		public int CloudLimitOnIsland(int islandId, int localAreaUnlocked)
		{
			int num = this.NumAreasOnIsland(islandId);
			int num2 = this.NumRubbleCoveredAreasOnIsland(islandId);
			bool flag = num2 < localAreaUnlocked;
			return (!flag) ? num2 : num;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0008BEA8 File Offset: 0x0008A2A8
		public int NumRubbleCoveredAreasOnIsland(int islandId)
		{
			return this.configs.Count((IslandAreaConfig config) => config.island_id == islandId && config.rubble_id != 0);
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0008BEDC File Offset: 0x0008A2DC
		public bool CouldHaveRubble(int globalArea)
		{
			return this.configs.First((IslandAreaConfig config) => config.global_area == globalArea).rubble_id != 0;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x0008BF18 File Offset: 0x0008A318
		public int GetRubbleIdForOrder(int rubbleOrder)
		{
			return this.configs.First((IslandAreaConfig config) => config.rubble_order == rubbleOrder).rubble_id;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0008BF50 File Offset: 0x0008A350
		public int IslandForArea(int globalArea)
		{
			return this.configs.First((IslandAreaConfig config) => config.global_area == globalArea).island_id;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x0008BF88 File Offset: 0x0008A388
		public int ClampGlobalToLocalArea(int islandId, int unlockedGlobalArea)
		{
			IslandAreaConfig islandAreaConfig = this.configs.LastOrDefault((IslandAreaConfig config) => config.island_id == islandId && config.global_area <= unlockedGlobalArea);
			return (islandAreaConfig == null) ? 0 : islandAreaConfig.local_area;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x0008BFD4 File Offset: 0x0008A3D4
		public bool HasOneChapterPerArea(int globalArea)
		{
			return this.configs.First((IslandAreaConfig config) => config.global_area == globalArea).chapter.Length == 1;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0008C010 File Offset: 0x0008A410
		public int FirstAreaCoveredByThisRubble(int rubbleId)
		{
			return this.configs.First((IslandAreaConfig config) => config.rubble_id == rubbleId).global_area;
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0008C048 File Offset: 0x0008A448
		public int LastAreaCoveredByThisRubble(int rubbleId)
		{
			return this.configs.Last((IslandAreaConfig config) => config.rubble_id == rubbleId).global_area;
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x0008C080 File Offset: 0x0008A480
		public int FirstChapterOfArea(int globalArea)
		{
			return this.configs.First((IslandAreaConfig config) => config.global_area == globalArea).chapter[0];
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0008C0B8 File Offset: 0x0008A4B8
		public int GetAreaForChapter(int chapter)
		{
			int result = 1;
			if (chapter == 9999)
			{
				result = 9999;
			}
			else if (chapter != 0)
			{
				result = this.configs.First((IslandAreaConfig island) => island.chapter.Contains(chapter)).global_area;
			}
			return result;
		}

		// Token: 0x04004C6B RID: 19563
		public IslandAreaConfig[] configs;

		// Token: 0x04004C6C RID: 19564
		private const int DEFAULT_AREA_FOR_CHAPTER = 1;

		// Token: 0x04004C6D RID: 19565
		private const int NO_SPECIFIC_CHAPTER = 0;

		// Token: 0x04004C6E RID: 19566
		private const int INVALID_CHAPTER = 9999;
	}
}
