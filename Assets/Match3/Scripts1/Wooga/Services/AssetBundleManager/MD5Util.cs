using System.Security.Cryptography;
using System.Text;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000323 RID: 803
	public static class MD5Util
	{
		// Token: 0x06001903 RID: 6403 RVA: 0x000713F8 File Offset: 0x0006F7F8
		public static string GetMd5Hash(string str)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				result = MD5Util.HashToString(md.ComputeHash(Encoding.UTF8.GetBytes(str)));
			}
			return result;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00071448 File Offset: 0x0006F848
		public static string GetMd5Hash(byte[] bytes)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				result = MD5Util.HashToString(md.ComputeHash(bytes));
			}
			return result;
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0007148C File Offset: 0x0006F88C
		public static bool VerifyMd5Hash(string input, string hash)
		{
			return MD5Util.VerifyMd5Hash(Encoding.UTF8.GetBytes(input), hash);
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x000714A0 File Offset: 0x0006F8A0
		public static bool VerifyMd5Hash(byte[] bytes, string hash)
		{
			bool result;
			using (MD5 md = MD5.Create())
			{
				result = (MD5Util.HashToString(md.ComputeHash(bytes)) == hash);
			}
			return result;
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x000714EC File Offset: 0x0006F8EC
		public static string HashToString(byte[] value)
		{
			return MD5Util.HashToString(value, 0, value.Length);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x000714F8 File Offset: 0x0006F8F8
		private static char GetHexValue(int i)
		{
			return (i >= 10) ? ((char)(i - 10 + 97)) : ((char)(i + 48));
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00071514 File Offset: 0x0006F914
		private static string HashToString(byte[] value, int startIndex, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length * 2;
			char[] array = new char[num];
			int num2 = startIndex;
			for (int i = 0; i < num; i += 2)
			{
				byte b = value[num2++];
				array[i] = MD5Util.GetHexValue((int)(b / 16));
				array[i + 1] = MD5Util.GetHexValue((int)(b % 16));
			}
			return new string(array);
		}
	}
}
