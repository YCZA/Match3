using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using TMPro;
using UnityEngine;

// Token: 0x02000995 RID: 2453
namespace Match3.Scripts1
{
	public class ChallengesIconController : MonoBehaviour
	{
		// Token: 0x06003BA0 RID: 15264 RVA: 0x00127EFE File Offset: 0x001262FE
		public void Init(ProgressionDataService.Service progression, ChallengeService challengeService, ILocalizationService localizationService)
		{
			this.progression = progression;
			this.challengeService = challengeService;
			this.localizationService = localizationService;
			this.AddSlowUpdate(new SlowUpdate(this.Refresh), 3);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x00127F28 File Offset: 0x00126328
		private void Refresh()
		{
			// eli todo 屏蔽挑战
			challengeService.Balancing.icon_enabled_level = 999999;
			challengeService.Balancing.play_minimum_level = 999999;
			base.gameObject.SetActive(false);
		
			if (this.progression.UnlockedLevel < this.challengeService.Balancing.icon_enabled_level)
			{
				base.gameObject.SetActive(false);
				return;
			}
			bool flag = this.progression.UnlockedLevel >= this.challengeService.Balancing.play_minimum_level;
			this.lockObject.SetActive(!flag);
			if (!flag)
			{
				string key = "ui.tournaments.teaser.icon";
				LocaParam locaParam = new LocaParam("{tournamentUnlockLevel}", this.challengeService.Balancing.play_minimum_level);
				this.unlockLevelLabel.text = this.localizationService.GetText(key, new LocaParam[]
				{
					locaParam
				});
			}
			this.spinner.SetActive(!this.challengeService.IsChallengeBundleAvailable());
			this.UpdateNotificationIndicator(flag);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x00128010 File Offset: 0x00126410
		private void UpdateNotificationIndicator(bool challengesUnlocked)
		{
			bool active = challengesUnlocked && this.challengeService.ShouldShowChallengeIconBadge();
			this.badge.SetActive(active);
		}

		// Token: 0x04006398 RID: 25496
		private ProgressionDataService.Service progression;

		// Token: 0x04006399 RID: 25497
		private ChallengeService challengeService;

		// Token: 0x0400639A RID: 25498
		private ILocalizationService localizationService;

		// Token: 0x0400639B RID: 25499
		[SerializeField]
		private TextMeshProUGUI unlockLevelLabel;

		// Token: 0x0400639C RID: 25500
		[SerializeField]
		private GameObject lockObject;

		// Token: 0x0400639D RID: 25501
		[SerializeField]
		private GameObject badge;

		// Token: 0x0400639E RID: 25502
		[SerializeField]
		private GameObject spinner;

		// Token: 0x0400639F RID: 25503
		private Coroutine refreshRoutine;
	}
}
