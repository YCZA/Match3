using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.Tracking.Parameters
{
	// Token: 0x02000447 RID: 1095
	public class IOSAdId
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x00085BDF File Offset: 0x00083FDF
		public static string adId
		{
			get
			{
				if (IOSAdId._id == null)
				{
					IOSAdId._id = IOSAdId.GetValueNative();
				}
				return IOSAdId._id;
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x00085BFC File Offset: 0x00083FFC
		public static void TrackIadInstall()
		{
			global::UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(TrackingLifeCycleDispatcher));
			if (array.Length > 0)
			{
				global::UnityEngine.Object @object = array[0];
				IOSAdId.CheckIsIadInstall(@object.name, "iAdCallback");
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x00085C36 File Offset: 0x00084036
		private static string GetValueNative()
		{
			return string.Empty;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x00085C3D File Offset: 0x0008403D
		private static void CheckIsIadInstall(string callbackObject, string callbackMethodName)
		{
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00085C40 File Offset: 0x00084040
		public static void HandleCallback(string result)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (!string.IsNullOrEmpty(result) && result.Length > 0)
			{
				JSONNode jsonnode = JSON.Deserialize(result);
				if (jsonnode != null)
				{
					JSONObject jsonobject = jsonnode.AsDictionary(null);
					if (jsonobject != null)
					{
						foreach (KeyValuePair<string, JSONNode> keyValuePair in jsonobject)
						{
							dictionary[keyValuePair.Key] = keyValuePair.Value.AsString();
						}
					}
				}
			}
			IOSAdId.CallbackFunc(dictionary);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x00085CF0 File Offset: 0x000840F0
		private static void TrackCallback(Dictionary<string, object> iAdInstallDetails)
		{
			if (iAdInstallDetails.Count > 0)
			{
				Tracking.TrackOnce("macd", iAdInstallDetails);
			}
			else
			{
				Tracking.MarkAsSent("macd");
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x00085D18 File Offset: 0x00084118
		// Note: this type is marked as 'beforefieldinit'.
		static IOSAdId()
		{
			if (IOSAdId._003C_003Ef__mg_0024cache0 == null)
			{
				IOSAdId._003C_003Ef__mg_0024cache0 = new Action<Dictionary<string, object>>(IOSAdId.TrackCallback);
			}
			IOSAdId.CallbackFunc = IOSAdId._003C_003Ef__mg_0024cache0;
		}

		// Token: 0x04004B48 RID: 19272
		private static string _id;

		// Token: 0x04004B49 RID: 19273
		public static Action<Dictionary<string, object>> CallbackFunc;

		// Token: 0x04004B4A RID: 19274
		[CompilerGenerated]
		private static Action<Dictionary<string, object>> _003C_003Ef__mg_0024cache0;
	}
}
