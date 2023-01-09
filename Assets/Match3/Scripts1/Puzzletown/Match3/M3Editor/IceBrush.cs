using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000505 RID: 1285
	public class IceBrush : ModifierBrush
	{
		// Token: 0x06002335 RID: 9013 RVA: 0x0009C31C File Offset: 0x0009A71C
		public IceBrush(GemModifier modifier, Sprite sprite, ABrush removal = null) : base(modifier, sprite, removal)
		{
			this.brushComponents.Add(new RandomColorBrushComponent());
		}
	}
}
