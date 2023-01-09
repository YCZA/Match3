using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A9 RID: 1961
	[Serializable]
	public class GlobalReplaceKey
	{
		// Token: 0x06003015 RID: 12309 RVA: 0x000E1C5E File Offset: 0x000E005E
		public GlobalReplaceKey(string replaceKey, string replaceValue)
		{
			this.key = "{" + replaceKey + "}";
			this.value = replaceValue;
		}

		// Token: 0x04005922 RID: 22818
		public string key;

		// Token: 0x04005923 RID: 22819
		public string value;
	}
}
