using Match3.Scripts1.Wooga.Services.Tracking.Tools.PersistentDataImpl;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools
{
	// Token: 0x02000460 RID: 1120
	public static class PersistentData
	{
		// Token: 0x06002088 RID: 8328 RVA: 0x00089D94 File Offset: 0x00088194
		public static PersistentData<T> Create<T>(string path) where T : class, new()
		{
			SqlitePersistentData<T> sqlitePersistentData = new SqlitePersistentData<T>(path);
			PersistentData.AtomicFileToSqlMigration<T>(path, sqlitePersistentData);
			return sqlitePersistentData;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00089DB0 File Offset: 0x000881B0
		private static void AtomicFileToSqlMigration<T>(string path, SqlitePersistentData<T> sqlitePersistentData) where T : class, new()
		{
			// if (!SqlitePersistentData.HasValue(path) && File.Exists(path))
			// {
				// PersistentData.MigrateToSql<T>(path, sqlitePersistentData);
			// }
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00089DD0 File Offset: 0x000881D0
		private static void MigrateToSql<T>(string path, SqlitePersistentData<T> result) where T : class, new()
		{
			AtomicFilePersistentData<T> atomicFilePersistentData = new AtomicFilePersistentData<T>(path);
			atomicFilePersistentData.Load();
			result.setData(atomicFilePersistentData.data);
			result.Write();
		}
	}
}
