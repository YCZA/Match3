namespace Match3.Scripts1.Spine
{
	// Token: 0x020001F5 RID: 501
	public class TrackEntry
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x00023A13 File Offset: 0x00021E13
		public Animation Animation
		{
			get
			{
				return this.animation;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00023A1B File Offset: 0x00021E1B
		// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x00023A23 File Offset: 0x00021E23
		public float Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				this.delay = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00023A2C File Offset: 0x00021E2C
		// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x00023A34 File Offset: 0x00021E34
		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00023A3D File Offset: 0x00021E3D
		// (set) Token: 0x06000EAB RID: 3755 RVA: 0x00023A45 File Offset: 0x00021E45
		public float LastTime
		{
			get
			{
				return this.lastTime;
			}
			set
			{
				this.lastTime = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00023A4E File Offset: 0x00021E4E
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x00023A56 File Offset: 0x00021E56
		public float EndTime
		{
			get
			{
				return this.endTime;
			}
			set
			{
				this.endTime = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x00023A5F File Offset: 0x00021E5F
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x00023A67 File Offset: 0x00021E67
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

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00023A70 File Offset: 0x00021E70
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00023A78 File Offset: 0x00021E78
		public float Mix
		{
			get
			{
				return this.mix;
			}
			set
			{
				this.mix = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00023A81 File Offset: 0x00021E81
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x00023A89 File Offset: 0x00021E89
		public bool Loop
		{
			get
			{
				return this.loop;
			}
			set
			{
				this.loop = value;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000EB4 RID: 3764 RVA: 0x00023A94 File Offset: 0x00021E94
		// (remove) Token: 0x06000EB5 RID: 3765 RVA: 0x00023ACC File Offset: 0x00021ECC
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.StartEndDelegate Start;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000EB6 RID: 3766 RVA: 0x00023B04 File Offset: 0x00021F04
		// (remove) Token: 0x06000EB7 RID: 3767 RVA: 0x00023B3C File Offset: 0x00021F3C
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.StartEndDelegate End;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000EB8 RID: 3768 RVA: 0x00023B74 File Offset: 0x00021F74
		// (remove) Token: 0x06000EB9 RID: 3769 RVA: 0x00023BAC File Offset: 0x00021FAC
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.EventDelegate Event;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000EBA RID: 3770 RVA: 0x00023BE4 File Offset: 0x00021FE4
		// (remove) Token: 0x06000EBB RID: 3771 RVA: 0x00023C1C File Offset: 0x0002201C
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.CompleteDelegate Complete;

		// Token: 0x06000EBC RID: 3772 RVA: 0x00023C52 File Offset: 0x00022052
		internal void OnStart(AnimationState state, int index)
		{
			if (this.Start != null)
			{
				this.Start(state, index);
			}
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00023C6C File Offset: 0x0002206C
		internal void OnEnd(AnimationState state, int index)
		{
			if (this.End != null)
			{
				this.End(state, index);
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00023C86 File Offset: 0x00022086
		internal void OnEvent(AnimationState state, int index, Event e)
		{
			if (this.Event != null)
			{
				this.Event(state, index, e);
			}
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00023CA1 File Offset: 0x000220A1
		internal void OnComplete(AnimationState state, int index, int loopCount)
		{
			if (this.Complete != null)
			{
				this.Complete(state, index, loopCount);
			}
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00023CBC File Offset: 0x000220BC
		public override string ToString()
		{
			return (this.animation != null) ? this.animation.name : "<none>";
		}

		// Token: 0x0400402C RID: 16428
		internal TrackEntry next;

		// Token: 0x0400402D RID: 16429
		internal TrackEntry previous;

		// Token: 0x0400402E RID: 16430
		internal Animation animation;

		// Token: 0x0400402F RID: 16431
		internal bool loop;

		// Token: 0x04004030 RID: 16432
		internal float delay;

		// Token: 0x04004031 RID: 16433
		internal float time;

		// Token: 0x04004032 RID: 16434
		internal float lastTime = -1f;

		// Token: 0x04004033 RID: 16435
		internal float endTime;

		// Token: 0x04004034 RID: 16436
		internal float timeScale = 1f;

		// Token: 0x04004035 RID: 16437
		internal float mixTime;

		// Token: 0x04004036 RID: 16438
		internal float mixDuration;

		// Token: 0x04004037 RID: 16439
		internal float mix = 1f;
	}
}
