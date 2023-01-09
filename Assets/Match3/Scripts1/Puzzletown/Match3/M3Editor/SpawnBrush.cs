using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000510 RID: 1296
	public class SpawnBrush : FieldModifierBrush
	{
		// Token: 0x06002341 RID: 9025 RVA: 0x0009C6DC File Offset: 0x0009AADC
		public SpawnBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.NormalSpawn));
			this.brushComponents.Add(new DropItemSpawnerBrushComponent(false));
			this.brushComponents.Add(new DropItemExitBrushComponent(false));
			this.brushComponents.Add(new ClimberSpawnerBrushComponent(false));
		}
	}
}
