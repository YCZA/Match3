using System;
using System.Collections;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000542 RID: 1346
	public class RemoveHiddenItemBrushComponent : IBrushComponent
	{
		// Token: 0x060023BC RID: 9148 RVA: 0x0009EB6C File Offset: 0x0009CF6C
		public void PaintField(Field field, Fields fields)
		{
			int hiddenItemId = field.hiddenItemId;
			if (hiddenItemId != 0)
			{
				IEnumerator enumerator = fields.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Field field2 = (Field)obj;
						if (field2.hiddenItemId == hiddenItemId)
						{
							field2.hiddenItemId = 0;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}
}
