using Match3.Scripts1.Wooga.Core.Exceptions.Network;
using UnityEngine;
using UnityEngine.Networking;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000361 RID: 865
	public class ConnectionMonitor
	{
		// Token: 0x06001A15 RID: 6677 RVA: 0x00075246 File Offset: 0x00073646
		public ConnectionMonitor(UnityWebRequest request, int connectTimeout, int readTimeout)
		{
			this._request = request;
			this._url = request.url;
			this._connectTimeout = connectTimeout;
			this._readTimeout = readTimeout;
			this._startedTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0007527C File Offset: 0x0007367C
		public void AssertLiveliness()
		{
			float num = this._request.uploadProgress + this._request.downloadProgress;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (num > this._lastProgress)
			{
				this._lastReadTime = realtimeSinceStartup;
				this._lastProgress = num;
			}
			else
			{
				if (this._lastProgress == 0f && realtimeSinceStartup - this._startedTime > (float)this._connectTimeout)
				{
					this._request.Abort();
					throw new SbsConnectTimeoutException(string.Format("Connection time out occured for url: {0}", this._url));
				}
				if (this._lastProgress > 0f && realtimeSinceStartup - this._lastReadTime > (float)this._readTimeout)
				{
					this._request.Abort();
					throw new SbsReadTimeoutException(string.Format("Read time out occured for url: {0}", this._url));
				}
			}
		}

		// Token: 0x0400485F RID: 18527
		private float _lastProgress;

		// Token: 0x04004860 RID: 18528
		private float _lastReadTime;

		// Token: 0x04004861 RID: 18529
		private readonly float _startedTime;

		// Token: 0x04004862 RID: 18530
		private readonly UnityWebRequest _request;

		// Token: 0x04004863 RID: 18531
		private readonly string _url;

		// Token: 0x04004864 RID: 18532
		private readonly int _connectTimeout;

		// Token: 0x04004865 RID: 18533
		private readonly int _readTimeout;
	}
}
