using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D5 RID: 1493
	public struct DroppableCollected : IScoreCollectedResult, IMatchResult
	{
		// Token: 0x060026BC RID: 9916 RVA: 0x000AD632 File Offset: 0x000ABA32
		public DroppableCollected(Gem gem)
		{
			this.position = gem.position;
			this.gem = gem;
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060026BD RID: 9917 RVA: 0x000AD648 File Offset: 0x000ABA48
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x000AD650 File Offset: 0x000ABA50
		public string Type
		{
			get
			{
				return "droppable";
			}
		}

		// Token: 0x04005184 RID: 20868
		private readonly IntVector2 position;

		// Token: 0x04005185 RID: 20869
		public Gem gem;
	}
}
