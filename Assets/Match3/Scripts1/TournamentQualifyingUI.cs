using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A54 RID: 2644
namespace Match3.Scripts1
{
	public class TournamentQualifyingUI : MonoBehaviour
	{
		// Token: 0x06003F4E RID: 16206 RVA: 0x0014398D File Offset: 0x00141D8D
		private void OnValidate()
		{
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0014398F File Offset: 0x00141D8F
		public void Setup(LeagueModel leagueModel, TournamentTaskSpriteManager tournamentTaskSpriteManager, ILocalizationService locaService)
		{
			this.SetupLabels(leagueModel, locaService);
			this.SetupProgressBar(leagueModel, tournamentTaskSpriteManager);
			this.SetupButtons(leagueModel);
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x001439A8 File Offset: 0x00141DA8
		private void SetupLabels(LeagueModel leagueModel, ILocalizationService locaService)
		{
			bool flag = leagueModel.playerStatus == PlayerLeagueStatus.NotQualified;
			TournamentType tournamentType = leagueModel.config.config.tournamentType;
			string text = locaService.GetText("ui.tournaments.qualifying.content_header", new LocaParam[0]);
			string text2 = locaService.GetText(TournamentConfig.GetLocaKeyForTournamentType(tournamentType), new LocaParam[0]);
			if (flag)
			{
				int pointsToQualify = leagueModel.config.config.pointsToQualify;
				string locaKeyForEventQualifyInfo = TournamentConfig.GetLocaKeyForEventQualifyInfo(tournamentType);
				LocaParam locaParam = new LocaParam("{tokenAmount}", pointsToQualify);
				string text3 = locaService.GetText(locaKeyForEventQualifyInfo, new LocaParam[]
				{
					locaParam
				});
				this.qualifyingInstructionsLabel.text = text3;
			}
			else
			{
				text = locaService.GetText("ui.tournaments.enter.content_header", new LocaParam[0]);
				string text4 = locaService.GetText("ui.tournaments.enter.text", new LocaParam[0]);
				this.qualifiedInfoLabel.text = text4;
			}
			this.tournamentNameLabel.text = text2;
			this.titleLabel.text = text;
			Transform parent = this.titleLabel.gameObject.transform.parent;
			if (parent != null)
			{
				parent.gameObject.SetActive(!flag);
			}
			this.qualifyingPanel.gameObject.SetActive(flag);
			this.qualifiedPanel.gameObject.SetActive(!flag);
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x00143AFC File Offset: 0x00141EFC
		private void SetupButtons(LeagueModel leagueModel)
		{
			bool flag = leagueModel.playerStatus == PlayerLeagueStatus.NotQualified;
			this.doItButton.gameObject.SetActive(flag);
			this.tipLabel.gameObject.SetActive(flag);
			this.enterButton.gameObject.SetActive(!flag);
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00143B4C File Offset: 0x00141F4C
		private void SetupProgressBar(LeagueModel leagueModel, TournamentTaskSpriteManager spriteManager)
		{
			int playerCurrentPoints = leagueModel.playerCurrentPoints;
			int pointsToQualify = leagueModel.config.config.pointsToQualify;
			this.SetupIcon(leagueModel.config.config.tournamentType, spriteManager);
			this.progressBar.SetProgress((float)playerCurrentPoints, (float)pointsToQualify);
			this.progressBar.gameObject.SetActive(true);
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x00143BA8 File Offset: 0x00141FA8
		private void SetupIcon(TournamentType eventType, TournamentTaskSpriteManager spriteManager)
		{
			Sprite sprite = spriteManager.GetSprite(eventType);
			this.taskIcon.sprite = sprite;
			this.taskIcon.gameObject.SetActive(sprite != null);
		}

		// Token: 0x040068D1 RID: 26833
		public TextMeshProUGUI titleLabel;

		// Token: 0x040068D2 RID: 26834
		public TextMeshProUGUI qualifyingInstructionsLabel;

		// Token: 0x040068D3 RID: 26835
		public TextMeshProUGUI qualifiedInfoLabel;

		// Token: 0x040068D4 RID: 26836
		public TextMeshProUGUI tournamentNameLabel;

		// Token: 0x040068D5 RID: 26837
		public ProgressBarUI progressBar;

		// Token: 0x040068D6 RID: 26838
		public Image taskIcon;

		// Token: 0x040068D7 RID: 26839
		public GameObject qualifyingPanel;

		// Token: 0x040068D8 RID: 26840
		public GameObject qualifiedPanel;

		// Token: 0x040068D9 RID: 26841
		public GameObject doItButton;

		// Token: 0x040068DA RID: 26842
		public GameObject enterButton;

		// Token: 0x040068DB RID: 26843
		public GameObject tipLabel;
	}
}
