using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000514 RID: 1300
	public class ClimberSpawnBrush : FieldModifierBrush
	{
		// Token: 0x06002345 RID: 9029 RVA: 0x0009C93C File Offset: 0x0009AD3C
		public ClimberSpawnBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new ClimberSpawnerBrushComponent(true));
			this.brushComponents.Add(new ClimberExitBrushComponent(false));
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.None));
			this.brushComponents.Add(new DropItemSpawnerBrushComponent(false));
		}
	}
}
