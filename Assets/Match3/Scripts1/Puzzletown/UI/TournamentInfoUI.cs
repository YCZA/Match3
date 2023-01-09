using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A50 RID: 2640
	public class TournamentInfoUI : MonoBehaviour
	{
		// Token: 0x06003F38 RID: 16184 RVA: 0x001431E5 File Offset: 0x001415E5
		private void OnValidate()
		{
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x001431E7 File Offset: 0x001415E7
		public void Setup(TournamentType tournamentType, ILocalizationService locaService, HelpshiftService helpshiftService)
		{
			this.SetupTaskIcon(this.spriteManager.GetSprite(tournamentType));
			this.SetupLabel(tournamentType, locaService);
			this.helpshiftService = helpshiftService;
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0014320A File Offset: 0x0014160A
		public void OnInfoButtonTap()
		{
			if (this.helpshiftService != null)
			{
				this.helpshiftService.ShowFAQSection(HelpshiftService.FAQ_SECTION_TOURNAMENT);
			}
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x00143227 File Offset: 0x00141627
		private void SetupTaskIcon(Sprite sprite)
		{
			this.taskIcon.sprite = sprite;
			this.taskIcon.gameObject.SetActive(sprite != null);
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0014324C File Offset: 0x0014164C
		private void SetupLabel(TournamentType tournamentType, ILocalizationService locaService)
		{
			string key = "unknown";
			switch (tournamentType)
			{
			case TournamentType.Bomb:
				key = "ui.tournaments.level_start.text_type1";
				break;
			case TournamentType.Butterfly:
				key = "ui.tournaments.level_start.text_type2";
				break;
			case TournamentType.Line:
				key = "ui.tournaments.level_start.text_type3";
				break;
			case TournamentType.Strawberry:
				key = "ui.tournaments.level_start.text_type4_strawberry";
				break;
			case TournamentType.Banana:
				key = "ui.tournaments.level_start.text_type5_banana";
				break;
			case TournamentType.Plum:
				key = "ui.tournaments.level_start.text_type6_plum";
				break;
			case TournamentType.Apple:
				key = "ui.tournaments.level_start.text_type7_apple";
				break;
			case TournamentType.Starfruit:
				key = "ui.tournaments.level_start.text_type8_starfruit";
				break;
			case TournamentType.Grape:
				key = "ui.tournaments.level_start.text_type9_grape";
				break;
			}
			this.tournamentInfoLabel.text = locaService.GetText(key, new LocaParam[0]);
		}

		// Token: 0x040068B8 RID: 26808
		public Image taskIcon;

		// Token: 0x040068B9 RID: 26809
		public TextMeshProUGUI tournamentInfoLabel;

		// Token: 0x040068BA RID: 26810
		public TournamentTaskSpriteManager spriteManager;

		// Token: 0x040068BB RID: 26811
		private HelpshiftService helpshiftService;
	}
}
