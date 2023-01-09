using System;
using System.IO;
using Match3.Scripts1.Wooga.Core.IO;
using Wooga.Core.Utilities;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools.PersistentDataImpl
{
	// Token: 0x02000462 RID: 1122
	public class AtomicFilePersistentData<T> : PersistentData<T> where T : class, new()
	{
		// Token: 0x0600208F RID: 8335 RVA: 0x00089E1E File Offset: 0x0008821E
		public AtomicFilePersistentData(string path) : base(path)
		{
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00089E28 File Offset: 0x00088228
		public override void Load()
		{
			if (!AtomicFile.ExistsAndRecoverFromAtomicWrite(this._path))
			{
				return;
			}
			try
			{
				string text = File.ReadAllText(this._path);
				if (!string.IsNullOrEmpty(text))
				{
					this._data = JSON.Deserialize<T>(text);
				}
			}
			catch (Exception ex)
			{
				Log.Warning(new object[]
				{
					"Failed to load persistent data path " + this._path + " " + ex.Message
				});
			}
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00089EB4 File Offset: 0x000882B4
		public override void Write()
		{
			try
			{
				string text = JSON.Serialize(this._data, false, 1, ' ');
				AtomicFile.WriteAllTextAtomic(this._path, text);
				IcloudBackup.Exclude(this._path);
			}
			catch (Exception ex)
			{
				Log.Warning(new object[]
				{
					"Failed to save persistent data path " + this._path + " " + ex.Message
				});
			}
		}
	}
}
