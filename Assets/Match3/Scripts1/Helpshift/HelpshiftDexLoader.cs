using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001CB RID: 459
	public class HelpshiftDexLoader : IWorkerMethodDispatcher
	{
		// Token: 0x06000D30 RID: 3376 RVA: 0x0001F78C File Offset: 0x0001DB8C
		private HelpshiftDexLoader()
		{
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0001F79F File Offset: 0x0001DB9F
		public static HelpshiftDexLoader getInstance()
		{
			if (HelpshiftDexLoader.dexLoader == null)
			{
				HelpshiftDexLoader.dexLoader = new HelpshiftDexLoader();
			}
			return HelpshiftDexLoader.dexLoader;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0001F7BC File Offset: 0x0001DBBC
		public void loadDex(IDexLoaderListener listener, AndroidJavaObject application)
		{
			this.application = application;
			// this.helpshiftLoaderClass = new AndroidJavaClass("com.helpshift.dex.HelpshiftDexLoader");
			// this.unityApiDelegateClass = new AndroidJavaClass("com.helpshift.supportCampaigns.UnityAPIDelegate");
			this.registerListener(listener);
			HelpshiftWorker.getInstance().registerClient("dexLoader", this);
			HelpshiftWorker.getInstance().enqueueApiCall("dexLoader", "loadHelpshiftDex", null, new object[]
			{
				this.helpshiftLoaderClass
			});
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0001F82B File Offset: 0x0001DC2B
		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
			if (methodIdentifier.Equals("loadHelpshiftDex"))
			{
				this.loadHelpshiftDex((AndroidJavaClass)args[0]);
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0001F84C File Offset: 0x0001DC4C
		private void loadHelpshiftDex(AndroidJavaClass helpshiftLoaderClass)
		{
			this.unityApiDelegateClass.CallStatic("installDex", new object[]
			{
				this.application
			});
			HelpshiftDexLoader.isDexLoaded = true;
			foreach (IDexLoaderListener dexLoaderListener in this.listeners)
			{
				dexLoaderListener.onDexLoaded();
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0001F8CC File Offset: 0x0001DCCC
		public void registerListener(IDexLoaderListener listener)
		{
			if (!this.listeners.Contains(listener))
			{
				this.listeners.Add(listener);
			}
			if (HelpshiftDexLoader.isDexLoaded)
			{
				listener.onDexLoaded();
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0001F8FC File Offset: 0x0001DCFC
		public AndroidJavaClass getHSDexLoaderJavaClass()
		{
			return this.helpshiftLoaderClass;
		}

		// Token: 0x04003F71 RID: 16241
		private static HelpshiftDexLoader dexLoader;

		// Token: 0x04003F72 RID: 16242
		private static bool isDexLoaded;

		// Token: 0x04003F73 RID: 16243
		private HashSet<IDexLoaderListener> listeners = new HashSet<IDexLoaderListener>();

		// Token: 0x04003F74 RID: 16244
		private AndroidJavaObject application;

		// Token: 0x04003F75 RID: 16245
		private AndroidJavaClass helpshiftLoaderClass;

		// Token: 0x04003F76 RID: 16246
		private AndroidJavaClass unityApiDelegateClass;
	}
}
