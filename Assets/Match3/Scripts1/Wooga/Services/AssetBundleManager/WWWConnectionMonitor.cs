using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200032C RID: 812
	public class WWWConnectionMonitor
	{
		// Token: 0x06001938 RID: 6456 RVA: 0x00072412 File Offset: 0x00070812
		public WWWConnectionMonitor(WWW www)
		{
			this._www = www;
			this._startedTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0007242C File Offset: 0x0007082C
		public void AssertLiveliness()
		{
			float progress = this._www.progress;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (progress > this._lastProgress)
			{
				this._lastReadTime = realtimeSinceStartup;
				this._lastProgress = progress;
			}
			else
			{
				if (this._lastProgress == 0f && realtimeSinceStartup - this._startedTime > 10f)
				{
					throw new TimeoutRemoteException("Connect Timeout");
				}
				if (this._lastProgress > 0f && realtimeSinceStartup - this._lastReadTime > 10f)
				{
					throw new TimeoutRemoteException("Read Timeout");
				}
			}
		}

		// Token: 0x040047F6 RID: 18422
		private const float CONNECT_TIMEOUT = 10f;

		// Token: 0x040047F7 RID: 18423
		private const float READ_TIMEOUT = 10f;

		// Token: 0x040047F8 RID: 18424
		private float _lastProgress;

		// Token: 0x040047F9 RID: 18425
		private float _lastReadTime;

		// Token: 0x040047FA RID: 18426
		private readonly float _startedTime;

		// Token: 0x040047FB RID: 18427
		private readonly WWW _www;
	}
}
