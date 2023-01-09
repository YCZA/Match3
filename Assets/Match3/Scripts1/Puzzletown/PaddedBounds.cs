using UnityEngine;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x02000A8D RID: 2701
	public class PaddedBounds
	{
		// Token: 0x06004046 RID: 16454 RVA: 0x0014B304 File Offset: 0x00149704
		public PaddedBounds(Bounds b, Padding padding)
		{
			this.corners[0] = new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z);
			this.corners[1] = new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z);
			this.corners[2] = new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z);
			this.corners[3] = new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z);
			this.corners[4] = new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z);
			this.corners[5] = new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z);
			this.corners[6] = new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z);
			this.corners[7] = new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z);
			this.AddPadding(padding);
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0014B6F0 File Offset: 0x00149AF0
		public Bounds GetBounds()
		{
			Bounds result = default(Bounds);
			result.SetMinMax(this.corners[7], this.corners[0]);
			return result;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0014B72F File Offset: 0x00149B2F
		public void AddPadding(Padding padding)
		{
			this.addPaddingTop(padding.top);
			this.addPaddingBottom(padding.bottom);
			this.addPaddingLeft(padding.left);
			this.addPaddingRight(padding.right);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0014B764 File Offset: 0x00149B64
		private void addPaddingTop(int amount)
		{
			this.corners[0] += Vector3.up * (float)amount * 0.05f;
			this.corners[1] += Vector3.up * (float)amount * 0.05f;
			this.corners[4] += Vector3.up * (float)amount * 0.05f;
			this.corners[5] += Vector3.up * (float)amount * 0.05f;
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0014B83C File Offset: 0x00149C3C
		private void addPaddingBottom(int amount)
		{
			this.corners[2] += Vector3.down * (float)amount * 0.05f;
			this.corners[3] += Vector3.down * (float)amount * 0.05f;
			this.corners[6] += Vector3.down * (float)amount * 0.05f;
			this.corners[7] += Vector3.down * (float)amount * 0.05f;
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0014B914 File Offset: 0x00149D14
		private void addPaddingLeft(int amount)
		{
			this.corners[4] += Vector3.left * (float)amount * 0.05f;
			this.corners[5] += Vector3.left * (float)amount * 0.05f;
			this.corners[6] += Vector3.left * (float)amount * 0.05f;
			this.corners[7] += Vector3.left * (float)amount * 0.05f;
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0014B9EC File Offset: 0x00149DEC
		private void addPaddingRight(int amount)
		{
			this.corners[0] += Vector3.right * (float)amount * 0.05f;
			this.corners[1] += Vector3.right * (float)amount * 0.05f;
			this.corners[2] += Vector3.right * (float)amount * 0.05f;
			this.corners[3] += Vector3.right * (float)amount * 0.05f;
		}

		// Token: 0x040069F1 RID: 27121
		private Vector3[] corners = new Vector3[8];

		// Token: 0x040069F2 RID: 27122
		private const float SCALEFACTOR = 0.05f;
	}
}
