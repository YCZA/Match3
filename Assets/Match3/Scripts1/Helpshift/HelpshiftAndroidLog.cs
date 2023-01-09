using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C4 RID: 452
	public class HelpshiftAndroidLog : IDexLoaderListener, IWorkerMethodDispatcher
	{
		// Token: 0x06000CE7 RID: 3303 RVA: 0x0001E8B1 File Offset: 0x0001CCB1
		private HelpshiftAndroidLog()
		{
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0001E8B9 File Offset: 0x0001CCB9
		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0001E8BB File Offset: 0x0001CCBB
		public void onDexLoaded()
		{
			//HelpshiftAndroidLog.logger = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftLogInstance", new object[0]);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0001E8DC File Offset: 0x0001CCDC
		private static void initLogger()
		{
			if (HelpshiftAndroidLog.logger == null)
			{
				HelpshiftWorker.getInstance().registerClient("helpshiftandroidlog", HelpshiftAndroidLog.helpshiftAndroidLog);
				HelpshiftDexLoader.getInstance().registerListener(HelpshiftAndroidLog.helpshiftAndroidLog);
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0001E90B File Offset: 0x0001CD0B
		public static int v(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			return HelpshiftAndroidLog.logger.CallStatic<int>("v", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0001E939 File Offset: 0x0001CD39
		public static int d(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			return HelpshiftAndroidLog.logger.CallStatic<int>("d", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0001E967 File Offset: 0x0001CD67
		public static int i(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			return HelpshiftAndroidLog.logger.CallStatic<int>("i", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0001E995 File Offset: 0x0001CD95
		public static int w(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			return HelpshiftAndroidLog.logger.CallStatic<int>("w", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0001E9C3 File Offset: 0x0001CDC3
		public static int e(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			return HelpshiftAndroidLog.logger.CallStatic<int>("e", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x04003F59 RID: 16217
		private static AndroidJavaObject logger = null;

		// Token: 0x04003F5A RID: 16218
		private static HelpshiftAndroidLog helpshiftAndroidLog = new HelpshiftAndroidLog();
	}
}
