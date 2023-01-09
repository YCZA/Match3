using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007D4 RID: 2004
	public class SortedLevelList
	{
		// Token: 0x06003170 RID: 12656 RVA: 0x000E8919 File Offset: 0x000E6D19
		public SortedLevelList()
		{
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000E8921 File Offset: 0x000E6D21
		public SortedLevelList(List<int> initializerList)
		{
			if (initializerList != null)
			{
				initializerList.Sort();
				this.levels = initializerList;
			}
		}

		// Token: 0x170007C0 RID: 1984
		public int this[int index]
		{
			get
			{
				int result = -1;
				if (index >= 0 && this.levels != null && index < this.levels.Count)
				{
					result = this.levels[index];
				}
				return result;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000E897C File Offset: 0x000E6D7C
		public int Count
		{
			get
			{
				return (this.levels == null) ? 0 : this.levels.Count;
			}
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000E899C File Offset: 0x000E6D9C
		public int GetLowestIndexForValueLargerThan(int value)
		{
			int num = -1;
			if (this.levels != null)
			{
				int num2 = 0;
				while (num2 < this.levels.Count && num == -1)
				{
					if (this.levels[num2] > value)
					{
						num = num2;
					}
					num2++;
				}
			}
			return num;
		}

		// Token: 0x04005A06 RID: 23046
		public const int INVALID = -1;

		// Token: 0x04005A07 RID: 23047
		protected List<int> levels;
	}
}
