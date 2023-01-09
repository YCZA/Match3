using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001CC RID: 460
	public class HelpshiftInboxAndroid : IWorkerMethodDispatcher, IDexLoaderListener
	{
		// Token: 0x06000D37 RID: 3383 RVA: 0x0001F904 File Offset: 0x0001DD04
		public HelpshiftInboxAndroid()
		{
			HelpshiftDexLoader.getInstance().registerListener(this);
			HelpshiftWorker.getInstance().registerClient("inbox", this);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0001F927 File Offset: 0x0001DD27
		public void onDexLoaded()
		{
			HelpshiftWorker.getInstance().registerClient("inbox", this);
			this.addHSApiCallToQueue("initializeInboxInstance", "getInboxInstance", null);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0001F94A File Offset: 0x0001DD4A
		private void addHSApiCallToQueue(string methodIdentifier, string api, object[] args)
		{
			HelpshiftWorker.getInstance().enqueueApiCall("inbox", methodIdentifier, api, args);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0001F95E File Offset: 0x0001DD5E
		private void hsInboxApiCall(string api, object[] args)
		{
			this.addHSApiCallToQueue("hsInboxApiCall", api, args);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0001F970 File Offset: 0x0001DD70
		public void resolveAndCallApi(string methodIdentifier, string apiName, object[] args)
		{
			if (methodIdentifier.Equals("hsInboxApiCall"))
			{
				this.hsInboxJavaInstance.Call(apiName, args);
			}
			else if (methodIdentifier.Equals("initializeInboxInstance"))
			{
				this.hsInboxJavaInstance = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>(apiName, new object[0]);
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0001F9CB File Offset: 0x0001DDCB
		private void synchronousWaitForHSApiCallQueue()
		{
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0001F9D8 File Offset: 0x0001DDD8
		public List<HelpshiftInboxMessage> GetAllInboxMessages()
		{
			this.synchronousWaitForHSApiCallQueue();
			try
			{
				AndroidJavaObject androidJavaObject = this.hsInboxJavaInstance.Call<AndroidJavaObject>("getAllInboxMessages", new object[0]);
				List<HelpshiftInboxMessage> list = new List<HelpshiftInboxMessage>();
				int num = androidJavaObject.Call<int>("size", new object[0]);
				for (int i = 0; i < num; i++)
				{
					AndroidJavaObject message = androidJavaObject.Call<AndroidJavaObject>("get", new object[]
					{
						i
					});
					list.Add(HelpshiftAndroidInboxMessage.createInboxMessage(message));
				}
				return list;
			}
			catch (Exception ex)
			{
				global::UnityEngine.Debug.Log("Helpshift : Error getting all inbox messages : " + ex.Message);
			}
			return null;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0001FA90 File Offset: 0x0001DE90
		public HelpshiftInboxMessage GetInboxMessage(string messageIdentifier)
		{
			this.synchronousWaitForHSApiCallQueue();
			try
			{
				AndroidJavaObject message = this.hsInboxJavaInstance.Call<AndroidJavaObject>("getInboxMessage", new object[]
				{
					messageIdentifier
				});
				return HelpshiftAndroidInboxMessage.createInboxMessage(message);
			}
			catch (Exception ex)
			{
				global::UnityEngine.Debug.Log("Helpshift : Error getting inbox message : " + ex.Message);
			}
			return null;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0001FAFC File Offset: 0x0001DEFC
		public void MarkInboxMessageAsRead(string messageIdentifier)
		{
			this.synchronousWaitForHSApiCallQueue();
			this.hsInboxJavaInstance.Call("markInboxMessageAsRead", new object[]
			{
				messageIdentifier
			});
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0001FB1E File Offset: 0x0001DF1E
		public void MarkInboxMessageAsSeen(string messageIdentifier)
		{
			this.synchronousWaitForHSApiCallQueue();
			this.hsInboxJavaInstance.Call("markInboxMessageAsSeen", new object[]
			{
				messageIdentifier
			});
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0001FB40 File Offset: 0x0001DF40
		public void DeleteInboxMessage(string messageIdentifier)
		{
			this.synchronousWaitForHSApiCallQueue();
			this.hsInboxJavaInstance.Call("deleteInboxMessage", new object[]
			{
				messageIdentifier
			});
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0001FB64 File Offset: 0x0001DF64
		public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
		{
			this.synchronousWaitForHSApiCallQueue();
			HelpshiftAndroidInboxDelegate helpshiftAndroidInboxDelegate = new HelpshiftAndroidInboxDelegate(externalDelegate);
			this.hsInboxJavaInstance.Call("setInboxMessageDelegate", new object[]
			{
				helpshiftAndroidInboxDelegate
			});
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0001FB98 File Offset: 0x0001DF98
		public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
		{
			this.synchronousWaitForHSApiCallQueue();
			HelpshiftAndroidInboxPushNotificationDelegate helpshiftAndroidInboxPushNotificationDelegate = new HelpshiftAndroidInboxPushNotificationDelegate(externalDelegate);
			this.hsInboxJavaInstance.Call("setInboxPushNotificationDelegate", new object[]
			{
				helpshiftAndroidInboxPushNotificationDelegate
			});
		}

		// Token: 0x04003F77 RID: 16247
		private AndroidJavaObject hsInboxJavaInstance;
	}
}
