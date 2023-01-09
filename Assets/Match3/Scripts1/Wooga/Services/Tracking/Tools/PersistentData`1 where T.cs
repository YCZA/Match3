using System;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools
{
	// Token: 0x02000461 RID: 1121
	public abstract class PersistentData<T> where T : class, new()
	{
		// Token: 0x0600208B RID: 8331 RVA: 0x00089DFC File Offset: 0x000881FC
		protected PersistentData(string path)
		{
			this._path = path;
			this._data = Activator.CreateInstance<T>();
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x00089E16 File Offset: 0x00088216
		public T data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x0600208D RID: 8333
		public abstract void Load();

		// Token: 0x0600208E RID: 8334
		public abstract void Write();

		// Token: 0x04004B8A RID: 19338
		protected T _data;

		// Token: 0x04004B8B RID: 19339
		protected string _path;
	}
}
