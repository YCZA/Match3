namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200052B RID: 1323
	public class FieldBrushComponent : IBrushComponent
	{
		// Token: 0x0600238B RID: 9099 RVA: 0x0009E664 File Offset: 0x0009CA64
		public FieldBrushComponent(bool isOn)
		{
			this.isOn = isOn;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0009E673 File Offset: 0x0009CA73
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.isOn = this.isOn;
				if (this.isOn)
				{
					field.isWindow = false;
				}
			}
		}

		// Token: 0x04004F49 RID: 20297
		private readonly bool isOn;
	}
}
