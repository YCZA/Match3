using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001CD RID: 461
	public class HelpshiftInternalLogger : IWorkerMethodDispatcher, IDexLoaderListener
	{
		// Token: 0x06000D44 RID: 3396 RVA: 0x0001FBCC File Offset: 0x0001DFCC
		private HelpshiftInternalLogger()
		{
			HelpshiftWorker.getInstance().registerClient("HelpshiftInternalLogger", this);
			HelpshiftDexLoader.getInstance().registerListener(this);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0001FBEF File Offset: 0x0001DFEF
		public static HelpshiftInternalLogger getInstance()
		{
			if (HelpshiftInternalLogger.internalLoggerInstance == null)
			{
				HelpshiftInternalLogger.internalLoggerInstance = new HelpshiftInternalLogger();
			}
			return HelpshiftInternalLogger.internalLoggerInstance;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0001FC0A File Offset: 0x0001E00A
		private void addApiCallToQueue(string apiName, object[] args)
		{
			HelpshiftWorker.getInstance().enqueueApiCall("HelpshiftInternalLogger", "hsLoggerWithArgs", apiName, args);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0001FC22 File Offset: 0x0001E022
		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
			this.hsInternalLogger.CallStatic(api, args);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0001FC31 File Offset: 0x0001E031
		public void onDexLoaded()
		{
			this.hsInternalLogger = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHSLoggerInstance", new object[0]);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0001FC53 File Offset: 0x0001E053
		public void d(string message)
		{
			this.addApiCallToQueue("d", new object[]
			{
				HelpshiftInternalLogger.TAG,
				message
			});
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0001FC72 File Offset: 0x0001E072
		public void e(string message)
		{
			this.addApiCallToQueue("e", new object[]
			{
				HelpshiftInternalLogger.TAG,
				message
			});
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0001FC91 File Offset: 0x0001E091
		public void w(string message)
		{
			this.addApiCallToQueue("w", new object[]
			{
				HelpshiftInternalLogger.TAG,
				message
			});
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0001FCB0 File Offset: 0x0001E0B0
		public void f(string message)
		{
			this.addApiCallToQueue("f", new object[]
			{
				HelpshiftInternalLogger.TAG,
				message
			});
		}

		// Token: 0x04003F78 RID: 16248
		private static string TAG = "HelpshiftUnityPlugin";

		// Token: 0x04003F79 RID: 16249
		private static HelpshiftInternalLogger internalLoggerInstance;

		// Token: 0x04003F7A RID: 16250
		private AndroidJavaObject hsInternalLogger;
	}
}
