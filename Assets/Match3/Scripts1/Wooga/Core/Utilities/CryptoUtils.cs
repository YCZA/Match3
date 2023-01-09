using System.Security.Cryptography;
using System.Text;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003A5 RID: 933
	public static class CryptoUtils
	{
		// Token: 0x06001C21 RID: 7201 RVA: 0x0007BE2C File Offset: 0x0007A22C
		public static int HexCharToHexInt(char c)
		{
			if ('0' <= c && c <= '9')
			{
				return (int)(c - '0');
			}
			if ('a' <= c && c <= 'f')
			{
				return (int)(c - 'a' + '\n');
			}
			if ('A' <= c && c <= 'F')
			{
				return (int)(c - 'A' + '\n');
			}
			return -1;
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0007BE7F File Offset: 0x0007A27F
		public static char HexIntToHexChar(int i)
		{
			return (i >= 10) ? ((char)(i - 10 + 97)) : ((char)(i + 48));
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0007BE9C File Offset: 0x0007A29C
		public static byte[] GetMD5Bytes(byte[] bytes)
		{
			byte[] result;
			using (MD5 md = MD5.Create())
			{
				result = md.ComputeHash(bytes);
			}
			return result;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0007BEDC File Offset: 0x0007A2DC
		public static byte[] GetMD5Bytes(string input)
		{
			return CryptoUtils.GetMD5Bytes(Encoding.UTF8.GetBytes(input));
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0007BEEE File Offset: 0x0007A2EE
		public static string GetMD5String(string input)
		{
			return CryptoUtils.HashToString(CryptoUtils.GetMD5Bytes(input));
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x0007BEFB File Offset: 0x0007A2FB
		public static string GetMD5String(byte[] bytes)
		{
			return CryptoUtils.HashToString(CryptoUtils.GetMD5Bytes(bytes));
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0007BF08 File Offset: 0x0007A308
		public static char[] GetMD5Chars(string input)
		{
			return CryptoUtils.GetMD5String(input).ToCharArray();
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0007BF15 File Offset: 0x0007A315
		public static bool VerifyMd5Hash(string input, string hash)
		{
			return CryptoUtils.VerifyMd5Hash(Encoding.UTF8.GetBytes(input), hash);
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0007BF28 File Offset: 0x0007A328
		public static bool VerifyMd5Hash(byte[] bytes, string hash)
		{
			bool result;
			using (MD5 md = MD5.Create())
			{
				result = (CryptoUtils.HashToString(md.ComputeHash(bytes)) == hash);
			}
			return result;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x0007BF74 File Offset: 0x0007A374
		public static string HashToString(byte[] value)
		{
			int num = value.Length;
			if (num == 0)
			{
				return string.Empty;
			}
			int num2 = num * 2;
			char[] array = new char[num2];
			int num3 = 0;
			for (int i = 0; i < num2; i += 2)
			{
				byte b = value[num3++];
				array[i] = CryptoUtils.HexIntToHexChar((int)(b / 16));
				array[i + 1] = CryptoUtils.HexIntToHexChar((int)(b % 16));
			}
			return new string(array);
		}

		// Token: 0x04004986 RID: 18822
		public const int MD5HashLength = 32;
	}
}
