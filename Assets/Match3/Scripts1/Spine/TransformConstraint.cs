using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000225 RID: 549
	public class TransformConstraint : IUpdatable
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x0002E3D8 File Offset: 0x0002C7D8
		public TransformConstraint(TransformConstraintData data, Skeleton skeleton)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			this.data = data;
			this.translateMix = data.translateMix;
			this.rotateMix = data.rotateMix;
			this.scaleMix = data.scaleMix;
			this.shearMix = data.shearMix;
			this.offsetRotation = data.offsetRotation;
			this.offsetX = data.offsetX;
			this.offsetY = data.offsetY;
			this.offsetScaleX = data.offsetScaleX;
			this.offsetScaleY = data.offsetScaleY;
			this.offsetShearY = data.offsetShearY;
			this.bone = skeleton.FindBone(data.bone.name);
			this.target = skeleton.FindBone(data.target.name);
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x0002E4BA File Offset: 0x0002C8BA
		public TransformConstraintData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0002E4C2 File Offset: 0x0002C8C2
		// (set) Token: 0x06001135 RID: 4405 RVA: 0x0002E4CA File Offset: 0x0002C8CA
		public Bone Bone
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

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x0002E4D3 File Offset: 0x0002C8D3
		// (set) Token: 0x06001137 RID: 4407 RVA: 0x0002E4DB File Offset: 0x0002C8DB
		public Bone Target
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

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x0002E4E4 File Offset: 0x0002C8E4
		// (set) Token: 0x06001139 RID: 4409 RVA: 0x0002E4EC File Offset: 0x0002C8EC
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

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x0002E4F5 File Offset: 0x0002C8F5
		// (set) Token: 0x0600113B RID: 4411 RVA: 0x0002E4FD File Offset: 0x0002C8FD
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

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x0002E506 File Offset: 0x0002C906
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x0002E50E File Offset: 0x0002C90E
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

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x0002E517 File Offset: 0x0002C917
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x0002E51F File Offset: 0x0002C91F
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0002E528 File Offset: 0x0002C928
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x0002E530 File Offset: 0x0002C930
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

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0002E539 File Offset: 0x0002C939
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x0002E541 File Offset: 0x0002C941
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

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0002E54A File Offset: 0x0002C94A
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x0002E552 File Offset: 0x0002C952
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

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0002E55B File Offset: 0x0002C95B
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x0002E563 File Offset: 0x0002C963
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

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0002E56C File Offset: 0x0002C96C
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x0002E574 File Offset: 0x0002C974
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

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0002E57D File Offset: 0x0002C97D
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x0002E585 File Offset: 0x0002C985
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

		// Token: 0x0600114C RID: 4428 RVA: 0x0002E58E File Offset: 0x0002C98E
		public void Apply()
		{
			this.Update();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0002E598 File Offset: 0x0002C998
		public void Update()
		{
			Bone bone = this.bone;
			Bone bone2 = this.target;
			if (this.rotateMix > 0f)
			{
				float a = bone.a;
				float b = bone.b;
				float c = bone.c;
				float d = bone.d;
				float num = MathUtils.Atan2(bone2.c, bone2.a) - MathUtils.Atan2(c, a) + this.offsetRotation * 0.017453292f;
				if (num > 3.1415927f)
				{
					num -= 6.2831855f;
				}
				else if (num < -3.1415927f)
				{
					num += 6.2831855f;
				}
				num *= this.rotateMix;
				float num2 = MathUtils.Cos(num);
				float num3 = MathUtils.Sin(num);
				bone.a = num2 * a - num3 * c;
				bone.b = num2 * b - num3 * d;
				bone.c = num3 * a + num2 * c;
				bone.d = num3 * b + num2 * d;
			}
			if (this.scaleMix > 0f)
			{
				float num4 = (float)Math.Sqrt((double)(bone.a * bone.a + bone.c * bone.c));
				float num5 = (float)Math.Sqrt((double)(bone2.a * bone2.a + bone2.c * bone2.c));
				float num6 = (num4 <= 1E-05f) ? 0f : ((num4 + (num5 - num4 + this.offsetScaleX) * this.scaleMix) / num4);
				bone.a *= num6;
				bone.c *= num6;
				num4 = (float)Math.Sqrt((double)(bone.b * bone.b + bone.d * bone.d));
				num5 = (float)Math.Sqrt((double)(bone2.b * bone2.b + bone2.d * bone2.d));
				num6 = ((num4 <= 1E-05f) ? 0f : ((num4 + (num5 - num4 + this.offsetScaleY) * this.scaleMix) / num4));
				bone.b *= num6;
				bone.d *= num6;
			}
			if (this.shearMix > 0f)
			{
				float b2 = bone.b;
				float d2 = bone.d;
				float num7 = MathUtils.Atan2(d2, b2);
				float num8 = MathUtils.Atan2(bone2.d, bone2.b) - MathUtils.Atan2(bone2.c, bone2.a) - (num7 - MathUtils.Atan2(bone.c, bone.a));
				if (num8 > 3.1415927f)
				{
					num8 -= 6.2831855f;
				}
				else if (num8 < -3.1415927f)
				{
					num8 += 6.2831855f;
				}
				num8 = num7 + (num8 + this.offsetShearY * 0.017453292f) * this.shearMix;
				float num9 = (float)Math.Sqrt((double)(b2 * b2 + d2 * d2));
				bone.b = MathUtils.Cos(num8) * num9;
				bone.d = MathUtils.Sin(num8) * num9;
			}
			float num10 = this.translateMix;
			if (num10 > 0f)
			{
				float num11;
				float num12;
				bone2.LocalToWorld(this.offsetX, this.offsetY, out num11, out num12);
				bone.worldX += (num11 - bone.worldX) * num10;
				bone.worldY += (num12 - bone.worldY) * num10;
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0002E917 File Offset: 0x0002CD17
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x04004192 RID: 16786
		internal TransformConstraintData data;

		// Token: 0x04004193 RID: 16787
		internal Bone bone;

		// Token: 0x04004194 RID: 16788
		internal Bone target;

		// Token: 0x04004195 RID: 16789
		internal float rotateMix;

		// Token: 0x04004196 RID: 16790
		internal float translateMix;

		// Token: 0x04004197 RID: 16791
		internal float scaleMix;

		// Token: 0x04004198 RID: 16792
		internal float shearMix;

		// Token: 0x04004199 RID: 16793
		internal float offsetRotation;

		// Token: 0x0400419A RID: 16794
		internal float offsetX;

		// Token: 0x0400419B RID: 16795
		internal float offsetY;

		// Token: 0x0400419C RID: 16796
		internal float offsetScaleX;

		// Token: 0x0400419D RID: 16797
		internal float offsetScaleY;

		// Token: 0x0400419E RID: 16798
		internal float offsetShearY;
	}
}
