namespace Wooga.Coroutines
{
	// Token: 0x020003D3 RID: 979
	public static class PoolablExtensions
	{
		// Token: 0x06001DA8 RID: 7592 RVA: 0x0007F0B4 File Offset: 0x0007D4B4
		public static void ReleaseIfPoolable(this object obj)
		{
			IPoolable poolable = obj as IPoolable;
			if (poolable != null)
			{
				ObjectPool.Get().Release(poolable);
			}
		}
	}
}
