using System;
using Match3.Scripts1.Wooga.Core.ThreadSafe;

namespace Match3.Scripts1.Wooga.Core.DeviceInfo
{
	// Token: 0x0200034E RID: 846
	internal class MemoryWarningDelegate
	{
		// Token: 0x060019DC RID: 6620 RVA: 0x00074BD1 File Offset: 0x00072FD1
		private static void _initializeMemoryDelegate()
		{
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00074BD3 File Offset: 0x00072FD3
		public static void _setMemoryCallback(MemoryWarningDelegate.MemoryCb callback)
		{
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00074BD5 File Offset: 0x00072FD5
		public static void SetCallback(Action callback)
		{
			if (!MemoryWarningDelegate._initialized)
			{
				MemoryWarningDelegate.Init();
			}
			MemoryWarningDelegate._warningCallback = callback;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00074BEC File Offset: 0x00072FEC
		public static void Init()
		{
			if (MemoryWarningDelegate._initialized)
			{
				return;
			}
			MemoryWarningDelegate._initializeMemoryDelegate();
			MemoryWarningDelegate._setMemoryCallback(new MemoryWarningDelegate.MemoryCb(MemoryWarningDelegate.OnWarning));
			MemoryWarningDelegate._initialized = true;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00074C15 File Offset: 0x00073015
		[MonoPInvokeCallback(typeof(MemoryWarningDelegate.MemoryCb))]
		private static void OnWarning()
		{
			if (MemoryWarningDelegate._warningCallback != null)
			{
				Scheduler.ExecuteOnMainThread(
					delegate()
					{
						MemoryWarningDelegate._warningCallback();
					});
			}
		}

		// Token: 0x04004851 RID: 18513
		private static Action _warningCallback;

		// Token: 0x04004852 RID: 18514
		private static bool _initialized;

		// Token: 0x0200034F RID: 847
		// (Invoke) Token: 0x060019E4 RID: 6628
		public delegate void MemoryCb();
	}
}
