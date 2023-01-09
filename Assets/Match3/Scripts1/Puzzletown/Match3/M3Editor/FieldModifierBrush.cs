using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000508 RID: 1288
	public class FieldModifierBrush : ABrush
	{
		// Token: 0x06002338 RID: 9016 RVA: 0x0009C3BE File Offset: 0x0009A7BE
		protected FieldModifierBrush(Sprite sprite, ABrush removal = null) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
		}
	}
}
