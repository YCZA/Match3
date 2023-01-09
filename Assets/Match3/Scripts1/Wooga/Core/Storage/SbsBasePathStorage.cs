using System.IO;
using Match3.Scripts1.Wooga.Core.Data;

namespace Wooga.Core.Storage
{
	// Token: 0x02000394 RID: 916
	public class SbsBasePathStorage : ISbsStorage
	{
		// Token: 0x06001BCF RID: 7119 RVA: 0x0007AE81 File Offset: 0x00079281
		public SbsBasePathStorage(string basePath, ISbsStorage storage)
		{
			this._basePath = basePath;
			this._storage = storage;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0007AE97 File Offset: 0x00079297
		public SbsRichData Load(string filename)
		{
			return this._storage.Load(this.GetFullPath(filename));
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0007AEAB File Offset: 0x000792AB
		public void Save(string filename, string data, int formatVersion)
		{
			this._storage.Save(this.GetFullPath(filename), data, formatVersion);
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0007AEC1 File Offset: 0x000792C1
		public void Save(string filename, SbsRichData data)
		{
			this._storage.Save(this.GetFullPath(filename), data);
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0007AED6 File Offset: 0x000792D6
		public void Delete(string filename)
		{
			this._storage.Delete(this.GetFullPath(filename));
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0007AEEA File Offset: 0x000792EA
		public bool Exists(string name)
		{
			return this._storage.Exists(this.GetFullPath(name));
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0007AEFE File Offset: 0x000792FE
		private string GetFullPath(string filename)
		{
			return Path.Combine(this._basePath, filename);
		}

		// Token: 0x0400496E RID: 18798
		private readonly string _basePath;

		// Token: 0x0400496F RID: 18799
		private readonly ISbsStorage _storage;
	}
}
