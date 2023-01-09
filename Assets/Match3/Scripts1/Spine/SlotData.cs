using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000224 RID: 548
	public class SlotData
	{
		// Token: 0x06001122 RID: 4386 RVA: 0x0002E2E8 File Offset: 0x0002C6E8
		public SlotData(string name, BoneData boneData)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			if (boneData == null)
			{
				throw new ArgumentNullException("boneData cannot be null.");
			}
			this.name = name;
			this.boneData = boneData;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x0002E357 File Offset: 0x0002C757
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x0002E35F File Offset: 0x0002C75F
		public BoneData BoneData
		{
			get
			{
				return this.boneData;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x0002E367 File Offset: 0x0002C767
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x0002E36F File Offset: 0x0002C76F
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

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x0002E378 File Offset: 0x0002C778
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x0002E380 File Offset: 0x0002C780
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

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x0002E389 File Offset: 0x0002C789
		// (set) Token: 0x0600112A RID: 4394 RVA: 0x0002E391 File Offset: 0x0002C791
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

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x0002E39A File Offset: 0x0002C79A
		// (set) Token: 0x0600112C RID: 4396 RVA: 0x0002E3A2 File Offset: 0x0002C7A2
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

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x0002E3AB File Offset: 0x0002C7AB
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x0002E3B3 File Offset: 0x0002C7B3
		public string AttachmentName
		{
			get
			{
				return this.attachmentName;
			}
			set
			{
				this.attachmentName = value;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x0002E3BC File Offset: 0x0002C7BC
		// (set) Token: 0x06001130 RID: 4400 RVA: 0x0002E3C4 File Offset: 0x0002C7C4
		public BlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
			}
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0002E3CD File Offset: 0x0002C7CD
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x0400418A RID: 16778
		internal string name;

		// Token: 0x0400418B RID: 16779
		internal BoneData boneData;

		// Token: 0x0400418C RID: 16780
		internal float r = 1f;

		// Token: 0x0400418D RID: 16781
		internal float g = 1f;

		// Token: 0x0400418E RID: 16782
		internal float b = 1f;

		// Token: 0x0400418F RID: 16783
		internal float a = 1f;

		// Token: 0x04004190 RID: 16784
		internal string attachmentName;

		// Token: 0x04004191 RID: 16785
		internal BlendMode blendMode;
	}
}
