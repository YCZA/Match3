using System;
using System.IO;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools.Migration
{
	// Token: 0x0200045F RID: 1119
	internal class PersistentToApplicationDataMigration : Migration
	{
		// Token: 0x06002082 RID: 8322 RVA: 0x00089BB3 File Offset: 0x00087FB3
		public PersistentToApplicationDataMigration(string name)
		{
			this._name = name;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00089BC2 File Offset: 0x00087FC2
		public override string Purpose()
		{
			return "Move tracking persistent data to the application data path instead of the persistent data path due to data loss issues";
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00089BCC File Offset: 0x00087FCC
		public override bool Required()
		{
			bool flag = true;
			bool flag2 = File.Exists(this.OldPath()) && !File.Exists(this.NewPath());
			return flag && !this.OldPath().Equals(this.NewPath()) && flag2;
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00089C20 File Offset: 0x00088020
		public override void Migrate()
		{
			try
			{
				// SqlitePersistentDataDB sqlitePersistentDataDB = new SqlitePersistentDataDB(this.OldPath());
				// SqlitePersistentDataDB sqlitePersistentDataDB2 = new SqlitePersistentDataDB(this.NewPath());
				// // foreach (PersistentDataValue persistentDataValue in sqlitePersistentDataDB.GetAll())
				// // {
				// 	// sqlitePersistentDataDB2.Set(persistentDataValue.Key, persistentDataValue.Value);
				// // }
				// sqlitePersistentDataDB2.Close();
				// if (File.Exists(this.OldPath()))
				// {
				// 	Log.Warning(new object[]
				// 	{
				// 		"Old db still exists at " + this.OldPath() + " - explicitly deleting"
				// 	});
				// 	File.Delete(this.OldPath());
				// }
				// Log.Warning(new object[]
				// {
				// 	"Migrated DB from " + this.OldPath() + " to " + this.NewPath()
				// });
			}
			catch (Exception ex)
			{
				Log.Error(new object[]
				{
					"Couldn't migrate tracking data to app data dir - " + ex.Message
				});
			}
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00089D44 File Offset: 0x00088144
		private string OldPath()
		{
			if (!string.IsNullOrEmpty(Unity3D.Paths.PersistentDataPath))
			{
				return Path.Combine(Unity3D.Paths.PersistentDataPath, this._name);
			}
			return this._name;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x00089D6C File Offset: 0x0008816C
		private string NewPath()
		{
			if (!string.IsNullOrEmpty(Unity3D.Paths.ApplicationDataPath))
			{
				return Path.Combine(Unity3D.Paths.ApplicationDataPath, this._name);
			}
			return this._name;
		}

		// Token: 0x04004B89 RID: 19337
		private readonly string _name;
	}
}
