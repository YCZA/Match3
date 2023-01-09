using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001E6 RID: 486
	public class RotateTimeline : CurveTimeline
	{
		// Token: 0x06000E32 RID: 3634 RVA: 0x00021C7A File Offset: 0x0002007A
		public RotateTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount << 1];
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x00021C91 File Offset: 0x00020091
		// (set) Token: 0x06000E34 RID: 3636 RVA: 0x00021C99 File Offset: 0x00020099
		public int BoneIndex
		{
			get
			{
				return this.boneIndex;
			}
			set
			{
				this.boneIndex = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00021CA2 File Offset: 0x000200A2
		// (set) Token: 0x06000E36 RID: 3638 RVA: 0x00021CAA File Offset: 0x000200AA
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

		// Token: 0x06000E37 RID: 3639 RVA: 0x00021CB3 File Offset: 0x000200B3
		public void SetFrame(int frameIndex, float time, float angle)
		{
			frameIndex *= 2;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = angle;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00021CD0 File Offset: 0x000200D0
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			if (this.__bakedFrames == null)
			{
				this.Bake();
			}
			if (time < this.frames[0])
			{
				return;
			}
			Bone bone = skeleton.bones.Items[this.boneIndex];
			float num;
			if (time >= this.frames[this.frames.Length - 2])
			{
				for (num = bone.data.rotation + this.frames[this.frames.Length - 1] - bone.rotation; num > 180f; num -= 360f)
				{
				}
				while (num < -180f)
				{
					num += 360f;
				}
				bone.rotation += num * alpha;
				return;
			}
			int num2 = Math.Min((int)(time * 30f), this.__bakedFrames.Length - 1);
			for (num = bone.data.rotation + this.__bakedFrames[num2] - bone.rotation; num > 180f; num -= 360f)
			{
			}
			while (num < -180f)
			{
				num += 360f;
			}
			bone.rotation += num * alpha;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00021E04 File Offset: 0x00020204
		private void Bake()
		{
			int num = (int)(this.frames[this.frames.Length - 2] * 30f);
			this.__bakedFrames = new float[num];
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 30f;
				if (num2 >= this.frames[0])
				{
					if (num2 < this.frames[this.frames.Length - 2])
					{
						int num3 = Animation.binarySearch(this.frames, num2, 2);
						float num4 = this.frames[Math.Max(num3 - 1, 0)];
						float num5 = this.frames[num3];
						float num6 = 1f - (num2 - num5) / (this.frames[num3 + -2] - num5);
						num6 = base.GetCurvePercent((num3 >> 1) - 1, (num6 >= 0f) ? ((num6 <= 1f) ? num6 : 1f) : 0f);
						float num7;
						for (num7 = this.frames[num3 + 1] - num4; num7 > 180f; num7 -= 360f)
						{
						}
						while (num7 < -180f)
						{
							num7 += 360f;
						}
						this.__bakedFrames[i] = num4 + num7 * num6;
					}
				}
			}
		}

		// Token: 0x04003FF6 RID: 16374
		internal const int PREV_TIME = -2;

		// Token: 0x04003FF7 RID: 16375
		internal const int VALUE = 1;

		// Token: 0x04003FF8 RID: 16376
		internal int boneIndex;

		// Token: 0x04003FF9 RID: 16377
		internal float[] frames;

		// Token: 0x04003FFA RID: 16378
		protected float[] __bakedFrames;
	}
}
