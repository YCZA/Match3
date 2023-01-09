using System;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Wooga.Core.DeviceInfo
{
	// Token: 0x0200034B RID: 843
	public static class BootTime
	{
		// Token: 0x060019C9 RID: 6601 RVA: 0x00074714 File Offset: 0x00072B14
		public static long GetBootTime()
		{
			long result = 0L;
			try
			{
				// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.SystemClock");
				// result = androidJavaClass.CallStatic<long>("elapsedRealtime", new object[0]);
			}
			catch (Exception ex)
			{
				Log.Error(new object[]
				{
					ex
				});
			}
			return result;
		}
	}
}
