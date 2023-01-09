namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200053B RID: 1339
	public class DropItemSpawnerBrushComponent : IBrushComponent
	{
		// Token: 0x060023AD RID: 9133 RVA: 0x0009EA26 File Offset: 0x0009CE26
		public DropItemSpawnerBrushComponent(bool isDropSpawner)
		{
			this.isDropSpawner = isDropSpawner;
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0009EA35 File Offset: 0x0009CE35
		public void PaintField(Field field, Fields fields)
		{
			field.isDropSpawner = this.isDropSpawner;
		}

		// Token: 0x04004F55 RID: 20309
		private readonly bool isDropSpawner;
	}
}
