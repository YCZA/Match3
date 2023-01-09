namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200053F RID: 1343
	public class PortalsBrushComponent : IBrushComponent
	{
		// Token: 0x060023B5 RID: 9141 RVA: 0x0009EAB0 File Offset: 0x0009CEB0
		public PortalsBrushComponent(int id)
		{
			this.id = id;
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x0009EABF File Offset: 0x0009CEBF
		public void PaintField(Field field, Fields fields)
		{
			field.portalId = this.id;
		}

		// Token: 0x04004F59 RID: 20313
		private readonly int id;
	}
}
