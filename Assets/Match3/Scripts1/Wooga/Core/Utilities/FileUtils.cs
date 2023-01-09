using System;
using System.IO;
using Match3.Scripts1;
using Match3.Scripts1.Wooga.Core.IO;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003A6 RID: 934
	public class FileUtils
	{
		// Token: 0x06001C2C RID: 7212 RVA: 0x0007BFEC File Offset: 0x0007A3EC
		public static void Touch(string fileName)
		{
			try
			{
				if (!File.Exists(fileName))
				{
					FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
					fileStream.Close();
					fileStream.Dispose();
				}
				File.SetLastWriteTimeUtc(fileName, DateTime.UtcNow);
			}
			catch
			{
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0007C040 File Offset: 0x0007A440
		public static DateTime GetLastWriteUtcTime(string fileName)
		{
			DateTime result;
			try
			{
				result = ((!File.Exists(fileName)) ? DateTime.MinValue : File.GetLastWriteTimeUtc(fileName));
			}
			catch
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0007C08C File Offset: 0x0007A48C
		[Obsolete("Use DeleteFile")]
		public static void SafeDeleteFile(string filePath)
		{
			FileUtils.DeleteFile(filePath);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0007C098 File Offset: 0x0007A498
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

		// Token: 0x06001C30 RID: 7216 RVA: 0x0007C0EC File Offset: 0x0007A4EC
		public static bool ClearDirectory(string directoryPath)
		{
			bool flag = true;
			if (Directory.Exists(directoryPath))
			{
				string[] files = Directory.GetFiles(directoryPath);
				foreach (string filePath in files)
				{
					flag &= FileUtils.DeleteFile(filePath);
				}
				string[] directories = Directory.GetDirectories(directoryPath);
				foreach (string directoryPath2 in directories)
				{
					flag &= FileUtils.DeleteDirectory(directoryPath2);
				}
			}
			return flag;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0007C16C File Offset: 0x0007A56C
		public static bool WriteAllBytes(string filePath, byte[] bytes)
		{
			bool result;
			try
			{
				AtomicFile.WriteAllBytesAtomic(filePath, bytes);
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

		// Token: 0x06001C32 RID: 7218 RVA: 0x0007C1B8 File Offset: 0x0007A5B8
		public static OptionalResult<byte[]> ReadAllBytes(string filePath)
		{
			return (!AtomicFile.ExistsAndRecoverFromAtomicWrite(filePath)) ? OptionalResult<byte[]>.None : File.ReadAllBytes(filePath);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0007C1DC File Offset: 0x0007A5DC
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
	}
}
