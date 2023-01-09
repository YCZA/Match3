namespace Wooga.Core.Extensions
{
	// Token: 0x0200035E RID: 862
	public static class AssertExtensions
	{
		// Token: 0x06001A08 RID: 6664 RVA: 0x00074FBE File Offset: 0x000733BE
		public static bool IsNotNull(this object obj)
		{
			if (obj is string)
			{
				return !string.IsNullOrEmpty(obj as string);
			}
			return obj != null;
		}
	}
}
