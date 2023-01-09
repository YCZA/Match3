using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Payload;
using Match3.Scripts1.Wooga.Services.TrackingCore.Networking;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003F2 RID: 1010
	public class EAUnityWebRequestReporter : IReporter, IConsumer
	{
		// Token: 0x06001E3F RID: 7743 RVA: 0x00080610 File Offset: 0x0007EA10
		public EAUnityWebRequestReporter(IProducerStrategy producer, ISender network, int congestedQueueWarningTimeout = 60)
		{
			this.GameId = string.Empty;
			this._network = network;
			this._producer = new SequentialProducer(producer);
			this._congestedQueueWarningTimeout = congestedQueueWarningTimeout;
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001E40 RID: 7744 RVA: 0x00080645 File Offset: 0x0007EA45
		// (set) Token: 0x06001E41 RID: 7745 RVA: 0x0008064D File Offset: 0x0007EA4D
		public string GameId { get; set; }

		// Token: 0x06001E42 RID: 7746 RVA: 0x00080656 File Offset: 0x0007EA56
		public IEnumerator ReportStart(string payload, Information.Sbs sbsInfo, Information.App appInfo, string customUserId)
		{
			return this.StoreReport(EAUnityWebRequestReporter.StartApi, payload, sbsInfo, appInfo, customUserId);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00080668 File Offset: 0x0007EA68
		public IEnumerator ReportError(string payload, Information.Sbs sbsInfo, Information.App appInfo, string customUserId)
		{
			return this.StoreReport(EAUnityWebRequestReporter.ErrorApi, payload, sbsInfo, appInfo, customUserId);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0008067C File Offset: 0x0007EA7C
		private IEnumerator StoreReport(Uri endpoint, string payload, Information.Sbs sbsInfo, Information.App appInfo, string customUserId)
		{
			if (this._producer.AvailableCount() < 20)
			{
				this._queueCongestedAtLastReport = false;
				return this._producer.Produce(Report.Create(endpoint, payload).Serialize());
			}
			int num = Time.EpochTime();
			if (!this._queueCongestedAtLastReport && this._lastDroppedReportsEpochTime + this._congestedQueueWarningTimeout < num)
			{
				string payload2 = this.QueueLimitPayload(sbsInfo, appInfo, customUserId, num);
				return this._producer.Produce(Report.Create(endpoint, payload2).Serialize());
			}
			this._queueCongestedAtLastReport = true;
			this._lastDroppedReportsEpochTime = num;
			return null;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x00080718 File Offset: 0x0007EB18
		private string QueueLimitPayload(Information.Sbs sbsInfo, Information.App appInfo, string customUserId, int currentEpochTime)
		{
			Payload.Payload.Event @event = new Payload.Payload.Event(Time.EpochTime(), ErrorAnalytics.LogSeverity.Warning, customUserId, base.GetType().ToString(), "EA Queue has hit limit and starts dropping error reports", appInfo, string.Empty, new ParsingUtility.StackTraceElement[0], new string[0], new Dictionary<string, object>(), new Dictionary<string, object>());
			string result = Payload.Payload.SerializeErrorPayload(sbsInfo, new Payload.Payload.Event[]
			{
				@event
			});
			this._lastDroppedReportsEpochTime = currentEpochTime;
			this._queueCongestedAtLastReport = true;
			return result;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x00080789 File Offset: 0x0007EB89
		public virtual IEnumerator DeliverReportsInBackground()
		{
			return this._producer.Map(this);
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00080797 File Offset: 0x0007EB97
		public IEnumerator<bool> Consume(string product)
		{
			return this.PostReport(product);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000807A0 File Offset: 0x0007EBA0
		private IEnumerator<bool> PostReport(string data)
		{
			Report report = Report.Create(data);
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"X-SBS-ID",
					this.GameId
				}
			};
			return this._network.Post(report.Endpoint, report.PayloadBytes(), headers).ContinueWith((HttpStatusCode result) => result >= (HttpStatusCode)599 || result < HttpStatusCode.InternalServerError);
		}

		// Token: 0x04004A1B RID: 18971
		// private static readonly Uri Host = new Uri("https://ea.sbs.wooga.com");
		private static readonly Uri Host = new Uri("host2333");

		// Token: 0x04004A1C RID: 18972
		public static readonly Uri ErrorApi = new Uri(EAUnityWebRequestReporter.Host, "/api/error");

		// Token: 0x04004A1D RID: 18973
		public static readonly Uri StartApi = new Uri(EAUnityWebRequestReporter.Host, "/api/start");

		// Token: 0x04004A1E RID: 18974
		public const int MaxNumQueuedCalls = 20;

		// Token: 0x04004A1F RID: 18975
		private readonly ISender _network;

		// Token: 0x04004A20 RID: 18976
		private readonly IProducerStrategy _producer;

		// Token: 0x04004A21 RID: 18977
		private readonly int _congestedQueueWarningTimeout = 60;

		// Token: 0x04004A22 RID: 18978
		private int _lastDroppedReportsEpochTime;

		// Token: 0x04004A23 RID: 18979
		private bool _queueCongestedAtLastReport;

		// Token: 0x04004A24 RID: 18980
		private bool _executing;
	}
}
