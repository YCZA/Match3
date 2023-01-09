using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F1 RID: 1265
	[Serializable]
	public class LevelData
	{
		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x0009A98C File Offset: 0x00098D8C
		public int DropItemsCount
		{
			get
			{
				foreach (MaterialAmount materialAmount in this.objectives)
				{
					if (materialAmount.type.EqualsIgnoreCase("droppable"))
					{
						return materialAmount.amount;
					}
				}
				return 0;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x0009A9E0 File Offset: 0x00098DE0
		public int ClimberCount
		{
			get
			{
				foreach (MaterialAmount materialAmount in this.objectives)
				{
					if (materialAmount.type.EqualsIgnoreCase("climber"))
					{
						return materialAmount.amount;
					}
				}
				return 0;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x0009AA34 File Offset: 0x00098E34
		public int ChameleonCount
		{
			get
			{
				foreach (MaterialAmount materialAmount in this.objectives)
				{
					if (materialAmount.type.EqualsIgnoreCase("chameleon"))
					{
						return materialAmount.amount;
					}
				}
				return 0;
			}
		}

		// Token: 0x04004EA5 RID: 20133
		public int moves;

		// Token: 0x04004EA6 RID: 20134
		public MaterialAmount[] gems;

		// Token: 0x04004EA7 RID: 20135
		public MaterialAmount[] objectives = new MaterialAmount[0];

		// Token: 0x04004EA8 RID: 20136
		public SpawnRatio spawnRatioDroppable;

		// Token: 0x04004EA9 RID: 20137
		public SpawnRatio[] spawnRatios;

		// Token: 0x04004EAA RID: 20138
		public string hiddenItemName = "monkey";
	}
}
