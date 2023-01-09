using System;
using System.Collections;

namespace Match3.Scripts1.Shared.DataStructures
{
	// Token: 0x02000ABC RID: 2748
	public class Map<T> : IEnumerable, ICloneable
	{
		// Token: 0x0600413D RID: 16701 RVA: 0x000A5FCB File Offset: 0x000A43CB
		public Map(T[,] other)
		{
			this.map = (T[,])other.Clone();
			this.size = this.map.GetLength(0);
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x000A5FF6 File Offset: 0x000A43F6
		public Map(int size)
		{
			this.map = new T[size, size];
			this.size = size;
		}

		// Token: 0x1700096D RID: 2413
		public T this[IntVector2 vec]
		{
			get
			{
				return this[vec.x, vec.y];
			}
			set
			{
				this[vec.x, vec.y] = value;
			}
		}

		// Token: 0x1700096E RID: 2414
		public T this[int x, int y]
		{
			get
			{
				return (!this.IsValid(x, y)) ? default(T) : this.__getTileUnsafe(x, y);
			}
			set
			{
				if (this.IsValid(x, y))
				{
					this.__setUnsafe(x, y, value);
				}
			}
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x000A6088 File Offset: 0x000A4488
		public T __getTileUnsafe(int x, int y)
		{
			return this.map[x, y];
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x000A6097 File Offset: 0x000A4497
		public void __setUnsafe(int x, int y, T value)
		{
			this.map[x, y] = value;
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x000A60A7 File Offset: 0x000A44A7
		public IEnumerator GetEnumerator()
		{
			return this.map.GetEnumerator();
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x000A60B4 File Offset: 0x000A44B4
		public bool IsValid(IntVector2 vec)
		{
			return vec.x >= 0 && vec.x < this.size && vec.y >= 0 && vec.y < this.size;
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x000A60F4 File Offset: 0x000A44F4
		public bool IsValid(int x, int y)
		{
			return x >= 0 && x < this.size && y >= 0 && y < this.size;
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x000A611C File Offset: 0x000A451C
		public void AssignArray(Map<T> other)
		{
			this.map = other.map;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x000A612C File Offset: 0x000A452C
		public object Clone()
		{
			return new Map<T>(this.map);
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x000A6146 File Offset: 0x000A4546
		public void Clear()
		{
			Array.Clear(this.map, 0, this.map.Length);
		}

		// Token: 0x04006AE6 RID: 27366
		private T[,] map;

		// Token: 0x04006AE7 RID: 27367
		public int size;
	}
}
