using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B21 RID: 2849
namespace Match3.Scripts1
{
	public abstract class ACountSpriteManager : SpriteManager
	{
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060042E5 RID: 17125
		public abstract int MaxCount { get; }

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060042E6 RID: 17126
		public abstract string Prefix { get; }

		// Token: 0x060042E7 RID: 17127 RVA: 0x000C1075 File Offset: 0x000BF475
		public virtual Sprite GetSprite(int count)
		{
			if (this.mapCountToString == null)
			{
				this.CreateCountMap();
			}
			return base[this.mapCountToString[count]];
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x000C109C File Offset: 0x000BF49C
		protected virtual void CreateCountMap()
		{
			this.mapCountToString = new Dictionary<int, string>();
			for (int i = 1; i <= this.MaxCount; i++)
			{
				this.mapCountToString[i] = string.Format(this.Prefix, i);
			}
		}

		// Token: 0x04006B9E RID: 27550
		protected Dictionary<int, string> mapCountToString;
	}
}
