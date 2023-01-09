using System.Threading;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Core.ThreadSafe
{
	// Token: 0x0200039D RID: 925
	public static class Unity3D
	{
		// Token: 0x06001C04 RID: 7172 RVA: 0x0007B970 File Offset: 0x00079D70
		public static void Init()
		{
			if (Unity3D.Threads.MainThread != null)
			{
				return;
			}
			Unity3D.Paths.PersistentDataPath = Application.persistentDataPath;
			Unity3D.Threads.MainThread = Thread.CurrentThread;
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			// Unity3D.Paths.ApplicationDataPath = @static.Call<AndroidJavaObject>("getApplicationInfo", new object[0]).Get<string>("dataDir") + "/files";
				
			// Unity3D.Paths.ApplicationDataPath = Application.dataPath; // 这个目录没有权限, 奇怪，为什么原代码要这么写
			Unity3D.Paths.ApplicationDataPath = Application.persistentDataPath;
			Unity3D.AttachMainThreadComputationQueue();
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x0007B9E4 File Offset: 0x00079DE4
		public static void AttachMainThreadComputationQueue()
		{
			if (!Unity3D._dispatcher && Application.isPlaying)
			{
				Unity3D._dispatcher = new GameObject(typeof(MainThreadComputationQueue).Name).AddComponent<MainThreadComputationQueue>();
				global::UnityEngine.Object.DontDestroyOnLoad(Unity3D._dispatcher);
			}
		}

		// Token: 0x0400497C RID: 18812
		private static MainThreadComputationQueue _dispatcher;

		// Token: 0x0200039E RID: 926
		public static class Paths
		{
			// Token: 0x17000471 RID: 1137
			// (get) Token: 0x06001C06 RID: 7174 RVA: 0x0007BA32 File Offset: 0x00079E32
			// (set) Token: 0x06001C07 RID: 7175 RVA: 0x0007BA39 File Offset: 0x00079E39
			public static string PersistentDataPath { get; internal set; }

			// Token: 0x17000472 RID: 1138
			// (get) Token: 0x06001C08 RID: 7176 RVA: 0x0007BA41 File Offset: 0x00079E41
			// (set) Token: 0x06001C09 RID: 7177 RVA: 0x0007BA48 File Offset: 0x00079E48
			public static string ApplicationDataPath { get; internal set; }
		}

		// Token: 0x0200039F RID: 927
		public static class Threads
		{
			// Token: 0x17000473 RID: 1139
			// (get) Token: 0x06001C0A RID: 7178 RVA: 0x0007BA50 File Offset: 0x00079E50
			// (set) Token: 0x06001C0B RID: 7179 RVA: 0x0007BA57 File Offset: 0x00079E57
			public static Thread MainThread { get; set; }

			// Token: 0x06001C0C RID: 7180 RVA: 0x0007BA5F File Offset: 0x00079E5F
			public static bool OnMainThread()
			{
				return Thread.CurrentThread.IsMainThread();
			}
		}

		// Token: 0x020003A0 RID: 928
		public static class Connectivity
		{
			// Token: 0x17000474 RID: 1140
			// (get) Token: 0x06001C0D RID: 7181 RVA: 0x0007BA6B File Offset: 0x00079E6B
			// (set) Token: 0x06001C0E RID: 7182 RVA: 0x0007BA72 File Offset: 0x00079E72
			public static ConnectivityInfo Status { get; set; }
		}
	}
}
