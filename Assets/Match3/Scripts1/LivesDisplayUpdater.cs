using System;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A12 RID: 2578
namespace Match3.Scripts1
{
	public class LivesDisplayUpdater : MonoBehaviour
	{
		// Token: 0x06003E01 RID: 15873 RVA: 0x0013A1A8 File Offset: 0x001385A8
		public void UpdateLifeTimer(LivesService livesService, GameStateService stateService, ILocalizationService localizationService)
		{
			if (livesService.IsCurrentlyUnlimitedLives)
			{
				this.labelLifeTimer.text = TimeFormatter.FormatTime(stateService.UnlimitedLivesEnd - DateTime.Now);
				this.unlimitedLivesIcon.gameObject.SetActive(true);
			}
			else if (livesService.SecondsRemaining == -1)
			{
				this.labelLifeTimer.text = localizationService.GetText("ui.dialog.lives.full", new LocaParam[0]);
				this.unlimitedLivesIcon.gameObject.SetActive(false);
			}
			else
			{
				this.labelLifeTimer.text = TimeFormatter.FormatTime(TimeSpan.FromSeconds((double)livesService.SecondsRemaining));
				this.unlimitedLivesIcon.gameObject.SetActive(false);
			}
		}

		// Token: 0x040066DA RID: 26330
		[SerializeField]
		private TextMeshProUGUI labelLifeTimer;

		// Token: 0x040066DB RID: 26331
		[SerializeField]
		private Image unlimitedLivesIcon;
	}
}
