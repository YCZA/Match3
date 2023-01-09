using System;

// Token: 0x0200085D RID: 2141
namespace Match3.Scripts1
{
	public static class EnumExtensions
	{
		// Token: 0x060034ED RID: 13549 RVA: 0x000FE26C File Offset: 0x000FC66C
		public static T EnumParse<T>(string value, bool ignorecase = true)
		{
			if (value.IsNullOrEmpty())
			{
				return default(T);
			}
			return (T)((object)Enum.Parse(typeof(T), value, ignorecase));
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000FE2A4 File Offset: 0x000FC6A4
		public static bool EnumTryParse<T>(string value, out T output, bool ignorecase = true)
		{
			if (value.IsNullOrEmpty())
			{
				output = default(T);
				return false;
			}
			bool result;
			try
			{
				output = (T)((object)Enum.Parse(typeof(T), value, ignorecase));
				result = true;
			}
			catch (ArgumentException)
			{
				output = default(T);
				result = false;
			}
			return result;
		}
	}
}
