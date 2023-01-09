using System;
using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200059E RID: 1438
	public class GemFactory : IGemFactory
	{
		// Token: 0x0600258B RID: 9611 RVA: 0x000A721C File Offset: 0x000A561C
		public GemFactory(LevelConfig config)
		{
			if (config.data == null)
			{
				throw new InvalidOperationException("levelData cannot be null");
			}
			foreach (MaterialAmount materialAmount in config.data.gems)
			{
				this.highestProbability = materialAmount.amount <= this.highestProbability ? this.highestProbability : materialAmount.amount;
				this.sum += materialAmount.amount;
				GemColor key = (GemColor)Enum.Parse(typeof(GemColor), materialAmount.type, true);
				this.probabilities.Add(new KeyValuePair<GemColor, int>(key, this.sum));
			}
			this.neededAmountDropItems = config.data.DropItemsCount;
			foreach (int num in config.layout.gemColors)
			{
				// 7表示dropitem
				if (num == 7)
				{
					this.neededAmountDropItems--;
				}
			}
			this.neededAmountClimberSpawn = config.data.ClimberCount;
			foreach (int num2 in config.layout.gemColors)
			{
				// 12表示climber
				if (num2 == 12)
				{
					this.neededAmountClimberSpawn--;
				}
			}
			this.neededAmountChameleonSpawn = config.data.ChameleonCount;
			foreach (int num3 in config.layout.gemTypes)
			{
				// ???
				if (num3 == 11 || num3 == 12)
				{
					this.neededAmountChameleonSpawn--;
				}
			}
			if (this.neededAmountClimberSpawn > 0)
			{
				this.spawnClimber = new SpawnRatio();
				this.spawnClimber.gemColor = GemColor.Climber;
				this.spawnClimber.gemType = GemType.Undefined;
				this.spawnClimber.minSpawn = 1;
				this.spawnClimber.maxSpawn = 1;
				this.spawnClimber.probability = 100;
			}
			this.spawnDroppable = config.data.spawnRatioDroppable;
			this.spawnRatios = config.data.spawnRatios;
			if (this.neededAmountChameleonSpawn > 0)
			{
				foreach (SpawnRatio spawnRatio in this.spawnRatios)
				{
					if (spawnRatio.gemType == GemType.ChameleonReduced)
					{
						this.spawnReducedChameleonRatio = new SpawnRatio
						{
							gemColor = GemColor.Undefined,
							gemType = spawnRatio.gemType,
							minSpawn = spawnRatio.minSpawn,
							maxSpawn = spawnRatio.maxSpawn,
							probability = spawnRatio.probability
						};
						this.spawnReducedChameleonRatio.gemColor = GemColor.Undefined;
					}
					else if (spawnRatio.gemType == GemType.Chameleon)
					{
						this.spawnAllChameleonRatio = new SpawnRatio
						{
							gemColor = GemColor.Undefined,
							gemType = spawnRatio.gemType,
							minSpawn = spawnRatio.minSpawn,
							maxSpawn = spawnRatio.maxSpawn,
							probability = spawnRatio.probability
						};
					}
				}
				this.StopSpawningGemsOfTypeFromRatios(GemType.ChameleonReduced);
				this.StopSpawningGemsOfTypeFromRatios(GemType.Chameleon);
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000A757C File Offset: 0x000A597C
		public void AddExtraColorProbability()
		{
			if (this.probabilities.Count >= 6)
			{
				return;
			}
			List<GemColor> list = new List<GemColor>(GemFactory.availableColors);
			foreach (KeyValuePair<GemColor, int> keyValuePair in this.probabilities)
			{
				if (list.Contains(keyValuePair.Key))
				{
					list.Remove(keyValuePair.Key);
				}
			}
			GemColor key = list[RandomHelper.Next(list.Count)];
			this.sum += this.highestProbability;
			this.probabilities.Add(new KeyValuePair<GemColor, int>(key, this.sum));
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000A764C File Offset: 0x000A5A4C
		public void StopSpawningMatchablesFromRatios()
		{
			List<SpawnRatio> list = ListPool<SpawnRatio>.Create(10);
			foreach (SpawnRatio spawnRatio in this.spawnRatios)
			{
				if (spawnRatio.gemColor == GemColor.Cannonball)
				{
					list.Add(spawnRatio);
				}
			}
			this.spawnRatios = list.ToArray();
			ListPool<SpawnRatio>.Release(list);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000A76A6 File Offset: 0x000A5AA6
		public bool StopSpawningClimberGems()
		{
			if (this.neededAmountClimberSpawn == 0)
			{
				this.StopSpawningGemsOfTypeFromRatios(GemType.ClimberGem);
				this.neededAmountClimberSpawn = -1;
			}
			return this.HasStoppedSpawningClimberGems();
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000A76C8 File Offset: 0x000A5AC8
		public bool HasStoppedSpawningClimberGems()
		{
			return this.neededAmountClimberSpawn < 0;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000A76E0 File Offset: 0x000A5AE0
		public void AddSpawnedClimber()
		{
			this.neededAmountClimberSpawn--;
			if (this.neededAmountClimberSpawn == 0)
			{
				this.spawnClimber = null;
			}
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000A7702 File Offset: 0x000A5B02
		public void StopSpawningChameleonGems()
		{
			if (this.neededAmountChameleonSpawn == 0)
			{
				this.StopSpawningGemsOfTypeFromRatios(GemType.Chameleon);
				this.StopSpawningGemsOfTypeFromRatios(GemType.ChameleonReduced);
				this.neededAmountChameleonSpawn = -1;
			}
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000A7726 File Offset: 0x000A5B26
		private void AddSpawnedChameleon()
		{
			this.neededAmountChameleonSpawn--;
			if (this.neededAmountChameleonSpawn == 0)
			{
				this.StopSpawningChameleonGems();
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000A7748 File Offset: 0x000A5B48
		public Gem GetRandomDifferentGem(GemColor excludedColor)
		{
			GemColor gemColor;
			for (gemColor = excludedColor; gemColor == excludedColor; gemColor = this.GetRandomColor())
			{
			}
			return new Gem(gemColor);
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000A7770 File Offset: 0x000A5B70
		public Gem CreateGem(Field field, Fields fields, Gem template = default(Gem), bool isTrickling = true)
		{
			field.AssignGem(this.CreateRandomGem());	// 从config.data.gems中随机一个颜色
			field.gem.type = template.type;
			field.gem.modifier = template.modifier;
			bool flag = false;
			if (isTrickling)	// 是流动下来的方块
			{
				bool canSpawnDropItems = field.CanSpawnDropItems && this.spawnDroppable != null && this.neededAmountDropItems > 0;
				bool canSpawnChameleons = field.CanSpawnChameleons && (this.spawnReducedChameleonRatio != null || this.spawnAllChameleonRatio != null) && this.neededAmountChameleonSpawn > 0;
				if (canSpawnDropItems)
				{
					if (fields.ShouldSpawnGems(this.spawnDroppable))
					{
						flag = true;
						field.gem.color = this.spawnDroppable.gemColor;
						this.neededAmountDropItems--;
					}
				}
				else if (canSpawnChameleons)
				{
					if (this.spawnReducedChameleonRatio != null && fields.ShouldSpawnGems(this.spawnReducedChameleonRatio))
					{
						flag = true;
						this.SetSpawningChameleon(field, this.spawnReducedChameleonRatio);
					}
					else if (this.spawnAllChameleonRatio != null && fields.ShouldSpawnGems(this.spawnAllChameleonRatio))
					{
						flag = true;
						this.SetSpawningChameleon(field, this.spawnAllChameleonRatio);
					}
				}
				if (!flag && !this.spawnRatios.IsNullOrEmptyCollection())
				{
					foreach (SpawnRatio spawnRatio in this.spawnRatios)
					{
						if (fields.ShouldSpawnGems(spawnRatio))
						{
							if (spawnRatio.gemColor != GemColor.Random)
							{
								field.gem.color = spawnRatio.gemColor;
							}
							field.gem.type = spawnRatio.gemType;
							break;
						}
					}
				}
			}
			if (Portal.IsExit(field.portalId))
			{
				field.gem.lastPortalUsed = field.portalId;
			}
			// Debug.LogError("CreateGem:" + field.gem.modifier);
			return field.gem;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000A7950 File Offset: 0x000A5D50
		private void SetSpawningChameleon(Field field, SpawnRatio spawnRatio)
		{
			field.gem.type = spawnRatio.gemType;
			field.gem.color = GemColor.Red;
			field.gem.processed = true;
			field.gem.direction = Gem.GetRandomGemDirection();
			this.AddSpawnedChameleon();
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000A799C File Offset: 0x000A5D9C
		public GemColor GetRandomColor()
		{
			if (this.probabilities.Count <= 2)
			{
				throw new ArgumentException("There are not enough colors to remove groups.");
			}
			int num = RandomHelper.Next(0, this.sum + 1);
			foreach (KeyValuePair<GemColor, int> keyValuePair in this.probabilities)
			{
				if (num <= keyValuePair.Value)
				{
					return keyValuePair.Key;
				}
			}
			return GemColor.Undefined;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000A7A3C File Offset: 0x000A5E3C
		private void StopSpawningGemsOfTypeFromRatios(GemType excludedType)
		{
			List<SpawnRatio> list = ListPool<SpawnRatio>.Create(10);
			foreach (SpawnRatio spawnRatio in this.spawnRatios)
			{
				if (spawnRatio.gemType != excludedType)
				{
					list.Add(spawnRatio);
				}
			}
			this.spawnRatios = list.ToArray();
			ListPool<SpawnRatio>.Release(list);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000A7A95 File Offset: 0x000A5E95
		private Gem CreateRandomGem()
		{
			return new Gem(this.GetRandomColor());
		}

		// Token: 0x040050E0 RID: 20704
		private readonly List<KeyValuePair<GemColor, int>> probabilities = new List<KeyValuePair<GemColor, int>>();

		// Token: 0x040050E1 RID: 20705
		private SpawnRatio[] spawnRatios;

		// Token: 0x040050E2 RID: 20706
		private readonly SpawnRatio spawnDroppable;

		// Token: 0x040050E3 RID: 20707
		private readonly SpawnRatio spawnReducedChameleonRatio;

		// Token: 0x040050E4 RID: 20708
		private readonly SpawnRatio spawnAllChameleonRatio;

		// Token: 0x040050E5 RID: 20709
		private SpawnRatio spawnClimber;

		// Token: 0x040050E6 RID: 20710
		private int neededAmountDropItems;

		// Token: 0x040050E7 RID: 20711
		private int neededAmountClimberSpawn;

		// Token: 0x040050E8 RID: 20712
		private int neededAmountChameleonSpawn;

		// Token: 0x040050E9 RID: 20713
		private readonly int highestProbability;

		// Token: 0x040050EA RID: 20714
		private int sum;

		// Token: 0x040050EB RID: 20715
		private static readonly List<GemColor> availableColors = new List<GemColor>
		{
			GemColor.Blue,
			GemColor.Green,
			GemColor.Orange,
			GemColor.Purple,
			GemColor.Red,
			GemColor.Yellow
		};
	}
}
