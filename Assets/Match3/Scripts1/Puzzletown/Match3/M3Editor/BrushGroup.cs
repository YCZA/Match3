using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000529 RID: 1321
	public class BrushGroup : IEquatable<BrushGroup>
	{
		// Token: 0x06002382 RID: 9090 RVA: 0x0009E53B File Offset: 0x0009C93B
		public BrushGroup(Sprite groupImage, List<ABrush> brushes)
		{
			this.groupImage = groupImage;
			this.brushes = brushes;
			this.maxCount = brushes.Count;
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x0009E55D File Offset: 0x0009C95D
		public Sprite GroupImage
		{
			get
			{
				return this.groupImage;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x0009E565 File Offset: 0x0009C965
		public ABrush CurrentBrush
		{
			get
			{
				return this.brushes[this.index];
			}
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0009E578 File Offset: 0x0009C978
		public void GoToNextBrush()
		{
			this.index = (this.index + 1) % this.maxCount;
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x0009E58F File Offset: 0x0009C98F
		public void GoToPreviousBrush()
		{
			this.index = (this.index - 1) % this.maxCount;
			this.index = ((this.index >= 0) ? this.index : (this.maxCount - 1));
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0009E5CB File Offset: 0x0009C9CB
		public void GoToBrushAtIndex(int newIndex)
		{
			this.index = ((newIndex < this.maxCount) ? newIndex : (this.maxCount - 1));
			this.index = ((this.index >= 0) ? this.index : 0);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0009E60B File Offset: 0x0009CA0B
		public void GoToBrush(ABrush brush)
		{
			this.index = this.brushes.IndexOf(brush);
			this.index = ((this.index >= 0) ? this.index : 0);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0009E63D File Offset: 0x0009CA3D
		public bool Equals(BrushGroup other)
		{
			return this.maxCount == other.maxCount && this.groupImage == other.groupImage;
		}

		// Token: 0x04004F45 RID: 20293
		public List<ABrush> brushes;

		// Token: 0x04004F46 RID: 20294
		private int index;

		// Token: 0x04004F47 RID: 20295
		private int maxCount;

		// Token: 0x04004F48 RID: 20296
		private Sprite groupImage;
	}
}
