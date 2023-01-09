namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C03 RID: 3075
	public abstract class SimpleMenu<T> : Menu<T> where T : SimpleMenu<T>
	{
		// Token: 0x06004857 RID: 18519 RVA: 0x001713A7 File Offset: 0x0016F7A7
		public static void Show()
		{
			Menu<T>.Open();
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x001713AE File Offset: 0x0016F7AE
		public static void Hide()
		{
			Menu<T>.Close();
		}
	}
}
