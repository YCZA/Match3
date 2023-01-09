namespace Match3.Scripts1.Spine
{
	// Token: 0x020001EC RID: 492
	public class EventTimeline : Timeline
	{
		// Token: 0x06000E57 RID: 3671 RVA: 0x00022800 File Offset: 0x00020C00
		public EventTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.events = new Event[frameCount];
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x00022820 File Offset: 0x00020C20
		// (set) Token: 0x06000E59 RID: 3673 RVA: 0x00022828 File Offset: 0x00020C28
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

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00022831 File Offset: 0x00020C31
		// (set) Token: 0x06000E5B RID: 3675 RVA: 0x00022839 File Offset: 0x00020C39
		public Event[] Events
		{
			get
			{
				return this.events;
			}
			set
			{
				this.events = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x00022842 File Offset: 0x00020C42
		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0002284C File Offset: 0x00020C4C
		public void SetFrame(int frameIndex, Event e)
		{
			this.frames[frameIndex] = e.Time;
			this.events[frameIndex] = e;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00022868 File Offset: 0x00020C68
		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			if (firedEvents == null)
			{
				return;
			}
			float[] array = this.frames;
			int num = array.Length;
			if (lastTime > time)
			{
				this.Apply(skeleton, lastTime, 2.1474836E+09f, firedEvents, alpha);
				lastTime = -1f;
			}
			else if (lastTime >= array[num - 1])
			{
				return;
			}
			if (time < array[0])
			{
				return;
			}
			int i;
			if (lastTime < array[0])
			{
				i = 0;
			}
			else
			{
				i = Animation.binarySearch(array, lastTime);
				float num2 = array[i];
				while (i > 0)
				{
					if (array[i - 1] != num2)
					{
						break;
					}
					i--;
				}
			}
			while (i < num && time >= array[i])
			{
				firedEvents.Add(this.events[i]);
				i++;
			}
		}

		// Token: 0x0400400B RID: 16395
		internal float[] frames;

		// Token: 0x0400400C RID: 16396
		private Event[] events;
	}
}
