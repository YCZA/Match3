using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001E3 RID: 483
	public class Animation
	{
		// Token: 0x06000E1E RID: 3614 RVA: 0x000217C4 File Offset: 0x0001FBC4
		public Animation(string name, ExposedList<Timeline> timelines, float duration)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			if (timelines == null)
			{
				throw new ArgumentNullException("timelines cannot be null.");
			}
			this.name = name;
			this.timelines = timelines;
			this.duration = duration;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00021803 File Offset: 0x0001FC03
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0002180B File Offset: 0x0001FC0B
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x00021813 File Offset: 0x0001FC13
		public ExposedList<Timeline> Timelines
		{
			get
			{
				return this.timelines;
			}
			set
			{
				this.timelines = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0002181C File Offset: 0x0001FC1C
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x00021824 File Offset: 0x0001FC24
		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00021830 File Offset: 0x0001FC30
		public void Apply(Skeleton skeleton, float lastTime, float time, bool loop, ExposedList<Event> events)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			if (loop && this.duration != 0f)
			{
				time %= this.duration;
				if (lastTime > 0f)
				{
					lastTime %= this.duration;
				}
			}
			ExposedList<Timeline> exposedList = this.timelines;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				exposedList.Items[i].Apply(skeleton, lastTime, time, events, 1f);
				i++;
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000218BC File Offset: 0x0001FCBC
		public void Mix(Skeleton skeleton, float lastTime, float time, bool loop, ExposedList<Event> events, float alpha)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			if (loop && this.duration != 0f)
			{
				time %= this.duration;
				if (lastTime > 0f)
				{
					lastTime %= this.duration;
				}
			}
			ExposedList<Timeline> exposedList = this.timelines;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				exposedList.Items[i].Apply(skeleton, lastTime, time, events, alpha);
				i++;
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00021944 File Offset: 0x0001FD44
		internal static int binarySearch(float[] values, float target, int step)
		{
			int num = 0;
			int num2 = values.Length / step - 2;
			if (num2 == 0)
			{
				return step;
			}
			int num3 = (int)((uint)num2 >> 1);
			for (;;)
			{
				if (values[(num3 + 1) * step] <= target)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
				if (num == num2)
				{
					break;
				}
				num3 = (int)((uint)(num + num2) >> 1);
			}
			return (num + 1) * step;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00021998 File Offset: 0x0001FD98
		internal static int binarySearch(float[] values, float target)
		{
			int num = 0;
			int num2 = values.Length - 2;
			if (num2 == 0)
			{
				return 1;
			}
			int num3 = (int)((uint)num2 >> 1);
			for (;;)
			{
				if (values[num3 + 1] <= target)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
				if (num == num2)
				{
					break;
				}
				num3 = (int)((uint)(num + num2) >> 1);
			}
			return num + 1;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000219E4 File Offset: 0x0001FDE4
		internal static int linearSearch(float[] values, float target, int step)
		{
			int i = 0;
			int num = values.Length - step;
			while (i <= num)
			{
				if (values[i] > target)
				{
					return i;
				}
				i += step;
			}
			return -1;
		}

		// Token: 0x04003FEC RID: 16364
		internal ExposedList<Timeline> timelines;

		// Token: 0x04003FED RID: 16365
		internal float duration;

		// Token: 0x04003FEE RID: 16366
		internal string name;

		// Token: 0x04003FEF RID: 16367
		public const int FPS = 30;
	}
}
