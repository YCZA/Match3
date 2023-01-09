using System;

namespace Match3.Scripts1.Shared.DataStructures
{
	// Token: 0x02000ABD RID: 2749
	public class Map1d<T>
	{
		// Token: 0x0600414B RID: 16715 RVA: 0x00152354 File Offset: 0x00150754
		public Map1d(int size)
		{
			this.map = new T[size * size];
			this.size = size;
		}

		// Token: 0x1700096F RID: 2415
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

		// Token: 0x17000970 RID: 2416
		public T this[int x, int y]
		{
			get
			{
				return (!this.IsValid(x, y)) ? default(T) : this.GetTileUnsafe(x, y);
			}
			set
			{
				if (this.IsValid(x, y))
				{
					this.SetUnsafe(x, y, value);
				}
			}
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x001523E8 File Offset: 0x001507E8
		public T GetTileUnsafe(int x, int y)
		{
			return this.map[x * this.size + y];
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x001523FF File Offset: 0x001507FF
		public void SetUnsafe(int x, int y, T value)
		{
			this.map[x * this.size + y] = value;
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00152417 File Offset: 0x00150817
		public bool IsValid(int x, int y)
		{
			return x >= 0 && x < this.size && y >= 0 && y < this.size;
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x0015243F File Offset: 0x0015083F
		public void Fill(T[] source)
		{
			Array.Copy(source, this.map, this.map.Length);
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00152455 File Offset: 0x00150855
		public void Clear()
		{
			Array.Clear(this.map, 0, this.map.Length);
		}

		// Token: 0x04006AE8 RID: 27368
		private readonly T[] map;

		// Token: 0x04006AE9 RID: 27369
		public readonly int size;
	}
}
