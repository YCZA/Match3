using System;
using System.Collections;
using Match3.Scripts1.Shared.DataStructures;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000596 RID: 1430
	public class FieldMappings
	{
		// Token: 0x0600253D RID: 9533 RVA: 0x000A5AE8 File Offset: 0x000A3EE8
		public FieldMappings(Fields fields)
		{
			this.fields = fields;
			int size = fields.size;
			this.map = new Map<FieldMappings.MapEntry>(size);
			this.UpdateFieldMappings(fields);
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000A5B1C File Offset: 0x000A3F1C
		public void UpdateFieldMappings(Fields fields)
		{
			int size = this.map.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[i, j];
					this.portalUsedBelow = 0;
					this.portalUsedAbove = 0;
					IntVector2 above = this.FindAbovePosition(field.gridPosition);
					IntVector2 below = this.FindBelowPosition(field.gridPosition);
					this.map[field.gridPosition] = new FieldMappings.MapEntry(above, below, this.portalUsedAbove, this.portalUsedBelow);
				}
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000A5BB4 File Offset: 0x000A3FB4
		public IntVector2 Below(IntVector2 position)
		{
			return this.map[position].below;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x000A5BD8 File Offset: 0x000A3FD8
		public IntVector2 Above(IntVector2 position)
		{
			return this.map[position].above;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000A5BFC File Offset: 0x000A3FFC
		public int PortalUsedBelow(IntVector2 position)
		{
			return this.map[position].portalUsedBelow;
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000A5C20 File Offset: 0x000A4020
		public int PortalUsedAbove(IntVector2 position)
		{
			return this.map[position].portalUsedAbove;
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000A5C44 File Offset: 0x000A4044
		public IntVector2 GetPortalExitPosition(IntVector2 position)
		{
			int num = this.PortalUsedAbove(position);
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.portalId == num)
					{
						return field.gridPosition;
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
			return Fields.invalidPos;
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000A5CCC File Offset: 0x000A40CC
		public IntVector2 GetPortalEntryPosition(IntVector2 position)
		{
			int num = this.PortalUsedBelow(position);
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.portalId == num)
					{
						return field.gridPosition;
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
			return Fields.invalidPos;
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000A5D54 File Offset: 0x000A4154
		private IntVector2 GetEntrance(int id)
		{
			int num = id - 1;
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.portalId == num)
					{
						return field.gridPosition;
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
			throw new ArgumentException("No Entrance found for " + id + "!");
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000A5DEC File Offset: 0x000A41EC
		private IntVector2 FindBelowPosition(IntVector2 gridPos)
		{
			do
			{
				int portalId = this.fields[gridPos].portalId;
				if (Portal.IsEntrance(portalId))
				{
					gridPos = this.GetExit(portalId);
					this.portalUsedBelow = portalId;
				}
				else
				{
					gridPos.y--;
				}
			}
			while (gridPos.y >= 0 && this.fields[gridPos].BehavesLikeWindow());
			return (!this.fields.IsValid(gridPos)) ? Fields.invalidPos : gridPos;
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000A5E7C File Offset: 0x000A427C
		private IntVector2 FindAbovePosition(IntVector2 gridPos)
		{
			do
			{
				int portalId = this.fields[gridPos].portalId;
				if (Portal.IsExit(portalId))
				{
					gridPos = this.GetEntrance(portalId);
					this.portalUsedAbove = portalId;
				}
				else
				{
					gridPos.y++;
				}
			}
			while (gridPos.y < this.fields.size && this.fields[gridPos].BehavesLikeWindow());
			return (!this.fields.IsValid(gridPos)) ? Fields.invalidPos : gridPos;
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000A5F14 File Offset: 0x000A4314
		private IntVector2 GetExit(int id)
		{
			int num = id + 1;
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.portalId == num)
					{
						return field.gridPosition;
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
			throw new ArgumentException("No Exit found for " + id + "!");
		}

		// Token: 0x04005097 RID: 20631
		private Fields fields;

		// Token: 0x04005098 RID: 20632
		private Map<FieldMappings.MapEntry> map;

		// Token: 0x04005099 RID: 20633
		private int portalUsedBelow;

		// Token: 0x0400509A RID: 20634
		private int portalUsedAbove;

		// Token: 0x02000597 RID: 1431
		public struct MapEntry
		{
			// Token: 0x06002549 RID: 9545 RVA: 0x000A5FAC File Offset: 0x000A43AC
			public MapEntry(IntVector2 above, IntVector2 below, int portalUsedAbove, int portalUsedBelow)
			{
				this.above = above;
				this.below = below;
				this.portalUsedAbove = portalUsedAbove;
				this.portalUsedBelow = portalUsedBelow;
			}

			// Token: 0x0400509B RID: 20635
			public IntVector2 above;

			// Token: 0x0400509C RID: 20636
			public IntVector2 below;

			// Token: 0x0400509D RID: 20637
			public readonly int portalUsedAbove;

			// Token: 0x0400509E RID: 20638
			public readonly int portalUsedBelow;
		}
	}
}
