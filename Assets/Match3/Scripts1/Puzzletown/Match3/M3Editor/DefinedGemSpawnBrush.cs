using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000511 RID: 1297
	public class DefinedGemSpawnBrush : FieldModifierBrush
	{
		// Token: 0x06002342 RID: 9026 RVA: 0x0009C748 File Offset: 0x0009AB48
		public DefinedGemSpawnBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveTilesBrushComponent());
			this.brushComponents.Add(new RemoveCannonBrushComponent());
			this.brushComponents.Add(new RemoveDirtAndTreasureComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new RemovePortalBrushComponent());
			this.brushComponents.Add(new RemoveChameleonComponent());
			this.brushComponents.Add(new DropItemSpawnerBrushComponent(false));
			this.brushComponents.Add(new DropItemExitBrushComponent(false));
			this.brushComponents.Add(new ClimberSpawnerBrushComponent(false));
			this.brushComponents.Add(new ClimberExitBrushComponent(false));
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.DefinedGem));
			this.brushComponents.Add(new RandomColorBrushComponent());
			this.brushComponents.Add(new HorizontalLinegemOnRandomColorBrushComponent());
		}
	}
}
