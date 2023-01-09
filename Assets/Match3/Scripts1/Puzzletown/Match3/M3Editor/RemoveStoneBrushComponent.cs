namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200054D RID: 1357
	public class RemoveStoneBrushComponent : IBrushComponent
	{
		// Token: 0x060023D6 RID: 9174 RVA: 0x0009F040 File Offset: 0x0009D440
		public void PaintField(Field field, Fields fields)
		{
			if (Stone.IsStone(field.blockerIndex))
			{
				field.blockerIndex = 0;
			}
		}
	}
}
