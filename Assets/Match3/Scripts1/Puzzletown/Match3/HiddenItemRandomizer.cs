using System.Collections.Generic;
using Match3.Scripts1.Shared.DataStructures;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000619 RID: 1561
	public class HiddenItemRandomizer
	{
		// Token: 0x060027D5 RID: 10197 RVA: 0x000B0F98 File Offset: 0x000AF398
		public HiddenItemInfoDict Randomize(LevelLayout layout)
		{
			HiddenItemInfoDict hiddenItems = HiddenItemProcessor.GetHiddenItems(layout);
			Map<int> map = this.InitGrid(layout, 9);
			Map<int> map2 = (Map<int>)map.Clone();
			foreach (int num in hiddenItems.Keys)
			{
				if (num > 4)
				{
					map2 = (Map<int>)map2.Clone();
					if (!this.FindPositions(map2, hiddenItems[num]))
					{
						return this.Randomize(layout);
					}
				}
			}
			for (int i = 0; i < layout.hiddenItems.Length; i++)
			{
				IntVector2 position = LevelLayout.GetPosition(i, layout.FieldSize);
				layout.hiddenItems[i] = ((map2[position] <= 0) ? 0 : map2[position]);
			}
			return HiddenItemProcessor.GetHiddenItems(layout);
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000B109C File Offset: 0x000AF49C
		private bool FindPositions(Map<int> map, HiddenItemInfo info)
		{
			List<IntVector2> possiblePositions = this.GetPossiblePositions(map, info.Size);
			while (possiblePositions.Count > 0)
			{
				Map<int> map2 = (Map<int>)map.Clone();
				if (this.CanBePlaced(possiblePositions.RandomElement(true), info, map2))
				{
					map.AssignArray(map2);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000B10F4 File Offset: 0x000AF4F4
		private Map<int> InitGrid(LevelLayout layout, int size)
		{
			Map<int> map = new Map<int>(size);
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					int index = LevelLayout.GetIndex(intVector, size);
					if (layout.hiddenItems[index].IsBetween(1, 5))
					{
						map[intVector] = layout.hiddenItems[index];
					}
					else
					{
						map[intVector] = ((layout.tiles[index] <= 0) ? -1 : 0);
					}
				}
			}
			return map;
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000B1188 File Offset: 0x000AF588
		private List<IntVector2> GetPossiblePositions(Map<int> map, int itemSize)
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < 81; i++)
			{
				IntVector2 position = LevelLayout.GetPosition(i, 9);
				int num = position.x + (itemSize - 1);
				int num2 = position.y + (itemSize - 1);
				if (num < 9 && num2 < 9 && map[position] == 0)
				{
					list.Add(position);
				}
			}
			return list;
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x000B11F8 File Offset: 0x000AF5F8
		private bool CanBePlaced(IntVector2 pos, HiddenItemInfo info, Map<int> map)
		{
			for (int i = 0; i < info.Size; i++)
			{
				for (int j = 0; j < info.Size; j++)
				{
					IntVector2 b = new IntVector2(i, j);
					if (!this.IsPositionValid(pos + b, map))
					{
						return false;
					}
					map[pos + b] = info.id;
				}
			}
			return true;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000B126A File Offset: 0x000AF66A
		private bool IsPositionValid(IntVector2 pos, Map<int> map)
		{
			return map[pos] == 0;
		}
	}
}
