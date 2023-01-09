using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000325 RID: 805
	public class FileCache
	{
		// Token: 0x06001910 RID: 6416 RVA: 0x00071EC3 File Offset: 0x000702C3
		public FileCache(string cacheDirectory, int cacheSize = 50)
		{
			this._cacheDirectory = cacheDirectory;
			this._cacheSize = cacheSize;
			this.Cleanup();
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00071EE0 File Offset: 0x000702E0
		public OptionalResult<string> ReadText(string path, string md5Hash = null)
		{
			string text = this.CreateFilePath(path);
			if (!File.Exists(text))
			{
				return OptionalResult<string>.None;
			}
			OptionalResult<string> result;
			try
			{
				if (string.IsNullOrEmpty(md5Hash))
				{
					result = File.ReadAllText(text);
				}
				else
				{
					byte[] bytes = File.ReadAllBytes(text);
					if (MD5Util.VerifyMd5Hash(bytes, md5Hash))
					{
						result = Encoding.UTF8.GetString(bytes);
					}
					else
					{
						FileUtil.DeleteFile(text);
						result = OptionalResult<string>.None;
					}
				}
			}
			catch (Exception ex)
			{
				Log.ErrorFormatted("Error verifying hash: {0}", new object[]
				{
					ex
				});
				result = OptionalResult<string>.None;
			}
			return result;
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00071F90 File Offset: 0x00070390
		public bool Write(string path, byte[] content)
		{
			string filePath = this.CreateFilePath(path);
			bool result;
			try
			{
				FileUtil.WriteAllBytes(filePath, content);
				result = true;
			}
			catch (Exception ex)
			{
				FileUtil.DeleteFile(filePath);
				Log.ErrorFormatted("Error writing file: {0}", new object[]
				{
					ex
				});
				result = false;
			}
			return result;
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00071FE8 File Offset: 0x000703E8
		public bool Write(string path, string content)
		{
			return this.Write(path, Encoding.UTF8.GetBytes(content));
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00071FFC File Offset: 0x000703FC
		public bool IsCached(string path, string md5Hash = null)
		{
			string path2 = this.CreateFilePath(path);
			if (!File.Exists(path2))
			{
				return false;
			}
			if (string.IsNullOrEmpty(md5Hash))
			{
				return true;
			}
			bool result;
			try
			{
				byte[] bytes = File.ReadAllBytes(path2);
				result = MD5Util.VerifyMd5Hash(bytes, md5Hash);
			}
			catch (Exception ex)
			{
				Log.ErrorFormatted("Error reading file: {0}", new object[]
				{
					ex
				});
				result = false;
			}
			return result;
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0007206C File Offset: 0x0007046C
		private string CreateFilePath(string path)
		{
			path = path.Replace("\\", "/");
			while (path[0] == '/')
			{
				path = path.Substring(1);
			}
			return Path.Combine(this._cacheDirectory, path);
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x000720A8 File Offset: 0x000704A8
		public void Cleanup()
		{
			if (!Directory.Exists(this._cacheDirectory))
			{
				return;
			}
			List<string> list = new List<string>();
			list.AddRange(Directory.GetFiles(this._cacheDirectory, "*", SearchOption.AllDirectories));
			if (list.Count > this._cacheSize)
			{
				list.Sort(FileCache.ByAccessTimeComparer.Instance);
				for (int i = list.Count - 1; i > this._cacheSize; i--)
				{
					FileUtil.DeleteFile(list[i]);
				}
			}
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0007212A File Offset: 0x0007052A
		public bool Clear()
		{
			return FileUtil.ClearDirectory(this._cacheDirectory);
		}

		// Token: 0x040047EC RID: 18412
		private readonly string _cacheDirectory;

		// Token: 0x040047ED RID: 18413
		private readonly int _cacheSize;

		// Token: 0x02000326 RID: 806
		public class ByAccessTimeComparer : IComparer<string>
		{
			// Token: 0x06001918 RID: 6424 RVA: 0x00072137 File Offset: 0x00070537
			private ByAccessTimeComparer()
			{
			}

			// Token: 0x06001919 RID: 6425 RVA: 0x00072140 File Offset: 0x00070540
			public int Compare(string x, string y)
			{
				return File.GetLastAccessTime(x).CompareTo(File.GetLastAccessTime(y));
			}

			// Token: 0x040047EE RID: 18414
			public static readonly FileCache.ByAccessTimeComparer Instance = new FileCache.ByAccessTimeComparer();
		}
	}
}
