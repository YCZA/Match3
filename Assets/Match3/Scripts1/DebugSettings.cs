using System;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x02000766 RID: 1894
namespace Match3.Scripts1
{
	[Serializable]
	public class DebugSettings : APlayerPrefsObject<DebugSettings>
	{
		// Token: 0x06002F0C RID: 12044 RVA: 0x000DBEE8 File Offset: 0x000DA2E8
		public bool GetBool(string key)
		{
			bool result = false;
			this.switches.TryGetValue(key, out result);
			return result;
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000DBF07 File Offset: 0x000DA307
		public void SetBool(string key, bool value)
		{
			this.switches[key] = value;
		}

		// Token: 0x0400582C RID: 22572
		public const string PlayerPrefsKey = "DebugSettings";

		// Token: 0x0400582D RID: 22573
		[SerializeField]
		private DictionaryStringToBool switches = new DictionaryStringToBool();

		// Token: 0x0400582E RID: 22574
		public string forcedAbTests = string.Empty;
	}
}
