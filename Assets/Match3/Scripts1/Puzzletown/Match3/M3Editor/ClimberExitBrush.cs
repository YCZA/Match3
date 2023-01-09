using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000515 RID: 1301
	public class ClimberExitBrush : FieldModifierBrush
	{
		// Token: 0x06002346 RID: 9030 RVA: 0x0009C9A8 File Offset: 0x0009ADA8
		public ClimberExitBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new ClimberExitBrushComponent(true));
			this.brushComponents.Add(new ClimberSpawnerBrushComponent(false));
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
		}
	}
}
