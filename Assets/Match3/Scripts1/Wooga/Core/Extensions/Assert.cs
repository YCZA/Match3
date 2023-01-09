using System;

namespace Wooga.Core.Extensions
{
	// Token: 0x0200035D RID: 861
	public class Assert
	{
		// Token: 0x06001A06 RID: 6662 RVA: 0x00074F9D File Offset: 0x0007339D
		private Assert()
		{
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00074FA5 File Offset: 0x000733A5
		public static void That(bool condition, string message)
		{
			if (!condition)
			{
				throw new ArgumentException("[SBS-SDK] Invalid Argument: " + message);
			}
		}
	}
}
