using Match3.Scripts1.Wooga.Core.Data;

namespace Wooga.Core.Storage
{
	// Token: 0x02000398 RID: 920
	public class SbsMigratingStorage : ISbsStorage
	{
		// Token: 0x06001BE8 RID: 7144 RVA: 0x0007B506 File Offset: 0x00079906
		public SbsMigratingStorage(ISbsStorage storage, ISbsStorage legacyStorage)
		{
			this._storage = storage;
			this._legacyStorage = legacyStorage;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0007B51C File Offset: 0x0007991C
		public SbsRichData Load(string name)
		{
			if (this._storage.Exists(name))
			{
				return this._storage.Load(name);
			}
			if (this._legacyStorage.Exists(name))
			{
				this._storage.Save(name, this._legacyStorage.Load(name));
				this._legacyStorage.Delete(name);
			}
			return this._storage.Load(name);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0007B588 File Offset: 0x00079988
		public void Save(string name, string contents, int format_version)
		{
			this._storage.Save(name, contents, format_version);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0007B598 File Offset: 0x00079998
		public void Save(string name, SbsRichData data)
		{
			this._storage.Save(name, data);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0007B5A7 File Offset: 0x000799A7
		public void Delete(string name)
		{
			this._storage.Delete(name);
			this._legacyStorage.Delete(name);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0007B5C1 File Offset: 0x000799C1
		public bool Exists(string name)
		{
			return this._storage.Exists(name) || this._legacyStorage.Exists(name);
		}

		// Token: 0x04004976 RID: 18806
		private readonly ISbsStorage _storage;

		// Token: 0x04004977 RID: 18807
		private readonly ISbsStorage _legacyStorage;
	}
}
