using Match3.Scripts1.Spine;
using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x0200023A RID: 570
	public struct SubmeshInstruction
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x000314F8 File Offset: 0x0002F8F8
		public int SlotCount
		{
			get
			{
				return this.endSlot - this.startSlot;
			}
		}

		// Token: 0x040041EB RID: 16875
		public Skeleton skeleton;

		// Token: 0x040041EC RID: 16876
		public int startSlot;

		// Token: 0x040041ED RID: 16877
		public int endSlot;

		// Token: 0x040041EE RID: 16878
		public Material material;

		// Token: 0x040041EF RID: 16879
		public int triangleCount;

		// Token: 0x040041F0 RID: 16880
		public int vertexCount;

		// Token: 0x040041F1 RID: 16881
		public int firstVertexIndex;

		// Token: 0x040041F2 RID: 16882
		public bool forceSeparate;
	}
}
