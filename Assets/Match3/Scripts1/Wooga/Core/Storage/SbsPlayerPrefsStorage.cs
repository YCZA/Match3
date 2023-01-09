using System;
using Match3.Scripts1.Wooga.Core.Data;
using UnityEngine;

namespace Wooga.Core.Storage
{
	// Token: 0x02000399 RID: 921
	public class SbsPlayerPrefsStorage : ISbsStorage
	{
		// Token: 0x06001BEE RID: 7150 RVA: 0x0007B5E3 File Offset: 0x000799E3
		public SbsPlayerPrefsStorage(string prefix)
		{
			if (string.IsNullOrEmpty(prefix))
			{
				throw new ArgumentException("SbsPlayerPrefsStorage needs to be initialised with a non-empty prefix.");
			}
			this._prefix = prefix;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0007B608 File Offset: 0x00079A08
		public SbsRichData Load(string name)
		{
			if (this.Exists(name))
			{
				string key = this.GetKey(name);
				string @string = PlayerPrefs.GetString(key);
				return new SbsRichData(@string, -1);
			}
			return new SbsRichData(string.Empty, -1);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0007B644 File Offset: 0x00079A44
		public void Save(string name, string contents, int format_version)
		{
			string key = this.GetKey(name);
			PlayerPrefs.SetString(key, contents);
			PlayerPrefs.Save();
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0007B665 File Offset: 0x00079A65
		public void Save(string name, SbsRichData data)
		{
			this.Save(name, data.Data, -1);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0007B678 File Offset: 0x00079A78
		public void Delete(string name)
		{
			string key = this.GetKey(name);
			PlayerPrefs.DeleteKey(key);
			PlayerPrefs.Save();
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0007B698 File Offset: 0x00079A98
		public bool Exists(string name)
		{
			string key = this.GetKey(name);
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0007B6B3 File Offset: 0x00079AB3
		private string GetKey(string name)
		{
			return string.Format("{0}.{1}", this._prefix, name);
		}

		// Token: 0x04004978 RID: 18808
		private string _prefix;
	}
}
