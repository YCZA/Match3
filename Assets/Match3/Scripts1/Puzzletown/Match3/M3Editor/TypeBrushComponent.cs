using System.Linq;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000530 RID: 1328
	public class TypeBrushComponent : IBrushComponent
	{
		// Token: 0x06002396 RID: 9110 RVA: 0x0009E79F File Offset: 0x0009CB9F
		public TypeBrushComponent(GemType type)
		{
			this.type = type;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0009E7B0 File Offset: 0x0009CBB0
		public void PaintField(Field field, Fields fields)
		{
			if (field.gem.type == GemType.Cannon && this.type != GemType.Cannon)
			{
				field.gem.modifier = GemModifier.Undefined;
			}
			if (ColorBrushComponent.removingTypeColors.Contains(field.gem.color))
			{
				field.gem.color = GemColor.Random;
			}
			field.gem.type = this.type;
		}

		// Token: 0x04004F4C RID: 20300
		private readonly GemType type;
	}
}
