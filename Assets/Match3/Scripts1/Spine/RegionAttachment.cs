using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000208 RID: 520
	public class RegionAttachment : Attachment
	{
		// Token: 0x06000F26 RID: 3878 RVA: 0x00024CB8 File Offset: 0x000230B8
		public RegionAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x00024D26 File Offset: 0x00023126
		// (set) Token: 0x06000F28 RID: 3880 RVA: 0x00024D2E File Offset: 0x0002312E
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00024D37 File Offset: 0x00023137
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x00024D3F File Offset: 0x0002313F
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

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00024D48 File Offset: 0x00023148
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x00024D50 File Offset: 0x00023150
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

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x00024D59 File Offset: 0x00023159
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x00024D61 File Offset: 0x00023161
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00024D6A File Offset: 0x0002316A
		// (set) Token: 0x06000F30 RID: 3888 RVA: 0x00024D72 File Offset: 0x00023172
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x00024D7B File Offset: 0x0002317B
		// (set) Token: 0x06000F32 RID: 3890 RVA: 0x00024D83 File Offset: 0x00023183
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x00024D8C File Offset: 0x0002318C
		// (set) Token: 0x06000F34 RID: 3892 RVA: 0x00024D94 File Offset: 0x00023194
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x00024D9D File Offset: 0x0002319D
		// (set) Token: 0x06000F36 RID: 3894 RVA: 0x00024DA5 File Offset: 0x000231A5
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

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00024DAE File Offset: 0x000231AE
		// (set) Token: 0x06000F38 RID: 3896 RVA: 0x00024DB6 File Offset: 0x000231B6
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

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x00024DBF File Offset: 0x000231BF
		// (set) Token: 0x06000F3A RID: 3898 RVA: 0x00024DC7 File Offset: 0x000231C7
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x00024DD0 File Offset: 0x000231D0
		// (set) Token: 0x06000F3C RID: 3900 RVA: 0x00024DD8 File Offset: 0x000231D8
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00024DE1 File Offset: 0x000231E1
		// (set) Token: 0x06000F3E RID: 3902 RVA: 0x00024DE9 File Offset: 0x000231E9
		public string Path { get; set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x00024DF2 File Offset: 0x000231F2
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x00024DFA File Offset: 0x000231FA
		public object RendererObject { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x00024E03 File Offset: 0x00023203
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x00024E0B File Offset: 0x0002320B
		public float RegionOffsetX
		{
			get
			{
				return this.regionOffsetX;
			}
			set
			{
				this.regionOffsetX = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x00024E14 File Offset: 0x00023214
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x00024E1C File Offset: 0x0002321C
		public float RegionOffsetY
		{
			get
			{
				return this.regionOffsetY;
			}
			set
			{
				this.regionOffsetY = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x00024E25 File Offset: 0x00023225
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x00024E2D File Offset: 0x0002322D
		public float RegionWidth
		{
			get
			{
				return this.regionWidth;
			}
			set
			{
				this.regionWidth = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x00024E36 File Offset: 0x00023236
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x00024E3E File Offset: 0x0002323E
		public float RegionHeight
		{
			get
			{
				return this.regionHeight;
			}
			set
			{
				this.regionHeight = value;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x00024E47 File Offset: 0x00023247
		// (set) Token: 0x06000F4A RID: 3914 RVA: 0x00024E4F File Offset: 0x0002324F
		public float RegionOriginalWidth
		{
			get
			{
				return this.regionOriginalWidth;
			}
			set
			{
				this.regionOriginalWidth = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x00024E58 File Offset: 0x00023258
		// (set) Token: 0x06000F4C RID: 3916 RVA: 0x00024E60 File Offset: 0x00023260
		public float RegionOriginalHeight
		{
			get
			{
				return this.regionOriginalHeight;
			}
			set
			{
				this.regionOriginalHeight = value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000F4D RID: 3917 RVA: 0x00024E69 File Offset: 0x00023269
		public float[] Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x00024E71 File Offset: 0x00023271
		public float[] UVs
		{
			get
			{
				return this.uvs;
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00024E7C File Offset: 0x0002327C
		public void SetUVs(float u, float v, float u2, float v2, bool rotate)
		{
			float[] array = this.uvs;
			if (rotate)
			{
				array[2] = u;
				array[3] = v2;
				array[4] = u;
				array[5] = v;
				array[6] = u2;
				array[7] = v;
				array[0] = u2;
				array[1] = v2;
			}
			else
			{
				array[0] = u;
				array[1] = v2;
				array[2] = u;
				array[3] = v;
				array[4] = u2;
				array[5] = v;
				array[6] = u2;
				array[7] = v2;
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00024EE0 File Offset: 0x000232E0
		public void UpdateOffset()
		{
			float num = this.width;
			float num2 = this.height;
			float num3 = this.scaleX;
			float num4 = this.scaleY;
			float num5 = num / this.regionOriginalWidth * num3;
			float num6 = num2 / this.regionOriginalHeight * num4;
			float num7 = -num / 2f * num3 + this.regionOffsetX * num5;
			float num8 = -num2 / 2f * num4 + this.regionOffsetY * num6;
			float num9 = num7 + this.regionWidth * num5;
			float num10 = num8 + this.regionHeight * num6;
			float num11 = this.rotation * 3.1415927f / 180f;
			float num12 = (float)Math.Cos((double)num11);
			float num13 = (float)Math.Sin((double)num11);
			float num14 = this.x;
			float num15 = this.y;
			float num16 = num7 * num12 + num14;
			float num17 = num7 * num13;
			float num18 = num8 * num12 + num15;
			float num19 = num8 * num13;
			float num20 = num9 * num12 + num14;
			float num21 = num9 * num13;
			float num22 = num10 * num12 + num15;
			float num23 = num10 * num13;
			float[] array = this.offset;
			array[0] = num16 - num19;
			array[1] = num18 + num17;
			array[2] = num16 - num23;
			array[3] = num22 + num17;
			array[4] = num20 - num23;
			array[5] = num22 + num21;
			array[6] = num20 - num19;
			array[7] = num18 + num21;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00025038 File Offset: 0x00023438
		public void ComputeWorldVertices(Bone bone, float[] worldVertices)
		{
			float num = bone.skeleton.x + bone.worldX;
			float num2 = bone.skeleton.y + bone.worldY;
			float num3 = bone.a;
			float num4 = bone.b;
			float c = bone.c;
			float d = bone.d;
			float[] array = this.offset;
			worldVertices[0] = array[0] * num3 + array[1] * num4 + num;
			worldVertices[1] = array[0] * c + array[1] * d + num2;
			worldVertices[2] = array[2] * num3 + array[3] * num4 + num;
			worldVertices[3] = array[2] * c + array[3] * d + num2;
			worldVertices[4] = array[4] * num3 + array[5] * num4 + num;
			worldVertices[5] = array[4] * c + array[5] * d + num2;
			worldVertices[6] = array[6] * num3 + array[7] * num4 + num;
			worldVertices[7] = array[6] * c + array[7] * d + num2;
		}

		// Token: 0x0400409E RID: 16542
		public const int X1 = 0;

		// Token: 0x0400409F RID: 16543
		public const int Y1 = 1;

		// Token: 0x040040A0 RID: 16544
		public const int X2 = 2;

		// Token: 0x040040A1 RID: 16545
		public const int Y2 = 3;

		// Token: 0x040040A2 RID: 16546
		public const int X3 = 4;

		// Token: 0x040040A3 RID: 16547
		public const int Y3 = 5;

		// Token: 0x040040A4 RID: 16548
		public const int X4 = 6;

		// Token: 0x040040A5 RID: 16549
		public const int Y4 = 7;

		// Token: 0x040040A6 RID: 16550
		internal float x;

		// Token: 0x040040A7 RID: 16551
		internal float y;

		// Token: 0x040040A8 RID: 16552
		internal float rotation;

		// Token: 0x040040A9 RID: 16553
		internal float scaleX = 1f;

		// Token: 0x040040AA RID: 16554
		internal float scaleY = 1f;

		// Token: 0x040040AB RID: 16555
		internal float width;

		// Token: 0x040040AC RID: 16556
		internal float height;

		// Token: 0x040040AD RID: 16557
		internal float regionOffsetX;

		// Token: 0x040040AE RID: 16558
		internal float regionOffsetY;

		// Token: 0x040040AF RID: 16559
		internal float regionWidth;

		// Token: 0x040040B0 RID: 16560
		internal float regionHeight;

		// Token: 0x040040B1 RID: 16561
		internal float regionOriginalWidth;

		// Token: 0x040040B2 RID: 16562
		internal float regionOriginalHeight;

		// Token: 0x040040B3 RID: 16563
		internal float[] offset = new float[8];

		// Token: 0x040040B4 RID: 16564
		internal float[] uvs = new float[8];

		// Token: 0x040040B5 RID: 16565
		internal float r = 1f;

		// Token: 0x040040B6 RID: 16566
		internal float g = 1f;

		// Token: 0x040040B7 RID: 16567
		internal float b = 1f;

		// Token: 0x040040B8 RID: 16568
		internal float a = 1f;
	}
}
