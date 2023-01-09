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
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Payload;
using Match3.Scripts1.Wooga.Services.TrackingCore.Networking;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics
{
	// Token: 0x020003EE RID: 1006
	public static class ErrorAnalytics
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0007FEA3 File Offset: 0x0007E2A3
		public static bool isInitialized
		{
			get
			{
				return ErrorAnalytics._isInitalized;
			}
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0007FEAA File Offset: 0x0007E2AA
		internal static void Init(IReporter reporter, string sbsGameId, MonoBehaviour coroutinesHost)
		{
			ErrorAnalytics._reporter = reporter;
			ErrorAnalytics.Init(sbsGameId, 50, false, coroutinesHost, false);
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0007FEBD File Offset: 0x0007E2BD
		public static void Init(string sbsGameId, int breadCrumbCapacity = 50, bool enableErrorHandling = true, MonoBehaviour coroutinesHost = null, bool ignoreFollowupFatals = false)
		{
			ErrorAnalytics.Init(sbsGameId, new Information.App(Bundle.version, Bundle.build, Bundle.identifier), Application.temporaryCachePath, breadCrumbCapacity, enableErrorHandling, coroutinesHost, ignoreFollowupFatals);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0007FEE3 File Offset: 0x0007E2E3
		public static void Init(string sbsGameId, Information.App appInfo, int breadCrumbCapacity = 50, bool enableErrorHandling = true, bool ignoreFollowupFatals = false)
		{
			ErrorAnalytics.Init(sbsGameId, appInfo, Application.temporaryCachePath, breadCrumbCapacity, enableErrorHandling, null, ignoreFollowupFatals);
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0007FEF8 File Offset: 0x0007E2F8
		internal static void Init(string sbsGameId, Information.App appInfo, string applicationTemporaryCachePath, int breadCrumbCapacity = 50, bool enableErrorHandling = true, MonoBehaviour coroutinesHost = null, bool ignoreFollowupFatals = false)
		{
			Handler.hadFatal = false;
			if (ErrorAnalytics.isInitialized)
			{
				return;
			}
			ErrorAnalytics._coroutinesHost = ((!(coroutinesHost == null)) ? coroutinesHost : CoroutineRunner.Instance);
			if (string.IsNullOrEmpty(ErrorAnalytics._customUserId))
			{
				ErrorAnalytics._customUserId = DeviceId.uniqueIdentifier;
			}
			ErrorAnalytics._breadrumbs = new Breadcrumbs(breadCrumbCapacity);
			ErrorAnalytics._sbsInfo.GameId = sbsGameId;
			ErrorAnalytics._sbsInfo.System = Information.GetSbsPlatform();
			ErrorAnalytics._sbsInfo.DeviceId = DeviceId.uniqueIdentifier;
			ErrorAnalytics._appInfo = appInfo;
			ErrorAnalytics._deviceInfo = Information.GetDevice();
			ErrorAnalytics._deviceInfoDict = ErrorAnalytics._deviceInfo.ToDict();
			IProducerStrategy producerStrategy = null;
			Unity3D.Init();
			ServicePointManager.ServerCertificateValidationCallback = ((object xsender, X509Certificate xcertificate, X509Chain xchain, SslPolicyErrors xerrors) => true);
			if (ErrorAnalytics._reporter == null)
			{
				producerStrategy = ErrorAnalytics.PersistentProducer(applicationTemporaryCachePath);
				UnityWebRequestClient network = new UnityWebRequestClient();
				ErrorAnalytics._reporter = new EAUnityWebRequestReporter(producerStrategy, network, 60)
				{
					GameId = sbsGameId
				};
			}
			ErrorAnalytics._isInitalized = true;
			SbsNativeErrorAnalytics.InitErrorAnalytics(sbsGameId, ErrorAnalytics._customUserId, appInfo);
			SbsNativeErrorAnalytics.SetBreadcrumbBufferSize(breadCrumbCapacity);
			ErrorAnalytics.UpdateSbsInfoNative(ErrorAnalytics._sbsInfo.DeviceId, ErrorAnalytics._sbsInfo.UserId);
			if (enableErrorHandling)
			{
				Handler.Init(ignoreFollowupFatals);
				Log.LogToErrorAnalytics = true;
			}
			if (producerStrategy != null)
			{
				Log.InfoBreadcrumb("initialized with producer:  " + producerStrategy.GetType().Name);
			}
			ErrorAnalytics.NotifyStart();
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x00080064 File Offset: 0x0007E464
		private static IProducerStrategy PersistentProducer(string applicationTemporaryCachePath)
		{
			string text = Path.Combine(applicationTemporaryCachePath, "ea_cache_unity");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return new PersistentSqliteStringProducer(Path.Combine(text, "errorDb.sql"), 1024L);
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000800A5 File Offset: 0x0007E4A5
		private static IProducerStrategy InMemoryProducer()
		{
			return new InMemoryStringProducer();
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000800AC File Offset: 0x0007E4AC
		public static void NotifyStart()
		{
			if (!ErrorAnalytics.isInitialized)
			{
				return;
			}
			int createdAt = global::Wooga.Core.Utilities.Time.EpochTime();
			Payload.Payload.Start payload = new Payload.Payload.Start(createdAt, ErrorAnalytics._deviceInfoDict, ErrorAnalytics._appInfo, ErrorAnalytics._customUserId);
			string payload2 = Payload.Payload.SerializeStartPayload(ErrorAnalytics._sbsInfo, payload);
			ErrorAnalytics._coroutinesHost.StartTask(ErrorAnalytics._reporter.ReportStart(payload2, ErrorAnalytics._sbsInfo, ErrorAnalytics._appInfo, ErrorAnalytics._customUserId).ContinueWith(() => ErrorAnalytics._reporter.DeliverReportsInBackground()));
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x00080134 File Offset: 0x0007E534
		public static void UpdateSbsInfo(string deviceId, string userId)
		{
			ErrorAnalytics._sbsInfo.DeviceId = deviceId;
			ErrorAnalytics._sbsInfo.UserId = userId;
			ErrorAnalytics.UpdateSbsInfoNative(deviceId, userId);
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x00080154 File Offset: 0x0007E554
		private static void UpdateSbsInfoNative(string deviceId, string userId)
		{
			if (ErrorAnalytics.isInitialized)
			{
				if (Unity3D.Threads.OnMainThread())
				{
					SbsNativeErrorAnalytics.SetSBSDeviceId(deviceId);
					SbsNativeErrorAnalytics.SetSBSUserId(userId);
				}
				else
				{
					Scheduler.StartOnMainThread(
					delegate
					{
						SbsNativeErrorAnalytics.SetSBSDeviceId(deviceId);
						SbsNativeErrorAnalytics.SetSBSUserId(userId);
					});
				}
			}
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000801B5 File Offset: 0x0007E5B5
		public static void UpdateCustomUserId(string customUserId)
		{
			ErrorAnalytics._customUserId = customUserId;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000801C0 File Offset: 0x0007E5C0
		public static void AddBreadcrumb(string breadcrumb)
		{
			// ErrorAnalytics._breadrumbs.Append(breadcrumb);
			if (Unity3D.Threads.OnMainThread())
			{
				SbsNativeErrorAnalytics.AddBreadcrumb(breadcrumb);
			}
			else
			{
				Scheduler.StartOnMainThread(
				delegate
				{
					SbsNativeErrorAnalytics.AddBreadcrumb(breadcrumb);
				});
			}
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00080215 File Offset: 0x0007E615
		public static void RegisterExceptionFilter(Predicate<ParsedException> predicate)
		{
			Handler.RegisterExceptionFilter(predicate);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0008021D File Offset: 0x0007E61D
		public static void UnregisterExceptionFilter(Predicate<ParsedException> predicate)
		{
			Handler.UnregisterExceptionFilter(predicate);
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x00080225 File Offset: 0x0007E625
		public static void SetParsedExceptionUpdater(IParsedExceptionUpdater parsedExceptionUpdater)
		{
			Handler.parsedExceptionUpdater = parsedExceptionUpdater;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0008022D File Offset: 0x0007E62D
		public static void SetExceptionCallback(Action<ParsedException> callback)
		{
			Handler.exceptionCallback = callback;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x00080238 File Offset: 0x0007E638
		public static void RegisterObjectToSerialize(string key, object obj)
		{
			object objectsToSerializeLock = ErrorAnalytics._objectsToSerializeLock;
			lock (objectsToSerializeLock)
			{
				ErrorAnalytics._objectsToSerialize[key] = obj;
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0008027C File Offset: 0x0007E67C
		public static void UnregisterObjectToSerialize(string key)
		{
			object objectsToSerializeLock = ErrorAnalytics._objectsToSerializeLock;
			lock (objectsToSerializeLock)
			{
				if (ErrorAnalytics._objectsToSerialize.ContainsKey(key))
				{
					ErrorAnalytics._objectsToSerialize.Remove(key);
				}
			}
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000802D0 File Offset: 0x0007E6D0
		public static DateTime LastErrorDateTime()
		{
			return FileUtils.GetLastWriteUtcTime(Path.Combine(Unity3D.Paths.ApplicationDataPath, ".last_error_marker"));
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000802E6 File Offset: 0x0007E6E6
		public static DateTime LastFatalDateTime()
		{
			return FileUtils.GetLastWriteUtcTime(Path.Combine(Unity3D.Paths.ApplicationDataPath, ".last_fatal_marker"));
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000802FC File Offset: 0x0007E6FC
		internal static void StoreAndForwardError(string errorType, string message, string rawStackTrace, List<ParsingUtility.StackTraceElement> stackTrace, ErrorAnalytics.LogSeverity severity, Dictionary<string, object> metaData)
		{
			if (!ErrorAnalytics.isInitialized)
			{
				return;
			}
			metaData = ErrorAnalytics.AddExternalObjectsToMetaData(metaData);
			Payload.Payload.Event @event = new Payload.Payload.Event(global::Wooga.Core.Utilities.Time.EpochTime(), severity, ErrorAnalytics._customUserId, errorType, message, ErrorAnalytics._appInfo, rawStackTrace, stackTrace.ToArray(), ErrorAnalytics._breadrumbs.ToArray(), ErrorAnalytics._deviceInfoDict, metaData);
			if (severity != ErrorAnalytics.LogSeverity.Error)
			{
				if (severity == ErrorAnalytics.LogSeverity.Fatal)
				{
					FileUtils.Touch(Path.Combine(Unity3D.Paths.ApplicationDataPath, ".last_fatal_marker"));
				}
			}
			else
			{
				FileUtils.Touch(Path.Combine(Unity3D.Paths.ApplicationDataPath, ".last_error_marker"));
			}
			string payload = Payload.Payload.SerializeErrorPayload(ErrorAnalytics._sbsInfo, new Payload.Payload.Event[]
			{
				@event
			});
			ErrorAnalytics._coroutinesHost.StartTask(ErrorAnalytics._reporter.ReportError(payload, ErrorAnalytics._sbsInfo, ErrorAnalytics._appInfo, ErrorAnalytics._customUserId).ContinueWith(() => ErrorAnalytics._reporter.DeliverReportsInBackground()), delegate(Exception exc)
			{
				global::UnityEngine.Debug.Log(string.Format("TODO: This should be handled by EA internally: {0}", exc.Message));
			}, null);
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0008041C File Offset: 0x0007E81C
		private static Dictionary<string, object> AddExternalObjectsToMetaData(Dictionary<string, object> metaData)
		{
			Dictionary<string, object> dictionary = (metaData == null) ? new Dictionary<string, object>() : new Dictionary<string, object>(metaData);
			if (ErrorAnalytics._objectsToSerialize.Count <= 0)
			{
				return dictionary;
			}
			object objectsToSerializeLock = ErrorAnalytics._objectsToSerializeLock;
			lock (objectsToSerializeLock)
			{
				try
				{
					dictionary["SerializedObjects"] = ErrorAnalytics._objectsToSerialize;
				}
				catch (Exception arg)
				{
					dictionary["SerializedObjects"] = "ErrorAnalytics: Failed to serialize user objects: " + arg;
				}
			}
			return dictionary;
		}

		// Token: 0x040049FC RID: 18940
		private static bool _isInitalized = false;

		// Token: 0x040049FD RID: 18941
		private static Breadcrumbs _breadrumbs;

		// Token: 0x040049FE RID: 18942
		private static IReporter _reporter;

		// Token: 0x040049FF RID: 18943
		private static Information.Sbs _sbsInfo;

		// Token: 0x04004A00 RID: 18944
		private static Information.App _appInfo;

		// Token: 0x04004A01 RID: 18945
		private static Information.Device _deviceInfo;

		// Token: 0x04004A02 RID: 18946
		private static Dictionary<string, object> _deviceInfoDict = null;

		// Token: 0x04004A03 RID: 18947
		private static string _customUserId = string.Empty;

		// Token: 0x04004A04 RID: 18948
		private static MonoBehaviour _coroutinesHost;

		// Token: 0x04004A05 RID: 18949
		private const string DataPath = "ea_cache_unity";

		// Token: 0x04004A06 RID: 18950
		private const string LastErrorMarkerFileName = ".last_error_marker";

		// Token: 0x04004A07 RID: 18951
		private const string LastFatalMarkerFileName = ".last_fatal_marker";

		// Token: 0x04004A08 RID: 18952
		private static readonly Dictionary<string, object> _objectsToSerialize = new Dictionary<string, object>();

		// Token: 0x04004A09 RID: 18953
		private static readonly object _objectsToSerializeLock = new object();

		// Token: 0x020003EF RID: 1007
		public enum LogSeverity
		{
			// Token: 0x04004A0F RID: 18959
			Fatal = 1,
			// Token: 0x04004A10 RID: 18960
			Error,
			// Token: 0x04004A11 RID: 18961
			Warning,
			// Token: 0x04004A12 RID: 18962
			Info,
			// Token: 0x04004A13 RID: 18963
			None
		}
	}
}
