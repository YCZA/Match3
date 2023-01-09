using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000500 RID: 1280
	public class ClimberBrush : ColorBrush
	{
		// Token: 0x0600232E RID: 9006 RVA: 0x0009C054 File Offset: 0x0009A454
		public ClimberBrush(Sprite sprite, ABrush removal) : base(GemColor.Climber, sprite, removal)
		{
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
		}
	}
}
