using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000507 RID: 1287
	public class ClimberGemBrush : ABrush
	{
		// Token: 0x06002337 RID: 9015 RVA: 0x0009C364 File Offset: 0x0009A764
		public ClimberGemBrush(GemColor color, GemType type, Sprite sprite, ABrush removal = null) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new ColorBrushComponent(color));
			this.brushComponents.Add(new TypeBrushComponent(type));
		}
	}
}
