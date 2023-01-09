using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000512 RID: 1298
	public class DropItemSpawnBrush : FieldModifierBrush
	{
		// Token: 0x06002343 RID: 9027 RVA: 0x0009C874 File Offset: 0x0009AC74
		public DropItemSpawnBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new DropItemSpawnerBrushComponent(true));
			this.brushComponents.Add(new DropItemExitBrushComponent(false));
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.None));
			this.brushComponents.Add(new ClimberSpawnerBrushComponent(false));
		}
	}
}
