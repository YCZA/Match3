namespace Match3.Scripts1.Spine
{
	// Token: 0x020001E9 RID: 489
	public class ShearTimeline : TranslateTimeline
	{
		// Token: 0x06000E44 RID: 3652 RVA: 0x00022346 File Offset: 0x00020746
		public ShearTimeline(int frameCount) : base(frameCount)
		{
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00022350 File Offset: 0x00020750
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] frames = this.frames;
			if (time < frames[0])
			{
				return;
			}
			Bone bone = skeleton.bones.Items[this.boneIndex];
			if (time >= frames[frames.Length - 3])
			{
				bone.shearX += (bone.data.shearX + frames[frames.Length - 2] - bone.shearX) * alpha;
				bone.shearY += (bone.data.shearY + frames[frames.Length - 1] - bone.shearY) * alpha;
				return;
			}
			int num = Animation.binarySearch(frames, time, 3);
			float num2 = frames[num - 2];
			float num3 = frames[num - 1];
			float num4 = frames[num];
			float num5 = 1f - (time - num4) / (frames[num + -3] - num4);
			num5 = base.GetCurvePercent(num / 3 - 1, (num5 >= 0f) ? ((num5 <= 1f) ? num5 : 1f) : 0f);
			bone.shearX += (bone.data.shearX + (num2 + (frames[num + 1] - num2) * num5) - bone.shearX) * alpha;
			bone.shearY += (bone.data.shearY + (num3 + (frames[num + 2] - num3) * num5) - bone.shearY) * alpha;
		}
	}
}
