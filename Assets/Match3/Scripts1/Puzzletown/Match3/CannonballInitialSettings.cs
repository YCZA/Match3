using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000566 RID: 1382
	[Serializable]
	public class CannonballInitialSettings : ALevelGeneratorInitialSettings
	{
		// Token: 0x0600244E RID: 9294 RVA: 0x000A16D9 File Offset: 0x0009FAD9
		public CannonballInitialSettings()
		{
			this.amountMin = 2;
			this.amountMax = 10;
			this.spawnRatioMin = 3;
			this.spawnRatioMax = 15;
		}
	}
}
