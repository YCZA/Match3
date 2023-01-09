using System.Collections.Generic;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using Match3.Scripts1.Wooga.Services.Tracking.Tools;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.Parameters
{
	// Token: 0x02000444 RID: 1092
	public class BaseParameters : IParameterProvider
	{
		// Token: 0x06001FD0 RID: 8144 RVA: 0x00085987 File Offset: 0x00083D87
		public BaseParameters()
		{
			this._deviceType = DeviceType.type;
			this._language = Language.SystemLanguageISOCode();
			this._osVersion = OperatingSystem.version;
			this._jailbroken = Platform.isJailbroken();
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000859BC File Offset: 0x00083DBC
		public void AddParametersTo(Dictionary<string, object> data)
		{
			data["device"] = this._deviceType;
			data["osversion"] = this._osVersion;
			data["language"] = this._language;
			data["jb"] = this._jailbroken;
			data["age"] = (int)Statistics.SinceAppInstall().TotalSeconds;
			data["mid"] = Tracking.Id;
			data["ns"] = BaseParameters.GetNetworkStatus();
			data["version"] = Bundle.version;
			data["bvn"] = Bundle.build;
			if (!data.ContainsKey("tscreated"))
			{
				data["tscreated"] = (long)global::Wooga.Core.Utilities.Time.EpochTime();
			}
			if (Tracking.Environment != Environment.NotSet)
			{
				data["env"] = (int)Tracking.Environment;
			}
			if (Tracking.Cheater != null)
			{
				data["fta"] = Tracking.Cheater.Value;
			}
			if (this.facebookUserId != null)
			{
				data["s"] = this.facebookUserId;
			}
			if (this.sbsUserId != null)
			{
				data["sbsuserid"] = this.sbsUserId;
			}
			if (this.abTestGroups != null)
			{
				data["ab"] = this.abTestGroups;
			}
			data["gadid"] = AndroidAdId.adId;
			data["android_id"] = DeviceId.AndroidId();
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00085B60 File Offset: 0x00083F60
		private static int GetNetworkStatus()
		{
			int result = 0;
			NetworkReachability internetReachability = Application.internetReachability;
			if (internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork)
			{
				if (internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
				{
					result = 2;
				}
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x04004B39 RID: 19257
		public string facebookUserId;

		// Token: 0x04004B3A RID: 19258
		public string sbsUserId;

		// Token: 0x04004B3B RID: 19259
		public string abTestGroups;

		// Token: 0x04004B3C RID: 19260
		private string _deviceType;

		// Token: 0x04004B3D RID: 19261
		private string _osVersion;

		// Token: 0x04004B3E RID: 19262
		private string _language;

		// Token: 0x04004B3F RID: 19263
		private bool _jailbroken;
	}
}
