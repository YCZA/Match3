namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200055B RID: 1371
	public class FieldViewEditorFactory : FieldViewFactory
	{
		// Token: 0x0600240D RID: 9229 RVA: 0x000A04FC File Offset: 0x0009E8FC
		public override FieldView Create(Field field, BoardView boardView)
		{
			bool flag = !field.isOn;
			field.isOn = true;
			FieldView result = base.Create(field, boardView);
			if (flag)
			{
				field.isOn = false;
			}
			return result;
		}
	}
}
