using System;
using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x02000ABB RID: 2747
	[Serializable]
	public struct IntVector2 : IEquatable<IntVector2>
	{
		// Token: 0x06004125 RID: 16677 RVA: 0x00151F00 File Offset: 0x00150300
		public IntVector2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x00151F10 File Offset: 0x00150310
		public IntVector2(IntVector2 other)
		{
			this.x = other.x;
			this.y = other.y;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x00151F2C File Offset: 0x0015032C
		public void Assign(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00151F3C File Offset: 0x0015033C
		public static explicit operator Vector3(IntVector2 pos)
		{
			return new Vector3((float)pos.x, (float)pos.y, 0f);
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x00151F58 File Offset: 0x00150358
		public static explicit operator IntVector2(Vector3 pos)
		{
			IntVector2 result = new IntVector2(Mathf.FloorToInt(pos.x + 0.5f), Mathf.FloorToInt(pos.y + 0.5f));
			return result;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x00151F94 File Offset: 0x00150394
		public static IntVector2 ProjectToGridXZ(Vector3 worldPosition)
		{
			IntVector2 result = new IntVector2(Mathf.FloorToInt(worldPosition.x + 0.5f), Mathf.FloorToInt(worldPosition.z + 0.5f));
			return result;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x00151FCD File Offset: 0x001503CD
		public Vector3 ProjectToVector3XZ()
		{
			return new Vector3((float)this.x, 0f, (float)this.y);
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600412C RID: 16684 RVA: 0x00151FE7 File Offset: 0x001503E7
		public int SqrMagnitude
		{
			get
			{
				return this.x * this.x + this.y * this.y;
			}
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x00152004 File Offset: 0x00150404
		public static IntVector2 operator +(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x + b.x, a.y + b.y);
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00152029 File Offset: 0x00150429
		public static IntVector2 operator -(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x - b.x, a.y - b.y);
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0015204E File Offset: 0x0015044E
		public static IntVector2 operator *(IntVector2 vec, int factor)
		{
			return new IntVector2(vec.x * factor, vec.y * factor);
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00152067 File Offset: 0x00150467
		public static IntVector2 operator *(int factor, IntVector2 vec)
		{
			return vec * factor;
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x00152070 File Offset: 0x00150470
		public static Vector2 operator *(float factor, IntVector2 vec)
		{
			return new Vector2((float)vec.x * factor, (float)vec.y * factor);
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x0015208B File Offset: 0x0015048B
		public static bool operator ==(IntVector2 a, IntVector2 b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00152095 File Offset: 0x00150495
		public static bool operator !=(IntVector2 a, IntVector2 b)
		{
			return !(a == b);
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x001520A1 File Offset: 0x001504A1
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.x, this.y);
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x001520C4 File Offset: 0x001504C4
		public override bool Equals(object other)
		{
			if (other == null || base.GetType() != other.GetType())
			{
				return false;
			}
			IntVector2 intVector = (IntVector2)other;
			return this.x == intVector.x && this.y == intVector.y;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0015211F File Offset: 0x0015051F
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00152146 File Offset: 0x00150546
		public bool Equals(IntVector2 other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x0015216C File Offset: 0x0015056C
		public static int SimpleDistance(IntVector2 a, IntVector2 b)
		{
			int a2 = Mathf.Abs(a.x - b.x);
			int b2 = Mathf.Abs(a.y - b.y);
			return Mathf.Max(a2, b2);
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x001521AC File Offset: 0x001505AC
		public static float Distance(IntVector2 a, IntVector2 b)
		{
			float num = (float)(a.x - b.x);
			float num2 = (float)(a.y - b.y);
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x001521E8 File Offset: 0x001505E8
		public static IntVector2 Direction(IntVector2 from, IntVector2 to)
		{
			IntVector2 result = to - from;
			result.x = Math.Sign(result.x);
			result.y = Math.Sign(result.y);
			return result;
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x00152224 File Offset: 0x00150624
		public static IntVector2 OppositeVector(IntVector2 a)
		{
			return new IntVector2(-a.x, -a.y);
		}

		// Token: 0x04006ADB RID: 27355
		public int x;

		// Token: 0x04006ADC RID: 27356
		public int y;

		// Token: 0x04006ADD RID: 27357
		public static readonly IntVector2 Zero = new IntVector2(0, 0);

		// Token: 0x04006ADE RID: 27358
		public static readonly IntVector2 Left = new IntVector2(-1, 0);

		// Token: 0x04006ADF RID: 27359
		public static readonly IntVector2 Right = new IntVector2(1, 0);

		// Token: 0x04006AE0 RID: 27360
		public static readonly IntVector2 Down = new IntVector2(0, -1);

		// Token: 0x04006AE1 RID: 27361
		public static readonly IntVector2 Up = new IntVector2(0, 1);

		// Token: 0x04006AE2 RID: 27362
		public static readonly IntVector2 One = new IntVector2(1, 1);

		// Token: 0x04006AE3 RID: 27363
		public static readonly IntVector2 Invalid = new IntVector2(int.MinValue, int.MinValue);

		// Token: 0x04006AE4 RID: 27364
		public static readonly IntVector2[] Sides = new IntVector2[]
		{
			IntVector2.Left,
			IntVector2.Up,
			IntVector2.Right,
			IntVector2.Down
		};

		// Token: 0x04006AE5 RID: 27365
		public static readonly IntVector2[] SelfSides = new IntVector2[]
		{
			IntVector2.Zero,
			IntVector2.Left,
			IntVector2.Right,
			IntVector2.Down,
			IntVector2.Up
		};
	}
}
