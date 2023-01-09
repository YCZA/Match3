using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using TMPro;

// Token: 0x02000A57 RID: 2647
namespace Match3.Scripts1
{
	public class TournamentRewardPreviewRoot : APtSceneRoot<LeagueModel>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003F6A RID: 16234 RVA: 0x001447A3 File Offset: 0x00142BA3
		private void OnValidate()
		{
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x001447A8 File Offset: 0x00142BA8
		public override void Setup(LeagueModel parameters)
		{
			if (parameters.IsValid())
			{
				TournamentRewardConfig rewards = parameters.config.config.rewards;
				TournamentType tournamentType = parameters.config.config.tournamentType;
				this.goldRewards.gameObject.SetActive(true);
				this.goldRewards.Setup(tournamentType, 1, rewards.gold, this.locaService);
				this.silverRewards.gameObject.SetActive(true);
				this.silverRewards.Setup(tournamentType, 2, rewards.silver, this.locaService);
				this.bronzeRewards.gameObject.SetActive(true);
				this.bronzeRewards.Setup(tournamentType, 3, rewards.bronze, this.locaService);
				this.woodRewards.gameObject.SetActive(true);
				this.woodRewards.Setup(tournamentType, 4, rewards.wood, this.locaService);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Trying to show preview of invalid model."
				});
				this.goldRewards.gameObject.SetActive(false);
				this.silverRewards.gameObject.SetActive(false);
				this.bronzeRewards.gameObject.SetActive(false);
				this.woodRewards.gameObject.SetActive(false);
			}
			base.Setup(parameters);
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x001448ED File Offset: 0x00142CED
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x00144924 File Offset: 0x00142D24
		private void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x0014495B File Offset: 0x00142D5B
		public void Handle(PopupOperation evt)
		{
			if (evt == PopupOperation.Close)
			{
				this.Close();
			}
		}

		// Token: 0x040068FF RID: 26879
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04006900 RID: 26880
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006901 RID: 26881
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006902 RID: 26882
		public AnimatedUi dialog;

		// Token: 0x04006903 RID: 26883
		public TextMeshProUGUI titleLabelPart1;

		// Token: 0x04006904 RID: 26884
		public TextMeshProUGUI titleLabelPart2;

		// Token: 0x04006905 RID: 26885
		public TextMeshProUGUI bodyLabel;

		// Token: 0x04006906 RID: 26886
		public RewardPreviewContainer goldRewards;

		// Token: 0x04006907 RID: 26887
		public RewardPreviewContainer silverRewards;

		// Token: 0x04006908 RID: 26888
		public RewardPreviewContainer bronzeRewards;

		// Token: 0x04006909 RID: 26889
		public RewardPreviewContainer woodRewards;
	}
}
