using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200020C RID: 524
	public class BoneData
	{
		// Token: 0x06000FBB RID: 4027 RVA: 0x00026034 File Offset: 0x00024434
		public BoneData(string name, BoneData parent)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
			this.parent = parent;
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0002608A File Offset: 0x0002448A
		public BoneData Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00026092 File Offset: 0x00024492
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0002609A File Offset: 0x0002449A
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x000260A2 File Offset: 0x000244A2
		public float Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x000260AB File Offset: 0x000244AB
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x000260B3 File Offset: 0x000244B3
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x000260BC File Offset: 0x000244BC
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x000260C4 File Offset: 0x000244C4
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

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000260CD File Offset: 0x000244CD
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x000260D5 File Offset: 0x000244D5
		public float Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				this.rotation = value;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x000260DE File Offset: 0x000244DE
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x000260E6 File Offset: 0x000244E6
		public float ScaleX
		{
			get
			{
				return this.scaleX;
			}
			set
			{
				this.scaleX = value;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x000260EF File Offset: 0x000244EF
		// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x000260F7 File Offset: 0x000244F7
		public float ScaleY
		{
			get
			{
				return this.scaleY;
			}
			set
			{
				this.scaleY = value;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00026100 File Offset: 0x00024500
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x00026108 File Offset: 0x00024508
		public float ShearX
		{
			get
			{
				return this.shearX;
			}
			set
			{
				this.shearX = value;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00026111 File Offset: 0x00024511
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x00026119 File Offset: 0x00024519
		public float ShearY
		{
			get
			{
				return this.shearY;
			}
			set
			{
				this.shearY = value;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00026122 File Offset: 0x00024522
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x0002612A File Offset: 0x0002452A
		public bool InheritScale
		{
			get
			{
				return this.inheritScale;
			}
			set
			{
				this.inheritScale = value;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00026133 File Offset: 0x00024533
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x0002613B File Offset: 0x0002453B
		public bool InheritRotation
		{
			get
			{
				return this.inheritRotation;
			}
			set
			{
				this.inheritRotation = value;
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00026144 File Offset: 0x00024544
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040040F4 RID: 16628
		internal BoneData parent;

		// Token: 0x040040F5 RID: 16629
		internal string name;

		// Token: 0x040040F6 RID: 16630
		internal float length;

		// Token: 0x040040F7 RID: 16631
		internal float x;

		// Token: 0x040040F8 RID: 16632
		internal float y;

		// Token: 0x040040F9 RID: 16633
		internal float rotation;

		// Token: 0x040040FA RID: 16634
		internal float scaleX = 1f;

		// Token: 0x040040FB RID: 16635
		internal float scaleY = 1f;

		// Token: 0x040040FC RID: 16636
		internal float shearX;

		// Token: 0x040040FD RID: 16637
		internal float shearY;

		// Token: 0x040040FE RID: 16638
		internal bool inheritScale = true;

		// Token: 0x040040FF RID: 16639
		internal bool inheritRotation = true;
	}
}
