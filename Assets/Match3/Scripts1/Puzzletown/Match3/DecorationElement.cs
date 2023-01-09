using Match3.Scripts1.Shared.DataStructures;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000691 RID: 1681
	public class DecorationElement
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x000BE550 File Offset: 0x000BC950
		public bool[,] Map
		{
			get
			{
				Texture2D texture = this.config.texture;
				Rect textureRect = this.config.textureRect;
				Color[] pixels = texture.GetPixels(Mathf.FloorToInt(textureRect.x), Mathf.FloorToInt(textureRect.y), this.width, this.height);
				bool[,] array = new bool[this.width, this.height];
				int i = 0;
				int num = 0;
				while (i < this.height)
				{
					int j = 0;
					while (j < this.width)
					{
						array[j, i] = (pixels[num].r > 0f);
						j++;
						num++;
					}
					i++;
				}
				return array;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000BE610 File Offset: 0x000BCA10
		public int width
		{
			get
			{
				return Mathf.FloorToInt(this.config.textureRect.width);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000BE638 File Offset: 0x000BCA38
		public int height
		{
			get
			{
				return Mathf.FloorToInt(this.config.textureRect.height);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x000BE660 File Offset: 0x000BCA60
		public int Mask
		{
			get
			{
				if (this.mask != null)
				{
					return this.mask.Value;
				}
				bool[,] map = this.Map;
				int num = 0;
				for (int i = 0; i < this.height; i++)
				{
					for (int j = 0; j < this.width; j++)
					{
						if (map[j, i])
						{
							num |= 1 << j + i * 4;
						}
					}
				}
				this.mask = new int?(num);
				return num;
			}
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000BE6EC File Offset: 0x000BCAEC
		public static int GenerateMask(Map<bool> map, IntVector2 pos)
		{
			int num = 0;
			int i = 0;
			int num2 = 0;
			while (i < 4)
			{
				int j = 0;
				while (j < 4)
				{
					if (map[pos + new IntVector2(j, i)])
					{
						num |= 1 << num2;
					}
					j++;
					num2++;
				}
				i++;
			}
			return num;
		}

		// Token: 0x04005344 RID: 21316
		public const int MAX_SIZE = 4;

		// Token: 0x04005345 RID: 21317
		public Sprite sprite;

		// Token: 0x04005346 RID: 21318
		public Sprite config;

		// Token: 0x04005347 RID: 21319
		private int? mask;
	}
}
