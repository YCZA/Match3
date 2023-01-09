namespace Shared.Pooling
{
	// Token: 0x02000B15 RID: 2837
	public static class IPoolabeExtensions
	{
		// Token: 0x060042C8 RID: 17096 RVA: 0x0015617A File Offset: 0x0015457A
		public static void ReturnToPool<T>(this T poolable) where T : IPoolable, new()
		{
			Pool<T>.ReturnToPool(poolable);
		}
	}
}
