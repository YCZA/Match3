using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200069A RID: 1690
	public class ChameleonModel
	{
		// Token: 0x06002A2C RID: 10796 RVA: 0x000C1382 File Offset: 0x000BF782
		public ChameleonModel()
		{
			this.colors = new List<GemColor>();
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000C1395 File Offset: 0x000BF795
		public void AddColor(GemColor color)
		{
			this.colors.Add(color);
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000C13A3 File Offset: 0x000BF7A3
		public void InsertColorAtBeginning(GemColor color)
		{
			this.colors.Insert(0, color);
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000C13B2 File Offset: 0x000BF7B2
		public GemColor GetFirstColor()
		{
			return this.colors[0];
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000C13C0 File Offset: 0x000BF7C0
		public bool IsLastColor(GemColor currentColor)
		{
			return currentColor == GemColor.Coins;
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000C13C7 File Offset: 0x000BF7C7
		public GemColor GetLastColor()
		{
			return this.colors[this.colors.Count - 1];
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000C13E4 File Offset: 0x000BF7E4
		public bool ContainsColor(GemColor currentColor)
		{
			foreach (GemColor gemColor in this.colors)
			{
				if (gemColor == currentColor)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000C144C File Offset: 0x000BF84C
		public void CheckColorModel()
		{
			if (this.HasColors())
			{
				if (this.GetFirstColor() != GemColor.Red)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"First color of chameleon color model is not Red! adding it now"
					});
					this.InsertColorAtBeginning(GemColor.Red);
				}
				if (this.GetLastColor() != GemColor.Coins)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Last color of chameleon color model is not Coins! adding it now"
					});
					this.AddColor(GemColor.Coins);
				}
			}
			else
			{
				this.AddColor(GemColor.Red);
				this.AddColor(GemColor.Coins);
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000C14C6 File Offset: 0x000BF8C6
		public bool HasColors()
		{
			return !this.colors.IsNullOrEmptyCollection();
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000C14D8 File Offset: 0x000BF8D8
		public GemColor GetNextColor(GemColor currentColor)
		{
			if (this.IsLastColor(currentColor))
			{
				return this.GetLastColor();
			}
			for (int i = 0; i < this.colors.Count; i++)
			{
				if (this.colors[i] == currentColor)
				{
					return this.colors[i + 1];
				}
			}
			return this.GetLastColor();
		}

		// Token: 0x04005397 RID: 21399
		private readonly List<GemColor> colors;
	}
}
