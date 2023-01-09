using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000527 RID: 1319
	public class ChameleonSpawnBrush : FieldModifierBrush
	{
		// Token: 0x06002361 RID: 9057 RVA: 0x0009D188 File Offset: 0x0009B588
		public ChameleonSpawnBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new DropItemSpawnerBrushComponent(false));
			this.brushComponents.Add(new DropItemExitBrushComponent(false));
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.ChameleonSpawn));
			this.brushComponents.Add(new ClimberSpawnerBrushComponent(false));
		}
	}
}
