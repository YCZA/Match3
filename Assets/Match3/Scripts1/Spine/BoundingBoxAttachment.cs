namespace Match3.Scripts1.Spine
{
	// Token: 0x02000205 RID: 517
	public class BoundingBoxAttachment : Attachment
	{
		// Token: 0x06000EE7 RID: 3815 RVA: 0x0002479F File Offset: 0x00022B9F
		public BoundingBoxAttachment(string name) : base(name)
		{
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x000247A8 File Offset: 0x00022BA8
		// (set) Token: 0x06000EE9 RID: 3817 RVA: 0x000247B0 File Offset: 0x00022BB0
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

		// Token: 0x06000EEA RID: 3818 RVA: 0x000247BC File Offset: 0x00022BBC
		public void ComputeWorldVertices(Bone bone, float[] worldVertices)
		{
			float num = bone.skeleton.x + bone.worldX;
			float num2 = bone.skeleton.y + bone.worldY;
			float a = bone.a;
			float b = bone.b;
			float c = bone.c;
			float d = bone.d;
			float[] array = this.vertices;
			int i = 0;
			int num3 = array.Length;
			while (i < num3)
			{
				float num4 = array[i];
				float num5 = array[i + 1];
				worldVertices[i] = num4 * a + num5 * b + num;
				worldVertices[i + 1] = num4 * c + num5 * d + num2;
				i += 2;
			}
		}

		// Token: 0x04004082 RID: 16514
		internal float[] vertices;
	}
}
