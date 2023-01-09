using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using Match3.Scripts1.Wooga.Services.Tracking.Sender;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking
{
	// Token: 0x0200046A RID: 1130
	public static class Tracking
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060020B7 RID: 8375 RVA: 0x0008A531 File Offset: 0x00088931
		public static bool IsInitialized
		{
			get
			{
				return Tracking._initialized;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x0008A538 File Offset: 0x00088938
		// (set) Token: 0x060020B9 RID: 8377 RVA: 0x0008A53F File Offset: 0x0008893F
		public static bool? Cheater { get; set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x0008A547 File Offset: 0x00088947
		public static string Id
		{
			get
			{
				return DeviceId.uniqueIdentifier;
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x0008A550 File Offset: 0x00088950
		public static void Init(string hostName, MonoBehaviour coroutinesHost = null)
		{
			if (Tracking._initialized)
			{
				return;
			}
			ServicePointManager.ServerCertificateValidationCallback = ((object xsender, X509Certificate xcertificate, X509Chain xchain, SslPolicyErrors xerrors) => true);
			Log.Info(new object[]
			{
				"Initializing tracking with endpoint " + hostName
			});
			Unity3D.Init();
			Tracking.AddLifeCycleDispatcher();
			Tracking._coroutinesHost = ((!(coroutinesHost == null)) ? coroutinesHost : CoroutineRunner.Instance);
			Tracking._requestQueue = new RequestQueue.RequestQueue(Tracking.GetTrackingDirectory("trk_queue"), null);
			Sender.Sender sender = new Sender.Sender(Tracking._requestQueue, new TrackingConsumer());
			Tracking._trackingClient = new TrackingClient(hostName, sender, "trk", Tracking.configuration, null, Tracking._coroutinesHost);
			Tracking._initialized = true;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x0008A610 File Offset: 0x00088A10
		public static void Track(string callName, Dictionary<string, object> data)
		{
			Tracking._coroutinesHost.StartTask(Tracking._trackingClient.Track(callName, data));
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x0008A629 File Offset: 0x00088A29
		public static void TrackOnce(string callName, Dictionary<string, object> data)
		{
			Tracking._coroutinesHost.StartTask(Tracking._trackingClient.TrackOnce(callName, data));
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x0008A642 File Offset: 0x00088A42
		public static void AddParameterProvider(IParameterProvider provider)
		{
			Tracking.configuration.parameterProviders.Add(provider);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x0008A654 File Offset: 0x00088A54
		public static void RemoveParameterProvider(IParameterProvider provider)
		{
			if (Tracking.configuration.parameterProviders.Contains(provider))
			{
				Tracking.configuration.parameterProviders.Remove(provider);
			}
			else
			{
				Log.Error(new object[]
				{
					"TRACKING : FAILED TO REMOVE PARAMETER PROVIDER -> NOT FOUND"
				});
			}
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x0008A694 File Offset: 0x00088A94
		public static void CleanUp()
		{
			if (Tracking._requestQueue != null)
			{
				Tracking._requestQueue.Close();
			}
		}
		
		// Token: 0x060020C1 RID: 8385 RVA: 0x0008A6AA File Offset: 0x00088AAA
		public static void MarkAsSent(string callId)
		{
			Tracking._trackingClient.MarkAsSent(callId);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x0008A6B8 File Offset: 0x00088AB8
		private static void AddLifeCycleDispatcher()
		{
			global::UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(TrackingLifeCycleDispatcher));
			if (array.Length <= 0)
			{
				Tracking._lifeCycle = new GameObject("TrackingLifeCycle");
				Tracking._lifeCycle.AddComponent<TrackingLifeCycleDispatcher>();
				global::UnityEngine.Object.DontDestroyOnLoad(Tracking._lifeCycle);
			}
			else
			{
				TrackingLifeCycleDispatcher trackingLifeCycleDispatcher = (TrackingLifeCycleDispatcher)array[0];
				Tracking._lifeCycle = trackingLifeCycleDispatcher.gameObject;
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x0008A720 File Offset: 0x00088B20
		private static string GetTrackingDirectory(string dataDir)
		{
			string text = Path.Combine(Unity3D.Paths.ApplicationDataPath, dataDir);
			if (!File.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060020C4 RID: 8388 RVA: 0x0008A74C File Offset: 0x00088B4C
		private static TrackingConfiguration configuration
		{
			get
			{
				if (Tracking._configuration == null)
				{
					Tracking._configuration = new TrackingConfiguration();
				}
				return Tracking._configuration;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060020C5 RID: 8389 RVA: 0x0008A767 File Offset: 0x00088B67
		public static string persistentDataPath
		{
			get
			{
				return Tracking._trackingClient.persistentDataPath;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (set) Token: 0x060020C6 RID: 8390 RVA: 0x0008A773 File Offset: 0x00088B73
		public static string facebookUserId
		{
			set
			{
				Tracking.baseParameters.facebookUserId = value;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (set) Token: 0x060020C7 RID: 8391 RVA: 0x0008A780 File Offset: 0x00088B80
		public static string sbsUserId
		{
			set
			{
				Tracking.baseParameters.sbsUserId = value;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (set) Token: 0x060020C8 RID: 8392 RVA: 0x0008A78D File Offset: 0x00088B8D
		public static string abTestGroups
		{
			set
			{
				Tracking.baseParameters.abTestGroups = value;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (set) Token: 0x060020C9 RID: 8393 RVA: 0x0008A79A File Offset: 0x00088B9A
		public static SessionTracker.SessionTracker.SessionStartCallback onSessionStart
		{
			set
			{
				Tracking.configuration.onSessionStart = value;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (set) Token: 0x060020CA RID: 8394 RVA: 0x0008A7A7 File Offset: 0x00088BA7
		public static Action<Dictionary<string, object>> onSessionEnd
		{
			set
			{
				Tracking.configuration.onSessionEnd = value;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x0008A7B4 File Offset: 0x00088BB4
		public static RoundTracker.RoundTracker roundTracker
		{
			get
			{
				return Tracking._trackingClient.roundTracker;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060020CC RID: 8396 RVA: 0x0008A7C0 File Offset: 0x00088BC0
		public static BaseParameters baseParameters
		{
			get
			{
				return Tracking._trackingClient.baseParameters;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x0008A7CC File Offset: 0x00088BCC
		public static SessionTracker.SessionTracker sessionTracker
		{
			get
			{
				return Tracking._trackingClient.sessionTracker;
			}
		}

		// Token: 0x04004B96 RID: 19350
		private static bool _initialized;

		// Token: 0x04004B97 RID: 19351
		private static RequestQueue.RequestQueue _requestQueue;

		// Token: 0x04004B98 RID: 19352
		private static TrackingClient _trackingClient;

		// Token: 0x04004B99 RID: 19353
		private const string PERSISTENT_DATA_DIRECTORY = "trk";

		// Token: 0x04004B9A RID: 19354
		private static GameObject _lifeCycle;

		// Token: 0x04004B9B RID: 19355
		public static global::Match3.Scripts1.Wooga.Services.Tracking.Parameters.Environment Environment = global::Match3.Scripts1.Wooga.Services.Tracking.Parameters.Environment.NotSet;

		// Token: 0x04004B9D RID: 19357
		private static TrackingConfiguration _configuration;

		// Token: 0x04004B9E RID: 19358
		private static MonoBehaviour _coroutinesHost;
	}
}
