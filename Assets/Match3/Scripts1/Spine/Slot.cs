using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000223 RID: 547
	public class Slot
	{
		// Token: 0x0600110B RID: 4363 RVA: 0x0002E0D4 File Offset: 0x0002C4D4
		public Slot(SlotData data, Bone bone)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentNullException("bone cannot be null.");
			}
			this.data = data;
			this.bone = bone;
			this.SetToSetupPose();
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x0002E129 File Offset: 0x0002C529
		public SlotData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x0002E131 File Offset: 0x0002C531
		public Bone Bone
		{
			get
			{
				return this.bone;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x0002E139 File Offset: 0x0002C539
		public Skeleton Skeleton
		{
			get
			{
				return this.bone.skeleton;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0002E146 File Offset: 0x0002C546
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x0002E14E File Offset: 0x0002C54E
		public float R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0002E157 File Offset: 0x0002C557
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x0002E15F File Offset: 0x0002C55F
		public float G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x0002E168 File Offset: 0x0002C568
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x0002E170 File Offset: 0x0002C570
		public float B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x0002E179 File Offset: 0x0002C579
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x0002E181 File Offset: 0x0002C581
		public float A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x0002E18A File Offset: 0x0002C58A
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x0002E192 File Offset: 0x0002C592
		public Attachment Attachment
		{
			get
			{
				return this.attachment;
			}
			set
			{
				if (this.attachment == value)
				{
					return;
				}
				this.attachment = value;
				this.attachmentTime = this.bone.skeleton.time;
				this.attachmentVerticesCount = 0;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x0002E1C5 File Offset: 0x0002C5C5
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x0002E1DE File Offset: 0x0002C5DE
		public float AttachmentTime
		{
			get
			{
				return this.bone.skeleton.time - this.attachmentTime;
			}
			set
			{
				this.attachmentTime = this.bone.skeleton.time - value;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x0002E1F8 File Offset: 0x0002C5F8
		// (set) Token: 0x0600111C RID: 4380 RVA: 0x0002E200 File Offset: 0x0002C600
		public float[] AttachmentVertices
		{
			get
			{
				return this.attachmentVertices;
			}
			set
			{
				this.attachmentVertices = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x0002E209 File Offset: 0x0002C609
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x0002E211 File Offset: 0x0002C611
		public int AttachmentVerticesCount
		{
			get
			{
				return this.attachmentVerticesCount;
			}
			set
			{
				this.attachmentVerticesCount = value;
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0002E21C File Offset: 0x0002C61C
		internal void SetToSetupPose(int slotIndex)
		{
			this.r = this.data.r;
			this.g = this.data.g;
			this.b = this.data.b;
			this.a = this.data.a;
			if (this.data.attachmentName == null)
			{
				this.Attachment = null;
			}
			else
			{
				this.attachment = null;
				this.Attachment = this.bone.skeleton.GetAttachment(slotIndex, this.data.attachmentName);
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0002E2B2 File Offset: 0x0002C6B2
		public void SetToSetupPose()
		{
			this.SetToSetupPose(this.bone.skeleton.data.slots.IndexOf(this.data));
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0002E2DA File Offset: 0x0002C6DA
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x04004180 RID: 16768
		internal SlotData data;

		// Token: 0x04004181 RID: 16769
		internal Bone bone;

		// Token: 0x04004182 RID: 16770
		internal float r;

		// Token: 0x04004183 RID: 16771
		internal float g;

		// Token: 0x04004184 RID: 16772
		internal float b;

		// Token: 0x04004185 RID: 16773
		internal float a;

		// Token: 0x04004186 RID: 16774
		internal Attachment attachment;

		// Token: 0x04004187 RID: 16775
		internal float attachmentTime;

		// Token: 0x04004188 RID: 16776
		internal float[] attachmentVertices = new float[0];

		// Token: 0x04004189 RID: 16777
		internal int attachmentVerticesCount;
	}
}
