using System.Collections.Generic;
using System.Net;

namespace Wooga.Core.Extensions
{
	// Token: 0x0200035F RID: 863
	public static class SbsExtensionMethods
	{
		// Token: 0x06001A09 RID: 6665 RVA: 0x00074FE1 File Offset: 0x000733E1
		public static bool Is(this HttpStatusCode statusCode, int intCode)
		{
			return statusCode == (HttpStatusCode)intCode;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00074FE8 File Offset: 0x000733E8
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

		// Token: 0x06001A0B RID: 6667 RVA: 0x0007501C File Offset: 0x0007341C
		public static string GetHeaderValue(this Dictionary<string, string> headerDict, string key)
		{
			if (headerDict == null)
			{
				return null;
			}
			string result;
			headerDict.TryGetValue(key, out result);
			return result;
		}
	}
}
