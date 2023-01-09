using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000526 RID: 1318
	public class ChameleonBrush : ABrush
	{
		// Token: 0x06002360 RID: 9056 RVA: 0x0009D0EC File Offset: 0x0009B4EC
		public ChameleonBrush(ChameleonVariant chameleonVariant, GemDirection chameleonDirection, Sprite sprite, ABrush removal, int rotationAngle) : base(sprite, removal, rotationAngle)
		{
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new RemoveDirtAndTreasureComponent());
			this.brushComponents.Add(new RemoveCannonBrushComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new ChameleonBrushComponent(chameleonVariant, chameleonDirection));
		}
	}
}
