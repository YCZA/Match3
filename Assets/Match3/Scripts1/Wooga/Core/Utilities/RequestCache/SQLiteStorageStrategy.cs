using System;
using System.IO;
using Match3.Scripts1.Wooga.Core.ThreadSafe; // using Wooga.Core.Sqlite;
using Wooga.Core.Extensions;

namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003AF RID: 943
	public class SQLiteStorageStrategy : IStorageStrategy
	{
		// Token: 0x06001C65 RID: 7269 RVA: 0x0007C750 File Offset: 0x0007AB50
		public SQLiteStorageStrategy(string dbName)
		{
			try
			{
				this.OpenDatabase(dbName);
			}
			catch
			{
				File.Delete(this.DBPath(dbName));
				this.OpenDatabase(dbName);
			}
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x0007C798 File Offset: 0x0007AB98
		public void Store(string path, string data, string etag)
		{
			this.RemoveValue(path);
			RequestData obj = new RequestData
			{
				Etag = etag,
				Resource = data,
				Path = path,
				LastUsed = this.UnixTimeNow()
			};
			// this._db.Insert(obj);
			this.EnforceLimit();
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0007C7E8 File Offset: 0x0007ABE8
		public bool HasValue(string path)
		{
			return this.RequestForPath(path).IsNotNull();
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x0007C7F8 File Offset: 0x0007ABF8
		public string GetValue(string path)
		{
			RequestData requestData = this.RequestForPath(path);
			if (requestData != null)
			{
				this.TouchRequest(requestData);
				return requestData.Resource;
			}
			return null;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0007C822 File Offset: 0x0007AC22
		public void SetEtagForPath(string path, string eTag)
		{
			// this._db.Query<RequestData>("UPDATE Requests SET Etag = ?, LastUsed = ? WHERE Path = ?", new object[]
			// {
				// eTag,
				// this.UnixTimeNow(),
				// path
			// });
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0007C854 File Offset: 0x0007AC54
		public string GetEtagForPath(string path)
		{
			// RequestData requestData = this._db.Query<RequestData>("SELECT Etag FROM Requests WHERE Path = ?", new object[]
			// {
				// path
			// }).FirstOrDefault<RequestData>();
			// if (requestData != null)
			// {
				// return requestData.Etag;
			// }
			return null;
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x0007C88F File Offset: 0x0007AC8F
		public void Clear()
		{
			// this._db.Execute("DELETE FROM Requests", new object[0]);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0007C8A8 File Offset: 0x0007ACA8
		public void Close()
		{
			// this._db.Close();
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x0007C8B5 File Offset: 0x0007ACB5
		public void RemoveValue(string path)
		{
			// this._db.Query<RequestData>("DELETE FROM Requests WHERE Path = ?", new object[]
			// {
				// path
			// });
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0007C8D2 File Offset: 0x0007ACD2
		private void TouchRequest(RequestData request)
		{
			request.LastUsed = this.UnixTimeNow();
			// this._db.Update(request);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x0007C8F0 File Offset: 0x0007ACF0
		private long UnixTimeNow()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x0007C920 File Offset: 0x0007AD20
		private void EnforceLimit()
		{
			Stats stats = this.GetStats();
			while (stats.TotalSize > (long)SQLiteStorageStrategy.MAX_REQUESTCACHE_SIZE_BYTES)
			{
				int num = this.PurgeLeastUsedRequest();
				stats.TotalSize -= (long)num;
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0007C960 File Offset: 0x0007AD60
		private Stats GetStats()
		{
			// return this._db.Query<Stats>("SELECT SUM(LENGTH(Resource)) as TotalSize FROM Requests", new object[0]).First<Stats>();
			return null;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x0007C980 File Offset: 0x0007AD80
		private int PurgeLeastUsedRequest()
		{
			// RequestData requestData = this._db.Query<RequestData>("SELECT * FROM Requests ORDER BY LastUsed ASC LIMIT 1", new object[0]).First<RequestData>();
			// this._db.Delete(requestData);
			// return requestData.Resource.Length;
			return 0;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x0007C9C1 File Offset: 0x0007ADC1
		private RequestData RequestForPath(string path)
		{
			// return this._db.Query<RequestData>("SELECT * FROM Requests WHERE Path = ?", new object[]
			// {
				// path
			// }).FirstOrDefault<RequestData>();
			return new RequestData();
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x0007C9E4 File Offset: 0x0007ADE4
		private void OpenDatabase(string dbName)
		{
			// string query = "create table if not exists \n            \"Requests\"(\"Resource\" BLOB ,\n                         \"Etag\" TEXT ,\n                         \"Path\" TEXT UNIQUE,\n                          \"LastUsed\" INTEGER)";
			// this._db = new SQLiteConnection(this.DBPath(dbName), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, false);
			// this._db.Execute(query, new object[0]);
			// IcloudBackup.Exclude(this.DBPath(dbName));
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0007CA2E File Offset: 0x0007AE2E
		public string DBPath(string name)
		{
			if (!string.IsNullOrEmpty(Unity3D.Paths.PersistentDataPath))
			{
				return Path.Combine(Unity3D.Paths.PersistentDataPath, name);
			}
			return name;
		}

		// Token: 0x04004997 RID: 18839
		// private SQLiteConnection _db;

		// Token: 0x04004998 RID: 18840
		public static int MAX_REQUESTCACHE_SIZE_BYTES = 20000000;
	}
}
