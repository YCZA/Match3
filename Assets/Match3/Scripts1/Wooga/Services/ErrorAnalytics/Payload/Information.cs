using System.Collections.Generic;
using System.Text.RegularExpressions;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Payload
{
	// Token: 0x020003FF RID: 1023
	public static class Information
	{
		// Token: 0x06001E87 RID: 7815 RVA: 0x00081370 File Offset: 0x0007F770
		internal static Information.Sbs.Platform GetSbsPlatform()
		{
			Information.Sbs.Platform result = Information.Sbs.Platform.Unknown;
			if (Application.platform == RuntimePlatform.Android)
			{
				result = Information.Sbs.Platform.Android;
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				result = Information.Sbs.Platform.IOS;
			}
			return result;
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000813A0 File Offset: 0x0007F7A0
		internal static Information.Device GetDevice()
		{
			string screenResolution = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
			return new Information.Device(DeviceId.uniqueIdentifier, DeviceId.AndroidModel(), (uint)SystemInfo.systemMemorySize, (uint)SystemInfo.systemMemorySize, Information.GetOSVersion(), (uint)DeviceId.AndroidApiLevel(), DeviceId.AndroidManufacturer(), DeviceId.AndroidDeviceCode(), Platform.isJailbroken(), Application.systemLanguage.ToString(), screenResolution, Information.GetOSName(), (uint)Screen.dpi, Screen.dpi > 218f);
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0008143C File Offset: 0x0007F83C
		internal static string GetOSVersion()
		{
			string operatingSystem = SystemInfo.operatingSystem;
			Regex regex = new Regex("(?<version>\\d+(\\.\\d)+)");
			System.Text.RegularExpressions.Match match = regex.Match(operatingSystem);
			return match.Groups["version"].Value;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x00081478 File Offset: 0x0007F878
		internal static string GetOSName()
		{
			string operatingSystem = SystemInfo.operatingSystem;
			Regex regex = new Regex("^(?<name>\\w+)");
			System.Text.RegularExpressions.Match match = regex.Match(operatingSystem);
			return match.Groups["name"].Value;
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000814B3 File Offset: 0x0007F8B3
		public static string BackendCompatibleString(this Information.Sbs.Platform system)
		{
			if (system == Information.Sbs.Platform.Android)
			{
				return "android";
			}
			if (system != Information.Sbs.Platform.IOS)
			{
				return "unknownsystem";
			}
			return "ios";
		}

		// Token: 0x02000400 RID: 1024
		internal struct Device
		{
			// Token: 0x06001E8C RID: 7820 RVA: 0x000814DC File Offset: 0x0007F8DC
			public Device(string id, string model, uint physicalRamSize, uint totalMemory, string osVersion, uint apiLevel, string manufacturer, string deviceCode, bool jailbroken, string locale, string screenResolution, string osName, uint screenDensity, bool isRetina)
			{
				this.Manufacturer = manufacturer;
				this.DeviceCode = deviceCode;
				this.Id = id;
				this.Model = model;
				this.PhysicalRamSize = physicalRamSize;
				this.TotalMemory = totalMemory;
				this.OsVersion = osVersion;
				this.ApiLevel = apiLevel;
				this.Jailbroken = jailbroken;
				this.Locale = locale;
				this.ScreenResolution = screenResolution;
				this.OsName = osName;
				this.ScreenDensity = screenDensity;
				this.IsRetina = isRetina;
			}

			// Token: 0x06001E8D RID: 7821 RVA: 0x00081558 File Offset: 0x0007F958
			public Dictionary<string, object> ToDict()
			{
				return new Dictionary<string, object>
				{
					{
						"manufacturer",
						this.Manufacturer
					},
					{
						"deviceCode",
						this.DeviceCode
					},
					{
						"model",
						this.Model
					},
					{
						"physicalRamSize",
						this.PhysicalRamSize
					},
					{
						"totalMemory",
						this.TotalMemory
					},
					{
						"osVersion",
						this.OsVersion
					},
					{
						"apiLevel",
						this.ApiLevel
					},
					{
						"jailbroken",
						this.Jailbroken
					},
					{
						"locale",
						this.Locale
					},
					{
						"screenResolution",
						this.ScreenResolution
					},
					{
						"osName",
						this.OsName
					},
					{
						"screenDensity",
						this.ScreenDensity
					},
					{
						"isRetina",
						this.IsRetina
					}
				};
			}

			// Token: 0x04004A54 RID: 19028
			public readonly uint ApiLevel;

			// Token: 0x04004A55 RID: 19029
			public readonly string Id;

			// Token: 0x04004A56 RID: 19030
			public readonly bool IsRetina;

			// Token: 0x04004A57 RID: 19031
			public readonly bool Jailbroken;

			// Token: 0x04004A58 RID: 19032
			public readonly string Locale;

			// Token: 0x04004A59 RID: 19033
			public readonly string Model;

			// Token: 0x04004A5A RID: 19034
			public readonly string Manufacturer;

			// Token: 0x04004A5B RID: 19035
			public readonly string DeviceCode;

			// Token: 0x04004A5C RID: 19036
			public readonly string OsName;

			// Token: 0x04004A5D RID: 19037
			public readonly string OsVersion;

			// Token: 0x04004A5E RID: 19038
			public readonly uint PhysicalRamSize;

			// Token: 0x04004A5F RID: 19039
			public readonly uint ScreenDensity;

			// Token: 0x04004A60 RID: 19040
			public readonly string ScreenResolution;

			// Token: 0x04004A61 RID: 19041
			public readonly uint TotalMemory;
		}

		// Token: 0x02000401 RID: 1025
		public struct App
		{
			// Token: 0x06001E8E RID: 7822 RVA: 0x00081667 File Offset: 0x0007FA67
			public App(string version, string technicalVersion, string bundleIdentifier)
			{
				this.BundleIdentifier = bundleIdentifier;
				this.TechnicalVersion = technicalVersion;
				this.Version = version;
			}

			// Token: 0x06001E8F RID: 7823 RVA: 0x00081680 File Offset: 0x0007FA80
			public Dictionary<string, object> ToDict()
			{
				return new Dictionary<string, object>
				{
					{
						"version",
						this.Version
					},
					{
						"technicalVersion",
						this.TechnicalVersion
					},
					{
						"bundleId",
						this.BundleIdentifier
					}
				};
			}

			// Token: 0x04004A62 RID: 19042
			public string BundleIdentifier;

			// Token: 0x04004A63 RID: 19043
			public string TechnicalVersion;

			// Token: 0x04004A64 RID: 19044
			public string Version;
		}

		// Token: 0x02000402 RID: 1026
		public struct Sbs
		{
			// Token: 0x06001E90 RID: 7824 RVA: 0x000816C7 File Offset: 0x0007FAC7
			public Sbs(string deviceId, string gameId, Information.Sbs.Platform system, string userId)
			{
				this.DeviceId = deviceId;
				this.GameId = gameId;
				this.System = system;
				this.UserId = userId;
			}

			// Token: 0x06001E91 RID: 7825 RVA: 0x000816E8 File Offset: 0x0007FAE8
			public Dictionary<string, object> ToDict()
			{
				return new Dictionary<string, object>
				{
					{
						"gameId",
						this.GameId
					},
					{
						"system",
						this.System.BackendCompatibleString()
					},
					{
						"userId",
						this.UserId
					},
					{
						"deviceId",
						this.DeviceId
					}
				};
			}

			// Token: 0x04004A65 RID: 19045
			public string DeviceId;

			// Token: 0x04004A66 RID: 19046
			public string GameId;

			// Token: 0x04004A67 RID: 19047
			public Information.Sbs.Platform System;

			// Token: 0x04004A68 RID: 19048
			public string UserId;

			// Token: 0x02000403 RID: 1027
			public enum Platform
			{
				// Token: 0x04004A6A RID: 19050
				Unknown,
				// Token: 0x04004A6B RID: 19051
				Android,
				// Token: 0x04004A6C RID: 19052
				IOS
			}
		}
	}
}
