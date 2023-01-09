namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000554 RID: 1364
	public class RemoveDefinedGemSpawnComponent : IBrushComponent
	{
		// Token: 0x060023E4 RID: 9188 RVA: 0x0009F18A File Offset: 0x0009D58A
		public void PaintField(Field field, Fields fields)
		{
			if (field.spawnType == SpawnTypes.DefinedGem)
			{
				field.spawnType = SpawnTypes.None;
			}
		}
	}
}
