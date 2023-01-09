using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Match3.Scripts1;
using Match3.Scripts1.Wooga.Core.IO;
using Wooga.Core.Utilities;

namespace Wooga.Core.Cache
{
	// Token: 0x02000343 RID: 835
	public class FileCache
	{
		// Token: 0x06001996 RID: 6550 RVA: 0x00073C69 File Offset: 0x00072069
		public FileCache(string cacheDirectory, int maxNumberOfFiles = 100, long maxSizeInBytes = -1L)
		{
			if (maxNumberOfFiles > 1000)
			{
				throw new Exception("Cache capping currently does support large caches efficiently");
			}
			this._cacheDirectory = cacheDirectory;
			this._maxNumberOfFiles = maxNumberOfFiles;
			this._maxSizeInBytes = maxSizeInBytes;
			this.Cleanup();
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00073CA4 File Offset: 0x000720A4
		public OptionalResult<byte[]> ReadBytes(string path, string md5Hash = null)
		{
			string filePath = this.CreateFilePath(path);
			OptionalResult<byte[]> result;
			try
			{
				OptionalResult<byte[]> optionalResult = FileUtils.ReadAllBytes(filePath);
				if (optionalResult.HasValue)
				{
					if (string.IsNullOrEmpty(md5Hash))
					{
						result = optionalResult;
					}
					else if (CryptoUtils.VerifyMd5Hash(optionalResult.Value, md5Hash))
					{
						result = optionalResult;
					}
					else
					{
						FileUtils.DeleteFile(filePath);
						result = OptionalResult<byte[]>.None;
					}
				}
				else
				{
					result = optionalResult;
				}
			}
			catch (Exception ex)
			{
				Log.ErrorFormatted("Error readin file: {0} {1}", new object[]
				{
					path,
					ex
				});
				result = OptionalResult<byte[]>.None;
			}
			return result;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00073D48 File Offset: 0x00072148
		public OptionalResult<string> ReadText(string path, string md5Hash = null)
		{
			OptionalResult<byte[]> optionalResult = this.ReadBytes(path, md5Hash);
			return (!optionalResult.HasValue) ? OptionalResult<string>.None : Encoding.UTF8.GetString(optionalResult.Value);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00073D8C File Offset: 0x0007218C
		public bool Write(string path, byte[] content)
		{
			string filePath = this.CreateFilePath(path);
			return FileUtils.WriteAllBytes(filePath, content);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00073DB0 File Offset: 0x000721B0
		public bool Write(string path, string content)
		{
			return this.Write(path, Encoding.UTF8.GetBytes(content));
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00073DC4 File Offset: 0x000721C4
		public bool IsCached(string path, string md5Hash = null)
		{
			string text = this.CreateFilePath(path);
			if (string.IsNullOrEmpty(md5Hash))
			{
				return AtomicFile.ExistsAndRecoverFromAtomicWrite(text);
			}
			OptionalResult<byte[]> optionalResult = FileUtils.ReadAllBytes(text);
			return optionalResult.HasValue && CryptoUtils.VerifyMd5Hash(optionalResult.Value, md5Hash);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00073E10 File Offset: 0x00072210
		public void Delete(string path)
		{
			string text = this.CreateFilePath(path);
			if (AtomicFile.ExistsAndRecoverFromAtomicWrite(text))
			{
				FileUtils.DeleteFile(text);
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00073E37 File Offset: 0x00072237
		private string CreateFilePath(string path)
		{
			path = path.Replace("\\", "/");
			while (path[0] == '/')
			{
				path = path.Substring(1);
			}
			return Path.Combine(this._cacheDirectory, path);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00073E74 File Offset: 0x00072274
		public void Cleanup()
		{
			if (!Directory.Exists(this._cacheDirectory))
			{
				return;
			}
			FileCache.CacheFileInfo[] array = null;
			int i = 0;
			if (this._maxSizeInBytes > 0L)
			{
				array = this.GetDirectoryListing();
				i = array.Length - 1;
				long num = 0L;
				long[] array2 = new long[array.Length];
				for (int j = 0; j < array.Length; j++)
				{
					long length = new FileInfo(array[j].Path).Length;
					array2[j] = length;
					num += length;
				}
				while (i >= 0 && num > this._maxSizeInBytes)
				{
					FileUtils.DeleteFile(array[i].Path);
					num -= array2[i];
					i--;
				}
			}
			if (this._maxNumberOfFiles > 0)
			{
				if (array == null)
				{
					array = this.GetDirectoryListing();
					Array.Sort<FileCache.CacheFileInfo>(array, FileCache.ReversedByAccessTimeComparer.Instance);
					i = array.Length - 1;
				}
				while (i >= this._maxNumberOfFiles)
				{
					FileUtils.DeleteFile(array[i].Path);
					i--;
				}
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00073F80 File Offset: 0x00072380
		private FileCache.CacheFileInfo[] GetDirectoryListing()
		{
			string[] files = Directory.GetFiles(this._cacheDirectory, "*", SearchOption.AllDirectories);
			FileCache.CacheFileInfo[] array = new FileCache.CacheFileInfo[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				array[i] = new FileCache.CacheFileInfo
				{
					Path = files[i],
					Ticks = Math.Max(File.GetLastAccessTime(files[i]).Ticks, File.GetCreationTime(files[i]).Ticks)
				};
			}
			Array.Sort<FileCache.CacheFileInfo>(array, FileCache.ReversedByAccessTimeComparer.Instance);
			return array;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00074015 File Offset: 0x00072415
		public bool Clear()
		{
			return FileUtils.ClearDirectory(this._cacheDirectory);
		}

		// Token: 0x04004835 RID: 18485
		private readonly string _cacheDirectory;

		// Token: 0x04004836 RID: 18486
		private readonly int _maxNumberOfFiles;

		// Token: 0x04004837 RID: 18487
		private readonly long _maxSizeInBytes;

		// Token: 0x02000344 RID: 836
		public struct CacheFileInfo
		{
			// Token: 0x04004838 RID: 18488
			public string Path;

			// Token: 0x04004839 RID: 18489
			public long Ticks;
		}

		// Token: 0x02000345 RID: 837
		public class ReversedByAccessTimeComparer : IComparer<FileCache.CacheFileInfo>
		{
			// Token: 0x060019A1 RID: 6561 RVA: 0x00074022 File Offset: 0x00072422
			private ReversedByAccessTimeComparer()
			{
			}

			// Token: 0x060019A2 RID: 6562 RVA: 0x0007402C File Offset: 0x0007242C
			public int Compare(FileCache.CacheFileInfo x, FileCache.CacheFileInfo y)
			{
				int num = y.Ticks.CompareTo(x.Ticks);
				return (num != 0) ? num : string.Compare(y.Path, x.Path, StringComparison.Ordinal);
			}

			// Token: 0x0400483A RID: 18490
			public static readonly FileCache.ReversedByAccessTimeComparer Instance = new FileCache.ReversedByAccessTimeComparer();
		}
	}
}
