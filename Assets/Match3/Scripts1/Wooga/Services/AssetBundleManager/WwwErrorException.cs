using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000329 RID: 809
	public class WwwErrorException : RemoteException
	{
		// Token: 0x0600191F RID: 6431 RVA: 0x000721A0 File Offset: 0x000705A0
		public WwwErrorException(HttpStatusCode statusCode, string error, string url)
		{
			this.OrigErrorText = error;
			this.StatusCode = statusCode;
			this.Url = url;
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x000721BD File Offset: 0x000705BD
		public WwwErrorException(string error, string url)
		{
			this.OrigErrorText = error;
			this.Url = url;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x000721D4 File Offset: 0x000705D4
		public WwwErrorException(WWW www)
		{
			try
			{
				this.OrigErrorText = www.error;
				this.Url = www.url;
				Regex regex = new Regex("[\\D]*(\\d)+");
				System.Text.RegularExpressions.Match match = regex.Match(www.error);
				int statusCode;
				if (match.Success && int.TryParse(match.Groups[0].ToString(), out statusCode))
				{
					this.StatusCode = (HttpStatusCode)statusCode;
				}
				this.ErrorReason = ((!www.responseHeaders.ContainsKey("X-ERROR")) ? this.OrigErrorText : www.responseHeaders["X-ERROR"]);
			}
			catch
			{
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x00072298 File Offset: 0x00070698
		// (set) Token: 0x06001923 RID: 6435 RVA: 0x000722A0 File Offset: 0x000706A0
		public string ErrorReason { get; private set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x000722A9 File Offset: 0x000706A9
		// (set) Token: 0x06001925 RID: 6437 RVA: 0x000722B1 File Offset: 0x000706B1
		public string OrigErrorText { get; private set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x000722BA File Offset: 0x000706BA
		// (set) Token: 0x06001927 RID: 6439 RVA: 0x000722C2 File Offset: 0x000706C2
		public HttpStatusCode StatusCode { get; private set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x000722CB File Offset: 0x000706CB
		// (set) Token: 0x06001929 RID: 6441 RVA: 0x000722D3 File Offset: 0x000706D3
		public string Url { get; private set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600192A RID: 6442 RVA: 0x000722DC File Offset: 0x000706DC
		public override string Message
		{
			get
			{
				return string.Format("{0} - {1}: {2}", (int)this.StatusCode, this.OrigErrorText, this.Url);
			}
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x000722FF File Offset: 0x000706FF
		public static WwwErrorException Create(WWW www)
		{
			if (AssetBundleLoadException.IsLoadException(www))
			{
				return new AssetBundleLoadException(www);
			}
			return new WwwErrorException(www);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00072319 File Offset: 0x00070719
		public override string ToString()
		{
			return this.Message;
		}
	}
}
