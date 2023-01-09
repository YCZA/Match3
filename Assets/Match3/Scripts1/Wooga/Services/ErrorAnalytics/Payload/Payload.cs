using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Payload
{
	// Token: 0x02000404 RID: 1028
	public static class Payload
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x00081748 File Offset: 0x0007FB48
		public static string SerializeErrorPayload(Information.Sbs sbsInfo, Payload.Event[] events)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"notifierVersion",
					Payload.NotifierVersion
				},
				{
					"sbsInfo",
					sbsInfo.ToDict()
				}
			};
			if (events != null)
			{
				Dictionary<string, object>[] array = new Dictionary<string, object>[events.Length];
				for (int i = 0; i < events.Length; i++)
				{
					Payload.Event @event = events[i];
					@event.MetaData = Payload.NormalizeMetaData(@event.MetaData);
					array[i] = @event.ToDict();
				}
				dictionary["events"] = array;
			}
			return JSON.Serialize(dictionary, false, 1, ' ');
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000817E4 File Offset: 0x0007FBE4
		public static string SerializeStartPayload(Information.Sbs sbsInfo, Payload.Start payload)
		{
			Dictionary<string, object> o = new Dictionary<string, object>
			{
				{
					"notifierVersion",
					Payload.NotifierVersion
				},
				{
					"sbsInfo",
					sbsInfo.ToDict()
				},
				{
					"createdAt",
					payload.CreatedAt
				},
				{
					"device",
					payload.DeviceInfo
				},
				{
					"app",
					payload.AppInfo.ToDict()
				},
				{
					"userId",
					payload.UserId
				}
			};
			return JSON.Serialize(o, false, 1, ' ');
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0008187C File Offset: 0x0007FC7C
		private static Dictionary<string, object> NormalizeMetaData(Dictionary<string, object> metaData)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, object> keyValuePair in metaData)
			{
				try
				{
					if (JSON.Serialize(new object[]
					{
						keyValuePair.Value
					}, false, 1, ' ').Length < 1500)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
					else
					{
						string text = keyValuePair.Value as string;
						if (text != null)
						{
							string value = text.Substring(0, 1500);
							dictionary[keyValuePair.Key] = value;
						}
						list.Add(keyValuePair.Key);
					}
				}
				catch (Exception arg)
				{
					dictionary[keyValuePair.Key] = "ErrorAnalytics: Failed to serialize object: " + arg;
				}
			}
			if (list.Count > 0)
			{
				string text2 = string.Empty;
				foreach (string str in list)
				{
					text2 = text2 + str + " ";
				}
				dictionary["__SDK_EA_overflow_error"] = "The following keys were truncated/removed because their values were too large: " + text2;
			}
			return dictionary;
		}

		// Token: 0x04004A6D RID: 19053
		public static readonly string NotifierVersion = "2.0.0";

		// Token: 0x02000405 RID: 1029
		public struct Start
		{
			// Token: 0x06001E96 RID: 7830 RVA: 0x00081A14 File Offset: 0x0007FE14
			public Start(int createdAt, Dictionary<string, object> deviceInfo, Information.App appInfo, string userId)
			{
				this.CreatedAt = createdAt;
				this.DeviceInfo = deviceInfo;
				this.AppInfo = appInfo;
				this.UserId = userId;
			}

			// Token: 0x04004A6E RID: 19054
			public readonly Information.App AppInfo;

			// Token: 0x04004A6F RID: 19055
			public readonly int CreatedAt;

			// Token: 0x04004A70 RID: 19056
			public readonly Dictionary<string, object> DeviceInfo;

			// Token: 0x04004A71 RID: 19057
			public readonly string UserId;
		}

		// Token: 0x02000406 RID: 1030
		public struct Event
		{
			// Token: 0x06001E97 RID: 7831 RVA: 0x00081A34 File Offset: 0x0007FE34
			public Event(int createdAt, ErrorAnalytics.LogSeverity severity, string userId, string errorType, string message, Information.App appInfo, string rawStackTrace, ParsingUtility.StackTraceElement[] stackTrace, string[] breadcrumbs, Dictionary<string, object> deviceInfo, Dictionary<string, object> metaData)
			{
				this.CreatedAt = createdAt;
				this.Severity = severity;
				this.UserId = userId;
				this.ErrorType = errorType;
				this.Message = message;
				this.AppInfo = appInfo;
				this.RawStacktrace = rawStackTrace;
				this.StackTrace = stackTrace;
				this.Breadcrumbs = breadcrumbs;
				this.DeviceInfo = deviceInfo;
				this.MetaData = metaData;
			}

			// Token: 0x06001E98 RID: 7832 RVA: 0x00081A98 File Offset: 0x0007FE98
			public Dictionary<string, object> ToDict()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>
				{
					{
						"createdAt",
						this.CreatedAt
					},
					{
						"severity",
						this.SeverityToString(this.Severity)
					},
					{
						"userId",
						this.UserId
					},
					{
						"errorType",
						this.ErrorType
					},
					{
						"message",
						this.Message
					},
					{
						"app",
						this.AppInfo.ToDict()
					},
					{
						"stacktraceRaw",
						this.RawStacktrace
					}
				};
				if (this.StackTrace != null)
				{
					dictionary["stacktrace"] = this.StackTrace;
				}
				if (this.Breadcrumbs != null)
				{
					dictionary["breadcrumbs"] = this.Breadcrumbs;
				}
				if (this.DeviceInfo != null)
				{
					dictionary["device"] = this.DeviceInfo;
				}
				if (this.MetaData != null)
				{
					dictionary["metaData"] = this.MetaData;
				}
				return dictionary;
			}

			// Token: 0x06001E99 RID: 7833 RVA: 0x00081BA8 File Offset: 0x0007FFA8
			private string SeverityToString(ErrorAnalytics.LogSeverity sev)
			{
				if (sev == ErrorAnalytics.LogSeverity.Warning)
				{
					return "warning";
				}
				if (sev == ErrorAnalytics.LogSeverity.Error)
				{
					return "error";
				}
				if (sev != ErrorAnalytics.LogSeverity.Fatal)
				{
					return "unknown severity";
				}
				return "fatal";
			}

			// Token: 0x04004A72 RID: 19058
			public readonly Information.App AppInfo;

			// Token: 0x04004A73 RID: 19059
			public readonly string[] Breadcrumbs;

			// Token: 0x04004A74 RID: 19060
			public readonly int CreatedAt;

			// Token: 0x04004A75 RID: 19061
			public readonly Dictionary<string, object> DeviceInfo;

			// Token: 0x04004A76 RID: 19062
			public readonly string ErrorType;

			// Token: 0x04004A77 RID: 19063
			public readonly string Message;

			// Token: 0x04004A78 RID: 19064
			public readonly string RawStacktrace;

			// Token: 0x04004A79 RID: 19065
			public readonly ErrorAnalytics.LogSeverity Severity;

			// Token: 0x04004A7A RID: 19066
			public readonly ParsingUtility.StackTraceElement[] StackTrace;

			// Token: 0x04004A7B RID: 19067
			public readonly string UserId;

			// Token: 0x04004A7C RID: 19068
			public Dictionary<string, object> MetaData;
		}
	}
}
