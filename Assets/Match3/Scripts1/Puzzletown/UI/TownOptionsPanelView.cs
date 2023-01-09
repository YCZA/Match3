using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.UI;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009E8 RID: 2536
	public class TownOptionsPanelView : UiSimpleView<TownOptionsCommand>, IDataView<FacebookService>
	{
		// Token: 0x06003D33 RID: 15667 RVA: 0x00134C4C File Offset: 0x0013304C
		public void Show(FacebookService fb)
		{
			this.facebookService = fb;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00134C58 File Offset: 0x00133058
		public override void Show(TownOptionsCommand data)
		{
			return;
			// 隐藏facebook相关东西
			if (this.loginPage && !this.facebookService.LoggedIn() && this.CheckMatch(data))
			{
				this.loginPage.Show();
				base.Hide();
			}
			else
			{
				base.Show(data);
			}
			if (this.loginRewardPanel != null)
			{
				this.loginRewardPanel.SetActive(!this.facebookService.RecievedLoginReward());
			}
		}

		// Token: 0x040065F2 RID: 26098
		public TownOptionsPanelView loginPage;

		// Token: 0x040065F3 RID: 26099
		public GameObject loginRewardPanel;

		// Token: 0x040065F4 RID: 26100
		private FacebookService facebookService;

		// Token: 0x040065F5 RID: 26101
		private GameStateService gameStateService;
	}
}
