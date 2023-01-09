using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000257 RID: 599
	[RequireComponent(typeof(Animator))]
	public class SkeletonAnimator : SkeletonRendererCPU, ISkeletonAnimation
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001274 RID: 4724 RVA: 0x0003A0F7 File Offset: 0x000384F7
		// (remove) Token: 0x06001275 RID: 4725 RVA: 0x0003A100 File Offset: 0x00038500
		public event UpdateBonesDelegate UpdateLocal;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06001276 RID: 4726 RVA: 0x0003A109 File Offset: 0x00038509
		// (remove) Token: 0x06001277 RID: 4727 RVA: 0x0003A112 File Offset: 0x00038512
		public event UpdateBonesDelegate UpdateWorld;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001278 RID: 4728 RVA: 0x0003A11B File Offset: 0x0003851B
		// (remove) Token: 0x06001279 RID: 4729 RVA: 0x0003A124 File Offset: 0x00038524
		public event UpdateBonesDelegate UpdateComplete;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600127A RID: 4730 RVA: 0x0003A130 File Offset: 0x00038530
		// (remove) Token: 0x0600127B RID: 4731 RVA: 0x0003A168 File Offset: 0x00038568
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UpdateBonesDelegate _UpdateLocal;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600127C RID: 4732 RVA: 0x0003A1A0 File Offset: 0x000385A0
		// (remove) Token: 0x0600127D RID: 4733 RVA: 0x0003A1D8 File Offset: 0x000385D8
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UpdateBonesDelegate _UpdateWorld;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600127E RID: 4734 RVA: 0x0003A210 File Offset: 0x00038610
		// (remove) Token: 0x0600127F RID: 4735 RVA: 0x0003A248 File Offset: 0x00038648
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UpdateBonesDelegate _UpdateComplete;

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0003A27E File Offset: 0x0003867E
		public Skeleton Skeleton
		{
			get
			{
				return this.skeleton;
			}
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0003A288 File Offset: 0x00038688
		public override void Initialize(bool overwrite)
		{
			if (this.valid && !overwrite)
			{
				return;
			}
			this.animator = base.GetComponent<Animator>();
			base.Initialize(overwrite);
			if (!this.valid)
			{
				return;
			}
			this.animationTable.Clear();
			this.clipNameHashCodeTable.Clear();
			SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(true);
			foreach (Animation animation in skeletonData.Animations)
			{
				this.animationTable.Add(animation.Name.GetHashCode(), animation);
			}
			this.lastTime = Time.time;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0003A354 File Offset: 0x00038754
		public override void LateUpdate()
		{
			if (!this.valid)
			{
				return;
			}
			base.LateUpdate();
			if (this.layerMixModes.Length != this.animator.layerCount)
			{
				Array.Resize<SkeletonAnimator.MixMode>(ref this.layerMixModes, this.animator.layerCount);
			}
			float num = Time.time - this.lastTime;
			this.skeleton.Update(Time.deltaTime);
			int layerCount = this.animator.layerCount;
			for (int i = 0; i < layerCount; i++)
			{
				float num2 = this.animator.GetLayerWeight(i);
				if (i == 0)
				{
					num2 = 1f;
				}
				AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(i);
				AnimatorStateInfo nextAnimatorStateInfo = this.animator.GetNextAnimatorStateInfo(i);
				AnimatorClipInfo[] currentAnimatorClipInfo = this.animator.GetCurrentAnimatorClipInfo(i);
				AnimatorClipInfo[] nextAnimatorClipInfo = this.animator.GetNextAnimatorClipInfo(i);
				SkeletonAnimator.MixMode mixMode = this.layerMixModes[i];
				if (mixMode == SkeletonAnimator.MixMode.AlwaysMix)
				{
					foreach (AnimatorClipInfo animatorClipInfo in currentAnimatorClipInfo)
					{
						float num3 = animatorClipInfo.weight * num2;
						if (num3 != 0f)
						{
							float num4 = currentAnimatorStateInfo.normalizedTime * animatorClipInfo.clip.length;
							Animation animation;
							if (this.animationTable.TryGetValue(this.GetAnimationClipNameHashCode(animatorClipInfo.clip), out animation))
							{
								animation.Mix(this.skeleton, Mathf.Max(0f, num4 - num), num4, currentAnimatorStateInfo.loop, this.events, num3);
							}
						}
					}
					if (nextAnimatorStateInfo.fullPathHash != 0)
					{
						foreach (AnimatorClipInfo animatorClipInfo2 in nextAnimatorClipInfo)
						{
							float num5 = animatorClipInfo2.weight * num2;
							if (num5 != 0f)
							{
								float num6 = nextAnimatorStateInfo.normalizedTime * animatorClipInfo2.clip.length;
								Animation animation2;
								if (this.animationTable.TryGetValue(this.GetAnimationClipNameHashCode(animatorClipInfo2.clip), out animation2))
								{
									animation2.Mix(this.skeleton, Mathf.Max(0f, num6 - num), num6, nextAnimatorStateInfo.loop, this.events, num5);
								}
							}
						}
					}
				}
				else if (mixMode >= SkeletonAnimator.MixMode.MixNext)
				{
					int l;
					for (l = 0; l < currentAnimatorClipInfo.Length; l++)
					{
						AnimatorClipInfo animatorClipInfo3 = currentAnimatorClipInfo[l];
						float num7 = animatorClipInfo3.weight * num2;
						if (num7 != 0f)
						{
							float num8 = currentAnimatorStateInfo.normalizedTime * animatorClipInfo3.clip.length;
							Animation animation3;
							if (this.animationTable.TryGetValue(this.GetAnimationClipNameHashCode(animatorClipInfo3.clip), out animation3))
							{
								animation3.Apply(this.skeleton, Mathf.Max(0f, num8 - num), num8, currentAnimatorStateInfo.loop, this.events);
							}
							break;
						}
					}
					while (l < currentAnimatorClipInfo.Length)
					{
						AnimatorClipInfo animatorClipInfo4 = currentAnimatorClipInfo[l];
						float num9 = animatorClipInfo4.weight * num2;
						if (num9 != 0f)
						{
							float num10 = currentAnimatorStateInfo.normalizedTime * animatorClipInfo4.clip.length;
							Animation animation4;
							if (this.animationTable.TryGetValue(this.GetAnimationClipNameHashCode(animatorClipInfo4.clip), out animation4))
							{
								animation4.Mix(this.skeleton, Mathf.Max(0f, num10 - num), num10, currentAnimatorStateInfo.loop, this.events, num9);
							}
						}
						l++;
					}
					l = 0;
					if (nextAnimatorStateInfo.fullPathHash != 0)
					{
						if (mixMode == SkeletonAnimator.MixMode.SpineStyle)
						{
							while (l < nextAnimatorClipInfo.Length)
							{
								AnimatorClipInfo animatorClipInfo5 = nextAnimatorClipInfo[l];
								float num11 = animatorClipInfo5.weight * num2;
								if (num11 != 0f)
								{
									float num12 = nextAnimatorStateInfo.normalizedTime * animatorClipInfo5.clip.length;
									Animation animation5;
									if (this.animationTable.TryGetValue(this.GetAnimationClipNameHashCode(animatorClipInfo5.clip), out animation5))
									{
										animation5.Apply(this.skeleton, Mathf.Max(0f, num12 - num), num12, nextAnimatorStateInfo.loop, this.events);
									}
									break;
								}
								l++;
							}
						}
						while (l < nextAnimatorClipInfo.Length)
						{
							AnimatorClipInfo animatorClipInfo6 = nextAnimatorClipInfo[l];
							float num13 = animatorClipInfo6.weight * num2;
							if (num13 != 0f)
							{
								float num14 = nextAnimatorStateInfo.normalizedTime * animatorClipInfo6.clip.length;
								Animation animation6;
								if (this.animationTable.TryGetValue(this.GetAnimationClipNameHashCode(animatorClipInfo6.clip), out animation6))
								{
									animation6.Mix(this.skeleton, Mathf.Max(0f, num14 - num), num14, nextAnimatorStateInfo.loop, this.events, num13);
								}
							}
							l++;
						}
					}
				}
			}
			if (this._UpdateLocal != null)
			{
				this._UpdateLocal(this);
			}
			this.skeleton.UpdateWorldTransform();
			if (this._UpdateWorld != null)
			{
				this._UpdateWorld(this);
				this.skeleton.UpdateWorldTransform();
			}
			if (this._UpdateComplete != null)
			{
				this._UpdateComplete(this);
			}
			this.lastTime = Time.time;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0003A8B8 File Offset: 0x00038CB8
		private int GetAnimationClipNameHashCode(AnimationClip clip)
		{
			int hashCode;
			if (!this.clipNameHashCodeTable.TryGetValue(clip, out hashCode))
			{
				hashCode = clip.name.GetHashCode();
				this.clipNameHashCodeTable.Add(clip, hashCode);
			}
			return hashCode;
		}

		// Token: 0x04004295 RID: 17045
		public SkeletonAnimator.MixMode[] layerMixModes = new SkeletonAnimator.MixMode[0];

		// Token: 0x04004299 RID: 17049
		private readonly Dictionary<int, Animation> animationTable = new Dictionary<int, Animation>();

		// Token: 0x0400429A RID: 17050
		private readonly Dictionary<AnimationClip, int> clipNameHashCodeTable = new Dictionary<AnimationClip, int>();

		// Token: 0x0400429B RID: 17051
		private Animator animator;

		// Token: 0x0400429C RID: 17052
		private float lastTime;

		// Token: 0x0400429D RID: 17053
		public readonly ExposedList<Event> events;

		// Token: 0x02000258 RID: 600
		public enum MixMode
		{
			// Token: 0x0400429F RID: 17055
			AlwaysMix,
			// Token: 0x040042A0 RID: 17056
			MixNext,
			// Token: 0x040042A1 RID: 17057
			SpineStyle
		}
	}
}
