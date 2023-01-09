using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200050E RID: 1294
	public class PortalBrush : FieldModifierBrush
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x0009C5F8 File Offset: 0x0009A9F8
		public PortalBrush(Sprite sprite, int id, ABrush removal = null) : base(sprite, removal)
		{
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new SpawnerBrushComponent(SpawnTypes.None));
			this.brushComponents.Add(new RemovePortalIdBrushComponent(id));
			this.brushComponents.Add(new PortalsBrushComponent(id));
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x0009C650 File Offset: 0x0009AA50
		public override bool RequiresRefreshAll
		{
			get
			{
				return true;
			}
		}
	}
}
