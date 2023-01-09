using System;
using System.Collections;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000541 RID: 1345
	public class RemovePortalIdBrushComponent : IBrushComponent
	{
		// Token: 0x060023B9 RID: 9145 RVA: 0x0009EADE File Offset: 0x0009CEDE
		public RemovePortalIdBrushComponent(int id)
		{
			this.id = id;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0009EAF0 File Offset: 0x0009CEF0
		public void PaintField(Field field, Fields fields)
		{
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field2 = (Field)obj;
					if (field2.portalId == this.id)
					{
						field2.portalId = 0;
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

		// Token: 0x04004F5A RID: 20314
		private readonly int id;
	}
}
