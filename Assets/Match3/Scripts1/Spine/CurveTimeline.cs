namespace Match3.Scripts1.Spine
{
	// Token: 0x020001E5 RID: 485
	public abstract class CurveTimeline : Timeline
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x00021A15 File Offset: 0x0001FE15
		public CurveTimeline(int frameCount)
		{
			this.curves = new float[(frameCount - 1) * 19];
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x00021A2E File Offset: 0x0001FE2E
		public int FrameCount
		{
			get
			{
				return this.curves.Length / 19 + 1;
			}
		}

		// Token: 0x06000E2C RID: 3628
		public abstract void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha);

		// Token: 0x06000E2D RID: 3629 RVA: 0x00021A3D File Offset: 0x0001FE3D
		public void SetLinear(int frameIndex)
		{
			this.curves[frameIndex * 19] = 0f;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00021A4F File Offset: 0x0001FE4F
		public void SetStepped(int frameIndex)
		{
			this.curves[frameIndex * 19] = 1f;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00021A64 File Offset: 0x0001FE64
		public void SetCurve(int frameIndex, float cx1, float cy1, float cx2, float cy2)
		{
			float num = 0.1f;
			float num2 = num * num;
			float num3 = num2 * num;
			float num4 = 3f * num;
			float num5 = 3f * num2;
			float num6 = 6f * num2;
			float num7 = 6f * num3;
			float num8 = -cx1 * 2f + cx2;
			float num9 = -cy1 * 2f + cy2;
			float num10 = (cx1 - cx2) * 3f + 1f;
			float num11 = (cy1 - cy2) * 3f + 1f;
			float num12 = cx1 * num4 + num8 * num5 + num10 * num3;
			float num13 = cy1 * num4 + num9 * num5 + num11 * num3;
			float num14 = num8 * num6 + num10 * num7;
			float num15 = num9 * num6 + num11 * num7;
			float num16 = num10 * num7;
			float num17 = num11 * num7;
			int i = frameIndex * 19;
			float[] array = this.curves;
			array[i++] = 2f;
			float num18 = num12;
			float num19 = num13;
			int num20 = i + 19 - 1;
			while (i < num20)
			{
				array[i] = num18;
				array[i + 1] = num19;
				num12 += num14;
				num13 += num15;
				num14 += num16;
				num15 += num17;
				num18 += num12;
				num19 += num13;
				i += 2;
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00021BA4 File Offset: 0x0001FFA4
		public float GetCurvePercent(int frameIndex, float percent)
		{
			float[] array = this.curves;
			int i = frameIndex * 19;
			float num = array[i];
			if (num == 0f)
			{
				return percent;
			}
			if (num == 1f)
			{
				return 0f;
			}
			i++;
			float num2 = 0f;
			int num3 = i;
			int num4 = i + 19 - 1;
			while (i < num4)
			{
				num2 = array[i];
				if (num2 >= percent)
				{
					float num5;
					float num6;
					if (i == num3)
					{
						num5 = 0f;
						num6 = 0f;
					}
					else
					{
						num5 = array[i - 2];
						num6 = array[i - 1];
					}
					return num6 + (array[i + 1] - num6) * (percent - num5) / (num2 - num5);
				}
				i += 2;
			}
			float num7 = array[i - 1];
			return num7 + (1f - num7) * (percent - num2) / (1f - num2);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00021C6D File Offset: 0x0002006D
		public float GetCurveType(int frameIndex)
		{
			return this.curves[frameIndex * 19];
		}

		// Token: 0x04003FF0 RID: 16368
		protected const float LINEAR = 0f;

		// Token: 0x04003FF1 RID: 16369
		protected const float STEPPED = 1f;

		// Token: 0x04003FF2 RID: 16370
		protected const float BEZIER = 2f;

		// Token: 0x04003FF3 RID: 16371
		protected const int BEZIER_SEGMENTS = 10;

		// Token: 0x04003FF4 RID: 16372
		protected const int BEZIER_SIZE = 19;

		// Token: 0x04003FF5 RID: 16373
		private float[] curves;
	}
}
