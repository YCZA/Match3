using System;

namespace Match3.Scripts1
{
	// Token: 0x02000ABA RID: 2746
	[Serializable]
	public struct IntRect
	{
		// Token: 0x0600411E RID: 16670 RVA: 0x00151D89 File Offset: 0x00150189
		public IntRect(int x, int y, int width, int height)
		{
			this.min = new IntVector2(x, y);
			this.max = new IntVector2(x + width, y + height);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x00151DAA File Offset: 0x001501AA
		public IntRect(IntVector2 position, IntVector2 size)
		{
			this.min = position;
			this.max = position + size;
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x00151DC0 File Offset: 0x001501C0
		public IntRect(IntRect other)
		{
			this.min = other.min;
			this.max = other.max;
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x00151DDC File Offset: 0x001501DC
		public bool Contains(IntVector2 other)
		{
			return this.min.x <= other.x && this.max.x > other.x && this.min.y <= other.y && this.max.y > other.y;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00151E4C File Offset: 0x0015024C
		public bool Overlaps(IntRect other)
		{
			return this.min.x < other.max.x && this.max.x > other.min.x && this.min.y < other.max.y && this.max.y > other.min.y;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x00151ECE File Offset: 0x001502CE
		public int width
		{
			get
			{
				return this.max.x - this.min.x;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06004124 RID: 16676 RVA: 0x00151EE7 File Offset: 0x001502E7
		public int height
		{
			get
			{
				return this.max.y - this.min.y;
			}
		}

		// Token: 0x04006AD9 RID: 27353
		public IntVector2 min;

		// Token: 0x04006ADA RID: 27354
		public IntVector2 max;
	}
}
