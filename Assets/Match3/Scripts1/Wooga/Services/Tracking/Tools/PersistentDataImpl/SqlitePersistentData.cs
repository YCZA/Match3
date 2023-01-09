using System.IO;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.Tracking.Tools.Migration;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools.PersistentDataImpl
{
	// Token: 0x02000464 RID: 1124
	public static class SqlitePersistentData
	{
		// Token: 0x0600209C RID: 8348 RVA: 0x0008A1B9 File Offset: 0x000885B9
		public static string DBPath()
		{
			return (SqlitePersistentData.__db != null) ? SqlitePersistentData.__db.Path : SqlitePersistentData.DBPath(SqlitePersistentData.__dbName);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x0008A1E0 File Offset: 0x000885E0
		private static void InitializeDB()
		{
			if (SqlitePersistentData.__db != null)
			{
				return;
			}
			PersistentToApplicationDataMigration persistentToApplicationDataMigration = new PersistentToApplicationDataMigration(SqlitePersistentData.__dbName);
			persistentToApplicationDataMigration.MigrateIfRequired();
			string path = SqlitePersistentData.DBPath(SqlitePersistentData.__dbName);
			SqlitePersistentData.__db = new SqlitePersistentDataDB(path);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0008A21F File Offset: 0x0008861F
		public static void Close()
		{
			if (SqlitePersistentData.__db != null)
			{
				SqlitePersistentData.__db.Close();
				SqlitePersistentData.__db = null;
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x0008A23B File Offset: 0x0008863B
		// public static bool HasValue(string key)
		// {
			// SqlitePersistentData.InitializeDB();
			// return SqlitePersistentData.__db.HasValue(key);
		// }

		// Token: 0x060020A0 RID: 8352 RVA: 0x0008A24D File Offset: 0x0008864D
		// public static PersistentDataValue Get(string key)
		// {
			// SqlitePersistentData.InitializeDB();
			// return SqlitePersistentData.__db.Get(key);
		// }

		// Token: 0x060020A1 RID: 8353 RVA: 0x0008A25F File Offset: 0x0008865F
		public static void Set(string key, string val)
		{
			SqlitePersistentData.InitializeDB();
			SqlitePersistentData.__db.Set(key, val);
			IcloudBackup.Exclude(SqlitePersistentData.DBPath());
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x0008A27C File Offset: 0x0008867C
		private static string DBPath(string name)
		{
			if (!string.IsNullOrEmpty(Unity3D.Paths.ApplicationDataPath))
			{
				return Path.Combine(Unity3D.Paths.ApplicationDataPath, name);
			}
			return name;
		}

		// Token: 0x04004B8E RID: 19342
		private static string __dbName = "trk_pd.db";

		// Token: 0x04004B8F RID: 19343
		private static SqlitePersistentDataDB __db;
	}
}
