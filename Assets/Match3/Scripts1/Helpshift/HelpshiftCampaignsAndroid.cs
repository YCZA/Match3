using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C9 RID: 457
	public class HelpshiftCampaignsAndroid : IWorkerMethodDispatcher, IDexLoaderListener
	{
		// Token: 0x06000D15 RID: 3349 RVA: 0x0001F034 File Offset: 0x0001D434
		public HelpshiftCampaignsAndroid()
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// this.currentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			HelpshiftDexLoader.getInstance().registerListener(this);
			HelpshiftWorker.getInstance().registerClient("campaigns", this);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0001F080 File Offset: 0x0001D480
		private AndroidJavaObject convertToJavaHashMap(Dictionary<string, object> configD)
		{
			// AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
			if (configD != null)
			{
				Dictionary<string, object> dictionary = (from kv in configD
				where kv.Value != null
				select kv).ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value);
				// IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				array[0] = (array[1] = null);
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					if (keyValuePair.Key != null && keyValuePair.Value != null)
					{
						// using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
						// {
						// 	keyValuePair.Key
						// }))
						// {
						// 	array[0] = androidJavaObject2;
						// 	string a = keyValuePair.Value.GetType().ToString();
						// 	if (a == "System.String")
						// 	{
						// 		array[1] = new AndroidJavaObject("java.lang.String", new object[]
						// 		{
						// 			keyValuePair.Value
						// 		});
						// 	}
						// 	else if (a == "System.Int32")
						// 	{
						// 		array[1] = new AndroidJavaObject("java.lang.Integer", new object[]
						// 		{
						// 			keyValuePair.Value
						// 		});
						// 	}
						// 	else if (a == "System.Int64")
						// 	{
						// 		array[1] = new AndroidJavaObject("java.lang.Long", new object[]
						// 		{
						// 			keyValuePair.Value
						// 		});
						// 	}
						// 	else if (a == "System.Boolean")
						// 	{
						// 		array[1] = new AndroidJavaObject("java.lang.Boolean", new object[]
						// 		{
						// 			keyValuePair.Value
						// 		});
						// 	}
						// 	else if (a == "System.DateTime")
						// 	{
						// 		TimeSpan timeSpan = new TimeSpan(((DateTime)keyValuePair.Value).Ticks);
						// 		double totalMilliseconds = timeSpan.TotalMilliseconds;
						// 		DateTime dateTime = new DateTime(1970, 1, 1);
						// 		double num = totalMilliseconds;
						// 		TimeSpan timeSpan2 = new TimeSpan(dateTime.Ticks);
						// 		double num2 = num - timeSpan2.TotalMilliseconds;
						// 		AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.Double", new object[]
						// 		{
						// 			num2
						// 		});
						// 		long num3 = androidJavaObject3.Call<long>("longValue", new object[0]);
						// 		array[1] = new AndroidJavaObject("java.util.Date", new object[]
						// 		{
						// 			num3
						// 		});
						// 	}
						// 	if (array[1] != null)
						// 	{
						// 		AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						// 	}
						// }
					}
				}
			}
			// return androidJavaObject;
			return null;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0001F394 File Offset: 0x0001D794
		private void addHSApiCallToQueue(string methodIdentifier, string api, object[] args)
		{
			HelpshiftWorker.getInstance().enqueueApiCall("campaigns", methodIdentifier, api, args);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0001F3A8 File Offset: 0x0001D7A8
		private void synchronousWaitForHSApiCallQueue()
		{
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0001F3B4 File Offset: 0x0001D7B4
		public void onDexLoaded()
		{
			//this.hsCampaignsClass = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftCampaignsInstance", new object[0]);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0001F3D8 File Offset: 0x0001D7D8
		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
			if (methodIdentifier.Equals("hsCampaignsApiCall"))
			{
				this.hsCampaignsClass.CallStatic(api, args);
			}
			else if (methodIdentifier.Equals("hsCampaignsApiCallWithoutArgs"))
			{
				this.hsCampaignsClass.CallStatic(api, new object[0]);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0001F429 File Offset: 0x0001D829
		private void hsCampaignsApiCall(string api, params object[] args)
		{
			this.addHSApiCallToQueue("hsCampaignsApiCall", api, args);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0001F438 File Offset: 0x0001D838
		private void hsCampaignsApiCall(string api)
		{
			this.addHSApiCallToQueue("hsCampaignsApiCallWithoutArgs", api, null);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0001F447 File Offset: 0x0001D847
		private bool hsCampaignsApiCallAndReturnBool(string api, params object[] args)
		{
			// this.synchronousWaitForHSApiCallQueue();
			// if (args != null)
			// {
			// 	return this.hsCampaignsClass.CallStatic<bool>(api, args);
			// }
			// return this.hsCampaignsClass.CallStatic<bool>(api, new object[0]);
			return false;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0001F475 File Offset: 0x0001D875
		private int hsCampaignsApiCallAndReturnInt(string api, params object[] args)
		{
			// this.synchronousWaitForHSApiCallQueue();
			// if (args != null)
			// {
			// 	return this.hsCampaignsClass.CallStatic<int>(api, args);
			// }
			// return this.hsCampaignsClass.CallStatic<int>(api, new object[0]);
			return 0;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0001F4A3 File Offset: 0x0001D8A3
		private AndroidJavaObject hsCampaignsApiCallAndReturnObject(string api, params object[] args)
		{
			// this.synchronousWaitForHSApiCallQueue();
			// if (args != null)
			// {
			// 	return this.hsCampaignsClass.CallStatic<AndroidJavaObject>(api, args);
			// }
			// return this.hsCampaignsClass.CallStatic<AndroidJavaObject>(api, new object[0]);
			return null;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0001F4D4 File Offset: 0x0001D8D4
		public bool AddProperty(string key, int value)
		{
			return this.hsCampaignsApiCallAndReturnBool("addProperty", new object[]
			{
				key,
				// new AndroidJavaObject("java.lang.Integer", new object[]
				// {
				// 	value
				// })
			});
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0001F514 File Offset: 0x0001D914
		public bool AddProperty(string key, long value)
		{
			return this.hsCampaignsApiCallAndReturnBool("addProperty", new object[]
			{
				key,
				new AndroidJavaObject("java.lang.Long", new object[]
				{
					value
				})
			});
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0001F554 File Offset: 0x0001D954
		public bool AddProperty(string key, string value)
		{
			return this.hsCampaignsApiCallAndReturnBool("addProperty", new object[]
			{
				key,
				new AndroidJavaObject("java.lang.String", new object[]
				{
					value
				})
			});
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0001F590 File Offset: 0x0001D990
		public bool AddProperty(string key, bool value)
		{
			return this.hsCampaignsApiCallAndReturnBool("addProperty", new object[]
			{
				key,
				new AndroidJavaObject("java.lang.Boolean", new object[]
				{
					value
				})
			});
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0001F5D0 File Offset: 0x0001D9D0
		public bool AddProperty(string key, DateTime value)
		{
			TimeSpan timeSpan = new TimeSpan(value.Ticks);
			double totalMilliseconds = timeSpan.TotalMilliseconds;
			DateTime dateTime = new DateTime(1970, 1, 1);
			double num = totalMilliseconds;
			TimeSpan timeSpan2 = new TimeSpan(dateTime.Ticks);
			double num2 = num - timeSpan2.TotalMilliseconds;
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Double", new object[]
			{
				num2
			});
			long num3 = androidJavaObject.Call<long>("longValue", new object[0]);
			return this.hsCampaignsApiCallAndReturnBool("addProperty", new object[]
			{
				key,
				new AndroidJavaObject("java.util.Date", new object[]
				{
					num3
				})
			});
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0001F67C File Offset: 0x0001DA7C
		public string[] AddProperties(Dictionary<string, object> value)
		{
			AndroidJavaObject androidJavaObject = this.hsCampaignsApiCallAndReturnObject("addProperties", new object[]
			{
				this.convertToJavaHashMap(value)
			});
			return AndroidJNIHelper.ConvertFromJNIArray<string[]>(androidJavaObject.GetRawObject());
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0001F6B2 File Offset: 0x0001DAB2
		public void ShowInbox(Dictionary<string, object> configMap)
		{
			this.hsCampaignsApiCall("showInbox", new object[]
			{
				this.currentActivity
			});
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0001F6CE File Offset: 0x0001DACE
		public int GetCountOfUnreadMessages()
		{
			return this.hsCampaignsApiCallAndReturnInt("getCountOfUnreadMessages", null);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0001F6DC File Offset: 0x0001DADC
		public void RequestUnreadMessagesCount()
		{
			this.hsCampaignsApiCall("requestUnreadMessagesCount");
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0001F6E9 File Offset: 0x0001DAE9
		public void ShowMessage(string messageIdentifier, Dictionary<string, object> configMap)
		{
			this.hsCampaignsApiCall("showMessage", new object[]
			{
				messageIdentifier,
				this.currentActivity
			});
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0001F70C File Offset: 0x0001DB0C
		public void SetInboxMessagesDelegate(IHelpshiftInboxDelegate inboxDelegate)
		{
			this.synchronousWaitForHSApiCallQueue();
			HelpshiftAndroidInboxDelegate helpshiftAndroidInboxDelegate = new HelpshiftAndroidInboxDelegate(inboxDelegate);
			this.hsCampaignsApiCall("setInboxMessageDelegate", new object[]
			{
				helpshiftAndroidInboxDelegate
			});
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0001F73C File Offset: 0x0001DB3C
		public void SetCampaignsDelegate(IHelpshiftCampaignsDelegate campaignsDelegate)
		{
			this.synchronousWaitForHSApiCallQueue();
			HelpshiftAndroidCampaignsDelegate helpshiftAndroidCampaignsDelegate = new HelpshiftAndroidCampaignsDelegate(campaignsDelegate);
			this.hsCampaignsApiCall("setCampaignsDelegate", new object[]
			{
				helpshiftAndroidCampaignsDelegate
			});
		}

		// Token: 0x04003F6B RID: 16235
		private AndroidJavaObject hsCampaignsClass;

		// Token: 0x04003F6C RID: 16236
		private AndroidJavaObject currentActivity;

		// Token: 0x04003F6D RID: 16237
		private AndroidJavaObject application;
	}
}
