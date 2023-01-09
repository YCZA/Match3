using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000506 RID: 1286
	public class DirtBrush : ModifierBrush
	{
		// Token: 0x06002336 RID: 9014 RVA: 0x0009C337 File Offset: 0x0009A737
		public DirtBrush(GemModifier modifier, Sprite sprite, ABrush removal = null) : base(modifier, sprite, removal)
		{
			this.brushComponents.Add(new DirtBrushComponent());
			this.brushComponents.Add(new RemoveChameleonComponent());
		}
	}
}
