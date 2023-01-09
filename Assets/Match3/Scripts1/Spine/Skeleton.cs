using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000219 RID: 537
	public class Skeleton
	{
		// Token: 0x0600106E RID: 4206 RVA: 0x000284D0 File Offset: 0x000268D0
		public Skeleton(SkeletonData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			this.data = data;
			this.bones = new ExposedList<Bone>(data.bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				Bone bone = (boneData.parent != null) ? this.bones.Items[data.bones.IndexOf(boneData.parent)] : null;
				Bone item = new Bone(boneData, this, bone);
				if (bone != null)
				{
					bone.children.Add(item);
				}
				this.bones.Add(item);
			}
			this.slots = new ExposedList<Slot>(data.slots.Count);
			this.drawOrder = new ExposedList<Slot>(data.slots.Count);
			foreach (SlotData slotData in data.slots)
			{
				Bone bone2 = this.bones.Items[data.bones.IndexOf(slotData.boneData)];
				Slot item2 = new Slot(slotData, bone2);
				this.slots.Add(item2);
				this.drawOrder.Add(item2);
			}
			this.ikConstraints = new ExposedList<IkConstraint>(data.ikConstraints.Count);
			foreach (IkConstraintData ikConstraintData in data.ikConstraints)
			{
				this.ikConstraints.Add(new IkConstraint(ikConstraintData, this));
			}
			this.transformConstraints = new ExposedList<TransformConstraint>(data.transformConstraints.Count);
			foreach (TransformConstraintData transformConstraintData in data.transformConstraints)
			{
				this.transformConstraints.Add(new TransformConstraint(transformConstraintData, this));
			}
			this.UpdateCache();
			this.UpdateWorldTransform();
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00028788 File Offset: 0x00026B88
		public SkeletonData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00028790 File Offset: 0x00026B90
		public ExposedList<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x00028798 File Offset: 0x00026B98
		public ExposedList<Slot> Slots
		{
			get
			{
				return this.slots;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x000287A0 File Offset: 0x00026BA0
		public ExposedList<Slot> DrawOrder
		{
			get
			{
				return this.drawOrder;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x000287A8 File Offset: 0x00026BA8
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x000287B0 File Offset: 0x00026BB0
		public ExposedList<IkConstraint> IkConstraints
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

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x000287B9 File Offset: 0x00026BB9
		// (set) Token: 0x06001076 RID: 4214 RVA: 0x000287C1 File Offset: 0x00026BC1
		public Skin Skin
		{
			get
			{
				return this.skin;
			}
			set
			{
				this.skin = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x000287CA File Offset: 0x00026BCA
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x000287D2 File Offset: 0x00026BD2
		public float R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x000287DB File Offset: 0x00026BDB
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x000287E3 File Offset: 0x00026BE3
		public float G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x000287EC File Offset: 0x00026BEC
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x000287F4 File Offset: 0x00026BF4
		public float B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x000287FD File Offset: 0x00026BFD
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x00028805 File Offset: 0x00026C05
		public float A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0002880E File Offset: 0x00026C0E
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x00028816 File Offset: 0x00026C16
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0002881F File Offset: 0x00026C1F
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x00028827 File Offset: 0x00026C27
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x00028830 File Offset: 0x00026C30
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x00028838 File Offset: 0x00026C38
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x00028841 File Offset: 0x00026C41
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x00028849 File Offset: 0x00026C49
		public bool FlipX
		{
			get
			{
				return this.flipX;
			}
			set
			{
				this.flipX = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x00028852 File Offset: 0x00026C52
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0002885A File Offset: 0x00026C5A
		public bool FlipY
		{
			get
			{
				return this.flipY;
			}
			set
			{
				this.flipY = value;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x00028863 File Offset: 0x00026C63
		public Bone RootBone
		{
			get
			{
				return (this.bones.Count != 0) ? this.bones.Items[0] : null;
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00028888 File Offset: 0x00026C88
		public void UpdateCache()
		{
			ExposedList<Bone> exposedList = this.bones;
			ExposedList<IUpdatable> exposedList2 = this.updateCache;
			ExposedList<IkConstraint> exposedList3 = this.ikConstraints;
			ExposedList<TransformConstraint> exposedList4 = this.transformConstraints;
			int count = exposedList3.Count;
			int count2 = exposedList4.Count;
			exposedList2.Clear(true);
			int i = 0;
			int count3 = exposedList.Count;
			while (i < count3)
			{
				Bone bone = exposedList.Items[i];
				exposedList2.Add(bone);
				for (int j = 0; j < count; j++)
				{
					IkConstraint ikConstraint = exposedList3.Items[j];
					if (bone == ikConstraint.bones.Items[ikConstraint.bones.Count - 1])
					{
						exposedList2.Add(ikConstraint);
						break;
					}
				}
				i++;
			}
			for (int k = 0; k < count2; k++)
			{
				TransformConstraint transformConstraint = exposedList4.Items[k];
				int num = exposedList2.Count - 1;
				while (k >= 0)
				{
					if (exposedList2.Items[num] == transformConstraint.bone)
					{
						exposedList2.Insert(num + 1, transformConstraint);
						break;
					}
					num--;
				}
			}
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x000289B0 File Offset: 0x00026DB0
		public void UpdateWorldTransform()
		{
			ExposedList<IUpdatable> exposedList = this.updateCache;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				exposedList.Items[i].Update();
				i++;
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x000289EA File Offset: 0x00026DEA
		public void SetToSetupPose()
		{
			this.SetBonesToSetupPose();
			this.SetSlotsToSetupPose();
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x000289F8 File Offset: 0x00026DF8
		public void SetBonesToSetupPose()
		{
			ExposedList<Bone> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				exposedList.Items[i].SetToSetupPose();
				i++;
			}
			ExposedList<IkConstraint> exposedList2 = this.ikConstraints;
			int j = 0;
			int count2 = exposedList2.Count;
			while (j < count2)
			{
				IkConstraint ikConstraint = exposedList2.Items[j];
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				ikConstraint.mix = ikConstraint.data.mix;
				j++;
			}
			ExposedList<TransformConstraint> exposedList3 = this.transformConstraints;
			int k = 0;
			int count3 = exposedList3.Count;
			while (k < count3)
			{
				TransformConstraint transformConstraint = exposedList3.Items[k];
				TransformConstraintData transformConstraintData = transformConstraint.data;
				transformConstraint.rotateMix = transformConstraintData.rotateMix;
				transformConstraint.translateMix = transformConstraintData.translateMix;
				transformConstraint.scaleMix = transformConstraintData.scaleMix;
				transformConstraint.shearMix = transformConstraintData.shearMix;
				k++;
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00028B00 File Offset: 0x00026F00
		public void SetSlotsToSetupPose()
		{
			ExposedList<Slot> exposedList = this.slots;
			this.drawOrder.Clear(true);
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				this.drawOrder.Add(exposedList.Items[i]);
				i++;
			}
			int j = 0;
			int count2 = exposedList.Count;
			while (j < count2)
			{
				exposedList.Items[j].SetToSetupPose(j);
				j++;
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00028B78 File Offset: 0x00026F78
		public Bone FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			ExposedList<Bone> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Bone bone = exposedList.Items[i];
				if (bone.data.name == boneName)
				{
					return bone;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00028BD8 File Offset: 0x00026FD8
		public int FindBoneIndex(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			ExposedList<Bone> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (exposedList.Items[i].data.name == boneName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00028C38 File Offset: 0x00027038
		public Slot FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Slot slot = exposedList.Items[i];
				if (slot.data.name == slotName)
				{
					return slot;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00028C98 File Offset: 0x00027098
		public int FindSlotIndex(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (exposedList.Items[i].data.name.Equals(slotName))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00028CF8 File Offset: 0x000270F8
		public void SetSkin(string skinName)
		{
			Skin skin = this.data.FindSkin(skinName);
			if (skin == null)
			{
				throw new ArgumentException("Skin not found: " + skinName);
			}
			this.SetSkin(skin);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00028D30 File Offset: 0x00027130
		public void SetSkin(Skin newSkin)
		{
			if (newSkin != null)
			{
				if (this.skin != null)
				{
					newSkin.AttachAll(this, this.skin);
				}
				else
				{
					ExposedList<Slot> exposedList = this.slots;
					int i = 0;
					int count = exposedList.Count;
					while (i < count)
					{
						Slot slot = exposedList.Items[i];
						string attachmentName = slot.data.attachmentName;
						if (attachmentName != null)
						{
							Attachment attachment = newSkin.GetAttachment(i, attachmentName);
							if (attachment != null)
							{
								slot.Attachment = attachment;
							}
						}
						i++;
					}
				}
			}
			this.skin = newSkin;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00028DBE File Offset: 0x000271BE
		public Attachment GetAttachment(string slotName, string attachmentName)
		{
			return this.GetAttachment(this.data.FindSlotIndex(slotName), attachmentName);
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00028DD4 File Offset: 0x000271D4
		public Attachment GetAttachment(int slotIndex, string attachmentName)
		{
			if (attachmentName == null)
			{
				throw new ArgumentNullException("attachmentName cannot be null.");
			}
			if (this.skin != null)
			{
				Attachment attachment = this.skin.GetAttachment(slotIndex, attachmentName);
				if (attachment != null)
				{
					return attachment;
				}
			}
			if (this.data.defaultSkin != null)
			{
				return this.data.defaultSkin.GetAttachment(slotIndex, attachmentName);
			}
			return null;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00028E38 File Offset: 0x00027238
		public void SetAttachment(string slotName, string attachmentName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Slot slot = exposedList.Items[i];
				if (slot.data.name == slotName)
				{
					Attachment attachment = null;
					if (attachmentName != null)
					{
						attachment = this.GetAttachment(i, attachmentName);
						if (attachment == null)
						{
							throw new Exception("Attachment not found: " + attachmentName + ", for slot: " + slotName);
						}
					}
					slot.Attachment = attachment;
					return;
				}
				i++;
			}
			throw new Exception("Slot not found: " + slotName);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00028EE0 File Offset: 0x000272E0
		public IkConstraint FindIkConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName cannot be null.");
			}
			ExposedList<IkConstraint> exposedList = this.ikConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				IkConstraint ikConstraint = exposedList.Items[i];
				if (ikConstraint.data.name == constraintName)
				{
					return ikConstraint;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00028F40 File Offset: 0x00027340
		public TransformConstraint FindTransformConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName cannot be null.");
			}
			ExposedList<TransformConstraint> exposedList = this.transformConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				TransformConstraint transformConstraint = exposedList.Items[i];
				if (transformConstraint.data.name == constraintName)
				{
					return transformConstraint;
				}
				i++;
			}
			return null;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00028FA0 File Offset: 0x000273A0
		public void Update(float delta)
		{
			this.time += delta;
		}

		// Token: 0x0400413E RID: 16702
		internal SkeletonData data;

		// Token: 0x0400413F RID: 16703
		internal ExposedList<Bone> bones;

		// Token: 0x04004140 RID: 16704
		internal ExposedList<Slot> slots;

		// Token: 0x04004141 RID: 16705
		internal ExposedList<Slot> drawOrder;

		// Token: 0x04004142 RID: 16706
		internal ExposedList<IkConstraint> ikConstraints;

		// Token: 0x04004143 RID: 16707
		internal ExposedList<TransformConstraint> transformConstraints;

		// Token: 0x04004144 RID: 16708
		private ExposedList<IUpdatable> updateCache = new ExposedList<IUpdatable>();

		// Token: 0x04004145 RID: 16709
		internal Skin skin;

		// Token: 0x04004146 RID: 16710
		internal float r = 1f;

		// Token: 0x04004147 RID: 16711
		internal float g = 1f;

		// Token: 0x04004148 RID: 16712
		internal float b = 1f;

		// Token: 0x04004149 RID: 16713
		internal float a = 1f;

		// Token: 0x0400414A RID: 16714
		internal float time;

		// Token: 0x0400414B RID: 16715
		internal bool flipX;

		// Token: 0x0400414C RID: 16716
		internal bool flipY;

		// Token: 0x0400414D RID: 16717
		internal float x;

		// Token: 0x0400414E RID: 16718
		internal float y;
	}
}
