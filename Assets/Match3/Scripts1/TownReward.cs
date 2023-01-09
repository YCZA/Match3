using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x0200090F RID: 2319
namespace Match3.Scripts1
{
	public class TownReward
	{
		// Token: 0x06003892 RID: 14482 RVA: 0x001164FB File Offset: 0x001148FB
		public TownReward(string type, Sprite sprite, int amount, bool blueprint)
		{
			this.type = type;
			this.sprite = sprite;
			this.amount = amount;
			this.blueprint = blueprint;
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x00116520 File Offset: 0x00114920
		public TownReward(MaterialAmount materialAmount)
		{
			this.materialAmount = materialAmount;
			this.type = materialAmount.type;
			this.amount = materialAmount.amount;
			this.blueprint = false;
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06003894 RID: 14484 RVA: 0x00116550 File Offset: 0x00114950
		// (set) Token: 0x06003895 RID: 14485 RVA: 0x00116558 File Offset: 0x00114958
		public int viewIndex { get; set; }

		// Token: 0x06003896 RID: 14486 RVA: 0x00116564 File Offset: 0x00114964
		public static List<TownReward> FromMaterials(Materials materials)
		{
			List<TownReward> list = new List<TownReward>();
			foreach (MaterialAmount materialAmount in materials)
			{
				list.Add(new TownReward(materialAmount));
			}
			return list;
		}

		// Token: 0x040060D4 RID: 24788
		public MaterialAmount materialAmount;

		// Token: 0x040060D5 RID: 24789
		public string type;

		// Token: 0x040060D6 RID: 24790
		public int amount;

		// Token: 0x040060D7 RID: 24791
		public Sprite sprite;

		// Token: 0x040060D8 RID: 24792
		public bool blueprint;
	}
}
