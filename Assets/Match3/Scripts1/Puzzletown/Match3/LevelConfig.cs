using System;
using System.Collections.Generic;
using System.Reflection;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F0 RID: 1264
	[Serializable]
	public class LevelConfig
	{
		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x0009A57A File Offset: 0x0009897A
		// (set) Token: 0x060022D9 RID: 8921 RVA: 0x0009A582 File Offset: 0x00098982
		public ALevelCollectionConfig LevelCollectionConfig { get; set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x0009A58B File Offset: 0x0009898B
		// (set) Token: 0x060022DB RID: 8923 RVA: 0x0009A593 File Offset: 0x00098993
		public AreaConfig.Tier SelectedTier { get; set; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x0009A59C File Offset: 0x0009899C
		public Level Level
		{
			get
			{
				int level = 0;
				int tier = 0;
				if (this.LevelCollectionConfig != null)
				{
					level = this.LevelCollectionConfig.level;
					tier = (int)this.SelectedTier;
				}
				return new Level(level, tier);
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x0009A5D4 File Offset: 0x000989D4
		public string Name
		{
			get
			{
				string arg = (this.levelSet == null) ? string.Empty : (this.levelSet.Value.ToString() + "-");
				string arg2 = string.Empty + this.LevelCollectionConfig.level.ToString().PadLeft(4, '0');
				return arg + arg2 + this.SelectedTier;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x0009A65A File Offset: 0x00098A5A
		public int Diamonds
		{
			get
			{
				return (!this.IsCompleted) ? this.CurrentTier.diamonds : 0;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x0009A678 File Offset: 0x00098A78
		// (set) Token: 0x060022E0 RID: 8928 RVA: 0x0009A680 File Offset: 0x00098A80
		public bool IsEditMode { get; set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x0009A689 File Offset: 0x00098A89
		// (set) Token: 0x060022E2 RID: 8930 RVA: 0x0009A691 File Offset: 0x00098A91
		public bool IsAutoPlayMode { get; set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x0009A69A File Offset: 0x00098A9A
		// (set) Token: 0x060022E4 RID: 8932 RVA: 0x0009A6A2 File Offset: 0x00098AA2
		public bool IsCompleted { get; set; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x0009A6AB File Offset: 0x00098AAB
		public bool HasCoins
		{
			get
			{
				return this.hasCoinsObjective || this.CheckGemsForCoins();
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0009A6C1 File Offset: 0x00098AC1
		public bool HasHiddenItems
		{
			get
			{
				return this.layout.HasHiddenItems;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0009A6CE File Offset: 0x00098ACE
		private TierConfig CurrentTier
		{
			get
			{
				return this.LevelCollectionConfig.tiers[(int)this.SelectedTier];
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x0009A6E4 File Offset: 0x00098AE4
		public void UpdateObjectives()
		{
			this.hasCoinsObjective = false;
			List<MaterialAmount> list = new List<MaterialAmount>();
			foreach (MaterialAmount materialAmount in this.data.objectives)
			{
				if (materialAmount.type.Equals(this.data.hiddenItemName))
				{
					break;
				}
				if (!this.hasCoinsObjective && materialAmount.type.EqualsIgnoreCase("coins"))
				{
					this.hasCoinsObjective = true;
				}
				MaterialAmount item = new MaterialAmount(materialAmount.type, materialAmount.amount, MaterialAmountUsage.Undefined, 0);
				if (item.amount < 0)
				{
					FieldInfo field = base.GetType().GetField(item.type);
					item.amount = LevelLayout.GetSum((int[])field.GetValue(this));
				}
				list.Add(item);
			}
			if (this.layout.HasHiddenItems)
			{
				MaterialAmount itemToAdd = new MaterialAmount(this.data.hiddenItemName, this.layout.NumHiddenItems, MaterialAmountUsage.Undefined, 0);
				list.AddIfNotAlreadyPresent(itemToAdd, false);
			}
			if (this.layout.HasDirtAndTreasure)
			{
				MaterialAmount itemToAdd2 = new MaterialAmount("treasure", this.layout.NumTreasure, MaterialAmountUsage.Undefined, 0);
				list.AddIfNotAlreadyPresent(itemToAdd2, false);
			}
			if (this.layout.HasWaterAndUnwateredFields)
			{
				MaterialAmount itemToAdd3 = new MaterialAmount("water", this.layout.NumUnwateredFields, MaterialAmountUsage.Undefined, 0);
				list.AddIfNotAlreadyPresent(itemToAdd3, false);
			}
			if (this.layout.NumResistantBlocker > 0)
			{
				MaterialAmount itemToAdd4 = new MaterialAmount("resistant_blocker", this.layout.NumResistantBlocker, MaterialAmountUsage.Undefined, 0);
				list.AddIfNotAlreadyPresent(itemToAdd4, false);
			}
			if (this.data.ChameleonCount > 0)
			{
				MaterialAmount itemToAdd5 = new MaterialAmount("chameleon", this.data.ChameleonCount, MaterialAmountUsage.Undefined, 0);
				list.AddIfNotAlreadyPresent(itemToAdd5, false);
			}
			if (list.Count == 0)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Level has no Objectives!"
				});
			}
			this.data.objectives = list.ToArray();
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x0009A904 File Offset: 0x00098D04
		private bool CheckGemsForCoins()
		{
			if (!this.data.gems.IsNullOrEmptyCollection())
			{
				foreach (MaterialAmount materialAmount in this.data.gems)
				{
					if (materialAmount.type.EqualsIgnoreCase("coins"))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004E94 RID: 20116
		public LevelData data;

		// Token: 0x04004E95 RID: 20117
		public LevelLayout layout;

		// Token: 0x04004E96 RID: 20118
		[NonSerialized]
		public int coinMultiplier;

		// Token: 0x04004E97 RID: 20119
		[NonSerialized]
		public int tournamentMultiplier;

		// Token: 0x04004E98 RID: 20120
		[NonSerialized]
		public PreBoostConfig preBoostConfig;

		// Token: 0x04004E99 RID: 20121
		[NonSerialized]
		public List<MaterialAmount> levelOfDayRewards;

		// Token: 0x04004E9A RID: 20122
		[NonSerialized]
		public List<MaterialAmount> diveForTreasureRewards;

		// Token: 0x04004E9B RID: 20123
		[NonSerialized]
		public List<MaterialAmount> pirateBreakoutRewards;

		// Token: 0x04004E9C RID: 20124
		[NonSerialized]
		public MaterialAmount seasonalRewards;

		// Token: 0x04004E9D RID: 20125
		[NonSerialized]
		public int? levelSet;

		// Token: 0x04004EA3 RID: 20131
		private bool hasCoinsObjective;

		// Token: 0x04004EA4 RID: 20132
		public string collectable;
	}
}
