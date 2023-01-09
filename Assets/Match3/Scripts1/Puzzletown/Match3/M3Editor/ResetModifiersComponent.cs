namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000551 RID: 1361
	public class ResetModifiersComponent : IBrushComponent
	{
		// Token: 0x060023DE RID: 9182 RVA: 0x0009F0F8 File Offset: 0x0009D4F8
		public void PaintField(Field field, Fields fields)
		{
			field.isWindow = false;
			field.spawnType = SpawnTypes.None;
			field.isDropSpawner = false;
			field.isDropExit = false;
			field.isClimberSpawner = false;
			field.isClimberExit = false;
			field.portalId = 0;
			field.numChains = 0;
			field.cratesIndex = 0;
			field.blockerIndex = 0;
			field.numTiles = 0;
		}
	}
}
