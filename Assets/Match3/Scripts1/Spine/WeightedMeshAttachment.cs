namespace Match3.Scripts1.Spine
{
	// Token: 0x02000209 RID: 521
	public class WeightedMeshAttachment : Attachment, IFfdAttachment
	{
		// Token: 0x06000F52 RID: 3922 RVA: 0x00025129 File Offset: 0x00023529
		public WeightedMeshAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0002515E File Offset: 0x0002355E
		// (set) Token: 0x06000F54 RID: 3924 RVA: 0x00025166 File Offset: 0x00023566
		public int HullLength { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0002516F File Offset: 0x0002356F
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x00025177 File Offset: 0x00023577
		public int[] Bones
		{
			get
			{
				return this.bones;
			}
			set
			{
				this.bones = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x00025180 File Offset: 0x00023580
		// (set) Token: 0x06000F58 RID: 3928 RVA: 0x00025188 File Offset: 0x00023588
		public float[] Weights
		{
			get
			{
				return this.weights;
			}
			set
			{
				this.weights = value;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00025191 File Offset: 0x00023591
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x00025199 File Offset: 0x00023599
		public float[] RegionUVs
		{
			get
			{
				return this.regionUVs;
			}
			set
			{
				this.regionUVs = value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x000251A2 File Offset: 0x000235A2
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x000251AA File Offset: 0x000235AA
		public float[] UVs
		{
			get
			{
				return this.uvs;
			}
			set
			{
				this.uvs = value;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x000251B3 File Offset: 0x000235B3
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x000251BB File Offset: 0x000235BB
		public int[] Triangles
		{
			get
			{
				return this.triangles;
			}
			set
			{
				this.triangles = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x000251C4 File Offset: 0x000235C4
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x000251CC File Offset: 0x000235CC
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

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x000251D5 File Offset: 0x000235D5
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x000251DD File Offset: 0x000235DD
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

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x000251E6 File Offset: 0x000235E6
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x000251EE File Offset: 0x000235EE
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x000251F7 File Offset: 0x000235F7
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x000251FF File Offset: 0x000235FF
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x00025208 File Offset: 0x00023608
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x00025210 File Offset: 0x00023610
		public string Path { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x00025219 File Offset: 0x00023619
		// (set) Token: 0x06000F6A RID: 3946 RVA: 0x00025221 File Offset: 0x00023621
		public object RendererObject { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0002522A File Offset: 0x0002362A
		// (set) Token: 0x06000F6C RID: 3948 RVA: 0x00025232 File Offset: 0x00023632
		public float RegionU { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0002523B File Offset: 0x0002363B
		// (set) Token: 0x06000F6E RID: 3950 RVA: 0x00025243 File Offset: 0x00023643
		public float RegionV { get; set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0002524C File Offset: 0x0002364C
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x00025254 File Offset: 0x00023654
		public float RegionU2 { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0002525D File Offset: 0x0002365D
		// (set) Token: 0x06000F72 RID: 3954 RVA: 0x00025265 File Offset: 0x00023665
		public float RegionV2 { get; set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0002526E File Offset: 0x0002366E
		// (set) Token: 0x06000F74 RID: 3956 RVA: 0x00025276 File Offset: 0x00023676
		public bool RegionRotate { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0002527F File Offset: 0x0002367F
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x00025287 File Offset: 0x00023687
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

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x00025290 File Offset: 0x00023690
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x00025298 File Offset: 0x00023698
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

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x000252A1 File Offset: 0x000236A1
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x000252A9 File Offset: 0x000236A9
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x000252B2 File Offset: 0x000236B2
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x000252BA File Offset: 0x000236BA
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x000252C3 File Offset: 0x000236C3
		// (set) Token: 0x06000F7E RID: 3966 RVA: 0x000252CB File Offset: 0x000236CB
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

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x000252D4 File Offset: 0x000236D4
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x000252DC File Offset: 0x000236DC
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

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x000252E5 File Offset: 0x000236E5
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x000252ED File Offset: 0x000236ED
		public bool InheritFFD
		{
			get
			{
				return this.inheritFFD;
			}
			set
			{
				this.inheritFFD = value;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x000252F6 File Offset: 0x000236F6
		// (set) Token: 0x06000F84 RID: 3972 RVA: 0x00025300 File Offset: 0x00023700
		public WeightedMeshAttachment ParentMesh
		{
			get
			{
				return this.parentMesh;
			}
			set
			{
				this.parentMesh = value;
				if (value != null)
				{
					this.bones = value.bones;
					this.weights = value.weights;
					this.regionUVs = value.regionUVs;
					this.triangles = value.triangles;
					this.HullLength = value.HullLength;
					this.Edges = value.Edges;
					this.Width = value.Width;
					this.Height = value.Height;
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0002537A File Offset: 0x0002377A
		// (set) Token: 0x06000F86 RID: 3974 RVA: 0x00025382 File Offset: 0x00023782
		public int[] Edges { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0002538B File Offset: 0x0002378B
		// (set) Token: 0x06000F88 RID: 3976 RVA: 0x00025393 File Offset: 0x00023793
		public float Width { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0002539C File Offset: 0x0002379C
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x000253A4 File Offset: 0x000237A4
		public float Height { get; set; }

		// Token: 0x06000F8B RID: 3979 RVA: 0x000253B0 File Offset: 0x000237B0
		public void UpdateUVs()
		{
			float regionU = this.RegionU;
			float regionV = this.RegionV;
			float num = this.RegionU2 - this.RegionU;
			float num2 = this.RegionV2 - this.RegionV;
			float[] array = this.regionUVs;
			if (this.uvs == null || this.uvs.Length != array.Length)
			{
				this.uvs = new float[array.Length];
			}
			float[] array2 = this.uvs;
			if (this.RegionRotate)
			{
				int i = 0;
				int num3 = array2.Length;
				while (i < num3)
				{
					array2[i] = regionU + array[i + 1] * num;
					array2[i + 1] = regionV + num2 - array[i] * num2;
					i += 2;
				}
			}
			else
			{
				int j = 0;
				int num4 = array2.Length;
				while (j < num4)
				{
					array2[j] = regionU + array[j] * num;
					array2[j + 1] = regionV + array[j + 1] * num2;
					j += 2;
				}
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x000254B0 File Offset: 0x000238B0
		private Bone FindRoot(Bone bone)
		{
			if (bone == null || bone.parent == null)
			{
				return bone;
			}
			Bone rootBone = bone.parent.Skeleton.RootBone;
			if (bone == rootBone)
			{
				return bone;
			}
			while (bone.parent != rootBone)
			{
				bone = bone.parent;
			}
			return bone;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00025504 File Offset: 0x00023904
		public void ComputeWorldVertices(Slot slot, float[] worldVertices)
		{
			Skeleton skeleton = slot.bone.skeleton;
			ExposedList<Bone> exposedList = skeleton.bones;
			float x = skeleton.x;
			float y = skeleton.y;
			float[] array = this.weights;
			int[] array2 = this.bones;
			if (this.rootBonesPerBone == null)
			{
				this.rootBonesPerBone = new Bone[exposedList.Items.Length];
				for (int i = 0; i < this.rootBonesPerBone.Length; i++)
				{
					this.rootBonesPerBone[i] = this.FindRoot(exposedList.Items[i]);
				}
			}
			if (slot.attachmentVerticesCount == 0)
			{
				int num = 0;
				int j = 0;
				int num2 = 0;
				int num3 = array2.Length;
				while (j < num3)
				{
					float num4 = 0f;
					float num5 = 0f;
					float num6 = 0f;
					int num7 = array2[j++] + j;
					while (j < num7)
					{
						Bone bone = exposedList.Items[array2[j]];
						Bone bone2 = this.rootBonesPerBone[array2[j]];
						float num8 = array[num2];
						float num9 = array[num2 + 1];
						float num10 = array[num2 + 2];
						num4 += (num8 * bone.a + num9 * bone.b + bone.worldX) * num10;
						num5 += (num8 * bone.c + num9 * bone.d + bone.worldY - bone2.worldY) * num10;
						num6 += bone2.worldY * num10;
						j++;
						num2 += 3;
					}
					worldVertices[num] = num4 + x;
					worldVertices[num + 1] = num5 + y;
					worldVertices[num + 2] = num6 * 2f;
					num += 3;
				}
			}
			else
			{
				float[] attachmentVertices = slot.attachmentVertices;
				int num11 = 0;
				int k = 0;
				int num12 = 0;
				int num13 = 0;
				int num14 = array2.Length;
				while (k < num14)
				{
					float num15 = 0f;
					float num16 = 0f;
					float num17 = 0f;
					int num18 = array2[k++] + k;
					while (k < num18)
					{
						Bone bone3 = exposedList.Items[array2[k]];
						Bone bone4 = this.rootBonesPerBone[array2[k]];
						float num19 = array[num12] + attachmentVertices[num13];
						float num20 = array[num12 + 1] + attachmentVertices[num13 + 1];
						float num21 = array[num12 + 2];
						num15 += (num19 * bone3.a + num20 * bone3.b + bone3.worldX) * num21;
						num16 += (num19 * bone3.c + num20 * bone3.d - bone4.worldY) * num21;
						num17 += bone4.worldY * num21;
						k++;
						num12 += 3;
						num13 += 2;
					}
					worldVertices[num11] = num15 + x;
					worldVertices[num11 + 1] = num16 + y;
					worldVertices[num11 + 2] = num17 * 2f;
					num11 += 3;
				}
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000257EA File Offset: 0x00023BEA
		public bool ApplyFFD(Attachment sourceAttachment)
		{
			return this == sourceAttachment || (this.inheritFFD && this.parentMesh == sourceAttachment);
		}

		// Token: 0x040040BB RID: 16571
		internal int[] bones;

		// Token: 0x040040BC RID: 16572
		internal float[] weights;

		// Token: 0x040040BD RID: 16573
		internal float[] uvs;

		// Token: 0x040040BE RID: 16574
		internal float[] regionUVs;

		// Token: 0x040040BF RID: 16575
		internal int[] triangles;

		// Token: 0x040040C0 RID: 16576
		internal float regionOffsetX;

		// Token: 0x040040C1 RID: 16577
		internal float regionOffsetY;

		// Token: 0x040040C2 RID: 16578
		internal float regionWidth;

		// Token: 0x040040C3 RID: 16579
		internal float regionHeight;

		// Token: 0x040040C4 RID: 16580
		internal float regionOriginalWidth;

		// Token: 0x040040C5 RID: 16581
		internal float regionOriginalHeight;

		// Token: 0x040040C6 RID: 16582
		internal float r = 1f;

		// Token: 0x040040C7 RID: 16583
		internal float g = 1f;

		// Token: 0x040040C8 RID: 16584
		internal float b = 1f;

		// Token: 0x040040C9 RID: 16585
		internal float a = 1f;

		// Token: 0x040040CA RID: 16586
		internal WeightedMeshAttachment parentMesh;

		// Token: 0x040040CB RID: 16587
		internal bool inheritFFD;

		// Token: 0x040040D4 RID: 16596
		public Bone[] rootBonesPerBone;
	}
}
