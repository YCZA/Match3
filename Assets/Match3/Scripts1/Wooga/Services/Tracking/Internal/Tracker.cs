using System.Collections;
using System.Collections.Generic;
using System.IO;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using Match3.Scripts1.Wooga.Services.Tracking.Sender;
using Match3.Scripts1.Wooga.Services.Tracking.Tools;
using Match3.Scripts1.Wooga.Tracking;

namespace Match3.Scripts1.Wooga.Services.Tracking.Internal
{
	// Token: 0x0200043D RID: 1085
	public class Tracker
	{
		// Token: 0x06001FA5 RID: 8101 RVA: 0x00085231 File Offset: 0x00083631
		public Tracker(ITrackingSender sender, string hostName, TrackingConfiguration configuration)
		{
			this._sender = sender;
			this._configuration = configuration;
			this._urlBuilder = new UrlBuilder(hostName);
			this.InitOnceTrackData(hostName);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0008525C File Offset: 0x0008365C
		public virtual IEnumerator Track(string callName, Dictionary<string, object> data)
		{
			data = (data ?? new Dictionary<string, object>());
			this.AddParametersTo(data);
			string text = this._urlBuilder.BuildUrl(callName, data);
			Log.Info(new object[]
			{
				"Track " + text
			});
			return this._sender.Queue(text).ContinueWith(() => this._sender.ConsumeAll());
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000852C4 File Offset: 0x000836C4
		public virtual IEnumerator TrackOnce(string callName, Dictionary<string, object> data)
		{
			if (!this._trackOnceData.data.TrackedOneTimeCalls.ContainsKey(callName))
			{
				return this.Track(callName, data).ContinueWith(delegate()
				{
					this.MarkAsSent(callName);
				});
			}
			return global::Wooga.Coroutines.Coroutines.Empty();
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x00085329 File Offset: 0x00083729
		public bool HasAlreadyTrackedOnce(string callName)
		{
			return this._trackOnceData.data.TrackedOneTimeCalls.ContainsKey(callName);
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x00085341 File Offset: 0x00083741
		protected void WritePersistentData()
		{
			this._trackOnceData.Write();
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0008534E File Offset: 0x0008374E
		internal void MarkAsSent(string callName)
		{
			this._trackOnceData.data.TrackedOneTimeCalls[callName] = true;
			this.WritePersistentData();
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x00085370 File Offset: 0x00083770
		private void AddParametersTo(Dictionary<string, object> data)
		{
			foreach (IParameterProvider parameterProvider in this._configuration.parameterProviders)
			{
				parameterProvider.AddParametersTo(data);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000853D4 File Offset: 0x000837D4
		private void InitOnceTrackData(string hostName)
		{
			string dataFileNameAndCreateFile = this.GetDataFileNameAndCreateFile(hostName);
			this._trackOnceData = PersistentData.Create<Tracker.TrackingCorePersistentData>(dataFileNameAndCreateFile);
			this._trackOnceData.Load();
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x00085400 File Offset: 0x00083800
		private string GetDataFileNameAndCreateFile(string hostName)
		{
			string dataDir = this.GetDataDir();
			if (!Directory.Exists(dataDir))
			{
				Directory.CreateDirectory(dataDir);
			}
			string path = hostName.Replace("/", "_");
			string text = Path.Combine(dataDir, path);
			if (!File.Exists(text))
			{
				File.Create(text).Dispose();
			}
			return text;
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x00085458 File Offset: 0x00083858
		protected string GetDataDir()
		{
			string applicationDataPath = Unity3D.Paths.ApplicationDataPath;
			return Path.Combine(applicationDataPath, Tracker.TrackOnceDataDirectory);
		}

		// Token: 0x04004B29 RID: 19241
		private readonly ITrackingSender _sender;

		// Token: 0x04004B2A RID: 19242
		private readonly TrackingConfiguration _configuration;

		// Token: 0x04004B2B RID: 19243
		protected UrlBuilder _urlBuilder;

		// Token: 0x04004B2C RID: 19244
		public static readonly string TrackOnceDataDirectory = "core-once";

		// Token: 0x04004B2D RID: 19245
		protected PersistentData<Tracker.TrackingCorePersistentData> _trackOnceData;

		// Token: 0x0200043E RID: 1086
		protected class TrackingCorePersistentData
		{
			// Token: 0x04004B2E RID: 19246
			public Dictionary<string, bool> TrackedOneTimeCalls = new Dictionary<string, bool>();
		}
	}
}
