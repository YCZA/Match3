using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000615 RID: 1557
	public class HiddenItemProcessor : IMatchProcessor
	{
		// Token: 0x060027C5 RID: 10181 RVA: 0x000B0ACF File Offset: 0x000AEECF
		public HiddenItemProcessor(LevelLayout layout)
		{
			this.HiddenItems = HiddenItemProcessor.GetHiddenItems(layout);
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060027C6 RID: 10182 RVA: 0x000B0AE3 File Offset: 0x000AEEE3
		// (set) Token: 0x060027C7 RID: 10183 RVA: 0x000B0AEB File Offset: 0x000AEEEB
		public HiddenItemInfoDict HiddenItems { get; private set; }

		// Token: 0x060027C8 RID: 10184 RVA: 0x000B0AF4 File Offset: 0x000AEEF4
		public void Reload(Fields fields)
		{
			HiddenItemInfoDict hiddenItems = HiddenItemProcessor.GetHiddenItems(fields);
			foreach (HiddenItemInfo hiddenItemInfo in hiddenItems.Values)
			{
				hiddenItemInfo.positions.Sort((IntVector2 x, IntVector2 y) => x.SqrMagnitude.CompareTo(y.SqrMagnitude));
			}
			this.HiddenItems = hiddenItems;
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x000B0B80 File Offset: 0x000AEF80
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> results)
		{
			List<IMatchResult> list = new List<IMatchResult>();
			foreach (IMatchResult matchResult in results)
			{
				if (matchResult is TileExplosion)
				{
					TileExplosion tileExplosion = (TileExplosion)matchResult;
					if (fields[tileExplosion.Position].numTiles == 0 || fields[tileExplosion.Position].numTiles > 2)
					{
						int[] array = this.HiddenItems.Keys.ToArray<int>();
						foreach (int key in array)
						{
							HiddenItemInfo hiddenItemInfo = this.HiddenItems[key];
							hiddenItemInfo.positions.Remove(tileExplosion.Position);
							if (hiddenItemInfo.positions.IsNullOrEmptyCollection())
							{
								list.Add(new HiddenItemFound(hiddenItemInfo.id, tileExplosion.Position));
								this.HiddenItems.Remove(key);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000B0CB0 File Offset: 0x000AF0B0
		public static HiddenItemInfoDict GetHiddenItems(LevelLayout level)
		{
			HiddenItemInfoDict hiddenItemInfoDict = new HiddenItemInfoDict();
			for (int i = 0; i < level.hiddenItems.Length; i++)
			{
				int num = level.hiddenItems[i];
				if (num > 0)
				{
					IntVector2 position = LevelLayout.GetPosition(i, 9);
					bool isCovered = level.tiles[i] > 0 && level.tiles[i] <= 2;
					HiddenItemProcessor.AddHiddenItem(hiddenItemInfoDict, num, position, isCovered);
				}
			}
			foreach (int key in hiddenItemInfoDict.Keys)
			{
				hiddenItemInfoDict[key].positions.Sort((IntVector2 x, IntVector2 y) => x.SqrMagnitude.CompareTo(y.SqrMagnitude));
			}
			return hiddenItemInfoDict;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000B0D9C File Offset: 0x000AF19C
		public static HiddenItemInfoDict GetHiddenItems(Fields fields)
		{
			HiddenItemInfoDict hiddenItemInfoDict = new HiddenItemInfoDict();
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.hiddenItemId > 0)
					{
						bool isCovered = field.numTiles > 0 && field.numTiles < 2;
						HiddenItemProcessor.AddHiddenItem(hiddenItemInfoDict, field.hiddenItemId, field.gridPosition, isCovered);
					}
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
			return hiddenItemInfoDict;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000B0E38 File Offset: 0x000AF238
		private static void AddHiddenItem(HiddenItemInfoDict hiddenItems, int id, IntVector2 pos, bool isCovered)
		{
			if (!hiddenItems.ContainsKey(id))
			{
				hiddenItems[id] = new HiddenItemInfo(id, pos);
			}
			IntVector2 bottomLeftPosition = hiddenItems[id].bottomLeftPosition;
			if (pos.x <= bottomLeftPosition.x && pos.y <= bottomLeftPosition.y)
			{
				hiddenItems[id].bottomLeftPosition = pos;
			}
			hiddenItems[id].totalPositionsCount++;
			if (isCovered)
			{
				hiddenItems[id].positions.Add(pos);
			}
		}
	}
}
