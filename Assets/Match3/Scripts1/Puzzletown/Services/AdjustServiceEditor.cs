using System;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000739 RID: 1849
	public class AdjustServiceEditor : AService, IAdjustService, IService, IInitializable
	{
		// Token: 0x06002DD4 RID: 11732 RVA: 0x000D5059 File Offset: 0x000D3459
		public AdjustServiceEditor()
		{
			base.OnInitialized.Dispatch();
		}
		
		// Token: 0x06002DD5 RID: 11733 RVA: 0x000D506C File Offset: 0x000D346C
		// public void TrackPurchase(Product product)
		// {
		// }

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000D506E File Offset: 0x000D346E
		public void TrackFbLogin()
		{
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000D5070 File Offset: 0x000D3470
		public void TrackTutorialFinished()
		{
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000D5072 File Offset: 0x000D3472
		public void TrackQuestComplete(string questId)
		{
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000D5074 File Offset: 0x000D3474
		public void TrackLevelComplete(Level level)
		{
			WoogaDebug.Log(new object[]
			{
				"Track level complete",
				level
			});
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000D5092 File Offset: 0x000D3492
		public void TrackThirdPurchase()
		{
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000D5094 File Offset: 0x000D3494
		public void TrackPurchaseScreenOpen()
		{
		}

		public void Init()
		{
			throw new NotImplementedException();
		}
	}
}
