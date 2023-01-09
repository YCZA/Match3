using System;
using UnityEngine;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001E7 RID: 487
	public class TranslateTimeline : CurveTimeline
	{
		// Token: 0x06000E3A RID: 3642 RVA: 0x00021F55 File Offset: 0x00020355
		public TranslateTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 3];
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00021F6C File Offset: 0x0002036C
		// (set) Token: 0x06000E3C RID: 3644 RVA: 0x00021F74 File Offset: 0x00020374
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

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00021F7D File Offset: 0x0002037D
		// (set) Token: 0x06000E3E RID: 3646 RVA: 0x00021F85 File Offset: 0x00020385
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

		// Token: 0x06000E3F RID: 3647 RVA: 0x00021F8E File Offset: 0x0002038E
		public void SetFrame(int frameIndex, float time, float x, float y)
		{
			frameIndex *= 3;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = x;
			this.frames[frameIndex + 2] = y;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00021FB8 File Offset: 0x000203B8
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			if (this.__bakedFrames == null)
			{
				this.Bake();
			}
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			Bone bone = skeleton.bones.Items[this.boneIndex];
			if (time >= array[array.Length - 3])
			{
				bone.x += (bone.data.x + array[array.Length - 2] - bone.x) * alpha;
				bone.y += (bone.data.y + array[array.Length - 1] - bone.y) * alpha;
				return;
			}
			int num = Math.Min((int)(time * 30f), this.__bakedFrames.Length - 1);
			Vector2 vector = this.__bakedFrames[num];
			bone.x += (bone.data.x + vector.x - bone.x) * alpha;
			bone.y += (bone.data.y + vector.y - bone.y) * alpha;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x000220DC File Offset: 0x000204DC
		public void Bake()
		{
			int num = (int)(this.frames[this.frames.Length - 3] * 30f);
			this.__bakedFrames = new Vector2[num];
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 30f;
				if (num2 >= this.frames[0])
				{
					if (num2 < this.frames[this.frames.Length - 3])
					{
						int num3 = Animation.binarySearch(this.frames, num2, 3);
						float num4 = this.frames[num3 - 2];
						float num5 = this.frames[num3 - 1];
						float num6 = this.frames[num3];
						float num7 = 1f - (num2 - num6) / (this.frames[num3 + -3] - num6);
						num7 = base.GetCurvePercent(num3 / 3 - 1, (num7 >= 0f) ? ((num7 <= 1f) ? num7 : 1f) : 0f);
						this.__bakedFrames[i] = new Vector2(num4 + (this.frames[num3 + 1] - num4) * num7, num5 + (this.frames[num3 + 2] - num5) * num7);
					}
				}
			}
		}

		// Token: 0x04003FFB RID: 16379
		protected const int PREV_TIME = -3;

		// Token: 0x04003FFC RID: 16380
		protected const int X = 1;

		// Token: 0x04003FFD RID: 16381
		protected const int Y = 2;

		// Token: 0x04003FFE RID: 16382
		internal int boneIndex;

		// Token: 0x04003FFF RID: 16383
		internal float[] frames;

		// Token: 0x04004000 RID: 16384
		protected Vector2[] __bakedFrames;
	}
}
