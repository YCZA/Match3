using System;
using Wooga.Core.Utilities;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools.PersistentDataImpl
{
	// Token: 0x02000465 RID: 1125
	public class SqlitePersistentData<T> : PersistentData<T> where T : class, new()
	{
		// Token: 0x060020A4 RID: 8356 RVA: 0x0008A2A6 File Offset: 0x000886A6
		public SqlitePersistentData(string path) : base(path)
		{
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x0008A2AF File Offset: 0x000886AF
		public void setData(T value)
		{
			this._data = value;
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x0008A2B8 File Offset: 0x000886B8
		public override void Load()
		{
			try
			{
				// PersistentDataValue persistentDataValue = SqlitePersistentData.Get(this._path);
				// if (persistentDataValue != null && !string.IsNullOrEmpty(persistentDataValue.Value))
				// {
					// this._data = JSON.Deserialize<T>(persistentDataValue.Value);
				// }
			}
			catch (Exception ex)
			{
				Log.Warning(new object[]
				{
					"Failed to load persistent data path " + this._path + " " + ex.ToString()
				});
			}
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x0008A344 File Offset: 0x00088744
		public override void Write()
		{
			try
			{
				string val = JSON.Serialize(this._data, false, 1, ' ');
				SqlitePersistentData.Set(this._path, val);
			}
			catch (Exception ex)
			{
				Log.Warning(new object[]
				{
					"Failed to save persistent data path='" + this._path + "' exception: " + ex.ToString()
				});
			}
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x0008A3B8 File Offset: 0x000887B8
		// public bool HasValue(string key)
		// {
			// return SqlitePersistentData.HasValue(key);
		// }
	}
}
