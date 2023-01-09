using System;
using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F2 RID: 1266
	[Serializable]
	public class LevelLayout
	{
		// Token: 0x060022EE RID: 8942 RVA: 0x0009AA88 File Offset: 0x00098E88
		public LevelLayout()
		{
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0009AA90 File Offset: 0x00098E90
		public LevelLayout(Fields fields)
		{
			int num = fields.size * fields.size;
			this.fields = new int[num];
			this.tiles = new int[num];
			this.stones = new int[num];
			this.spawning = new int[num];
			this.dropItems = new int[num];
			this.chains = new int[num];
			this.hiddenItems = new int[num];
			this.gemColors = new int[num];
			this.gemTypes = new int[num];
			this.gemModifier = new int[num];
			this.gemDirection = new int[num];
			this.crates = new int[num];
			this.portals = new int[num];
			this.climbers = new int[num];
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					int index = LevelLayout.GetIndex(field.gridPosition, fields.size);
					int num2 = 0;
					if (field.isWindow)
					{
						num2 = 2;
					}
					else if (field.isGrowingWindow)
					{
						num2 = 3;
					}
					else if (field.isOn)
					{
						num2 = 1;
					}
					this.fields[index] = num2;
					this.spawning[index] = (int)field.spawnType;
					int num3 = 0;
					if (field.isDropSpawner)
					{
						num3 = 1;
					}
					else if (field.isDropExit)
					{
						num3 = 2;
					}
					this.dropItems[index] = num3;
					int num4 = 0;
					if (field.isClimberSpawner)
					{
						num4 = 1;
					}
					else if (field.isClimberExit)
					{
						num4 = 2;
					}
					this.climbers[index] = num4;
					this.tiles[index] = field.numTiles;
					this.stones[index] = field.blockerIndex;
					this.chains[index] = field.numChains;
					this.crates[index] = field.cratesIndex;
					this.hiddenItems[index] = field.hiddenItemId;
					this.portals[index] = field.portalId;
					this.gemColors[index] = (int)field.gem.color;
					this.gemTypes[index] = (int)field.gem.type;
					this.gemModifier[index] = (int)field.gem.modifier;
					this.gemDirection[index] = (int)field.gem.direction;
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
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x0009AD10 File Offset: 0x00099110
		public int NumTiles
		{
			get
			{
				return LevelLayout.GetSum(this.tiles);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x0009AD1D File Offset: 0x0009911D
		public int FieldSize
		{
			get
			{
				if (this._fieldSize == 0)
				{
					this._fieldSize = (int)Mathf.Sqrt((float)this.fields.Length);
				}
				return this._fieldSize;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x0009AD48 File Offset: 0x00099148
		public bool HasHiddenItems
		{
			get
			{
				foreach (int num in this.hiddenItems)
				{
					if (num > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x0009AD80 File Offset: 0x00099180
		public bool HasDirtAndTreasure
		{
			get
			{
				if (this.gemModifier == null)
				{
					return false;
				}
				bool flag = false;
				foreach (int num in this.gemModifier)
				{
					flag = (num == 4 || num == 5 || num == 6);
					if (flag)
					{
						break;
					}
				}
				return flag && this.NumTreasure > 0;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x0009ADF0 File Offset: 0x000991F0
		public bool HasWaterAndUnwateredFields
		{
			get
			{
				if (this.tiles == null)
				{
					return false;
				}
				bool flag = false;
				foreach (int num in this.tiles)
				{
					flag = (num == 3);
					if (flag)
					{
						break;
					}
				}
				return flag && this.NumUnwateredFields > 0;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x0009AE50 File Offset: 0x00099250
		public bool HasResistantBlocker
		{
			get
			{
				if (this.stones == null)
				{
					return false;
				}
				bool flag = false;
				foreach (int blockerIndex in this.stones)
				{
					flag = ResistantBlocker.IsResistantBlocker(blockerIndex);
					if (flag)
					{
						break;
					}
				}
				return flag;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x0009AEA0 File Offset: 0x000992A0
		public int NumResistantBlocker
		{
			get
			{
				int num = 0;
				foreach (int blockerIndex in this.stones)
				{
					if (ResistantBlocker.IsResistantBlocker(blockerIndex))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x0009AEE0 File Offset: 0x000992E0
		public int NumTreasure
		{
			get
			{
				int num = 0;
				foreach (int num2 in this.gemColors)
				{
					if (num2 == 13)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0009AF1C File Offset: 0x0009931C
		public int NumUnwateredFields
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.fields.Length; i++)
				{
					int num2 = this.fields[i];
					int num3 = this.tiles[i];
					int num4 = this.spawning[i];
					if ((num2 == 1 || num2 == 3) && num3 != 3 && num4 != 2)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x0009AF84 File Offset: 0x00099384
		public int NumHiddenItems
		{
			get
			{
				HiddenItemInfoDict hiddenItemInfoDict = HiddenItemProcessor.GetHiddenItems(this);
				return hiddenItemInfoDict.Count;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x0009AFA0 File Offset: 0x000993A0
		public bool HasGrowingWindows
		{
			get
			{
				if (this.fields == null)
				{
					return false;
				}
				bool flag = false;
				foreach (int num in this.fields)
				{
					flag = (num == 3);
					if (flag)
					{
						break;
					}
				}
				return flag;
			}
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0009AFEC File Offset: 0x000993EC
		public static int GetSum(int[] arr)
		{
			int num = 0;
			foreach (int num2 in arr)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0009B01A File Offset: 0x0009941A
		public static int GetIndex(IntVector2 gridPosition, int size)
		{
			return gridPosition.x + size * (size - 1 - gridPosition.y);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0009B034 File Offset: 0x00099434
		public static IntVector2 GetPosition(int index, int size)
		{
			return new IntVector2
			{
				x = index % size,
				y = size - index / size - 1
			};
		}

		// Token: 0x04004EAB RID: 20139
		public int[] fields;

		// Token: 0x04004EAC RID: 20140
		public int[] tiles;

		// Token: 0x04004EAD RID: 20141
		public int[] stones;

		// Token: 0x04004EAE RID: 20142
		public int[] spawning;

		// Token: 0x04004EAF RID: 20143
		public int[] dropItems;

		// Token: 0x04004EB0 RID: 20144
		public int[] chains;

		// Token: 0x04004EB1 RID: 20145
		public int[] hiddenItems;

		// Token: 0x04004EB2 RID: 20146
		public int[] gemColors;

		// Token: 0x04004EB3 RID: 20147
		public int[] gemTypes;

		// Token: 0x04004EB4 RID: 20148
		public int[] gemModifier;

		// Token: 0x04004EB5 RID: 20149
		public int[] gemDirection;

		// Token: 0x04004EB6 RID: 20150
		public int[] portals;

		// Token: 0x04004EB7 RID: 20151
		public int[] crates;

		// Token: 0x04004EB8 RID: 20152
		public int[] climbers;

		// Token: 0x04004EB9 RID: 20153
		private int _fieldSize;
	}
}
