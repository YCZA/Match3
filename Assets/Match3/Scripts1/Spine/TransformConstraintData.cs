using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000226 RID: 550
	public class TransformConstraintData
	{
		// Token: 0x0600114F RID: 4431 RVA: 0x0002E924 File Offset: 0x0002CD24
		public TransformConstraintData(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x0002E944 File Offset: 0x0002CD44
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0002E94C File Offset: 0x0002CD4C
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x0002E954 File Offset: 0x0002CD54
		public BoneData Bone
		{
			get
			{
				return this.bone;
			}
			set
			{
				this.bone = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0002E95D File Offset: 0x0002CD5D
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x0002E965 File Offset: 0x0002CD65
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

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0002E96E File Offset: 0x0002CD6E
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x0002E976 File Offset: 0x0002CD76
		public float RotateMix
		{
			get
			{
				return this.rotateMix;
			}
			set
			{
				this.rotateMix = value;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x0002E97F File Offset: 0x0002CD7F
		// (set) Token: 0x06001158 RID: 4440 RVA: 0x0002E987 File Offset: 0x0002CD87
		public float TranslateMix
		{
			get
			{
				return this.translateMix;
			}
			set
			{
				this.translateMix = value;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x0002E990 File Offset: 0x0002CD90
		// (set) Token: 0x0600115A RID: 4442 RVA: 0x0002E998 File Offset: 0x0002CD98
		public float ScaleMix
		{
			get
			{
				return this.scaleMix;
			}
			set
			{
				this.scaleMix = value;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x0002E9A1 File Offset: 0x0002CDA1
		// (set) Token: 0x0600115C RID: 4444 RVA: 0x0002E9A9 File Offset: 0x0002CDA9
		public float ShearMix
		{
			get
			{
				return this.shearMix;
			}
			set
			{
				this.shearMix = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x0002E9B2 File Offset: 0x0002CDB2
		// (set) Token: 0x0600115E RID: 4446 RVA: 0x0002E9BA File Offset: 0x0002CDBA
		public float OffsetRotation
		{
			get
			{
				return this.offsetRotation;
			}
			set
			{
				this.offsetRotation = value;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x0002E9C3 File Offset: 0x0002CDC3
		// (set) Token: 0x06001160 RID: 4448 RVA: 0x0002E9CB File Offset: 0x0002CDCB
		public float OffsetX
		{
			get
			{
				return this.offsetX;
			}
			set
			{
				this.offsetX = value;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x0002E9D4 File Offset: 0x0002CDD4
		// (set) Token: 0x06001162 RID: 4450 RVA: 0x0002E9DC File Offset: 0x0002CDDC
		public float OffsetY
		{
			get
			{
				return this.offsetY;
			}
			set
			{
				this.offsetY = value;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x0002E9E5 File Offset: 0x0002CDE5
		// (set) Token: 0x06001164 RID: 4452 RVA: 0x0002E9ED File Offset: 0x0002CDED
		public float OffsetScaleX
		{
			get
			{
				return this.offsetScaleX;
			}
			set
			{
				this.offsetScaleX = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x0002E9F6 File Offset: 0x0002CDF6
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x0002E9FE File Offset: 0x0002CDFE
		public float OffsetScaleY
		{
			get
			{
				return this.offsetScaleY;
			}
			set
			{
				this.offsetScaleY = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x0002EA07 File Offset: 0x0002CE07
		// (set) Token: 0x06001168 RID: 4456 RVA: 0x0002EA0F File Offset: 0x0002CE0F
		public float OffsetShearY
		{
			get
			{
				return this.offsetShearY;
			}
			set
			{
				this.offsetShearY = value;
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0002EA18 File Offset: 0x0002CE18
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x0400419F RID: 16799
		internal string name;

		// Token: 0x040041A0 RID: 16800
		internal BoneData bone;

		// Token: 0x040041A1 RID: 16801
		internal BoneData target;

		// Token: 0x040041A2 RID: 16802
		internal float rotateMix;

		// Token: 0x040041A3 RID: 16803
		internal float translateMix;

		// Token: 0x040041A4 RID: 16804
		internal float scaleMix;

		// Token: 0x040041A5 RID: 16805
		internal float shearMix;

		// Token: 0x040041A6 RID: 16806
		internal float offsetRotation;

		// Token: 0x040041A7 RID: 16807
		internal float offsetX;

		// Token: 0x040041A8 RID: 16808
		internal float offsetY;

		// Token: 0x040041A9 RID: 16809
		internal float offsetScaleX;

		// Token: 0x040041AA RID: 16810
		internal float offsetScaleY;

		// Token: 0x040041AB RID: 16811
		internal float offsetShearY;
	}
}
