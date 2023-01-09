using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200020B RID: 523
	public class Bone : IUpdatable
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x00025810 File Offset: 0x00023C10
		public Bone(BoneData data, Skeleton skeleton, Bone parent)
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
			this.skeleton = skeleton;
			this.parent = parent;
			this.SetToSetupPose();
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0002586B File Offset: 0x00023C6B
		public BoneData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x00025873 File Offset: 0x00023C73
		public Skeleton Skeleton
		{
			get
			{
				return this.skeleton;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0002587B File Offset: 0x00023C7B
		public Bone Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00025883 File Offset: 0x00023C83
		public ExposedList<Bone> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0002588B File Offset: 0x00023C8B
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x00025893 File Offset: 0x00023C93
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0002589C File Offset: 0x00023C9C
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x000258A4 File Offset: 0x00023CA4
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x000258AD File Offset: 0x00023CAD
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x000258B5 File Offset: 0x00023CB5
		public float Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				this.rotation = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x000258BE File Offset: 0x00023CBE
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x000258C6 File Offset: 0x00023CC6
		public float AppliedRotation
		{
			get
			{
				return this.appliedRotation;
			}
			set
			{
				this.appliedRotation = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x000258CF File Offset: 0x00023CCF
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x000258D7 File Offset: 0x00023CD7
		public float AppliedScaleX
		{
			get
			{
				return this.appliedScaleX;
			}
			set
			{
				this.appliedScaleX = value;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x000258E0 File Offset: 0x00023CE0
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x000258E8 File Offset: 0x00023CE8
		public float AppliedScaleY
		{
			get
			{
				return this.appliedScaleY;
			}
			set
			{
				this.appliedScaleY = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x000258F1 File Offset: 0x00023CF1
		// (set) Token: 0x06000FA1 RID: 4001 RVA: 0x000258F9 File Offset: 0x00023CF9
		public float ScaleX
		{
			get
			{
				return this.scaleX;
			}
			set
			{
				this.scaleX = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00025902 File Offset: 0x00023D02
		// (set) Token: 0x06000FA3 RID: 4003 RVA: 0x0002590A File Offset: 0x00023D0A
		public float ScaleY
		{
			get
			{
				return this.scaleY;
			}
			set
			{
				this.scaleY = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x00025913 File Offset: 0x00023D13
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x0002591B File Offset: 0x00023D1B
		public float ShearX
		{
			get
			{
				return this.shearX;
			}
			set
			{
				this.shearX = value;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00025924 File Offset: 0x00023D24
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0002592C File Offset: 0x00023D2C
		public float ShearY
		{
			get
			{
				return this.shearY;
			}
			set
			{
				this.shearY = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00025935 File Offset: 0x00023D35
		public float A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0002593D File Offset: 0x00023D3D
		public float B
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x00025945 File Offset: 0x00023D45
		public float C
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0002594D File Offset: 0x00023D4D
		public float D
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00025955 File Offset: 0x00023D55
		public float WorldX
		{
			get
			{
				return this.worldX;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0002595D File Offset: 0x00023D5D
		public float WorldY
		{
			get
			{
				return this.worldY;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x00025965 File Offset: 0x00023D65
		public float WorldSignX
		{
			get
			{
				return this.worldSignX;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0002596D File Offset: 0x00023D6D
		public float WorldSignY
		{
			get
			{
				return this.worldSignY;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x00025975 File Offset: 0x00023D75
		public float WorldRotationX
		{
			get
			{
				return MathUtils.Atan2(this.c, this.a) * 57.295776f;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0002598E File Offset: 0x00023D8E
		public float WorldRotationY
		{
			get
			{
				return MathUtils.Atan2(this.d, this.b) * 57.295776f;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x000259A7 File Offset: 0x00023DA7
		public float WorldScaleX
		{
			get
			{
				return (float)Math.Sqrt((double)(this.a * this.a + this.b * this.b)) * this.worldSignX;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x000259D2 File Offset: 0x00023DD2
		public float WorldScaleY
		{
			get
			{
				return (float)Math.Sqrt((double)(this.c * this.c + this.d * this.d)) * this.worldSignY;
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000259FD File Offset: 0x00023DFD
		public void Update()
		{
			this.UpdateWorldTransform(this.x, this.y, this.rotation, this.scaleX, this.scaleY, this.shearX, this.shearY);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00025A2F File Offset: 0x00023E2F
		public void UpdateWorldTransform()
		{
			this.UpdateWorldTransform(this.x, this.y, this.rotation, this.scaleX, this.scaleY, this.shearX, this.shearY);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00025A64 File Offset: 0x00023E64
		public void UpdateWorldTransform(float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY)
		{
			this.appliedRotation = rotation;
			this.appliedScaleX = scaleX;
			this.appliedScaleY = scaleY;
			float degrees = rotation + 90f + shearY;
			float num = MathUtils.CosDeg(rotation + shearX) * scaleX;
			float num2 = MathUtils.CosDeg(degrees) * scaleY;
			float num3 = MathUtils.SinDeg(rotation + shearX) * scaleX;
			float num4 = MathUtils.SinDeg(degrees) * scaleY;
			Bone bone = this.parent;
			if (bone == null)
			{
				Skeleton skeleton = this.skeleton;
				if (skeleton.flipX)
				{
					x = -x;
					num = -num;
					num2 = -num2;
				}
				if (skeleton.flipY != Bone.yDown)
				{
					y = -y;
					num3 = -num3;
					num4 = -num4;
				}
				this.a = num;
				this.b = num2;
				this.c = num3;
				this.d = num4;
				this.worldX = x;
				this.worldY = y;
				this.worldSignX = (float)Math.Sign(scaleX);
				this.worldSignY = (float)Math.Sign(scaleY);
				return;
			}
			float num5 = bone.a;
			float num6 = bone.b;
			float num7 = bone.c;
			float num8 = bone.d;
			this.worldX = num5 * x + num6 * y + bone.worldX;
			this.worldY = num7 * x + num8 * y + bone.worldY;
			this.worldSignX = bone.worldSignX * (float)Math.Sign(scaleX);
			this.worldSignY = bone.worldSignY * (float)Math.Sign(scaleY);
			if (this.data.inheritRotation && this.data.inheritScale)
			{
				this.a = num5 * num + num6 * num3;
				this.b = num5 * num2 + num6 * num4;
				this.c = num7 * num + num8 * num3;
				this.d = num7 * num2 + num8 * num4;
			}
			else
			{
				if (this.data.inheritRotation)
				{
					num5 = 1f;
					num6 = 0f;
					num7 = 0f;
					num8 = 1f;
					do
					{
						float num9 = MathUtils.CosDeg(bone.appliedRotation);
						float num10 = MathUtils.SinDeg(bone.appliedRotation);
						float num11 = num5 * num9 + num6 * num10;
						num6 = num5 * -num10 + num6 * num9;
						num5 = num11;
						num11 = num7 * num9 + num8 * num10;
						num8 = num7 * -num10 + num8 * num9;
						num7 = num11;
						if (!bone.data.inheritRotation)
						{
							break;
						}
						bone = bone.parent;
					}
					while (bone != null);
					this.a = num5 * num + num6 * num3;
					this.b = num5 * num2 + num6 * num4;
					this.c = num7 * num + num8 * num3;
					this.d = num7 * num2 + num8 * num4;
				}
				else if (this.data.inheritScale)
				{
					num5 = 1f;
					num6 = 0f;
					num7 = 0f;
					num8 = 1f;
					do
					{
						float num12 = bone.appliedRotation;
						float num13 = MathUtils.CosDeg(num12);
						float num14 = MathUtils.SinDeg(num12);
						float num15 = bone.appliedScaleX;
						float num16 = bone.appliedScaleY;
						float num17 = num13 * num15;
						float num18 = -num14 * num16;
						float num19 = num14 * num15;
						float num20 = num13 * num16;
						float num21 = num5 * num17 + num6 * num19;
						num6 = num5 * num18 + num6 * num20;
						num5 = num21;
						num21 = num7 * num17 + num8 * num19;
						num8 = num7 * num18 + num8 * num20;
						num7 = num21;
						if (num15 < 0f)
						{
							num12 = -num12;
						}
						num13 = MathUtils.CosDeg(-num12);
						num14 = MathUtils.SinDeg(-num12);
						num21 = num5 * num13 + num6 * num14;
						num6 = num5 * -num14 + num6 * num13;
						num5 = num21;
						num21 = num7 * num13 + num8 * num14;
						num8 = num7 * -num14 + num8 * num13;
						num7 = num21;
						if (!bone.data.inheritScale)
						{
							break;
						}
						bone = bone.parent;
					}
					while (bone != null);
					this.a = num5 * num + num6 * num3;
					this.b = num5 * num2 + num6 * num4;
					this.c = num7 * num + num8 * num3;
					this.d = num7 * num2 + num8 * num4;
				}
				else
				{
					this.a = num;
					this.b = num2;
					this.c = num3;
					this.d = num4;
				}
				if (this.skeleton.flipX)
				{
					this.a = -this.a;
					this.b = -this.b;
				}
				if (this.skeleton.flipY != Bone.yDown)
				{
					this.c = -this.c;
					this.d = -this.d;
				}
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00025F18 File Offset: 0x00024318
		public void SetToSetupPose()
		{
			BoneData boneData = this.data;
			this.x = boneData.x;
			this.y = boneData.y;
			this.rotation = boneData.rotation;
			this.scaleX = boneData.scaleX;
			this.scaleY = boneData.scaleY;
			this.shearX = boneData.shearX;
			this.shearY = boneData.shearY;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00025F80 File Offset: 0x00024380
		public void WorldToLocal(float worldX, float worldY, out float localX, out float localY)
		{
			float num = worldX - this.worldX;
			float num2 = worldY - this.worldY;
			float num3 = this.a;
			float num4 = this.b;
			float num5 = this.c;
			float num6 = this.d;
			float num7 = 1f / (num3 * num6 - num4 * num5);
			localX = num * num6 * num7 - num2 * num4 * num7;
			localY = num2 * num3 * num7 - num * num5 * num7;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00025FEF File Offset: 0x000243EF
		public void LocalToWorld(float localX, float localY, out float worldX, out float worldY)
		{
			worldX = localX * this.a + localY * this.b + this.worldX;
			worldY = localX * this.c + localY * this.d + this.worldY;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00026026 File Offset: 0x00024426
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x040040DD RID: 16605
		public static bool yDown;

		// Token: 0x040040DE RID: 16606
		internal BoneData data;

		// Token: 0x040040DF RID: 16607
		internal Skeleton skeleton;

		// Token: 0x040040E0 RID: 16608
		internal Bone parent;

		// Token: 0x040040E1 RID: 16609
		internal ExposedList<Bone> children = new ExposedList<Bone>();

		// Token: 0x040040E2 RID: 16610
		internal float x;

		// Token: 0x040040E3 RID: 16611
		internal float y;

		// Token: 0x040040E4 RID: 16612
		internal float rotation;

		// Token: 0x040040E5 RID: 16613
		internal float scaleX;

		// Token: 0x040040E6 RID: 16614
		internal float scaleY;

		// Token: 0x040040E7 RID: 16615
		internal float shearX;

		// Token: 0x040040E8 RID: 16616
		internal float shearY;

		// Token: 0x040040E9 RID: 16617
		internal float appliedRotation;

		// Token: 0x040040EA RID: 16618
		internal float appliedScaleX;

		// Token: 0x040040EB RID: 16619
		internal float appliedScaleY;

		// Token: 0x040040EC RID: 16620
		internal float a;

		// Token: 0x040040ED RID: 16621
		internal float b;

		// Token: 0x040040EE RID: 16622
		internal float worldX;

		// Token: 0x040040EF RID: 16623
		internal float c;

		// Token: 0x040040F0 RID: 16624
		internal float d;

		// Token: 0x040040F1 RID: 16625
		internal float worldY;

		// Token: 0x040040F2 RID: 16626
		internal float worldSignX;

		// Token: 0x040040F3 RID: 16627
		internal float worldSignY;
	}
}
