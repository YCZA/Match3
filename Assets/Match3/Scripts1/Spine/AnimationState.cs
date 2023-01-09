using System;
using System.Text;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001F1 RID: 497
	public class AnimationState
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x0002306C File Offset: 0x0002146C
		public AnimationState(AnimationStateData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			this.data = data;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x000230B8 File Offset: 0x000214B8
		public AnimationStateData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000230C0 File Offset: 0x000214C0
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x000230C8 File Offset: 0x000214C8
		public float TimeScale
		{
			get
			{
				return this.timeScale;
			}
			set
			{
				this.timeScale = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000E84 RID: 3716 RVA: 0x000230D4 File Offset: 0x000214D4
		// (remove) Token: 0x06000E85 RID: 3717 RVA: 0x0002310C File Offset: 0x0002150C
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.StartEndDelegate Start;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000E86 RID: 3718 RVA: 0x00023144 File Offset: 0x00021544
		// (remove) Token: 0x06000E87 RID: 3719 RVA: 0x0002317C File Offset: 0x0002157C
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.StartEndDelegate End;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000E88 RID: 3720 RVA: 0x000231B4 File Offset: 0x000215B4
		// (remove) Token: 0x06000E89 RID: 3721 RVA: 0x000231EC File Offset: 0x000215EC
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.EventDelegate Event;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000E8A RID: 3722 RVA: 0x00023224 File Offset: 0x00021624
		// (remove) Token: 0x06000E8B RID: 3723 RVA: 0x0002325C File Offset: 0x0002165C
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.CompleteDelegate Complete;

		// Token: 0x06000E8C RID: 3724 RVA: 0x00023294 File Offset: 0x00021694
		public void Update(float delta)
		{
			delta *= this.timeScale;
			for (int i = 0; i < this.tracks.Count; i++)
			{
				TrackEntry trackEntry = this.tracks.Items[i];
				if (trackEntry != null)
				{
					float num = delta * trackEntry.timeScale;
					float num2 = trackEntry.time + num;
					float endTime = trackEntry.endTime;
					trackEntry.time = num2;
					if (trackEntry.previous != null)
					{
						trackEntry.previous.time += num;
						trackEntry.mixTime += num;
					}
					if ((!trackEntry.loop) ? (trackEntry.lastTime < endTime && num2 >= endTime) : (trackEntry.lastTime % endTime > num2 % endTime))
					{
						int loopCount = (int)(num2 / endTime);
						trackEntry.OnComplete(this, i, loopCount);
						if (this.Complete != null)
						{
							this.Complete(this, i, loopCount);
						}
					}
					TrackEntry next = trackEntry.next;
					if (next != null)
					{
						next.time = trackEntry.lastTime - next.delay;
						if (next.time >= 0f)
						{
							this.SetCurrent(i, next);
						}
					}
					else if (!trackEntry.loop && trackEntry.lastTime >= trackEntry.endTime)
					{
						this.ClearTrack(i);
					}
				}
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x000233FC File Offset: 0x000217FC
		public void Apply(Skeleton skeleton)
		{
			ExposedList<Event> exposedList = this.events;
			for (int i = 0; i < this.tracks.Count; i++)
			{
				TrackEntry trackEntry = this.tracks.Items[i];
				if (trackEntry != null)
				{
					exposedList.Clear(true);
					float num = trackEntry.time;
					bool loop = trackEntry.loop;
					if (!loop && num > trackEntry.endTime)
					{
						num = trackEntry.endTime;
					}
					TrackEntry previous = trackEntry.previous;
					if (previous == null)
					{
						if (trackEntry.mix == 1f)
						{
							trackEntry.animation.Apply(skeleton, trackEntry.lastTime, num, loop, exposedList);
						}
						else
						{
							trackEntry.animation.Mix(skeleton, trackEntry.lastTime, num, loop, exposedList, trackEntry.mix);
						}
					}
					else
					{
						float num2 = previous.time;
						if (!previous.loop && num2 > previous.endTime)
						{
							num2 = previous.endTime;
						}
						previous.animation.Apply(skeleton, previous.lastTime, num2, previous.loop, null);
						previous.lastTime = num2;
						float num3 = trackEntry.mixTime / trackEntry.mixDuration * trackEntry.mix;
						if (num3 >= 1f)
						{
							num3 = 1f;
							trackEntry.previous = null;
						}
						trackEntry.animation.Mix(skeleton, trackEntry.lastTime, num, loop, exposedList, num3);
					}
					int j = 0;
					int count = exposedList.Count;
					while (j < count)
					{
						Event e = exposedList.Items[j];
						trackEntry.OnEvent(this, i, e);
						if (this.Event != null)
						{
							this.Event(this, i, e);
						}
						j++;
					}
					trackEntry.lastTime = trackEntry.time;
				}
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x000235C8 File Offset: 0x000219C8
		public void ClearTracks()
		{
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				this.ClearTrack(i);
				i++;
			}
			this.tracks.Clear(true);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00023608 File Offset: 0x00021A08
		public void ClearTrack(int trackIndex)
		{
			if (trackIndex >= this.tracks.Count)
			{
				return;
			}
			TrackEntry trackEntry = this.tracks.Items[trackIndex];
			if (trackEntry == null)
			{
				return;
			}
			trackEntry.OnEnd(this, trackIndex);
			if (this.End != null)
			{
				this.End(this, trackIndex);
			}
			this.tracks.Items[trackIndex] = null;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0002366C File Offset: 0x00021A6C
		private TrackEntry ExpandToIndex(int index)
		{
			if (index < this.tracks.Count)
			{
				return this.tracks.Items[index];
			}
			while (index >= this.tracks.Count)
			{
				this.tracks.Add(null);
			}
			return null;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000236BC File Offset: 0x00021ABC
		private void SetCurrent(int index, TrackEntry entry)
		{
			TrackEntry trackEntry = this.ExpandToIndex(index);
			if (trackEntry != null)
			{
				TrackEntry previous = trackEntry.previous;
				trackEntry.previous = null;
				trackEntry.OnEnd(this, index);
				if (this.End != null)
				{
					this.End(this, index);
				}
				entry.mixDuration = this.data.GetMix(trackEntry.animation, entry.animation);
				if (entry.mixDuration > 0f)
				{
					entry.mixTime = 0f;
					if (previous != null && trackEntry.mixTime / trackEntry.mixDuration < 0.5f)
					{
						entry.previous = previous;
					}
					else
					{
						entry.previous = trackEntry;
					}
				}
			}
			this.tracks.Items[index] = entry;
			entry.OnStart(this, index);
			if (this.Start != null)
			{
				this.Start(this, index);
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0002379C File Offset: 0x00021B9C
		public TrackEntry SetAnimation(int trackIndex, string animationName, bool loop)
		{
			Animation animation = this.data.skeletonData.FindAnimation(animationName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + animationName);
			}
			return this.SetAnimation(trackIndex, animation, loop);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000237DC File Offset: 0x00021BDC
		public TrackEntry SetAnimation(int trackIndex, Animation animation, bool loop)
		{
			if (animation == null)
			{
				throw new ArgumentException("animation cannot be null.");
			}
			TrackEntry trackEntry = new TrackEntry();
			trackEntry.animation = animation;
			trackEntry.loop = loop;
			trackEntry.time = 0f;
			trackEntry.endTime = animation.Duration;
			this.SetCurrent(trackIndex, trackEntry);
			return trackEntry;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00023830 File Offset: 0x00021C30
		public TrackEntry AddAnimation(int trackIndex, string animationName, bool loop, float delay)
		{
			Animation animation = this.data.skeletonData.FindAnimation(animationName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + animationName);
			}
			return this.AddAnimation(trackIndex, animation, loop, delay);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00023874 File Offset: 0x00021C74
		public TrackEntry AddAnimation(int trackIndex, Animation animation, bool loop, float delay)
		{
			if (animation == null)
			{
				throw new ArgumentException("animation cannot be null.");
			}
			TrackEntry trackEntry = new TrackEntry();
			trackEntry.animation = animation;
			trackEntry.loop = loop;
			trackEntry.time = 0f;
			trackEntry.endTime = animation.Duration;
			TrackEntry trackEntry2 = this.ExpandToIndex(trackIndex);
			if (trackEntry2 != null)
			{
				while (trackEntry2.next != null)
				{
					trackEntry2 = trackEntry2.next;
				}
				trackEntry2.next = trackEntry;
			}
			else
			{
				this.tracks.Items[trackIndex] = trackEntry;
			}
			if (delay <= 0f)
			{
				if (trackEntry2 != null)
				{
					delay += trackEntry2.endTime - this.data.GetMix(trackEntry2.animation, animation);
				}
				else
				{
					delay = 0f;
				}
			}
			trackEntry.delay = delay;
			return trackEntry;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00023941 File Offset: 0x00021D41
		public TrackEntry GetCurrent(int trackIndex)
		{
			if (trackIndex >= this.tracks.Count)
			{
				return null;
			}
			return this.tracks.Items[trackIndex];
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00023964 File Offset: 0x00021D64
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = this.tracks.Items[i];
				if (trackEntry != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(trackEntry.ToString());
				}
				i++;
			}
			if (stringBuilder.Length == 0)
			{
				return "<none>";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004024 RID: 16420
		private AnimationStateData data;

		// Token: 0x04004025 RID: 16421
		private ExposedList<TrackEntry> tracks = new ExposedList<TrackEntry>();

		// Token: 0x04004026 RID: 16422
		private ExposedList<Event> events = new ExposedList<Event>();

		// Token: 0x04004027 RID: 16423
		private float timeScale = 1f;

		// Token: 0x020001F2 RID: 498
		// (Invoke) Token: 0x06000E99 RID: 3737
		public delegate void StartEndDelegate(AnimationState state, int trackIndex);

		// Token: 0x020001F3 RID: 499
		// (Invoke) Token: 0x06000E9D RID: 3741
		public delegate void EventDelegate(AnimationState state, int trackIndex, Event e);

		// Token: 0x020001F4 RID: 500
		// (Invoke) Token: 0x06000EA1 RID: 3745
		public delegate void CompleteDelegate(AnimationState state, int trackIndex, int loopCount);
	}
}
