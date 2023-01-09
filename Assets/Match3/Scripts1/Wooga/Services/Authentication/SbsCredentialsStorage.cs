using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Core.Storage;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.Authentication
{
	// Token: 0x020003BD RID: 957
	public class SbsCredentialsStorage
	{
		// Token: 0x06001CE2 RID: 7394 RVA: 0x0007D85B File Offset: 0x0007BC5B
		public SbsCredentialsStorage(ISbsStorage fileStorage) : this(fileStorage, new SbsPlayerPrefsStorage("Wooga.Services.Authentication.SbsCredentialsStorage"))
		{
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0007D86E File Offset: 0x0007BC6E
		public SbsCredentialsStorage(ISbsStorage fileStorage, ISbsStorage backupStorage)
		{
			this._fileStorage = fileStorage;
			this._backupStorage = backupStorage;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x0007D884 File Offset: 0x0007BC84
		public SbsCredentials LoadCredentials()
		{
			SbsRichData sbsRichData = this.LoadFromFileStorage();
			if (sbsRichData == null)
			{
				sbsRichData = this.RestoreFileStorageFromBackup();
			}
			return (sbsRichData == null) ? null : JSON.Deserialize<SbsCredentials>(sbsRichData.Data);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0007D8BC File Offset: 0x0007BCBC
		public void SaveCredentials(SbsCredentials credentials)
		{
			if (credentials != null)
			{
				string contents = JSON.Serialize(credentials, false, 1, ' ');
				this.SaveToFileStorage(contents);
				this.SaveToBackupStorage(contents);
			}
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0007D8E8 File Offset: 0x0007BCE8
		public void ClearCredentials()
		{
			this._fileStorage.Delete("Wooga.Sbs.Credentials.json");
			this._backupStorage.Delete("Wooga.Sbs.Credentials.json");
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x0007D90C File Offset: 0x0007BD0C
		private SbsRichData LoadFromFileStorage()
		{
			SbsRichData sbsRichData = this._fileStorage.Load("Wooga.Sbs.Credentials.json");
			if (sbsRichData != null && !this._backupStorage.Exists("Wooga.Sbs.Credentials.json"))
			{
				this.SaveToBackupStorage(sbsRichData.Data);
			}
			return sbsRichData;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0007D954 File Offset: 0x0007BD54
		private SbsRichData RestoreFileStorageFromBackup()
		{
			SbsRichData sbsRichData = this._backupStorage.Load("Wooga.Sbs.Credentials.json");
			if (sbsRichData != null && !string.IsNullOrEmpty(sbsRichData.Data))
			{
				this.SaveToFileStorage(sbsRichData.Data);
			}
			return sbsRichData;
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0007D995 File Offset: 0x0007BD95
		private void SaveToFileStorage(string contents)
		{
			this._fileStorage.Save("Wooga.Sbs.Credentials.json", contents, -1);
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x0007D9A9 File Offset: 0x0007BDA9
		private void SaveToBackupStorage(string contents)
		{
			this._backupStorage.Save("Wooga.Sbs.Credentials.json", contents, -1);
		}

		// Token: 0x040049B0 RID: 18864
		public const string STORAGE_NAME = "Wooga.Sbs.Credentials.json";

		// Token: 0x040049B1 RID: 18865
		private readonly ISbsStorage _fileStorage;

		// Token: 0x040049B2 RID: 18866
		private readonly ISbsStorage _backupStorage;
	}
}
