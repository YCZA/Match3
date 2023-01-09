namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003B2 RID: 946
	public interface IStorageStrategy
	{
		// Token: 0x06001C83 RID: 7299
		void Store(string path, string data, string etag);

		// Token: 0x06001C84 RID: 7300
		bool HasValue(string path);

		// Token: 0x06001C85 RID: 7301
		void RemoveValue(string path);

		// Token: 0x06001C86 RID: 7302
		string GetValue(string path);

		// Token: 0x06001C87 RID: 7303
		void SetEtagForPath(string path, string eTag);

		// Token: 0x06001C88 RID: 7304
		string GetEtagForPath(string path);

		// Token: 0x06001C89 RID: 7305
		void Clear();
	}
}
