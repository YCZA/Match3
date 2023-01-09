using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001EE RID: 494
	public class FfdTimeline : CurveTimeline
	{
		// Token: 0x06000E67 RID: 3687 RVA: 0x00022A53 File Offset: 0x00020E53
		public FfdTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount];
			this.frameVertices = new float[frameCount][];
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00022A74 File Offset: 0x00020E74
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x00022A7C File Offset: 0x00020E7C
		public int SlotIndex
		{
			get
			{
				return this.slotIndex;
			}
			set
			{
				this.slotIndex = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x00022A85 File Offset: 0x00020E85
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x00022A8D File Offset: 0x00020E8D
		public float[] Frames
		{
			get
			{
				return this.frames;
			}
			set
			{
				this.frames = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x00022A96 File Offset: 0x00020E96
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x00022A9E File Offset: 0x00020E9E
		public float[][] Vertices
		{
			get
			{
				return this.frameVertices;
			}
			set
			{
				this.frameVertices = value;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00022AA7 File Offset: 0x00020EA7
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x00022AAF File Offset: 0x00020EAF
		public Attachment Attachment
		{
			get
			{
				return this.attachment;
			}
			set
			{
				this.attachment = value;
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00022AB8 File Offset: 0x00020EB8
		public void SetFrame(int frameIndex, float time, float[] vertices)
		{
			this.frames[frameIndex] = time;
			this.frameVertices[frameIndex] = vertices;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00022ACC File Offset: 0x00020ECC
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			Slot slot = skeleton.slots.Items[this.slotIndex];
			IFfdAttachment ffdAttachment = slot.attachment as IFfdAttachment;
			if (ffdAttachment == null || !ffdAttachment.ApplyFFD(this.attachment))
			{
				return;
			}
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			float[][] array2 = this.frameVertices;
			int num = array2[0].Length;
			float[] array3 = slot.attachmentVertices;
			if (slot.attachmentVerticesCount != num)
			{
				alpha = 1f;
			}
			if (array3.Length < num)
			{
				array3 = new float[num];
				slot.attachmentVertices = array3;
			}
			slot.attachmentVerticesCount = num;
			if (time >= array[array.Length - 1])
			{
				float[] array4 = array2[array.Length - 1];
				if (alpha < 1f)
				{
					for (int i = 0; i < num; i++)
					{
						float num2 = array3[i];
						array3[i] = num2 + (array4[i] - num2) * alpha;
					}
				}
				else
				{
					Array.Copy(array4, 0, array3, 0, num);
				}
				return;
			}
			int num3 = Animation.binarySearch(array, time);
			float num4 = array[num3];
			float num5 = 1f - (time - num4) / (array[num3 - 1] - num4);
			num5 = base.GetCurvePercent(num3 - 1, (num5 >= 0f) ? ((num5 <= 1f) ? num5 : 1f) : 0f);
			float[] array5 = array2[num3 - 1];
			float[] array6 = array2[num3];
			if (alpha < 1f)
			{
				for (int j = 0; j < num; j++)
				{
					float num6 = array5[j];
					float num7 = array3[j];
					array3[j] = num7 + (num6 + (array6[j] - num6) * num5 - num7) * alpha;
				}
			}
			else
			{
				for (int k = 0; k < num; k++)
				{
					float num8 = array5[k];
					array3[k] = num8 + (array6[k] - num8) * num5;
				}
			}
		}

		// Token: 0x0400400F RID: 16399
		internal int slotIndex;

		// Token: 0x04004010 RID: 16400
		internal float[] frames;

		// Token: 0x04004011 RID: 16401
		private float[][] frameVertices;

		// Token: 0x04004012 RID: 16402
		internal Attachment attachment;
	}
}
