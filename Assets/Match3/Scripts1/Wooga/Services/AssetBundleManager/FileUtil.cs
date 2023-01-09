using System;
using System.IO;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000322 RID: 802
	public class FileUtil
	{
		// Token: 0x060018FF RID: 6399 RVA: 0x0007127C File Offset: 0x0006F67C
		public static bool ClearDirectory(string directoryPath)
		{
			bool flag = true;
			if (Directory.Exists(directoryPath))
			{
				string[] files = Directory.GetFiles(directoryPath);
				foreach (string filePath in files)
				{
					flag &= FileUtil.DeleteFile(filePath);
				}
				string[] directories = Directory.GetDirectories(directoryPath);
				foreach (string directoryPath2 in directories)
				{
					flag &= FileUtil.DeleteDirectory(directoryPath2);
				}
			}
			return flag;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x000712FC File Offset: 0x0006F6FC
		public static bool WriteAllBytes(string filePath, byte[] bytes)
		{
			bool result;
			try
			{
				Directory.CreateDirectory(Directory.GetParent(filePath).FullName);
				File.WriteAllBytes(filePath, bytes);
				result = true;
			}
			catch (Exception ex)
			{
				Log.ErrorFormatted("Error writing file: {0}. {1}", new object[]
				{
					filePath,
					ex
				});
				result = false;
			}
			return result;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0007135C File Offset: 0x0006F75C
		public static bool DeleteDirectory(string directoryPath)
		{
			bool result;
			try
			{
				Directory.Delete(directoryPath, true);
				result = true;
			}
			catch (Exception ex)
			{
				Log.ErrorFormatted("Error deleting directory: {0}", new object[]
				{
					ex
				});
				result = false;
			}
			return result;
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x000713A4 File Offset: 0x0006F7A4
		public static bool DeleteFile(string filePath)
		{
			bool result;
			try
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
				result = true;
			}
			catch (Exception ex)
			{
				Log.ErrorFormatted("Error deleting file: {0}", new object[]
				{
					ex
				});
				result = false;
			}
			return result;
		}
	}
}
