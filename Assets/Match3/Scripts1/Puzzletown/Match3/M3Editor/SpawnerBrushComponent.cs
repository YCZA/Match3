namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200053A RID: 1338
	public class SpawnerBrushComponent : IBrushComponent
	{
		// Token: 0x060023AB RID: 9131 RVA: 0x0009E9FE File Offset: 0x0009CDFE
		public SpawnerBrushComponent(SpawnTypes spawnType)
		{
			this.spawnType = spawnType;
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0009EA0D File Offset: 0x0009CE0D
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.spawnType = this.spawnType;
			}
		}

		// Token: 0x04004F54 RID: 20308
		private readonly SpawnTypes spawnType;
	}
}
