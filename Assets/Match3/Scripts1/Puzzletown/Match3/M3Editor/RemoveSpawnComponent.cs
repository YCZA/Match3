namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000553 RID: 1363
	public class RemoveSpawnComponent : IBrushComponent
	{
		// Token: 0x060023E2 RID: 9186 RVA: 0x0009F16B File Offset: 0x0009D56B
		public void PaintField(Field field, Fields fields)
		{
			field.spawnType = SpawnTypes.None;
			field.isDropSpawner = false;
			field.isClimberSpawner = false;
		}
	}
}
