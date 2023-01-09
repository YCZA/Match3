using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000504 RID: 1284
	public class CannonBrush : ABrush
	{
		// Token: 0x06002332 RID: 9010 RVA: 0x0009C234 File Offset: 0x0009A634
		public CannonBrush(GemModifier modifier, Sprite sprite, ABrush removal = null) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new CannonBrushComponent(modifier));
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0009C2BC File Offset: 0x0009A6BC
		public override void PaintField(Field field, Fields fields)
		{
			base.PaintField(field, fields);
			if (!CannonBrush.IsColorValidForCannon(field.gem.color))
			{
				field.gem.color = GemColor.Coins;
			}
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0009C2E8 File Offset: 0x0009A6E8
		public static bool IsColorValidForCannon(GemColor color)
		{
			return color == GemColor.Red || color == GemColor.Green || color == GemColor.Blue || color == GemColor.Yellow || color == GemColor.Purple || color == GemColor.Orange || color == GemColor.Coins;
		}
	}
}
