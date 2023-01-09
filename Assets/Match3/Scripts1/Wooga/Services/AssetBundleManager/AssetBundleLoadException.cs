using System.Net;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200032A RID: 810
	public class AssetBundleLoadException : WwwErrorException
	{
		// Token: 0x0600192D RID: 6445 RVA: 0x00072321 File Offset: 0x00070721
		public AssetBundleLoadException(WWW www) : base(www)
		{
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0007232A File Offset: 0x0007072A
		public AssetBundleLoadException(string error, string url) : base((HttpStatusCode)0, error, url)
		{
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00072335 File Offset: 0x00070735
		public static bool IsLoadException(WWW www)
		{
			return www.error != null && www.error.StartsWith(AssetBundleLoadException.ErrorStart);
		}

		// Token: 0x040047F4 RID: 18420
		private static readonly string ErrorStart = "The AssetBundle";
	}
}
