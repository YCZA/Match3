using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200051F RID: 1311
	public class RemoveFieldBrush : ResetFieldBrush
	{
		// Token: 0x06002359 RID: 9049 RVA: 0x0009CFBB File Offset: 0x0009B3BB
		public RemoveFieldBrush(Sprite sprite) : base(sprite)
		{
			this.brushComponents.Add(new FieldBrushComponent(false));
		}
	}
}
