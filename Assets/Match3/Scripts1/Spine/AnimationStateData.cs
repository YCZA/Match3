using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001F6 RID: 502
	public class AnimationStateData
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x00023CDE File Offset: 0x000220DE
		public AnimationStateData(SkeletonData skeletonData)
		{
			this.skeletonData = skeletonData;
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x00023CFD File Offset: 0x000220FD
		public SkeletonData SkeletonData
		{
			get
			{
				return this.skeletonData;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00023D05 File Offset: 0x00022105
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x00023D0D File Offset: 0x0002210D
		public float DefaultMix
		{
			get
			{
				return this.defaultMix;
			}
			set
			{
				this.defaultMix = value;
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00023D18 File Offset: 0x00022118
		public void SetMix(string fromName, string toName, float duration)
		{
			Animation animation = this.skeletonData.FindAnimation(fromName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + fromName);
			}
			Animation animation2 = this.skeletonData.FindAnimation(toName);
			if (animation2 == null)
			{
				throw new ArgumentException("Animation not found: " + toName);
			}
			this.SetMix(animation, animation2, duration);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00023D78 File Offset: 0x00022178
		public void SetMix(Animation from, Animation to, float duration)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from cannot be null.");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to cannot be null.");
			}
			AnimationStateData.AnimationPair key = new AnimationStateData.AnimationPair(from, to);
			this.animationToMixTime.Remove(key);
			this.animationToMixTime.Add(key, duration);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00023DCC File Offset: 0x000221CC
		public float GetMix(Animation from, Animation to)
		{
			AnimationStateData.AnimationPair key = new AnimationStateData.AnimationPair(from, to);
			float result;
			if (this.animationToMixTime.TryGetValue(key, out result))
			{
				return result;
			}
			return this.defaultMix;
		}

		// Token: 0x0400403C RID: 16444
		internal SkeletonData skeletonData;

		// Token: 0x0400403D RID: 16445
		private Dictionary<AnimationStateData.AnimationPair, float> animationToMixTime = new Dictionary<AnimationStateData.AnimationPair, float>(AnimationStateData.AnimationPairComparer.Instance);

		// Token: 0x0400403E RID: 16446
		internal float defaultMix;

		// Token: 0x020001F7 RID: 503
		private struct AnimationPair
		{
			// Token: 0x06000EC8 RID: 3784 RVA: 0x00023DFD File Offset: 0x000221FD
			public AnimationPair(Animation a1, Animation a2)
			{
				this.a1 = a1;
				this.a2 = a2;
			}

			// Token: 0x0400403F RID: 16447
			public readonly Animation a1;

			// Token: 0x04004040 RID: 16448
			public readonly Animation a2;
		}

		// Token: 0x020001F8 RID: 504
		private class AnimationPairComparer : IEqualityComparer<AnimationStateData.AnimationPair>
		{
			// Token: 0x06000ECA RID: 3786 RVA: 0x00023E15 File Offset: 0x00022215
			bool IEqualityComparer<AnimationStateData.AnimationPair>.Equals(AnimationStateData.AnimationPair x, AnimationStateData.AnimationPair y)
			{
				return object.ReferenceEquals(x.a1, y.a1) && object.ReferenceEquals(x.a2, y.a2);
			}

			// Token: 0x06000ECB RID: 3787 RVA: 0x00023E48 File Offset: 0x00022248
			int IEqualityComparer<AnimationStateData.AnimationPair>.GetHashCode(AnimationStateData.AnimationPair obj)
			{
				int hashCode = obj.a1.GetHashCode();
				return (hashCode << 5) + hashCode ^ obj.a2.GetHashCode();
			}

			// Token: 0x04004041 RID: 16449
			internal static readonly AnimationStateData.AnimationPairComparer Instance = new AnimationStateData.AnimationPairComparer();
		}
	}
}
