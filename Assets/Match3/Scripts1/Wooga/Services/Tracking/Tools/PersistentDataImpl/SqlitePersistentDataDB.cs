using System;
using System.IO;
using Wooga.Core.Utilities; // using Wooga.Core.Sqlite;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools.PersistentDataImpl
{
	// Token: 0x02000463 RID: 1123
	public class SqlitePersistentDataDB
	{
		// Token: 0x06002092 RID: 8338 RVA: 0x00089F34 File Offset: 0x00088334
		public SqlitePersistentDataDB(string path)
		{
			this.Path = path;
			Log.Info(new object[]
			{
				"Creating tracking persistent data sql db handle at " + path
			});
			string query = "CREATE TABLE IF NOT EXISTS PersistentData (Key TEXT UNIQUE, Value TEXT);";
			FileInfo fileInfo = new FileInfo(path);
			try
			{
				if (fileInfo.Directory != null)
				{
					fileInfo.Directory.Create();
				}
			}
			catch (IOException ex)
			{
				Log.Error(new object[]
				{
					"Could not create tracking persistent data dir - " + ex.Message
				});
				return;
			}
			// this._connection = new SQLiteConnection(fileInfo.FullName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, false);
			// this._connection.Execute(query, new object[0]);
			// IcloudBackup.Exclude(path);
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x00089FF4 File Offset: 0x000883F4
		// (set) Token: 0x06002094 RID: 8340 RVA: 0x00089FFC File Offset: 0x000883FC
		public string Path { get; private set; }

		// Token: 0x06002095 RID: 8341 RVA: 0x0008A005 File Offset: 0x00088405
		public void Close()
		{
			// if (this._connection != null)
			// {
			// 	this._connection.Close();
			// 	this._connection = null;
			// }
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x0008A024 File Offset: 0x00088424
		// public bool HasValue(string key)
		// {
			// key = this.TruncateFilePathKey(key);
			// int num = this._connection.ExecuteScalar<int>("SELECT EXISTS(SELECT 1 FROM PersistentData WHERE key LIKE ? LIMIT 1)", new object[]
			// {
			// 	this.WildcardKey(key)
			// });
			// return num == 1;
		// }

		// Token: 0x06002097 RID: 8343 RVA: 0x0008A05F File Offset: 0x0008845F
		// public PersistentDataValue Get(string key)
		// {
			// key = this.TruncateFilePathKey(key);
			// return this._connection.Query<PersistentDataValue>("SELECT * FROM PersistentData WHERE key LIKE ? ", new object[]
			// {
			// 	this.WildcardKey(key)
			// }).FirstOrDefault<PersistentDataValue>();
		// }

		// Token: 0x06002098 RID: 8344 RVA: 0x0008A08F File Offset: 0x0008848F
		// public List<PersistentDataValue> GetAll()
		// {
		// 	return this._connection.Query<PersistentDataValue>("SELECT * FROM PersistentData", new object[0]);
		// }

		// Token: 0x06002099 RID: 8345 RVA: 0x0008A0A8 File Offset: 0x000884A8
		public void Set(string key, string val)
		{
			// key = this.TruncateFilePathKey(key);
			// int num = this._connection.Execute("UPDATE PersistentData SET Value = ? WHERE Key LIKE ? ", new object[]
			// {
			// 	val,
			// 	this.WildcardKey(key)
			// });
			// if (num < 1)
			// {
			// 	Log.Debug(new object[]
			// 	{
			// 		"Inserting new key " + key + " with value " + val
			// 	});
			// 	this._connection.Execute("INSERT OR REPLACE INTO PersistentData (key, value) VALUES (?, ?)", new object[]
			// 	{
			// 		key,
			// 		val
			// 	});
			// }
			// else
			// {
			// 	Log.Debug(new object[]
			// 	{
			// 		"Updated existing key " + key + " with value " + val
			// 	});
			// }
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0008A14C File Offset: 0x0008854C
		private string TruncateFilePathKey(string key)
		{
			string result = key;
			try
			{
				string[] array = key.Split(new char[]
				{
					System.IO.Path.DirectorySeparatorChar
				});
				if (array.Length > 2)
				{
					result = System.IO.Path.Combine(array[array.Length - 2], array[array.Length - 1]);
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0008A1AC File Offset: 0x000885AC
		private string WildcardKey(string key)
		{
			return "%" + key;
		}

		// Token: 0x04004B8C RID: 19340
		// private SQLiteConnection _connection;
	}
}
