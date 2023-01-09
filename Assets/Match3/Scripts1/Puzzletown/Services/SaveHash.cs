using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A8 RID: 1960
	[Serializable]
	public class SaveHash
	{
		// Token: 0x06003014 RID: 12308 RVA: 0x000E1C48 File Offset: 0x000E0048
		public SaveHash(string device, string hash)
		{
			this.deviceId = device;
			this.saveHash = hash;
		}

		// Token: 0x04005920 RID: 22816
		public string deviceId;

		// Token: 0x04005921 RID: 22817
		public string saveHash;
	}
}
