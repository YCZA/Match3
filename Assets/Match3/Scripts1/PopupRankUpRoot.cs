using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;

// Token: 0x02000849 RID: 2121
namespace Match3.Scripts1
{
	public class PopupRankUpRoot : APtSceneRoot<VillageRank>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003489 RID: 13449 RVA: 0x000FB7D4 File Offset: 0x000F9BD4
		protected override void Go()
		{
			if (base.registeredFirst)
			{
				this.parameters = new VillageRank
				{
					village_rank = 3,
					harmony_goal = 120,
					reward_diamonds = 10,
					reward_coins = 10,
					reward_booster_type = string.Empty,
					reward_booster_amount = 5
				};
			}
			this.rank = this.parameters;
			this.newVillageRank.text = this.rank.village_rank.ToString();
			this.newVillageRankMessage.text = this.localizationService.GetText("ui.popup.village_rank_up.desc", new LocaParam[]
			{
				new LocaParam("{rank}", this.rank.village_rank)
			});
			this.rewardMaterials = new MaterialAmount[]
			{
				new MaterialAmount("diamonds", this.rank.reward_diamonds, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("coins", this.rank.reward_coins, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount(this.rank.reward_booster_type, this.rank.reward_booster_amount, MaterialAmountUsage.Undefined, 0)
			};
			this.rewards.Show(this.rewardMaterials);
			this.dialog.Show();
			this.canvasGroup.interactable = true;
			this.externalGamesService.ShowVillageRankAchievement(this.parameters.village_rank);
			this.audioService.PlaySFX(AudioId.PopupShowVillageRankUp, false, false, false);
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000FB96B File Offset: 0x000F9D6B
		private void CollectAndClose()
		{
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.trackingService.TrackVillageRank(this.rank);
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000FB9A0 File Offset: 0x000F9DA0
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Close)
			{
				this.canvasGroup.interactable = false;
				// base.Invoke("CollectAndClose", 1.5f);
				CollectAndClose();
				this.gameState.Resources.AddMaterials(this.rewardMaterials, true, "家园升级");
				MaterialAmountView.CollectMaterials(this.resourcePanel, base.gameObject);
			}
		}

		// Token: 0x04005C82 RID: 23682
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot resourcePanel;

		// Token: 0x04005C83 RID: 23683
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005C84 RID: 23684
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005C85 RID: 23685
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005C86 RID: 23686
		[WaitForService(true, true)]
		private ExternalGamesService externalGamesService;

		// Token: 0x04005C87 RID: 23687
		public MaterialsDataSource rewards;

		// Token: 0x04005C88 RID: 23688
		public TMP_Text newVillageRank;

		// Token: 0x04005C89 RID: 23689
		public TMP_Text newVillageRankMessage;

		// Token: 0x04005C8A RID: 23690
		public AnimatedUi dialog;

		// Token: 0x04005C8B RID: 23691
		public CanvasGroup canvasGroup;

		// Token: 0x04005C8C RID: 23692
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005C8D RID: 23693
		private MaterialAmount[] rewardMaterials;

		// Token: 0x04005C8E RID: 23694
		private VillageRank rank;
	}
}
