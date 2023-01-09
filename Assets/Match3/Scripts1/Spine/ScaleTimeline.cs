using System;
using UnityEngine;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001E8 RID: 488
	public class ScaleTimeline : TranslateTimeline
	{
		// Token: 0x06000E42 RID: 3650 RVA: 0x0002221A File Offset: 0x0002061A
		public ScaleTimeline(int frameCount) : base(frameCount)
		{
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00022224 File Offset: 0x00020624
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			if (this.__bakedFrames == null)
			{
				base.Bake();
			}
			float[] frames = this.frames;
			if (time < frames[0])
			{
				return;
			}
			Bone bone = skeleton.bones.Items[this.boneIndex];
			if (time >= frames[frames.Length - 3])
			{
				bone.scaleX += (bone.data.scaleX * frames[frames.Length - 2] - bone.scaleX) * alpha;
				bone.scaleY += (bone.data.scaleY * frames[frames.Length - 1] - bone.scaleY) * alpha;
				return;
			}
			int num = Math.Min((int)(time * 30f), this.__bakedFrames.Length - 1);
			Vector2 vector = this.__bakedFrames[num];
			bone.scaleX += (bone.data.scaleX * vector.x - bone.scaleX) * alpha;
			bone.scaleY += (bone.data.scaleY * vector.y - bone.scaleY) * alpha;
		}
	}
}
