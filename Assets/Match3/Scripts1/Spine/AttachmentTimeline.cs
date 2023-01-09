namespace Match3.Scripts1.Spine
{
	// Token: 0x020001EB RID: 491
	public class AttachmentTimeline : Timeline
	{
		// Token: 0x06000E4D RID: 3661 RVA: 0x000226E4 File Offset: 0x00020AE4
		public AttachmentTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.attachmentNames = new string[frameCount];
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00022704 File Offset: 0x00020B04
		// (set) Token: 0x06000E4F RID: 3663 RVA: 0x0002270C File Offset: 0x00020B0C
		public int SlotIndex
		{
			get
			{
				return this.slotIndex;
			}
			set
			{
				this.slotIndex = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00022715 File Offset: 0x00020B15
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x0002271D File Offset: 0x00020B1D
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

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00022726 File Offset: 0x00020B26
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x0002272E File Offset: 0x00020B2E
		public string[] AttachmentNames
		{
			get
			{
				return this.attachmentNames;
			}
			set
			{
				this.attachmentNames = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00022737 File Offset: 0x00020B37
		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00022741 File Offset: 0x00020B41
		public void SetFrame(int frameIndex, float time, string attachmentName)
		{
			this.frames[frameIndex] = time;
			this.attachmentNames[frameIndex] = attachmentName;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00022758 File Offset: 0x00020B58
		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] array = this.frames;
			if (time < array[0])
			{
				if (lastTime > time)
				{
					this.Apply(skeleton, lastTime, 2.1474836E+09f, null, 0f);
				}
				return;
			}
			if (lastTime > time)
			{
				lastTime = -1f;
			}
			int num = ((time < array[array.Length - 1]) ? Animation.binarySearch(array, time) : array.Length) - 1;
			if (array[num] < lastTime)
			{
				return;
			}
			string text = this.attachmentNames[num];
			skeleton.slots.Items[this.slotIndex].Attachment = ((text != null) ? skeleton.GetAttachment(this.slotIndex, text) : null);
		}

		// Token: 0x04004008 RID: 16392
		internal int slotIndex;

		// Token: 0x04004009 RID: 16393
		internal float[] frames;

		// Token: 0x0400400A RID: 16394
		private string[] attachmentNames;
	}
}
