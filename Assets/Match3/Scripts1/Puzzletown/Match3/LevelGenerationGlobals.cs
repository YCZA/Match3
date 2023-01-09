using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000565 RID: 1381
	public class LevelGenerationGlobals : MonoBehaviour
	{
		// Token: 0x04004FB6 RID: 20406
		private const int DROP_ITEM_INITIAL_SPAWN_RATIO_MIN = 1;

		// Token: 0x04004FB7 RID: 20407
		private const int DROP_ITEM_INITIAL_SPAWN_RATIO_MAX = 3;

		// Token: 0x04004FB8 RID: 20408
		private const int STACKED_GEM_INITIAL_AMOUNT_MIN = 1;

		// Token: 0x04004FB9 RID: 20409
		private const int STACKED_GEM_INITIAL_AMOUNT_MAX = 3;

		// Token: 0x04004FBA RID: 20410
		private const int STACKED_GEM_INITIAL_SPAWN_RATIO_MIN = 1;

		// Token: 0x04004FBB RID: 20411
		private const int STACKED_GEM_INITIAL_SPAWN_RATIO_MAX = 5;

		// Token: 0x04004FBC RID: 20412
		private const int STACKED_GEM_INITIAL_SPAWN_MIN = 0;

		// Token: 0x04004FBD RID: 20413
		private const int STACKED_GEM_INITIAL_SPAWN_MAX_MIN = 1;

		// Token: 0x04004FBE RID: 20414
		private const int STACKED_GEM_INITIAL_SPAWN_MAX_MAX = 2;

		// Token: 0x04004FBF RID: 20415
		public int DropItemInitSpawnRatioMin = 1;

		// Token: 0x04004FC0 RID: 20416
		public int DropItemInitSpawnRatioMax = 3;

		// Token: 0x04004FC1 RID: 20417
		public int StackedGemInitAmountMin = 1;

		// Token: 0x04004FC2 RID: 20418
		public int StackedGemInitAmountMax = 3;

		// Token: 0x04004FC3 RID: 20419
		public int StackedGemInitSpawnRatioMin = 1;

		// Token: 0x04004FC4 RID: 20420
		public int StackedGemInitSpawnRatioMax = 5;

		// Token: 0x04004FC5 RID: 20421
		public int StackedGemInitSpawnMin;

		// Token: 0x04004FC6 RID: 20422
		public int StackedGemInitSpawnMaxMin = 1;

		// Token: 0x04004FC7 RID: 20423
		public int StackedGemInitSpawnMaxMax = 2;

		// Token: 0x04004FC8 RID: 20424
		public CannonballInitialSettings cannonball;
	}
}
