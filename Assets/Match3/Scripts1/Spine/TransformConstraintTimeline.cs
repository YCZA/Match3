namespace Match3.Scripts1.Spine
{
	// Token: 0x020001F0 RID: 496
	public class TransformConstraintTimeline : CurveTimeline
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x00022E29 File Offset: 0x00021229
		public TransformConstraintTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 5];
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x00022E40 File Offset: 0x00021240
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x00022E48 File Offset: 0x00021248
		public int TransformConstraintIndex
		{
			get
			{
				return this.transformConstraintIndex;
			}
			set
			{
				this.transformConstraintIndex = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00022E51 File Offset: 0x00021251
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x00022E59 File Offset: 0x00021259
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

		// Token: 0x06000E7E RID: 3710 RVA: 0x00022E62 File Offset: 0x00021262
		public void SetFrame(int frameIndex, float time, float rotateMix, float translateMix, float scaleMix, float shearMix)
		{
			frameIndex *= 5;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = rotateMix;
			this.frames[frameIndex + 2] = translateMix;
			this.frames[frameIndex + 3] = scaleMix;
			this.frames[frameIndex + 4] = shearMix;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00022EA4 File Offset: 0x000212A4
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			TransformConstraint transformConstraint = skeleton.transformConstraints.Items[this.transformConstraintIndex];
			if (time >= array[array.Length - 5])
			{
				int num = array.Length - 1;
				transformConstraint.rotateMix += (array[num - 3] - transformConstraint.rotateMix) * alpha;
				transformConstraint.translateMix += (array[num - 2] - transformConstraint.translateMix) * alpha;
				transformConstraint.scaleMix += (array[num - 1] - transformConstraint.scaleMix) * alpha;
				transformConstraint.shearMix += (array[num] - transformConstraint.shearMix) * alpha;
				return;
			}
			int num2 = Animation.binarySearch(array, time, 5);
			float num3 = array[num2];
			float num4 = 1f - (time - num3) / (array[num2 + -5] - num3);
			num4 = base.GetCurvePercent(num2 / 5 - 1, (num4 >= 0f) ? ((num4 <= 1f) ? num4 : 1f) : 0f);
			float num5 = array[num2 + -4];
			float num6 = array[num2 + -3];
			float num7 = array[num2 + -2];
			float num8 = array[num2 + -1];
			transformConstraint.rotateMix += (num5 + (array[num2 + 1] - num5) * num4 - transformConstraint.rotateMix) * alpha;
			transformConstraint.translateMix += (num6 + (array[num2 + 2] - num6) * num4 - transformConstraint.translateMix) * alpha;
			transformConstraint.scaleMix += (num7 + (array[num2 + 3] - num7) * num4 - transformConstraint.scaleMix) * alpha;
			transformConstraint.shearMix += (num8 + (array[num2 + 4] - num8) * num4 - transformConstraint.shearMix) * alpha;
		}

		// Token: 0x04004019 RID: 16409
		private const int PREV_TIME = -5;

		// Token: 0x0400401A RID: 16410
		private const int PREV_ROTATE_MIX = -4;

		// Token: 0x0400401B RID: 16411
		private const int PREV_TRANSLATE_MIX = -3;

		// Token: 0x0400401C RID: 16412
		private const int PREV_SCALE_MIX = -2;

		// Token: 0x0400401D RID: 16413
		private const int PREV_SHEAR_MIX = -1;

		// Token: 0x0400401E RID: 16414
		private const int ROTATE_MIX = 1;

		// Token: 0x0400401F RID: 16415
		private const int TRANSLATE_MIX = 2;

		// Token: 0x04004020 RID: 16416
		private const int SCALE_MIX = 3;

		// Token: 0x04004021 RID: 16417
		private const int SHEAR_MIX = 4;

		// Token: 0x04004022 RID: 16418
		internal int transformConstraintIndex;

		// Token: 0x04004023 RID: 16419
		internal float[] frames;
	}
}
