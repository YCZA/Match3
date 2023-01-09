using System.Collections.Generic;
using System.Net;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000477 RID: 1143
	public static class ResponseExtensionMethods
	{
		// Token: 0x0600210D RID: 8461 RVA: 0x0008B304 File Offset: 0x00089704
		public static bool Is(this HttpStatusCode statusCode, int intCode)
		{
			return statusCode == (HttpStatusCode)intCode;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0008B30C File Offset: 0x0008970C
		public static bool In(this HttpStatusCode statusCode, params int[] intCodes)
		{
			foreach (int num in intCodes)
			{
				if (statusCode == (HttpStatusCode)num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0008B340 File Offset: 0x00089740
		public static string GetHeaderValue(this Dictionary<string, string> headerDict, string key)
		{
			string result;
			headerDict.TryGetValue(key, out result);
			return result;
		}
	}
}
