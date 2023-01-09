using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D2 RID: 1490
	public struct ColorWheelMatch : IMatchResult
	{
		// Token: 0x060026B7 RID: 9911 RVA: 0x000AD5CC File Offset: 0x000AB9CC
		public ColorWheelMatch(GemColor gemColor, ColorWheelModel colorWheelModel, IntVector2 gridPosition, IntVector2 createdFrom)
		{
			this.gridPosition = gridPosition;
			this.createdFrom = createdFrom;
			this.gemColor = gemColor;
		}

		// Token: 0x0400517B RID: 20859
		public IntVector2 gridPosition;

		// Token: 0x0400517C RID: 20860
		public IntVector2 createdFrom;

		// Token: 0x0400517D RID: 20861
		public GemColor gemColor;
	}
}
