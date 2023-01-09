using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x0200079A RID: 1946
namespace Match3.Scripts1
{
	public class FPSService : AService
	{
		// Token: 0x06002FBE RID: 12222 RVA: 0x000E0E4A File Offset: 0x000DF24A
		public FPSService()
		{
			Application.targetFrameRate = FPSService.TargetFrameRate;
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002FBF RID: 12223 RVA: 0x000E0E5C File Offset: 0x000DF25C
		public static int TargetFrameRate
		{
			get
			{
				return FPSService.TARGET_FRAME_RATE_DEFAULT;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000E0E63 File Offset: 0x000DF263
		public static int ReducedTargetFrameRate
		{
			get
			{
				return FPSService.TARGET_FRAME_RATE_REDUCED;
			}
		}

		// Token: 0x040058DF RID: 22751
		private static int TARGET_FRAME_RATE_REDUCED = 30;

		// Token: 0x040058E0 RID: 22752
		private static int TARGET_FRAME_RATE_DEFAULT = 60;
	}
}
