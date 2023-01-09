namespace Match3.Scripts1.Spine
{
	// Token: 0x020001EA RID: 490
	public class ColorTimeline : CurveTimeline
	{
		// Token: 0x06000E46 RID: 3654 RVA: 0x000224AF File Offset: 0x000208AF
		public ColorTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 5];
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x000224C6 File Offset: 0x000208C6
		// (set) Token: 0x06000E48 RID: 3656 RVA: 0x000224CE File Offset: 0x000208CE
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x000224D7 File Offset: 0x000208D7
		// (set) Token: 0x06000E4A RID: 3658 RVA: 0x000224DF File Offset: 0x000208DF
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

		// Token: 0x06000E4B RID: 3659 RVA: 0x000224E8 File Offset: 0x000208E8
		public void SetFrame(int frameIndex, float time, float r, float g, float b, float a)
		{
			frameIndex *= 5;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = r;
			this.frames[frameIndex + 2] = g;
			this.frames[frameIndex + 3] = b;
			this.frames[frameIndex + 4] = a;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00022528 File Offset: 0x00020928
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			float num2;
			float num3;
			float num4;
			float num5;
			if (time >= array[array.Length - 5])
			{
				int num = array.Length - 1;
				num2 = array[num - 3];
				num3 = array[num - 2];
				num4 = array[num - 1];
				num5 = array[num];
			}
			else
			{
				int num6 = Animation.binarySearch(array, time, 5);
				float num7 = array[num6];
				float num8 = 1f - (time - num7) / (array[num6 + -5] - num7);
				num8 = base.GetCurvePercent(num6 / 5 - 1, (num8 >= 0f) ? ((num8 <= 1f) ? num8 : 1f) : 0f);
				num2 = array[num6 - 4];
				num3 = array[num6 - 3];
				num4 = array[num6 - 2];
				num5 = array[num6 - 1];
				num2 += (array[num6 + 1] - num2) * num8;
				num3 += (array[num6 + 2] - num3) * num8;
				num4 += (array[num6 + 3] - num4) * num8;
				num5 += (array[num6 + 4] - num5) * num8;
			}
			Slot slot = skeleton.slots.Items[this.slotIndex];
			if (alpha < 1f)
			{
				slot.r += (num2 - slot.r) * alpha;
				slot.g += (num3 - slot.g) * alpha;
				slot.b += (num4 - slot.b) * alpha;
				slot.a += (num5 - slot.a) * alpha;
			}
			else
			{
				slot.r = num2;
				slot.g = num3;
				slot.b = num4;
				slot.a = num5;
			}
		}

		// Token: 0x04004001 RID: 16385
		protected const int PREV_TIME = -5;

		// Token: 0x04004002 RID: 16386
		protected const int R = 1;

		// Token: 0x04004003 RID: 16387
		protected const int G = 2;

		// Token: 0x04004004 RID: 16388
		protected const int B = 3;

		// Token: 0x04004005 RID: 16389
		protected const int A = 4;

		// Token: 0x04004006 RID: 16390
		internal int slotIndex;

		// Token: 0x04004007 RID: 16391
		internal float[] frames;
	}
}
