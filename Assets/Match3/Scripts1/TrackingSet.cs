using System;
using System.Collections.Generic;
using System.IO;
using Wooga.Foundation.Json;

// Token: 0x02000824 RID: 2084
namespace Match3.Scripts1
{
	public class TrackingSet
	{
		// Token: 0x060033BF RID: 13247 RVA: 0x000F7844 File Offset: 0x000F5C44
		public TrackingSet(string storageFilePath)
		{
			this._storageFilePath = storageFilePath;
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000F7853 File Offset: 0x000F5C53
		public void Init()
		{
			this.Load();
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000F785B File Offset: 0x000F5C5B
		public bool Has(string eventID)
		{
			return this._entries.Contains(eventID);
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000F7869 File Offset: 0x000F5C69
		public bool DidInsert(string eventID)
		{
			if (this._entries.Contains(eventID))
			{
				return false;
			}
			this._entries.Add(eventID);
			this.Save();
			return true;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000F7894 File Offset: 0x000F5C94
		public void Load()
		{
			try
			{
				string input = File.ReadAllText(this._storageFilePath);
				this._entries = JSON.Deserialize<HashSet<string>>(input);
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Warning loading TrackingSet: ",
					this._storageFilePath,
					ex
				});
			}
			if (this._entries == null)
			{
				this._entries = new HashSet<string>();
			}
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000F790C File Offset: 0x000F5D0C
		public void Save()
		{
			try
			{
				string contents = JSON.Serialize(this._entries, false, 1, ' ');
				File.WriteAllText(this._storageFilePath, contents);
			}
			catch (Exception)
			{
				WoogaDebug.Log(new object[]
				{
					"Error saving the Tracking set to ",
					this._storageFilePath
				});
			}
		}

		// Token: 0x04005BC6 RID: 23494
		private string _storageFilePath;

		// Token: 0x04005BC7 RID: 23495
		private HashSet<string> _entries;
	}
}
