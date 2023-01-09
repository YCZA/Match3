namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200053D RID: 1341
	public class ClimberSpawnerBrushComponent : IBrushComponent
	{
		// Token: 0x060023B1 RID: 9137 RVA: 0x0009EA60 File Offset: 0x0009CE60
		public ClimberSpawnerBrushComponent(bool isClimberSpawner)
		{
			this.isClimberSpawner = isClimberSpawner;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0009EA6F File Offset: 0x0009CE6F
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.isClimberSpawner = this.isClimberSpawner;
			}
		}

		// Token: 0x04004F57 RID: 20311
		private readonly bool isClimberSpawner;
	}
}
