namespace Match3.Scripts1.Spine
{
	// Token: 0x02000207 RID: 519
	public class MeshAttachment : Attachment, IFfdAttachment
	{
		// Token: 0x06000EEC RID: 3820 RVA: 0x00024864 File Offset: 0x00022C64
		public MeshAttachment(string name) : base(name)
		{
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x00024899 File Offset: 0x00022C99
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x000248A1 File Offset: 0x00022CA1
		public int HullLength { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x000248AA File Offset: 0x00022CAA
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x000248B2 File Offset: 0x00022CB2
		public float[] Vertices
		{
			get
			{
				return this.vertices;
			}
			set
			{
				this.vertices = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x000248BB File Offset: 0x00022CBB
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x000248C3 File Offset: 0x00022CC3
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x000248CC File Offset: 0x00022CCC
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x000248D4 File Offset: 0x00022CD4
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

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x000248DD File Offset: 0x00022CDD
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x000248E5 File Offset: 0x00022CE5
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

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x000248EE File Offset: 0x00022CEE
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x000248F6 File Offset: 0x00022CF6
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

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x000248FF File Offset: 0x00022CFF
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x00024907 File Offset: 0x00022D07
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

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00024910 File Offset: 0x00022D10
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x00024918 File Offset: 0x00022D18
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

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00024921 File Offset: 0x00022D21
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x00024929 File Offset: 0x00022D29
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

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00024932 File Offset: 0x00022D32
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0002493A File Offset: 0x00022D3A
		public string Path { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x00024943 File Offset: 0x00022D43
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x0002494B File Offset: 0x00022D4B
		public object RendererObject { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x00024954 File Offset: 0x00022D54
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x0002495C File Offset: 0x00022D5C
		public float RegionU { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x00024965 File Offset: 0x00022D65
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0002496D File Offset: 0x00022D6D
		public float RegionV { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x00024976 File Offset: 0x00022D76
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0002497E File Offset: 0x00022D7E
		public float RegionU2 { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x00024987 File Offset: 0x00022D87
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x0002498F File Offset: 0x00022D8F
		public float RegionV2 { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00024998 File Offset: 0x00022D98
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x000249A0 File Offset: 0x00022DA0
		public bool RegionRotate { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x000249A9 File Offset: 0x00022DA9
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x000249B1 File Offset: 0x00022DB1
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

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x000249BA File Offset: 0x00022DBA
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x000249C2 File Offset: 0x00022DC2
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

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x000249CB File Offset: 0x00022DCB
		// (set) Token: 0x06000F12 RID: 3858 RVA: 0x000249D3 File Offset: 0x00022DD3
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

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x000249DC File Offset: 0x00022DDC
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x000249E4 File Offset: 0x00022DE4
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

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x000249ED File Offset: 0x00022DED
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x000249F5 File Offset: 0x00022DF5
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

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x000249FE File Offset: 0x00022DFE
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00024A06 File Offset: 0x00022E06
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

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00024A0F File Offset: 0x00022E0F
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00024A17 File Offset: 0x00022E17
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

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00024A20 File Offset: 0x00022E20
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x00024A28 File Offset: 0x00022E28
		public MeshAttachment ParentMesh
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
					this.vertices = value.vertices;
					this.regionUVs = value.regionUVs;
					this.triangles = value.triangles;
					this.HullLength = value.HullLength;
					this.Edges = value.Edges;
					this.Width = value.Width;
					this.Height = value.Height;
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00024A96 File Offset: 0x00022E96
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x00024A9E File Offset: 0x00022E9E
		public int[] Edges { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00024AA7 File Offset: 0x00022EA7
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x00024AAF File Offset: 0x00022EAF
		public float Width { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00024AB8 File Offset: 0x00022EB8
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x00024AC0 File Offset: 0x00022EC0
		public float Height { get; set; }

		// Token: 0x06000F23 RID: 3875 RVA: 0x00024ACC File Offset: 0x00022ECC
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

		// Token: 0x06000F24 RID: 3876 RVA: 0x00024BCC File Offset: 0x00022FCC
		public void ComputeWorldVertices(Slot slot, float[] worldVertices)
		{
			Bone bone = slot.bone;
			float num = bone.skeleton.x + bone.worldX;
			float num2 = bone.skeleton.y + bone.worldY;
			float num3 = bone.a;
			float num4 = bone.b;
			float c = bone.c;
			float d = bone.d;
			float[] attachmentVertices = this.vertices;
			int num5 = attachmentVertices.Length;
			if (slot.attachmentVerticesCount == num5)
			{
				attachmentVertices = slot.AttachmentVertices;
			}
			for (int i = 0; i < num5; i += 2)
			{
				float num6 = attachmentVertices[i];
				float num7 = attachmentVertices[i + 1];
				worldVertices[i] = num6 * num3 + num7 * num4 + num;
				worldVertices[i + 1] = num6 * c + num7 * d + num2;
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00024C92 File Offset: 0x00023092
		public bool ApplyFFD(Attachment sourceAttachment)
		{
			return this == sourceAttachment || (this.inheritFFD && this.parentMesh == sourceAttachment);
		}

		// Token: 0x04004083 RID: 16515
		internal float[] vertices;

		// Token: 0x04004084 RID: 16516
		internal float[] uvs;

		// Token: 0x04004085 RID: 16517
		internal float[] regionUVs;

		// Token: 0x04004086 RID: 16518
		internal int[] triangles;

		// Token: 0x04004087 RID: 16519
		internal float regionOffsetX;

		// Token: 0x04004088 RID: 16520
		internal float regionOffsetY;

		// Token: 0x04004089 RID: 16521
		internal float regionWidth;

		// Token: 0x0400408A RID: 16522
		internal float regionHeight;

		// Token: 0x0400408B RID: 16523
		internal float regionOriginalWidth;

		// Token: 0x0400408C RID: 16524
		internal float regionOriginalHeight;

		// Token: 0x0400408D RID: 16525
		internal float r = 1f;

		// Token: 0x0400408E RID: 16526
		internal float g = 1f;

		// Token: 0x0400408F RID: 16527
		internal float b = 1f;

		// Token: 0x04004090 RID: 16528
		internal float a = 1f;

		// Token: 0x04004091 RID: 16529
		internal MeshAttachment parentMesh;

		// Token: 0x04004092 RID: 16530
		internal bool inheritFFD;
	}
}
