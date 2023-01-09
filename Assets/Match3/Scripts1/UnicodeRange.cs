using System.Collections.Generic;
using System.Linq;

// Token: 0x02000AEC RID: 2796
namespace Match3.Scripts1
{
	public class UnicodeRange
	{
		// Token: 0x06004218 RID: 16920 RVA: 0x00154F06 File Offset: 0x00153306
		public UnicodeRange(int startIndex, int endIndex)
		{
			this.start = startIndex;
			this.end = endIndex;
			this.range = Enumerable.Range(this.start, this.Count);
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x00154F33 File Offset: 0x00153333
		public int Count
		{
			get
			{
				return this.end - this.start;
			}
		}

		// Token: 0x04006B41 RID: 27457
		public IEnumerable<int> range;

		// Token: 0x04006B42 RID: 27458
		public int start;

		// Token: 0x04006B43 RID: 27459
		public int end;
	}
}
