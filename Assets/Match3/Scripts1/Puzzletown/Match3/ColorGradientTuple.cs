using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004D2 RID: 1234
	[Serializable]
	public class ColorGradientTuple
	{
		// Token: 0x06002276 RID: 8822 RVA: 0x0009849B File Offset: 0x0009689B
		public ColorGradientTuple(Color colorA, Color colorB, Color colorC)
		{
			this.colorA = colorA;
			this.colorB = colorB;
			this.colorC = colorC;
		}

		// Token: 0x04004DEA RID: 19946
		public Color colorA;

		// Token: 0x04004DEB RID: 19947
		public Color colorB;

		// Token: 0x04004DEC RID: 19948
		public Color colorC;
	}
}
