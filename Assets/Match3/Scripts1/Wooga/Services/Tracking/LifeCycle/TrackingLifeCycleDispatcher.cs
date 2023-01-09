using System.Collections.Generic;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.LifeCycle
{
	// Token: 0x02000440 RID: 1088
	public class TrackingLifeCycleDispatcher : MonoBehaviour
	{
		// Token: 0x06001FB7 RID: 8119 RVA: 0x000854CC File Offset: 0x000838CC
		private void Awake()
		{
			this.isAwake = true;
			base.gameObject.AddComponent<AndroidAdIdReceiver>();
			TrackingLifeCycleDispatcher.Shared = this;
			int i = 0;
			int count = TrackingLifeCycleDispatcher._receivers.Count;
			while (i < count)
			{
				ILifeCycleReceiver lifeCycleReceiver = TrackingLifeCycleDispatcher._receivers[i];
				lifeCycleReceiver.Awake();
				i++;
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00085524 File Offset: 0x00083924
		private void Start()
		{
			this.isStarted = true;
			if (TrackingLifeCycleDispatcher._receiversToAwake.Count > 0)
			{
				TrackingLifeCycleDispatcher.AwakeReceivers();
			}
			int i = 0;
			int count = TrackingLifeCycleDispatcher._receivers.Count;
			while (i < count)
			{
				ILifeCycleReceiver lifeCycleReceiver = TrackingLifeCycleDispatcher._receivers[i];
				lifeCycleReceiver.Start();
				i++;
			}
			TrackingLifeCycleDispatcher.CheckAppLinks();
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x00085584 File Offset: 0x00083984
		public void Update()
		{
			if (TrackingLifeCycleDispatcher._receiversToAwake.Count > 0)
			{
				TrackingLifeCycleDispatcher.AwakeReceivers();
			}
			if (TrackingLifeCycleDispatcher._receiversToStart.Count > 0)
			{
				foreach (ILifeCycleReceiver lifeCycleReceiver in TrackingLifeCycleDispatcher._receiversToStart)
				{
					lifeCycleReceiver.Start();
				}
				TrackingLifeCycleDispatcher._receiversToStart.RemoveAll((ILifeCycleReceiver check) => true);
			}
			float deltaTime = global::UnityEngine.Time.deltaTime;
			int i = 0;
			int count = TrackingLifeCycleDispatcher._receivers.Count;
			while (i < count)
			{
				ILifeCycleReceiver lifeCycleReceiver2 = TrackingLifeCycleDispatcher._receivers[i];
				lifeCycleReceiver2.Update(deltaTime);
				i++;
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x00085668 File Offset: 0x00083A68
		private static void AwakeReceivers()
		{
			foreach (ILifeCycleReceiver lifeCycleReceiver in TrackingLifeCycleDispatcher._receiversToAwake)
			{
				lifeCycleReceiver.Awake();
			}
			TrackingLifeCycleDispatcher._receiversToAwake.RemoveAll((ILifeCycleReceiver check) => true);
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000856EC File Offset: 0x00083AEC
		private void OnApplicationPause(bool paused)
		{
			int i = 0;
			int count = TrackingLifeCycleDispatcher._receivers.Count;
			while (i < count)
			{
				ILifeCycleReceiver lifeCycleReceiver = TrackingLifeCycleDispatcher._receivers[i];
				lifeCycleReceiver.OnApplicationPause(paused);
				i++;
			}
			if (!paused)
			{
				TrackingLifeCycleDispatcher.CheckAppLinks();
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00085734 File Offset: 0x00083B34
		private void iAdCallback(string message)
		{
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00085738 File Offset: 0x00083B38
		public static void CheckAppLinks()
		{
			Log.Info(new object[]
			{
				"Checking applinks"
			});
			string text = string.Empty;
			//AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.wooga.sdk.AppLinkData");
			//text = androidJavaClass.CallStatic<string>("GetValue", new object[0]);
			//androidJavaClass.CallStatic("set__appLinkData", new object[]
			//{
			//	string.Empty
			//});
			//if (!string.IsNullOrEmpty(text))
			//{
			//	AppLinks.Track(text);
			//}
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000857A8 File Offset: 0x00083BA8
		internal static void AddReceiver(ILifeCycleReceiver receiver)
		{
			TrackingLifeCycleDispatcher._receivers.Add(receiver);
			if (TrackingLifeCycleDispatcher.Shared != null && TrackingLifeCycleDispatcher.Shared.isAwake)
			{
				TrackingLifeCycleDispatcher._receiversToAwake.Add(receiver);
				if (TrackingLifeCycleDispatcher.Shared.isStarted)
				{
					TrackingLifeCycleDispatcher._receiversToStart.Add(receiver);
				}
			}
		}

		// Token: 0x04004B2F RID: 19247
		private static List<ILifeCycleReceiver> _receivers = new List<ILifeCycleReceiver>();

		// Token: 0x04004B30 RID: 19248
		private static List<ILifeCycleReceiver> _receiversToAwake = new List<ILifeCycleReceiver>();

		// Token: 0x04004B31 RID: 19249
		private static List<ILifeCycleReceiver> _receiversToStart = new List<ILifeCycleReceiver>();

		// Token: 0x04004B32 RID: 19250
		private bool isAwake;

		// Token: 0x04004B33 RID: 19251
		private bool isStarted;

		// Token: 0x04004B34 RID: 19252
		public static TrackingLifeCycleDispatcher Shared;
	}
}
