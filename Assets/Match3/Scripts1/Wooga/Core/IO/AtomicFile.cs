using System;
using System.IO;

namespace Match3.Scripts1.Wooga.Core.IO
{
	// Token: 0x02000360 RID: 864
	public static class AtomicFile
	{
		// Token: 0x06001A0C RID: 6668 RVA: 0x0007503C File Offset: 0x0007343C
		public static void WriteAllTextAtomic(string fullPath, string text)
		{
			AtomicFile.WriteDataAtomic(fullPath, delegate(string newPath)
			{
				File.WriteAllText(newPath, text);
			});
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00075068 File Offset: 0x00073468
		public static void WriteAllBytesAtomic(string fullPath, byte[] data)
		{
			AtomicFile.WriteDataAtomic(fullPath, delegate(string newPath)
			{
				File.WriteAllBytes(newPath, data);
			});
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x00075094 File Offset: 0x00073494
		public static void WriteDataAtomic(string fullPath, Action<string> writeDataCallback)
		{
			AtomicFile.AssertLegalPath(fullPath);
			AtomicFile.RevertFailedSaveIfNeeded(fullPath);
			AtomicFile.CreateDirectoryForFile(fullPath);
			string newFilePath = AtomicFile.GetNewFilePath(fullPath);
			string tempFilePath = AtomicFile.GetTempFilePath(fullPath);
			writeDataCallback(newFilePath);
			if (File.Exists(fullPath))
			{
				File.Move(fullPath, tempFilePath);
			}
			File.Move(newFilePath, fullPath);
			if (File.Exists(tempFilePath))
			{
				File.Delete(tempFilePath);
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000750F2 File Offset: 0x000734F2
		public static bool ExistsAndRecoverFromAtomicWrite(string path)
		{
			AtomicFile.RevertFailedSaveIfNeeded(path);
			return File.Exists(path);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00075100 File Offset: 0x00073500
		private static void RevertFailedSaveIfNeeded(string originalPath)
		{
			string newFilePath = AtomicFile.GetNewFilePath(originalPath);
			string tempFilePath = AtomicFile.GetTempFilePath(originalPath);
			bool flag = File.Exists(originalPath);
			bool flag2 = File.Exists(newFilePath);
			bool flag3 = File.Exists(tempFilePath);
			if (!flag && flag3 && flag2)
			{
				File.Move(tempFilePath, originalPath);
				File.Delete(newFilePath);
			}
			else if (flag && flag3 && !flag2)
			{
				File.Delete(tempFilePath);
			}
			else if (flag2)
			{
				File.Delete(newFilePath);
			}
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00075180 File Offset: 0x00073580
		private static void AssertLegalPath(string fullPath)
		{
			if (fullPath.EndsWith("._tt") || fullPath.EndsWith("._nn"))
			{
				throw new ArgumentException("The saved file cannot end in '._tt' or '._nn'");
			}
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000751B0 File Offset: 0x000735B0
		private static void CreateDirectoryForFile(string fullPath)
		{
			string directoryName = Path.GetDirectoryName(fullPath);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000751E4 File Offset: 0x000735E4
		private static string GetTempFilePath(string fullPath)
		{
			return fullPath + "._tt";
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00075200 File Offset: 0x00073600
		private static string GetNewFilePath(string fullPath)
		{
			return fullPath + "._nn";
		}

		// Token: 0x0400485D RID: 18525
		public const string TEMP_FILE_EXTENSION = "._tt";

		// Token: 0x0400485E RID: 18526
		public const string NEW_FILE_EXTENSION = "._nn";
	}
}
