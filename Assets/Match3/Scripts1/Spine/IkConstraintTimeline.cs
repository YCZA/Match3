namespace Match3.Scripts1.Spine
{
	// Token: 0x020001EF RID: 495
	public class IkConstraintTimeline : CurveTimeline
	{
		// Token: 0x06000E72 RID: 3698 RVA: 0x00022CC5 File Offset: 0x000210C5
		public IkConstraintTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 3];
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00022CDC File Offset: 0x000210DC
		// (set) Token: 0x06000E74 RID: 3700 RVA: 0x00022CE4 File Offset: 0x000210E4
		public int IkConstraintIndex
		{
			get
			{
				return this.ikConstraintIndex;
			}
			set
			{
				this.ikConstraintIndex = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00022CED File Offset: 0x000210ED
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x00022CF5 File Offset: 0x000210F5
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

		// Token: 0x06000E77 RID: 3703 RVA: 0x00022CFE File Offset: 0x000210FE
		public void SetFrame(int frameIndex, float time, float mix, int bendDirection)
		{
			frameIndex *= 3;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = mix;
			this.frames[frameIndex + 2] = (float)bendDirection;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00022D28 File Offset: 0x00021128
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			IkConstraint ikConstraint = skeleton.ikConstraints.Items[this.ikConstraintIndex];
			if (time >= array[array.Length - 3])
			{
				ikConstraint.mix += (array[array.Length + -2] - ikConstraint.mix) * alpha;
				ikConstraint.bendDirection = (int)array[array.Length + -1];
				return;
			}
			int num = Animation.binarySearch(array, time, 3);
			float num2 = array[num];
			float num3 = 1f - (time - num2) / (array[num + -3] - num2);
			num3 = base.GetCurvePercent(num / 3 - 1, (num3 >= 0f) ? ((num3 <= 1f) ? num3 : 1f) : 0f);
			float num4 = array[num + -2];
			ikConstraint.mix += (num4 + (array[num + 1] - num4) * num3 - ikConstraint.mix) * alpha;
			ikConstraint.bendDirection = (int)array[num + -1];
		}

		// Token: 0x04004013 RID: 16403
		private const int PREV_TIME = -3;

		// Token: 0x04004014 RID: 16404
		private const int PREV_MIX = -2;

		// Token: 0x04004015 RID: 16405
		private const int PREV_BEND_DIRECTION = -1;

		// Token: 0x04004016 RID: 16406
		private const int MIX = 1;

		// Token: 0x04004017 RID: 16407
		internal int ikConstraintIndex;

		// Token: 0x04004018 RID: 16408
		internal float[] frames;
	}
}
