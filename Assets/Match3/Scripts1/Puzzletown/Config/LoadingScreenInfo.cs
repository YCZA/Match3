using System;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000485 RID: 1157
	[Serializable]
	public class LoadingScreenInfo
	{
		// Token: 0x06002133 RID: 8499 RVA: 0x0008BBF7 File Offset: 0x00089FF7
		public string GetFullPath()
		{
			return string.Format("Assets/Puzzletown/Shared/LoadingScreen/LoadingScreens/{0}.png", this.id);
		}

		// Token: 0x04004C11 RID: 19473
		public string id;

		// Token: 0x04004C12 RID: 19474
		public string assetBundle;

		// Token: 0x04004C13 RID: 19475
		public string season;

		// Token: 0x04004C14 RID: 19476
		public string condition;
	}
}
