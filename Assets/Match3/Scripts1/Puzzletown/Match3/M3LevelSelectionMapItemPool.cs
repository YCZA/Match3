using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000701 RID: 1793
	public class M3LevelSelectionMapItemPool : AListItemPool<LevelMapActiveView, M3_LevelMapItemState>
	{
		// Token: 0x06002C6D RID: 11373 RVA: 0x000CCAFC File Offset: 0x000CAEFC
		protected override LevelMapActiveView FindPrototypeCell(M3_LevelMapItemState state)
		{
			return Array.Find<LevelMapActiveView>(this.prototypeCells, (LevelMapActiveView cell) => cell.CheckMatch(state));
		}

		// Token: 0x040055B4 RID: 21940
		public LevelMapActiveView[] prototypeCells;
	}
}
