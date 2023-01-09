using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000502 RID: 1282
	public class TypeBrush : ABrush
	{
		// Token: 0x06002330 RID: 9008 RVA: 0x0009C134 File Offset: 0x0009A534
		public TypeBrush(GemType type, Sprite sprite, ABrush removal = null) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RandomColorBrushComponent());
			this.brushComponents.Add(new TypeBrushComponent(type));
		}
	}
}
