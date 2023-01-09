namespace Match3.Scripts1.Spine
{
	// Token: 0x020001ED RID: 493
	public class DrawOrderTimeline : Timeline
	{
		// Token: 0x06000E5F RID: 3679 RVA: 0x00022926 File Offset: 0x00020D26
		public DrawOrderTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.drawOrders = new int[frameCount][];
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00022946 File Offset: 0x00020D46
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x0002294E File Offset: 0x00020D4E
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

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x00022957 File Offset: 0x00020D57
		// (set) Token: 0x06000E63 RID: 3683 RVA: 0x0002295F File Offset: 0x00020D5F
		public int[][] DrawOrders
		{
			get
			{
				return this.drawOrders;
			}
			set
			{
				this.drawOrders = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x00022968 File Offset: 0x00020D68
		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00022972 File Offset: 0x00020D72
		public void SetFrame(int frameIndex, float time, int[] drawOrder)
		{
			this.frames[frameIndex] = time;
			this.drawOrders[frameIndex] = drawOrder;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00022988 File Offset: 0x00020D88
		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			int num;
			if (time >= array[array.Length - 1])
			{
				num = array.Length - 1;
			}
			else
			{
				num = Animation.binarySearch(array, time) - 1;
			}
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			ExposedList<Slot> slots = skeleton.slots;
			int[] array2 = this.drawOrders[num];
			if (array2 == null)
			{
				drawOrder.Clear(true);
				int i = 0;
				int count = slots.Count;
				while (i < count)
				{
					drawOrder.Add(slots.Items[i]);
					i++;
				}
			}
			else
			{
				int j = 0;
				int num2 = array2.Length;
				while (j < num2)
				{
					drawOrder.Items[j] = slots.Items[array2[j]];
					j++;
				}
			}
		}

		// Token: 0x0400400D RID: 16397
		internal float[] frames;

		// Token: 0x0400400E RID: 16398
		private int[][] drawOrders;
	}
}
