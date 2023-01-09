using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A0 RID: 1440
	public class Group : List<Gem>, IMatchResult, IPoolable
	{
		// Token: 0x0600259B RID: 9627 RVA: 0x000A7B10 File Offset: 0x000A5F10
		public Group()
		{
			this.Color = GemColor.Undefined;
			this.superGemEnumerator = new Group.SuperGemEnumerator(this);
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000A7B2B File Offset: 0x000A5F2B
		public Group(int capacity) : this()
		{
			base.Capacity = capacity;
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000A7B3A File Offset: 0x000A5F3A
		public Group(Gem a) : this()
		{
			this.Color = a.color;
			base.Add(a);
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000A7B56 File Offset: 0x000A5F56
		public Group(Gem a, Gem b) : this(a)
		{
			base.Add(a);
			base.Add(b);
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x000A7B6D File Offset: 0x000A5F6D
		public bool Empty
		{
			get
			{
				return base.Count == 0;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000A7B78 File Offset: 0x000A5F78
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x000A7B80 File Offset: 0x000A5F80
		public GemColor Color { get; set; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000A7B8C File Offset: 0x000A5F8C
		public bool Vertical
		{
			get
			{
				for (int i = 1; i < base.Count; i++)
				{
					if (base[i].position.x != base[i - 1].position.x)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000A7BE4 File Offset: 0x000A5FE4
		public bool Horizontal
		{
			get
			{
				for (int i = 1; i < base.Count; i++)
				{
					if (base[i].position.y != base[i - 1].position.y)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000A7C3A File Offset: 0x000A603A
		public List<IntVector2> Positions
		{
			get
			{
				return (from g in this
				select g.position).ToList<IntVector2>();
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000A7C64 File Offset: 0x000A6064
		public Group.SuperGemEnumerator SuperGems
		{
			get
			{
				return this.superGemEnumerator;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x000A7C6C File Offset: 0x000A606C
		public Gem LeftGem
		{
			get
			{
				Gem result = base[0];
				for (int i = 1; i < base.Count; i++)
				{
					if (result.position.x > base[i].position.x)
					{
						result = base[i];
					}
				}
				return result;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x000A7CC8 File Offset: 0x000A60C8
		public Gem RightGem
		{
			get
			{
				Gem result = base[0];
				for (int i = 1; i < base.Count; i++)
				{
					if (result.position.x < base[i].position.x)
					{
						result = base[i];
					}
				}
				return result;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000A7D24 File Offset: 0x000A6124
		public Gem DownGem
		{
			get
			{
				Gem result = base[0];
				for (int i = 1; i < base.Count; i++)
				{
					if (result.position.y > base[i].position.y)
					{
						result = base[i];
					}
				}
				return result;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000A7D80 File Offset: 0x000A6180
		public Gem UpGem
		{
			get
			{
				Gem result = base[0];
				for (int i = 1; i < base.Count; i++)
				{
					if (result.position.y < base[i].position.y)
					{
						result = base[i];
					}
				}
				return result;
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000A7DDC File Offset: 0x000A61DC
		public Gem GetGemAtPosition(IntVector2 pos)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].position == pos)
				{
					return base[i];
				}
			}
			return default(Gem);
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000A7E2C File Offset: 0x000A622C
		public IntVector2 LowerLeft
		{
			get
			{
				return new IntVector2(this.LeftGem.position.x, this.DownGem.position.y);
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x000A7E64 File Offset: 0x000A6264
		public int Width
		{
			get
			{
				return this.RightGem.position.x - this.LeftGem.position.x + 1;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x000A7E9C File Offset: 0x000A629C
		public int Height
		{
			get
			{
				return this.UpGem.position.y - this.DownGem.position.y + 1;
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000A7ED2 File Offset: 0x000A62D2
		public void Release()
		{
			base.Clear();
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000A7EDC File Offset: 0x000A62DC
		public bool Includes(IntVector2 pos)
		{
			for (int i = 0; i < base.Count; i++)
			{
				Gem gem = base[i];
				if (pos.x == gem.position.x && pos.y == gem.position.y)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000A7F3C File Offset: 0x000A633C
		public void Merge(Group other)
		{
			foreach (Gem item in other)
			{
				if (!this.Includes(item.position))
				{
					base.Add(item);
				}
			}
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000A7FA8 File Offset: 0x000A63A8
		public void RemoveDuplicates(Group other)
		{
			foreach (Gem gem in other)
			{
				this.RemovePosition(gem.position);
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000A8008 File Offset: 0x000A6408
		public void RemovePosition(IntVector2 position)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].position == position)
				{
					base.RemoveAt(i);
					break;
				}
			}
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000A8054 File Offset: 0x000A6454
		public void ReplacePosition(Gem gem)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].position == gem.position)
				{
					base[i] = gem;
					break;
				}
			}
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000A80A8 File Offset: 0x000A64A8
		public bool Contains(IntVector2 position)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].position == position)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000A80EC File Offset: 0x000A64EC
		public bool IsCompletelyCovered()
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (!base[i].IsCovered)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040050EC RID: 20716
		private Group.SuperGemEnumerator superGemEnumerator;

		// Token: 0x020005A1 RID: 1441
		public struct SuperGemEnumerator : IEnumerator<Gem>, IEnumerator, IDisposable
		{
			// Token: 0x060025B7 RID: 9655 RVA: 0x000A8130 File Offset: 0x000A6530
			public SuperGemEnumerator(Group group)
			{
				this.group = group;
				this.index = -1;
			}

			// Token: 0x060025B8 RID: 9656 RVA: 0x000A8140 File Offset: 0x000A6540
			public bool MoveNext()
			{
				do
				{
					this.index++;
				}
				while (this.index >= 0 && this.index < this.group.Count && this.group[this.index].type == GemType.Undefined);
				return this.index < this.group.Count;
			}

			// Token: 0x060025B9 RID: 9657 RVA: 0x000A81B0 File Offset: 0x000A65B0
			public void RemoveCurrent()
			{
				this.group.Remove(this.group[this.index--]);
			}

			// Token: 0x060025BA RID: 9658 RVA: 0x000A81E5 File Offset: 0x000A65E5
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x060025BB RID: 9659 RVA: 0x000A81EE File Offset: 0x000A65EE
			public Gem Current
			{
				get
				{
					return this.group[this.index];
				}
			}

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x060025BC RID: 9660 RVA: 0x000A8201 File Offset: 0x000A6601
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060025BD RID: 9661 RVA: 0x000A820E File Offset: 0x000A660E
			public void Dispose()
			{
			}

			// Token: 0x040050EF RID: 20719
			private Group group;

			// Token: 0x040050F0 RID: 20720
			private int index;
		}
	}
}
