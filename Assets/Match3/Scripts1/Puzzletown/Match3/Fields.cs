using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000598 RID: 1432
	public class Fields : Map<Field>
	{
		// Token: 0x0600254A RID: 9546 RVA: 0x000A615F File Offset: 0x000A455F
		public Fields(int size) : base(size)
		{
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x000A617E File Offset: 0x000A457E
		public IEnumerable<IMatchResult> Groups
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000A6184 File Offset: 0x000A4584
		public IEnumerable GetBlock(IntVector2 topLeft, int width, int height)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					IntVector2 pos = topLeft + new IntVector2(x, -y);
					yield return base[pos];
				}
			}
			yield break;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000A61BC File Offset: 0x000A45BC
		public void ResetExplosions()
		{
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					base[i, j].ResetExplosion();
				}
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000A6204 File Offset: 0x000A4604
		public void ResetPortals()
		{
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					base[i, j].ResetPortal();
				}
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000A624C File Offset: 0x000A464C
		public void ResetProcessedGems()
		{
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					base[i, j].ResetProcessedGem();
				}
			}
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000A6294 File Offset: 0x000A4694
		public bool IsSwapPossible(IntVector2 from, IntVector2 to)
		{
			return base.IsValid(from) && base.IsValid(to) && ((base[from].CanSwap && base[to].CanSwap) || (base[from].CanSwap && base[to].NeedsGem) || (base[to].CanSwap && base[from].NeedsGem));
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000A6324 File Offset: 0x000A4724
		public bool IsReplaceable(IntVector2 position)
		{
			return !this.prePositionedGems.Contains(position);
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000A6338 File Offset: 0x000A4738
		public void SwapGems(IntVector2 from, IntVector2 to)
		{
			Gem gem = base[to].gem;
			base[to].AssignGem(base[from].gem);
			base[from].AssignGem(gem);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000A6378 File Offset: 0x000A4778
		public List<IntVector2> GetReplaceablePositions()
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					Field field = base[i, j];
					if (!field.GemBlocked && field.gem.IsMatchable && field.CanSwap && field.gem.type == GemType.Undefined && !this.prePositionedGems.Contains(field.gridPosition))
					{
						list.Add(field.gridPosition);
					}
				}
			}
			return list;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000A641C File Offset: 0x000A481C
		public IntVector2 GetRandomModifierPosition(Predicate<Field> condition)
		{
			IntVector2 result = Fields.invalidPos;
			IntVector2[] allModifierFieldPositions = this.GetAllModifierFieldPositions(condition);
			if (allModifierFieldPositions.Length > 0)
			{
				int num = RandomHelper.Next(0, allModifierFieldPositions.Length);
				result = allModifierFieldPositions[num];
			}
			return result;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000A6458 File Offset: 0x000A4858
		public IntVector2[] GetAllModifierFieldPositions(Predicate<Field> condition)
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					Field field = base[i, j];
					if (field.isOn && condition(field))
					{
						list.Add(field.gridPosition);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000A64CC File Offset: 0x000A48CC
		public bool CheckForModifier(Predicate<Field> condition)
		{
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					Field field = base[i, j];
					if (field.isOn && field.CanExplode && condition(field))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000A6538 File Offset: 0x000A4938
		public IntVector2 GetRandomMatchableGemPosition(GemColor color = GemColor.Undefined)
		{
			IntVector2 result = Fields.invalidPos;
			IntVector2[] allMatchableGemsPositions = this.GetAllMatchableGemsPositions(color);
			if (allMatchableGemsPositions.Length > 0)
			{
				int num = RandomHelper.Next(0, allMatchableGemsPositions.Length);
				result = allMatchableGemsPositions[num];
			}
			return result;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000A6574 File Offset: 0x000A4974
		public IntVector2[] GetAllMatchableGemsPositions(GemColor color)
		{
			return (from g in this.GetAllMatchableGems(color)
			select g.position).ToArray<IntVector2>();
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000A65A4 File Offset: 0x000A49A4
		public Group GetAllMatchableGems(GemColor color)
		{
			return this.GetAllColoredGemsWithType(color, GemType.Undefined, true, false, false);
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000A65B1 File Offset: 0x000A49B1
		public Group GetAllGemsWithType(GemType type)
		{
			return this.GetAllColoredGemsWithType(GemColor.Undefined, type, false, true, false);
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000A65BE File Offset: 0x000A49BE
		public int GetGemCountForRatio(SpawnRatio ratio)
		{
			return this.GetAllColoredGemsWithType(ratio.gemColor, ratio.gemType, false, true, true).Count;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000A65DC File Offset: 0x000A49DC
		private Group GetAllColoredGemsWithType(GemColor color, GemType type, bool checkIfMatchable = true, bool ignoreBlocking = false, bool ignoreCanExplode = false)
		{
			Group group = new Group();
			bool isRandomOrUndefined = color == GemColor.Undefined || color == GemColor.Random;
			bool isTypeUndefined = type == GemType.Undefined;
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					Field field = base[i, j];
					if (field != null && field.isOn && field.HasGem && (ignoreCanExplode || field.CanExplode) && (ignoreBlocking || !field.GemBlocked) && (!checkIfMatchable || field.gem.IsMatchable) && (isRandomOrUndefined || color == field.gem.color) && (isTypeUndefined || type == field.gem.type))
					{
						group.Add(field.gem);
					}
				}
			}
			return group;
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000A66DC File Offset: 0x000A4ADC
		public bool CheckForColor(GemColor color)
		{
			bool flag = color == GemColor.Undefined;
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					Field field = base[i, j];
					if (field.isOn && !field.IsBlocked && field.CanExplode && (flag || color == field.gem.color))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000A6760 File Offset: 0x000A4B60
		public Gem GetRandomGemWithColor(GemColor color)
		{
			Group allColoredGemsWithType = this.GetAllColoredGemsWithType(color, GemType.Undefined, false, false, false);
			return allColoredGemsWithType.RandomElement(false);
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000A6780 File Offset: 0x000A4B80
		public Gem GetRandomGemOfDifferentColor(GemColor color)
		{
			IntVector2 vec = default(IntVector2);
			Field field;
			do
			{
				vec.x = RandomHelper.Next(this.size);
				vec.y = RandomHelper.Next(this.size);
				field = base[vec];
			}
			while (!field.HasGem || !field.isOn || !field.gem.IsMatchable || color == field.gem.color);
			return field.gem;
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000A6804 File Offset: 0x000A4C04
		public bool ShouldSpawnGems(SpawnRatio ratio)
		{
			int gemCountForRatio = this.GetGemCountForRatio(ratio);
			return gemCountForRatio < ratio.minSpawn || (gemCountForRatio < ratio.maxSpawn && RandomHelper.Next(0, 100) < ratio.probability);
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000A6854 File Offset: 0x000A4C54
		public void AddColorWheel(IntVector2 gridPosition, LevelConfig levelConfig, bool isColorWheelVariant)
		{
			ColorWheelModel colorWheelModel = new ColorWheelModel();
			colorWheelModel.colors = Fields.GetColorsFromConfig(isColorWheelVariant, levelConfig);
			this.colorWheelModels.Add(gridPosition, colorWheelModel);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000A6884 File Offset: 0x000A4C84
		private static HashSet<GemColor> GetColorsFromConfig(bool skipLowAmountColors, LevelConfig levelConfig)
		{
			HashSet<GemColor> hashSet = new HashSet<GemColor>();
			if (levelConfig == null)
			{
				return null;
			}
			foreach (MaterialAmount materialAmount in levelConfig.data.gems)
			{
				if (!skipLowAmountColors || materialAmount.amount >= 25)
				{
					hashSet.Add((GemColor)Enum.Parse(typeof(GemColor), materialAmount.type, true));
				}
			}
			return hashSet;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000A6904 File Offset: 0x000A4D04
		public IntVector2 GetColorWheelCorner(IntVector2 gridPosition)
		{
			IntVector2 result = default(IntVector2);
			if (base[gridPosition].IsColorWheel)
			{
				IntVector2 intVector = new IntVector2(gridPosition.x - 1, gridPosition.y);
				IntVector2 intVector2 = new IntVector2(gridPosition.x, gridPosition.y + 1);
				IntVector2 intVector3 = new IntVector2(gridPosition.x - 1, gridPosition.y + 1);
				if (base[gridPosition].IsColorWheelCorner)
				{
					result = gridPosition;
				}
				else if (base[intVector] != null && base[intVector].IsColorWheelCorner)
				{
					result = intVector;
				}
				else if (base[intVector2] != null && base[intVector2].IsColorWheelCorner)
				{
					result = intVector2;
				}
				else if (base[intVector3] != null && base[intVector3].IsColorWheelCorner)
				{
					result = intVector3;
				}
			}
			return result;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000A69F8 File Offset: 0x000A4DF8
		public void RemoveColorWheel(IntVector2 gridPosition)
		{
			IntVector2 colorWheelCorner = this.GetColorWheelCorner(gridPosition);
			if (colorWheelCorner != default(IntVector2))
			{
				IEnumerator enumerator = this.GetBlock(colorWheelCorner, 2, 2).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Field field = (Field)obj;
						field.blockerIndex = 0;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this.colorWheelModels.Remove(colorWheelCorner);
			}
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000A6A8C File Offset: 0x000A4E8C
		public void SetupChameleonModels(LevelConfig levelConfig)
		{
			Fields.chameleonModels.Clear();
			this.SetupChameleonModel(ChameleonVariant.All, levelConfig);
			this.SetupChameleonModel(ChameleonVariant.Reduced, levelConfig);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000A6AAC File Offset: 0x000A4EAC
		private void SetupChameleonModel(ChameleonVariant chameleonVariant, LevelConfig levelConfig)
		{
			bool skipLowAmountColors = chameleonVariant == ChameleonVariant.Reduced;
			HashSet<GemColor> colorsFromConfig = Fields.GetColorsFromConfig(skipLowAmountColors, levelConfig);
			ChameleonModel chameleonModel = new ChameleonModel();
			foreach (GemColor gemColor in Gem.GEM_ORDER)
			{
				if (colorsFromConfig.Contains(gemColor))
				{
					chameleonModel.AddColor(gemColor);
				}
			}
			chameleonModel.CheckColorModel();
			Fields.chameleonModels.Add(chameleonVariant, chameleonModel);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000A6B3C File Offset: 0x000A4F3C
		public ChameleonModel GetChameleonModel(ChameleonVariant chameleonVariant)
		{
			return Fields.chameleonModels[chameleonVariant];
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000A6B4C File Offset: 0x000A4F4C
		public Fields DeepCopyFields()
		{
			Fields fields = new Fields(this.size);
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					fields[i, j] = (Field)base[i, j].Clone();
				}
			}
			return fields;
		}

		// Token: 0x0400509F RID: 20639
		public static readonly IntVector2 invalidPos = new IntVector2(-1, -1);

		// Token: 0x040050A0 RID: 20640
		public static readonly Dictionary<ChameleonVariant, ChameleonModel> chameleonModels = new Dictionary<ChameleonVariant, ChameleonModel>();

		// Token: 0x040050A1 RID: 20641
		public List<IntVector2> prePositionedGems = new List<IntVector2>();

		// Token: 0x040050A2 RID: 20642
		public Dictionary<IntVector2, ColorWheelModel> colorWheelModels = new Dictionary<IntVector2, ColorWheelModel>();

		// Token: 0x040050A3 RID: 20643
		private const int STANDARD_GEM_AMOUNT = 25;
	}
}
