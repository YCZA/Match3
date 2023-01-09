using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000513 RID: 1299
	public class DropItemExitBrush : FieldModifierBrush
	{
		// Token: 0x06002344 RID: 9028 RVA: 0x0009C8E0 File Offset: 0x0009ACE0
		public DropItemExitBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new DropItemExitBrushComponent(true));
			this.brushComponents.Add(new DropItemSpawnerBrushComponent(false));
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.None));
		}
	}
}
