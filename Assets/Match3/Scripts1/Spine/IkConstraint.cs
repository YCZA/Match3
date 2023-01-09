using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000211 RID: 529
	public class IkConstraint : IUpdatable
	{
		// Token: 0x06001032 RID: 4146 RVA: 0x00027028 File Offset: 0x00025428
		public IkConstraint(IkConstraintData data, Skeleton skeleton)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			this.data = data;
			this.mix = data.mix;
			this.bendDirection = data.bendDirection;
			this.bones = new ExposedList<Bone>(data.bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				this.bones.Add(skeleton.FindBone(boneData.name));
			}
			this.target = skeleton.FindBone(data.target.name);
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00027114 File Offset: 0x00025514
		public IkConstraintData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0002711C File Offset: 0x0002551C
		public ExposedList<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00027124 File Offset: 0x00025524
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x0002712C File Offset: 0x0002552C
		public Bone Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x00027135 File Offset: 0x00025535
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x0002713D File Offset: 0x0002553D
		public int BendDirection
		{
			get
			{
				return this.bendDirection;
			}
			set
			{
				this.bendDirection = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x00027146 File Offset: 0x00025546
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x0002714E File Offset: 0x0002554E
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

		// Token: 0x0600103B RID: 4155 RVA: 0x00027157 File Offset: 0x00025557
		public void Update()
		{
			this.Apply();
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00027160 File Offset: 0x00025560
		public void Apply()
		{
			Bone bone = this.target;
			ExposedList<Bone> exposedList = this.bones;
			int count = exposedList.Count;
			if (count != 1)
			{
				if (count == 2)
				{
					IkConstraint.Apply(exposedList.Items[0], exposedList.Items[1], bone.worldX, bone.worldY, this.bendDirection, this.mix);
				}
			}
			else
			{
				IkConstraint.Apply(exposedList.Items[0], bone.worldX, bone.worldY, this.mix);
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000271EB File Offset: 0x000255EB
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000271F8 File Offset: 0x000255F8
		public static void Apply(Bone bone, float targetX, float targetY, float alpha)
		{
			Bone parent = bone.parent;
			float num = 1f / (parent.a * parent.d - parent.b * parent.c);
			float num2 = targetX - parent.worldX;
			float num3 = targetY - parent.worldY;
			float x = (num2 * parent.d - num3 * parent.b) * num - bone.x;
			float y = (num3 * parent.a - num2 * parent.c) * num - bone.y;
			float num4 = MathUtils.Atan2(y, x) * 57.295776f - bone.shearX;
			if (bone.scaleX < 0f)
			{
				num4 += 180f;
			}
			if (num4 > 180f)
			{
				num4 -= 360f;
			}
			else if (num4 < -180f)
			{
				num4 += 360f;
			}
			bone.UpdateWorldTransform(bone.x, bone.y, bone.rotation + (num4 - bone.rotation) * alpha, bone.appliedScaleX, bone.appliedScaleY, bone.shearX, bone.shearY);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00027318 File Offset: 0x00025718
		public static void Apply(Bone parent, Bone child, float targetX, float targetY, int bendDir, float alpha)
		{
			if (alpha == 0f)
			{
				return;
			}
			float x = parent.x;
			float y = parent.y;
			float num = parent.appliedScaleX;
			float num2 = parent.appliedScaleY;
			int num3;
			int num4;
			if (num < 0f)
			{
				num = -num;
				num3 = 180;
				num4 = -1;
			}
			else
			{
				num3 = 0;
				num4 = 1;
			}
			if (num2 < 0f)
			{
				num2 = -num2;
				num4 = -num4;
			}
			float x2 = child.x;
			float num5 = child.y;
			float num6 = child.appliedScaleX;
			bool flag = Math.Abs(num - num2) <= 0.0001f;
			if (!flag && num5 != 0f)
			{
				child.worldX = parent.a * x2 + parent.worldX;
				child.worldY = parent.c * x2 + parent.worldY;
				num5 = 0f;
			}
			int num7;
			if (num6 < 0f)
			{
				num6 = -num6;
				num7 = 180;
			}
			else
			{
				num7 = 0;
			}
			Bone parent2 = parent.parent;
			float a = parent2.a;
			float b = parent2.b;
			float c = parent2.c;
			float d = parent2.d;
			float num8 = 1f / (a * d - b * c);
			float num9 = targetX - parent2.worldX;
			float num10 = targetY - parent2.worldY;
			float num11 = (num9 * d - num10 * b) * num8 - x;
			float num12 = (num10 * a - num9 * c) * num8 - y;
			num9 = child.worldX - parent2.worldX;
			num10 = child.worldY - parent2.worldY;
			float num13 = (num9 * d - num10 * b) * num8 - x;
			float num14 = (num10 * a - num9 * c) * num8 - y;
			float num15 = (float)Math.Sqrt((double)(num13 * num13 + num14 * num14));
			float num16 = child.data.length * num6;
			float num18;
			float num21;
			if (flag)
			{
				num16 *= num;
				float num17 = (num11 * num11 + num12 * num12 - num15 * num15 - num16 * num16) / (2f * num15 * num16);
				if (num17 < -1f)
				{
					num17 = -1f;
				}
				else if (num17 > 1f)
				{
					num17 = 1f;
				}
				num18 = (float)Math.Acos((double)num17) * (float)bendDir;
				float num19 = num15 + num16 * num17;
				float num20 = num16 * MathUtils.Sin(num18);
				num21 = MathUtils.Atan2(num12 * num19 - num11 * num20, num11 * num19 + num12 * num20);
			}
			else
			{
				float num22 = num * num16;
				float num23 = num2 * num16;
				float num24 = MathUtils.Atan2(num12, num11);
				float num25 = num22 * num22;
				float num26 = num23 * num23;
				float num27 = num15 * num15;
				float num28 = num11 * num11 + num12 * num12;
				float num29 = num26 * num27 + num25 * num28 - num25 * num26;
				float num30 = -2f * num26 * num15;
				float num31 = num26 - num25;
				float num32 = num30 * num30 - 4f * num31 * num29;
				if (num32 >= 0f)
				{
					float num33 = (float)Math.Sqrt((double)num32);
					if (num30 < 0f)
					{
						num33 = -num33;
					}
					num33 = -(num30 + num33) / 2f;
					float num34 = num33 / num31;
					float num35 = num29 / num33;
					float num36 = (Math.Abs(num34) >= Math.Abs(num35)) ? num35 : num34;
					if (num36 * num36 <= num28)
					{
						num10 = (float)Math.Sqrt((double)(num28 - num36 * num36)) * (float)bendDir;
						num21 = num24 - MathUtils.Atan2(num10, num36);
						num18 = MathUtils.Atan2(num10 / num2, (num36 - num15) / num);
						goto IL_4CF;
					}
				}
				float num37 = 0f;
				float num38 = float.MaxValue;
				float x3 = 0f;
				float num39 = 0f;
				float num40 = 0f;
				float num41 = 0f;
				float x4 = 0f;
				float num42 = 0f;
				num9 = num15 + num22;
				num32 = num9 * num9;
				if (num32 > num41)
				{
					num40 = 0f;
					num41 = num32;
					x4 = num9;
				}
				num9 = num15 - num22;
				num32 = num9 * num9;
				if (num32 < num38)
				{
					num37 = 3.1415927f;
					num38 = num32;
					x3 = num9;
				}
				float num43 = (float)Math.Acos((double)(-(double)num22 * num15 / (num25 - num26)));
				num9 = num22 * MathUtils.Cos(num43) + num15;
				num10 = num23 * MathUtils.Sin(num43);
				num32 = num9 * num9 + num10 * num10;
				if (num32 < num38)
				{
					num37 = num43;
					num38 = num32;
					x3 = num9;
					num39 = num10;
				}
				if (num32 > num41)
				{
					num40 = num43;
					num41 = num32;
					x4 = num9;
					num42 = num10;
				}
				if (num28 <= (num38 + num41) / 2f)
				{
					num21 = num24 - MathUtils.Atan2(num39 * (float)bendDir, x3);
					num18 = num37 * (float)bendDir;
				}
				else
				{
					num21 = num24 - MathUtils.Atan2(num42 * (float)bendDir, x4);
					num18 = num40 * (float)bendDir;
				}
			}
			IL_4CF:
			float num44 = MathUtils.Atan2(num5, x2) * (float)num4;
			num21 = (num21 - num44) * 57.295776f + (float)num3;
			num18 = ((num18 + num44) * 57.295776f - child.shearX) * (float)num4 + (float)num7;
			if (num21 > 180f)
			{
				num21 -= 360f;
			}
			else if (num21 < -180f)
			{
				num21 += 360f;
			}
			if (num18 > 180f)
			{
				num18 -= 360f;
			}
			else if (num18 < -180f)
			{
				num18 += 360f;
			}
			float rotation = parent.rotation;
			parent.UpdateWorldTransform(x, y, rotation + (num21 - rotation) * alpha, parent.appliedScaleX, parent.appliedScaleY, 0f, 0f);
			rotation = child.rotation;
			child.UpdateWorldTransform(x2, num5, rotation + (num18 - rotation) * alpha, child.appliedScaleX, child.appliedScaleY, child.shearX, child.shearY);
		}

		// Token: 0x04004112 RID: 16658
		internal IkConstraintData data;

		// Token: 0x04004113 RID: 16659
		internal ExposedList<Bone> bones = new ExposedList<Bone>();

		// Token: 0x04004114 RID: 16660
		internal Bone target;

		// Token: 0x04004115 RID: 16661
		internal int bendDirection;

		// Token: 0x04004116 RID: 16662
		internal float mix;
	}
}
