using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006AC RID: 1708
	public class GemColorToColor : MonoBehaviour
	{
		// Token: 0x06002AAF RID: 10927 RVA: 0x000C350F File Offset: 0x000C190F
		public Color GetColorFromGem(Gem gem)
		{
			return this.colors[(int)gem.color];
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000C3528 File Offset: 0x000C1928
		public Color GetColorFromGemColor(GemColor color)
		{
			return this.colors[(int)color];
		}

		// Token: 0x040053FB RID: 21499
		public Color[] colors;
	}
}
