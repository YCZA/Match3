using System.IO;
using System.Text;
using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Core.Extensions;
using Match3.Scripts1.Wooga.Core.IO;
using Wooga.Core.Utilities;

namespace Wooga.Core.Storage
{
	// Token: 0x02000397 RID: 919
	public class SbsFileStorage : ISbsStorage
	{
		// Token: 0x06001BDD RID: 7133 RVA: 0x0007B108 File Offset: 0x00079508
		public SbsRichData Load(string fullPath)
		{
			return this._contextLock.With<SbsRichData>(fullPath, delegate()
			{
				if (!AtomicFile.ExistsAndRecoverFromAtomicWrite(fullPath))
				{
					return null;
				}
				SbsRichData sbsRichData = SbsFileStorage.LoadData(fullPath);
				if (sbsRichData == null)
				{
					sbsRichData = this.LoadDataLegacy(fullPath);
				}
				return sbsRichData;
			});
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0007B146 File Offset: 0x00079546
		public void Save(string fullPath, string data, int formatVersion)
		{
			this.Save(fullPath, new SbsRichData(data, formatVersion));
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0007B158 File Offset: 0x00079558
		public void Save(string fullPath, SbsRichData data)
		{
			this._contextLock.With<bool>(fullPath, delegate()
			{
				AtomicFile.WriteAllTextAtomic(fullPath, data.Serialize());
				this.CleanUpLegacyData(fullPath);
				IcloudBackup.Exclude(fullPath);
				return true;
			});
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x0007B1A0 File Offset: 0x000795A0
		public void Delete(string fullPath)
		{
			this._contextLock.With<bool>(fullPath, delegate()
			{
				SbsFileStorage.DeleteFileIfItExists(fullPath);
				string tempFilePath = SbsFileStorage.GetTempFilePath(fullPath);
				SbsFileStorage.DeleteFileIfItExists(tempFilePath);
				string newFilePath = SbsFileStorage.GetNewFilePath(fullPath);
				SbsFileStorage.DeleteFileIfItExists(newFilePath);
				return true;
			});
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x0007B1D8 File Offset: 0x000795D8
		private static string GetTempFilePath(string fullPath)
		{
			return fullPath + "._tt";
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0007B1F4 File Offset: 0x000795F4
		private static string GetNewFilePath(string fullPath)
		{
			return fullPath + "._nn";
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0007B210 File Offset: 0x00079610
		public bool Exists(string path)
		{
			return this._contextLock.With<bool>(path, () => File.Exists(path));
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0007B247 File Offset: 0x00079647
		private static void DeleteFileIfItExists(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0007B25C File Offset: 0x0007965C
		private void CleanUpLegacyData(string fullPath)
		{
			string path = fullPath + ".sbsmeta";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0007B288 File Offset: 0x00079688
		private static SbsRichData LoadData(string fullPath)
		{
			SbsRichData result = null;
			bool flag = false;
			using (StreamReader streamReader = File.OpenText(fullPath))
			{
				int num = 0;
				while (num < "WDK_META_DATA_V1".Length && !streamReader.EndOfStream)
				{
					if ("WDK_META_DATA_V1"[num] != (char)streamReader.Read())
					{
						flag = true;
						break;
					}
					num++;
				}
				if (!flag && (ushort)streamReader.Read() == 10)
				{
					int num2 = 0;
					int.TryParse(streamReader.ReadLine(), out num2);
					if (num2 > 0)
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < num2; i++)
						{
							if (streamReader.EndOfStream)
							{
								flag = true;
								break;
							}
							stringBuilder.AppendFormat("{0}\n", streamReader.ReadLine());
						}
						if (!flag)
						{
							SbsMetaData sbsMetaData = SbsMetaData.TryCreateFromString(stringBuilder.ToString());
							if (sbsMetaData != null)
							{
								result = new SbsRichData(streamReader.ReadToEnd(), sbsMetaData);
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0007B3A4 File Offset: 0x000797A4
		private SbsRichData LoadDataLegacy(string fullPath)
		{
			SbsMetaData sbsMetaData = null;
			if (!File.Exists(fullPath))
			{
				return null;
			}
			string text = File.ReadAllText(fullPath);
			Assert.That(text != null, "Data should not be null");
			string path = fullPath + ".sbsmeta";
			if (File.Exists(path))
			{
				string data = File.ReadAllText(path);
				sbsMetaData = SbsMetaData.TryCreateFromString(data);
			}
			SbsRichData result;
			if (sbsMetaData != null)
			{
				result = new SbsRichData(text, sbsMetaData);
			}
			else
			{
				result = new SbsRichData(text, -1);
			}
			return result;
		}

		// Token: 0x04004973 RID: 18803
		public const string TEMP_FILE_EXTENSION = "._tt";

		// Token: 0x04004974 RID: 18804
		public const string NEW_FILE_EXTENSION = "._nn";

		// Token: 0x04004975 RID: 18805
		private SbsContextLock _contextLock = new SbsContextLock();
	}
}
