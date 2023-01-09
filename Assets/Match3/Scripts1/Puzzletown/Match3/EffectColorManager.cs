using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004D3 RID: 1235
	[CreateAssetMenu(fileName = "EffectColorManager", menuName = "Puzzletown/Effect Color Manager")]
	public class EffectColorManager : ScriptableObject
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002278 RID: 8824 RVA: 0x00098730 File Offset: 0x00096B30
		private Dictionary<GemColor, ColorGradientTuple> GemColors
		{
			get
			{
				if (this.gemColors.IsNullOrEmptyCollection())
				{
					this.gemColors.Add(GemColor.Blue, this.blue);
					this.gemColors.Add(GemColor.Green, this.green);
					this.gemColors.Add(GemColor.Red, this.red);
					this.gemColors.Add(GemColor.Yellow, this.yellow);
					this.gemColors.Add(GemColor.Purple, this.purple);
					this.gemColors.Add(GemColor.Orange, this.orange);
					this.gemColors.Add(GemColor.Coins, this.coins);
				}
				return this.gemColors;
			}
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000987D4 File Offset: 0x00096BD4
		public ColorGradientTuple GetColors(GemColor gemColor)
		{
			ColorGradientTuple result;
			this.GemColors.TryGetValue(gemColor, out result);
			return result;
		}

		// Token: 0x04004DED RID: 19949
		public ColorGradientTuple blue = new ColorGradientTuple(new Color(0.2902f, 0.4078f, 0.8118f, 1f), new Color(0.2902f, 0.8745f, 0.9804f, 1f), new Color(0f, 1f, 0.9176f, 1f));

		// Token: 0x04004DEE RID: 19950
		public ColorGradientTuple green = new ColorGradientTuple(new Color(0.2f, 0.4118f, 0.0941f, 1f), new Color(0.5098f, 0.851f, 0.0824f, 1f), new Color(0.1569f, 1f, 0f, 1f));

		// Token: 0x04004DEF RID: 19951
		public ColorGradientTuple red = new ColorGradientTuple(new Color(0.7725f, 0.102f, 0.102f, 1f), new Color(0.9961f, 0.4549f, 0.3765f, 1f), new Color(1f, 0.1843f, 0f, 1f));

		// Token: 0x04004DF0 RID: 19952
		public ColorGradientTuple yellow = new ColorGradientTuple(new Color(0.7529f, 0.5804f, 0.1216f, 1f), new Color(0.9529f, 0.9216f, 0.0627f, 1f), new Color(0f, 1f, 0.1725f, 1f));

		// Token: 0x04004DF1 RID: 19953
		public ColorGradientTuple purple = new ColorGradientTuple(new Color(0.2235f, 0.0549f, 0.3569f, 1f), new Color(0.5373f, 0.2549f, 0.7216f, 1f), new Color(1f, 0f, 0f, 1f));

		// Token: 0x04004DF2 RID: 19954
		public ColorGradientTuple orange = new ColorGradientTuple(new Color(0.6784f, 0.1333f, 0.4824f, 1f), new Color(0.9333f, 0.4667f, 0.8039f, 1f), new Color(0.6118f, 0f, 0.6824f, 1f));

		// Token: 0x04004DF3 RID: 19955
		public ColorGradientTuple coins = new ColorGradientTuple(new Color(0.6824f, 0.3922f, 0.0196f, 1f), new Color(0.9804f, 0.8157f, 0.2078f, 1f), new Color(1f, 1f, 1f, 1f));

		// Token: 0x04004DF4 RID: 19956
		private readonly Dictionary<GemColor, ColorGradientTuple> gemColors = new Dictionary<GemColor, ColorGradientTuple>();
	}
}
