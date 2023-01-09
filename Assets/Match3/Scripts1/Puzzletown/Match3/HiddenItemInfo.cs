using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000617 RID: 1559
	public class HiddenItemInfo
	{
		// Token: 0x060027D0 RID: 10192 RVA: 0x000B0F1B File Offset: 0x000AF31B
		public HiddenItemInfo(int id, IntVector2 pos)
		{
			this.id = id;
			this.bottomLeftPosition = pos;
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x000B0F3C File Offset: 0x000AF33C
		public int Size
		{
			get
			{
				if (this._size == 0)
				{
					this._size = (int)Mathf.Sqrt((float)this.totalPositionsCount);
				}
				return this._size;
			}
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000B0F62 File Offset: 0x000AF362
		public override string ToString()
		{
			return string.Format("[HiddenItemInfo: positions={0}, id={1}]", this.positions, this.id);
		}

		// Token: 0x0400521C RID: 21020
		public List<IntVector2> positions = new List<IntVector2>();

		// Token: 0x0400521D RID: 21021
		public int id;

		// Token: 0x0400521E RID: 21022
		public int totalPositionsCount;

		// Token: 0x0400521F RID: 21023
		public IntVector2 bottomLeftPosition;

		// Token: 0x04005220 RID: 21024
		private int _size;
	}
}
