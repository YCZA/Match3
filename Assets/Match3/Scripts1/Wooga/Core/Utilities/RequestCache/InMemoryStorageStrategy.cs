using System.Collections.Generic;

namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003B3 RID: 947
	public class InMemoryStorageStrategy : IStorageStrategy
	{
		// Token: 0x06001C8A RID: 7306 RVA: 0x0007CABD File Offset: 0x0007AEBD
		public InMemoryStorageStrategy()
		{
			this._storage = new Dictionary<string, KeyValuePair<string, string>>();
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0007CAD0 File Offset: 0x0007AED0
		public void Store(string path, string data, string etag)
		{
			this._storage[path] = new KeyValuePair<string, string>(etag, data);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0007CAE5 File Offset: 0x0007AEE5
		public bool HasValue(string path)
		{
			return this._storage.ContainsKey(path);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0007CAF3 File Offset: 0x0007AEF3
		public void RemoveValue(string path)
		{
			this._storage.Remove(path);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0007CB04 File Offset: 0x0007AF04
		public string GetValue(string path)
		{
			return (!this.HasValue(path)) ? null : this._storage[path].Value;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0007CB38 File Offset: 0x0007AF38
		public void SetEtagForPath(string path, string eTag)
		{
			if (this.HasValue(path))
			{
				this._storage[path] = new KeyValuePair<string, string>(eTag, this._storage[path].Value);
			}
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0007CB78 File Offset: 0x0007AF78
		public string GetEtagForPath(string path)
		{
			return (!this.HasValue(path)) ? null : this._storage[path].Key;
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0007CBAB File Offset: 0x0007AFAB
		public void Clear()
		{
			this._storage = new Dictionary<string, KeyValuePair<string, string>>();
		}

		// Token: 0x0400499E RID: 18846
		private Dictionary<string, KeyValuePair<string, string>> _storage;
	}
}
