using System;
using System.Collections;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000543 RID: 1347
	public class HiddenItemBrushComponent : IBrushComponent
	{
		// Token: 0x060023BD RID: 9149 RVA: 0x0009EBE8 File Offset: 0x0009CFE8
		public HiddenItemBrushComponent(int size, bool isRandom)
		{
			this.size = size;
			this.isRandom = isRandom;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0009EC00 File Offset: 0x0009D000
		public void PaintField(Field field, Fields fields)
		{
			this.fields = fields;
			if (this.CanBePlaced(field))
			{
				List<int> oldIds = this.Place(field.gridPosition, this.size);
				this.RemoveOldIds(oldIds);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Item would be out of bounds!",
					this.size,
					field.gridPosition
				});
			}
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0009EC70 File Offset: 0x0009D070
		private List<int> Place(IntVector2 pos, int size)
		{
			List<int> list = new List<int>();
			int availableId = this.GetAvailableId();
			IEnumerator enumerator = this.fields.GetBlock(pos, size, size).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					int hiddenItemId = field.hiddenItemId;
					if (hiddenItemId > 0)
					{
						list.AddIfNotAlreadyPresent(hiddenItemId, false);
					}
					field.hiddenItemId = availableId;
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
			return list;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0009ED08 File Offset: 0x0009D108
		private void RemoveOldIds(List<int> oldIds)
		{
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					int hiddenItemId = field.hiddenItemId;
					if (oldIds.Contains(hiddenItemId))
					{
						field.hiddenItemId = 0;
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

		// Token: 0x060023C1 RID: 9153 RVA: 0x0009ED80 File Offset: 0x0009D180
		private int GetAvailableId()
		{
			List<int> list = new List<int>();
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					int hiddenItemId = field.hiddenItemId;
					if (hiddenItemId > 0)
					{
						list.AddIfNotAlreadyPresent(hiddenItemId, false);
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
			for (int i = 1; i <= 4; i++)
			{
				int num = (!this.isRandom) ? i : (i + 4);
				if (!list.Contains(num))
				{
					return num;
				}
			}
			throw new Exception("You can have only 4 hidden items in a level!");
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0009EE48 File Offset: 0x0009D248
		private bool CanBePlaced(Field field)
		{
			IEnumerator enumerator = this.fields.GetBlock(field.gridPosition, this.size, this.size).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field2 = (Field)obj;
					if (field2 == null || !field2.isOn)
					{
						return false;
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
			return true;
		}

		// Token: 0x04004F5B RID: 20315
		private readonly int size;

		// Token: 0x04004F5C RID: 20316
		private readonly bool isRandom;

		// Token: 0x04004F5D RID: 20317
		private Fields fields;
	}
}
