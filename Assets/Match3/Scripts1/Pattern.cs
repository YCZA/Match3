using Match3.Scripts1.Puzzletown.Match3;

// Token: 0x020004E3 RID: 1251
namespace Match3.Scripts1
{
	public class Pattern
	{
		// Token: 0x060022BC RID: 8892 RVA: 0x00099F00 File Offset: 0x00098300
		public Pattern(int[,] positions)
		{
			this.positions = positions;
			this.width = positions.GetLength(1);
			this.height = positions.GetLength(0);
			int length = positions.GetLength(0);
			int length2 = positions.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					int num = positions[i, j];
					this.count += num;
				}
			}
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00099F8B File Offset: 0x0009838B
		public Pattern(Group group)
		{
			this.group = group;
			this.width = group.Width;
			this.height = group.Height;
			this.count = group.Count;
		}

		// Token: 0x04004E68 RID: 20072
		public int[,] positions;

		// Token: 0x04004E69 RID: 20073
		public Group group;

		// Token: 0x04004E6A RID: 20074
		public readonly int width;

		// Token: 0x04004E6B RID: 20075
		public readonly int height;

		// Token: 0x04004E6C RID: 20076
		public readonly int count;
	}
}
