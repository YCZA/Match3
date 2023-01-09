namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000544 RID: 1348
	public class ColorWheelBrushComponent : IBrushComponent
	{
		// Token: 0x060023C3 RID: 9155 RVA: 0x0009EED8 File Offset: 0x0009D2D8
		public ColorWheelBrushComponent(bool isDefaultWheel, bool isCorner)
		{
			this.isDefaultWheel = isDefaultWheel;
			this.isCorner = isCorner;
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0009EEF0 File Offset: 0x0009D2F0
		public void PaintField(Field field, Fields fields)
		{
			int num = (!this.isDefaultWheel) ? 12 : 10;
			num -= ((!this.isCorner) ? 0 : 1);
			field.blockerIndex = num;
		}

		// Token: 0x04004F5E RID: 20318
		private readonly bool isDefaultWheel;

		// Token: 0x04004F5F RID: 20319
		private readonly bool isCorner;
	}
}
