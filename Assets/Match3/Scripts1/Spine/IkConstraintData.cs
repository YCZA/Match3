using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000212 RID: 530
	public class IkConstraintData
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x000278F8 File Offset: 0x00025CF8
		public IkConstraintData(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x00027935 File Offset: 0x00025D35
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0002793D File Offset: 0x00025D3D
		public List<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x00027945 File Offset: 0x00025D45
		// (set) Token: 0x06001044 RID: 4164 RVA: 0x0002794D File Offset: 0x00025D4D
		public BoneData Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00027956 File Offset: 0x00025D56
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x0002795E File Offset: 0x00025D5E
		public int BendDirection
		{
			get
			{
				return this.bendDirection;
			}
			set
			{
				this.bendDirection = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x00027967 File Offset: 0x00025D67
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x0002796F File Offset: 0x00025D6F
		public float Mix
		{
			get
			{
				return this.mix;
			}
			set
			{
				this.mix = value;
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00027978 File Offset: 0x00025D78
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04004117 RID: 16663
		internal string name;

		// Token: 0x04004118 RID: 16664
		internal List<BoneData> bones = new List<BoneData>();

		// Token: 0x04004119 RID: 16665
		internal BoneData target;

		// Token: 0x0400411A RID: 16666
		internal int bendDirection = 1;

		// Token: 0x0400411B RID: 16667
		internal float mix = 1f;
	}
}
