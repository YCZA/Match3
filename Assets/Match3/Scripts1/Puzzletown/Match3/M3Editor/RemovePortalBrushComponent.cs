namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000540 RID: 1344
	public class RemovePortalBrushComponent : IBrushComponent
	{
		// Token: 0x060023B8 RID: 9144 RVA: 0x0009EAD5 File Offset: 0x0009CED5
		public void PaintField(Field field, Fields fields)
		{
			field.portalId = 0;
		}
	}
}
