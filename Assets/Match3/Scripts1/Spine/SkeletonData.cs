using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200021D RID: 541
	public class SkeletonData
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x0002B2B8 File Offset: 0x000296B8
		// (set) Token: 0x060010CE RID: 4302 RVA: 0x0002B2C0 File Offset: 0x000296C0
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0002B2C9 File Offset: 0x000296C9
		public ExposedList<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0002B2D1 File Offset: 0x000296D1
		public ExposedList<SlotData> Slots
		{
			get
			{
				return this.slots;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x0002B2D9 File Offset: 0x000296D9
		// (set) Token: 0x060010D2 RID: 4306 RVA: 0x0002B2E1 File Offset: 0x000296E1
		public ExposedList<Skin> Skins
		{
			get
			{
				return this.skins;
			}
			set
			{
				this.skins = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x0002B2EA File Offset: 0x000296EA
		// (set) Token: 0x060010D4 RID: 4308 RVA: 0x0002B2F2 File Offset: 0x000296F2
		public Skin DefaultSkin
		{
			get
			{
				return this.defaultSkin;
			}
			set
			{
				this.defaultSkin = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x0002B2FB File Offset: 0x000296FB
		// (set) Token: 0x060010D6 RID: 4310 RVA: 0x0002B303 File Offset: 0x00029703
		public ExposedList<EventData> Events
		{
			get
			{
				return this.events;
			}
			set
			{
				this.events = value;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0002B30C File Offset: 0x0002970C
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x0002B314 File Offset: 0x00029714
		public ExposedList<Animation> Animations
		{
			get
			{
				return this.animations;
			}
			set
			{
				this.animations = value;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0002B31D File Offset: 0x0002971D
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x0002B325 File Offset: 0x00029725
		public ExposedList<IkConstraintData> IkConstraints
		{
			get
			{
				return this.ikConstraints;
			}
			set
			{
				this.ikConstraints = value;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0002B32E File Offset: 0x0002972E
		// (set) Token: 0x060010DC RID: 4316 RVA: 0x0002B336 File Offset: 0x00029736
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0002B33F File Offset: 0x0002973F
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x0002B347 File Offset: 0x00029747
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0002B350 File Offset: 0x00029750
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x0002B358 File Offset: 0x00029758
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0002B361 File Offset: 0x00029761
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x0002B369 File Offset: 0x00029769
		public string Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0002B374 File Offset: 0x00029774
		public BoneData FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			ExposedList<BoneData> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				BoneData boneData = exposedList.Items[i];
				if (boneData.name == boneName)
				{
					return boneData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0002B3D0 File Offset: 0x000297D0
		public int FindBoneIndex(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			ExposedList<BoneData> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (exposedList.Items[i].name == boneName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0002B42C File Offset: 0x0002982C
		public SlotData FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<SlotData> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				SlotData slotData = exposedList.Items[i];
				if (slotData.name == slotName)
				{
					return slotData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0002B488 File Offset: 0x00029888
		public int FindSlotIndex(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<SlotData> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (exposedList.Items[i].name == slotName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0002B4E4 File Offset: 0x000298E4
		public Skin FindSkin(string skinName)
		{
			if (skinName == null)
			{
				throw new ArgumentNullException("skinName cannot be null.");
			}
			foreach (Skin skin in this.skins)
			{
				if (skin.name == skinName)
				{
					return skin;
				}
			}
			return null;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002B568 File Offset: 0x00029968
		public EventData FindEvent(string eventDataName)
		{
			if (eventDataName == null)
			{
				throw new ArgumentNullException("eventDataName cannot be null.");
			}
			foreach (EventData eventData in this.events)
			{
				if (eventData.name == eventDataName)
				{
					return eventData;
				}
			}
			return null;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002B5EC File Offset: 0x000299EC
		public Animation FindAnimation(string animationName)
		{
			if (animationName == null)
			{
				throw new ArgumentNullException("animationName cannot be null.");
			}
			ExposedList<Animation> exposedList = this.animations;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Animation animation = exposedList.Items[i];
				if (animation.name == animationName)
				{
					return animation;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0002B648 File Offset: 0x00029A48
		public IkConstraintData FindIkConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName cannot be null.");
			}
			ExposedList<IkConstraintData> exposedList = this.ikConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				IkConstraintData ikConstraintData = exposedList.Items[i];
				if (ikConstraintData.name == constraintName)
				{
					return ikConstraintData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0002B6A4 File Offset: 0x00029AA4
		public TransformConstraintData FindTransformConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName cannot be null.");
			}
			ExposedList<TransformConstraintData> exposedList = this.transformConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				TransformConstraintData transformConstraintData = exposedList.Items[i];
				if (transformConstraintData.name == constraintName)
				{
					return transformConstraintData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0002B6FF File Offset: 0x00029AFF
		public override string ToString()
		{
			return this.name ?? base.ToString();
		}

		// Token: 0x04004165 RID: 16741
		internal string name;

		// Token: 0x04004166 RID: 16742
		internal ExposedList<BoneData> bones = new ExposedList<BoneData>();

		// Token: 0x04004167 RID: 16743
		internal ExposedList<SlotData> slots = new ExposedList<SlotData>();

		// Token: 0x04004168 RID: 16744
		internal ExposedList<Skin> skins = new ExposedList<Skin>();

		// Token: 0x04004169 RID: 16745
		internal Skin defaultSkin;

		// Token: 0x0400416A RID: 16746
		internal ExposedList<EventData> events = new ExposedList<EventData>();

		// Token: 0x0400416B RID: 16747
		internal ExposedList<Animation> animations = new ExposedList<Animation>();

		// Token: 0x0400416C RID: 16748
		internal ExposedList<IkConstraintData> ikConstraints = new ExposedList<IkConstraintData>();

		// Token: 0x0400416D RID: 16749
		internal ExposedList<TransformConstraintData> transformConstraints = new ExposedList<TransformConstraintData>();

		// Token: 0x0400416E RID: 16750
		internal float width;

		// Token: 0x0400416F RID: 16751
		internal float height;

		// Token: 0x04004170 RID: 16752
		internal string version;

		// Token: 0x04004171 RID: 16753
		internal string hash;

		// Token: 0x04004172 RID: 16754
		internal string imagesPath;
	}
}
